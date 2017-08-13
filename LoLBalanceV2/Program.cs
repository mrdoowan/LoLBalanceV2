using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Drawing;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.CSharp;

namespace LoLBalanceV2
{
    class Program
    {
        private const int ROLE_CHANGE_OPTION = 1;
        private const int START_SEED = 501;
        private const int MAX_CHECKS = 500;
        private const int MIN_PLAYERS = 40;
        private const int RANDOMIZE = 10;
        private const int RANGE_THRESHOLD = 3;
        private const int NUM_ROLES = 5;
        // Color Codes for Ranks
        private const string LEVELHEX = "#B4A7D6";
        private const string BRONZEHEX = "#9B5105";
        private const string SILVERHEX = "#C0C0C0";
        private const string GOLDHEX = "#F6B26B";
        private const string PLATHEX = "#5CBFA1";
        private const string DIAMONDHEX = "#A4C2F4";
        private const string MASTERHEX = "#FFD966";
        private const string CHALLENGERHEX = "#FFD966";

        [STAThread]
        // Main Application
        public static void Main(string[] args) {
            // Assume for Input is a .csv
            // Name(0), IGN(1), Tier(2), Division(3), Pri Role(4), Sec Role(5), Duo(6)
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV files (*.csv)|*.csv";
            ofd.Title = "Open Spreadsheet";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK) {
                // ----- PART 1: Parse Inputs
                int parseRow = 0;
                // IGN -> Player Obj. IGN ALWAYS LOWERCASE
                Dictionary<string, Player> roster = new Dictionary<string, Player>();
                // IGN -> duoIGN. ALWAYS IN LOWERCASE
                Dictionary<string, string> duoList = new Dictionary<string, string>();
                try {
                    string openPath = ofd.FileName;
                    string csvText = File.ReadAllText(openPath);
                    string[] rowList = csvText.Split('\n');
                    foreach (string rowPlayer in rowList) {
                        parseRow++;
                        string rowPlayerTrimmed = rowPlayer.TrimEnd('\r');
                        string[] rowParts = rowPlayerTrimmed.Split(',');
                        if (rowParts.Length == 1) { break; }
                        string playerName = rowParts[0];
                        string playerIGN = rowParts[1];
                        Tier playerTier = STRING_TO_TIER[rowParts[2]];
                        int playerDiv = int.Parse(rowParts[3]);
                        Role playerPrimary = STRING_TO_ROLE[rowParts[4]];
                        Role playerSecond = (playerPrimary == Role.FILL) ? 
                            Role.FILL : STRING_TO_ROLE[rowParts[5]];
                        string playerDuo = rowParts[6];
                        duoList.Add(playerIGN.ToLower(), playerDuo.ToLower());
                        roster.Add(playerIGN.ToLower(), new Player(playerName, playerIGN, 
                            playerTier, playerDiv, playerPrimary, playerSecond, playerDuo));
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("ERROR - Parsing Inputs: " + ex.Message + '\n' +
                        "Check Row: " + parseRow);
                    return;
                }
                // ----- Check number of Players
                if (roster.Count < MIN_PLAYERS) {
                    Console.WriteLine("ERROR - Total number of Players does not exceed 40");
                    return;
                }
                if (roster.Count % NUM_ROLES != 0) {
                    Console.WriteLine("ERROR - Total number of Players is not divisible by 5");
                    return;
                }
                int numTeams = roster.Count / NUM_ROLES;
                // ----- Validate duos and their Roles
                List<string> noDuoList = new List<string>();
                foreach (string summoner in duoList.Keys) {
                    try {
                        string duoIGNLower = duoList[summoner.ToLower()];
                        string origIGNLower = duoList[duoIGNLower];
                        if (summoner.ToLower() != origIGNLower) {
                            throw new Exception();
                            // Pretty much being treated as not finding a Key
                            // It's bad practice, but suitable
                        }
                        // At this point, the duos are confirmed.
                        // Now both duos roles for primary and secondary must be different
                        // If not, Fill for both primary and secondary
                        // 1) If Primary/Secondary are the same
                        Player origPlayer = roster[summoner];
                        Player duoPlayer = roster[duoIGNLower];
                        if ((origPlayer.primaryRole == duoPlayer.secondRole && 
                            origPlayer.secondRole == duoPlayer.primaryRole) ||
                            (origPlayer.primaryRole == duoPlayer.primaryRole &&
                            origPlayer.secondRole == duoPlayer.secondRole)) {
                            origPlayer.primaryRole = Role.FILL;
                            origPlayer.secondRole = Role.FILL;
                            duoPlayer.primaryRole = Role.FILL;
                            duoPlayer.secondRole = Role.FILL;
                        }
                        // 2) If both Primary is same and both Secondary is Fill
                        // Roll a number on who would get Filled
                        if (origPlayer.primaryRole == duoPlayer.primaryRole &&
                            origPlayer.secondRole == Role.FILL &&
                            duoPlayer.secondRole == Role.FILL) {
                            if (randomNumber(1, 2) == 1) { origPlayer.primaryRole = Role.FILL; }
                            else { duoPlayer.primaryRole = Role.FILL; }
                        }
                    }
                    catch {
                        // Player couldn't be found or incorrect, so we blank out duo
                        Player player = roster[summoner];
                        player.duo = "";
                        // Can't modify duoList yet, so store the name
                        noDuoList.Add(summoner);
                    }
                }
                // Blank out duoList
                foreach (string summoner in noDuoList) {
                    duoList[summoner] = "";
                }
                // Store lowest ranked player
                Player lowestPlayer = roster.Values.ToList().Min();

                // ----- PART 2: ASSIGN ROLES
                int lowestRange = RANGE_THRESHOLD + 1, randSeed = START_SEED, currChecks = 0;
                Balance bestBalance = new Balance(); // This will store our bestBalance
                while (lowestRange > RANGE_THRESHOLD && currChecks < MAX_CHECKS) {
                    // Preparations: Make a copy of roster that can be edited
                    Dictionary<string, Player> masterList = deepClone(roster);
                    randSeed++;

                    // Begin
                    Dictionary<Role, List<Player>> assignRoleList = new Dictionary<Role, List<Player>>() {
                        { Role.TOP, new List<Player>() },
                        { Role.JNG, new List<Player>() },
                        { Role.MID, new List<Player>() },
                        { Role.BOT, new List<Player>() },
                        { Role.SUP, new List<Player>() },
                    };
                    // Assign non-Fill Primary Roles
                    try {
                        assignRoles(ref masterList, ref assignRoleList, duoList, 
                            numTeams, randSeed, true);
                    }
                    catch (Exception ex) {
                        Console.WriteLine("ERROR - Assigning primary roles\n" +
                            "Reason: " + ex.Message);
                        return;
                    }
                    // Assign non-Fill Secondary Roles
                    try {
                        assignRoles(ref masterList, ref assignRoleList, duoList, 
                            numTeams, randSeed, false);
                    }
                    catch (Exception ex) {
                        Console.WriteLine("ERROR - Assigning secondary roles\n" +
                            "Reason: " + ex.Message);
                        return;
                    }
                    // the remaining masterList is now fillList
                    while (masterList.Count > 0) {
                        // Find role with smallest points, and add highest Ranked player
                        Role addFillRole = roleMinPointsFill(assignRoleList, numTeams);
                        Player addPlayer = masterList.Values.ToList().Max();
                        addPlayer.assignedRole = addFillRole;
                        assignRoleList[addFillRole].Add(addPlayer);
                        masterList.Remove(addPlayer.ign.ToLower());
                    }
                    // Validation: Should hopefully never occur
                    foreach (List<Player> roleList in assignRoleList.Values) {
                        if (roleList.Count != numTeams) {
                            Console.WriteLine("ERROR - assignRoleList does not have numTeams");
                            return;
                        }
                    }

                    // ---- PART 3: BALANCING
                    // Initialize the Teams
                    Balance currBalance = new Balance(assignRoleList, out lowestRange);
                    int currMaxIndex = currBalance.getMaxTeamIndex();
                    int currMinIndex = currBalance.getMinTeamIndex();
                    int rolesRemain = NUM_ROLES;
                    // All queues acting as a clock queue
                    Queue<Role> roleQ = new Queue<Role>();
                    roleQ.Enqueue(Role.TOP); roleQ.Enqueue(Role.JNG); roleQ.Enqueue(Role.MID);
                    roleQ.Enqueue(Role.BOT); roleQ.Enqueue(Role.SUP);
                    Queue<int> maxIndexQ = new Queue<int>();
                    Queue<int> minIndexQ = new Queue<int>();
                    for (int i = 0; i < numTeams; ++i) {
                        maxIndexQ.Enqueue(i);
                        minIndexQ.Enqueue(i);
                    }
                    // Conditions:
                    // 1) rolesRemaining == 0
                    // If lowestRange changes: rolesRemaining set back to 5
                    // To move onto the next Role
                    // 1) Check every iteration of minTeamIndex (minCheckRemain == 0)
                    // 2) Check every iteration of maxTeamIndex (maxCheckRemain == 0)
                    // If lowestRange changes: both checkRemaining set back to numTeams
                    // Break entirely if: 
                    // 1) lowestRange <= THRESHOLD
                    while (rolesRemain > 0) {
                        Role checkRole = roleQ.Peek();
                        int minCheckRemain = numTeams, maxCheckRemain = numTeams;
                        bool minFlag = true;
                        // Check within each Role
                        while (!(minCheckRemain == 0 && maxCheckRemain == 0)) {
                            // Work on minIndex first
                            int swapIndex = (minFlag) ? minIndexQ.Peek() : maxIndexQ.Peek();
                            int newRange = (minFlag) ?
                                currBalance.switchPlayer(checkRole, currMinIndex, swapIndex) :
                                currBalance.switchPlayer(checkRole, currMaxIndex, swapIndex);
                            if (newRange < lowestRange) {
                                // Condition 2: New minRange
                                bestBalance = deepClone(currBalance);
                                currMinIndex = currBalance.getMinTeamIndex();
                                currMaxIndex = currBalance.getMaxTeamIndex();
                                lowestRange = newRange;
                                Console.WriteLine(lowestRange);
                                minCheckRemain = numTeams;
                                maxCheckRemain = numTeams;
                                rolesRemain = NUM_ROLES;
                                minFlag = true;
                            }
                            else {
                                // Condition 3: Continue iterating. Switch currBalance back
                                if (minFlag) {
                                    minCheckRemain--;
                                    currBalance.switchPlayer(checkRole, currMinIndex, swapIndex);
                                }
                                else {
                                    maxCheckRemain--;
                                    currBalance.switchPlayer(checkRole, currMaxIndex, swapIndex);
                                }
                                if (minCheckRemain == 0) {
                                    minFlag = false;
                                }
                            }
                            // Cycle through the clock queue
                            if (minFlag) {
                                minIndexQ.Dequeue();
                                minIndexQ.Enqueue(swapIndex);
                            }
                            else {
                                maxIndexQ.Dequeue();
                                maxIndexQ.Enqueue(swapIndex);
                            }
                        }
                        // Made it to the end of a complete role check
                        rolesRemain--;
                        roleQ.Dequeue();
                        roleQ.Enqueue(checkRole);
                    }
                    currChecks++;
                }

                // ---- PART 4: OUTPUT BEST
                // If we got to this point, that means bestBalance has our best teams
                if (lowestRange <= RANGE_THRESHOLD &&
                    MessageBox.Show("A balance was found with a Range of " + lowestRange + " at seed " + randSeed +
                    ".\nWould you like to save it?", 
                    "Finished", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    SaveFileDialog sfd_Excel = new SaveFileDialog();
                    sfd_Excel.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
                    sfd_Excel.Title = "Save Teams";
                    if (sfd_Excel.ShowDialog() == DialogResult.OK) {
                        //Application.DoEvents();
                        //Cursor.Current = Cursors.WaitCursor;
                        // Make an Excel Sheet
                        Excel.Application xlApp = new Excel.Application();
                        Excel.Workbook xlWorkBook;
                        Excel.Worksheet xlWorkSheet;
                        object mis = System.Reflection.Missing.Value;
                        xlWorkBook = xlApp.Workbooks.Add(mis);
                        xlWorkSheet = (Excel.Worksheet)xlWorkBook.ActiveSheet;
                        xlWorkSheet.Name = "Teams Balance";
                        try {
                            xlWorkSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaper11x17;
                            xlWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                        }
                        catch { }

                        #region Make Excel Sheet
                        xlWorkSheet.Columns["A"].ColumnWidth = 20.00;   // Name
                        xlWorkSheet.Columns["B"].ColumnWidth = 20.00;   // Summoner Name
                        xlWorkSheet.Columns["C"].ColumnWidth = 20.00;   // Assigned Role
                        xlWorkSheet.Columns["D"].ColumnWidth = 15.00;   // Ranking
                        xlWorkSheet.Cells[1, 1] = "Name";
                        xlWorkSheet.Cells[1, 2] = "Summoner Name";
                        xlWorkSheet.Cells[1, 3] = "Assigned Role";
                        xlWorkSheet.Cells[1, 4] = "Ranking";
                        xlWorkSheet.Rows[1].Font.Bold = true;
                        xlWorkSheet.Rows[1].Font.Underline = true;
                        xlWorkSheet.get_Range("A1", "D1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        int row = 3, team = 1;
                        List<Role> roleList = new List<Role>(new Role[] {
                            Role.TOP, Role.JNG, Role.MID, Role.BOT, Role.SUP });
                        Dictionary<Role, string> role2String = new Dictionary<Role, string>() {
                            { Role.TOP, "Top" },
                            { Role.JNG, "Jungle" },
                            { Role.MID, "Mid" },
                            { Role.BOT, "ADC" },
                            { Role.SUP, "Support" },
                            { Role.NONE, "None" }
                        };
                        Dictionary<Tier, string> tier2String = new Dictionary<Tier, string>() {
                            { Tier.BRONZE, "Bronze" },
                            { Tier.SILVER, "Silver" },
                            { Tier.GOLD, "Gold" },
                            { Tier.PLATINUM, "Platinum" },
                            { Tier.DIAMOND, "Diamond" },
                            { Tier.MASTER, "Masters" }
                        };
                        foreach (Team teamSel in bestBalance.teams) {
                            xlWorkSheet.Cells[row, 1] = "Team " + team;
                            xlWorkSheet.get_Range("A" + row, "D" + row).Merge();
                            
                            foreach (Role role in roleList) {
                                row++;
                                Player player = teamSel.getPlayerRole(role);
                                xlWorkSheet.Cells[row, 1] = player.name;
                                string ignString = player.ign;
                                if (player.hasDuo()) { ignString += " (D)"; }
                                xlWorkSheet.Cells[row, 2] = ignString;
                                string roleString = role2String[player.assignedRole];
                                if (player.isAutoFilled()) { roleString += " (Autofilled)"; }
                                xlWorkSheet.Cells[row, 3] = roleString;
                                string ranking = tier2String[player.tier] + " ";
                                if (player.tier != Tier.MASTER) { ranking += player.division; }
                                xlWorkSheet.Cells[row, 4] = ranking;
                                switch (player.tier) {
                                    case Tier.BRONZE:
                                        xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(BRONZEHEX));
                                        break;
                                    case Tier.SILVER:
                                        xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(SILVERHEX));
                                        break;
                                    case Tier.GOLD:
                                        xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(GOLDHEX));
                                        break;
                                    case Tier.PLATINUM:
                                        xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(PLATHEX));
                                        break;
                                    case Tier.DIAMOND:
                                        xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(DIAMONDHEX));
                                        break;
                                    case Tier.MASTER:
                                        xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(CHALLENGERHEX));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            row++; row++; team++;
                        }
                        #endregion

                        releaseObject(xlWorkSheet);
                        string filename = sfd_Excel.FileName;
                        try {
                            xlApp.DisplayAlerts = false;
                            xlWorkBook.SaveAs(filename);
                            xlApp.Visible = true;
                        }
                        catch (Exception ex) {
                            MessageBox.Show("Can't overwrite file. Please save it as another name." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                        //Cursor.Current = Cursors.Default;
                    }
                }
                else if (lowestRange > RANGE_THRESHOLD) {
                    MessageBox.Show("Could not find a balance with that threshold.");
                }
            }
        }
        
        // String -> Tier
        private static Dictionary<string, Tier> STRING_TO_TIER = new Dictionary<string, Tier>() {
            { "Bronze", Tier.BRONZE },
            { "Silver", Tier.SILVER },
            { "Gold", Tier.GOLD },
            { "Platinum", Tier.PLATINUM },
            { "Diamond", Tier.DIAMOND },
            { "Masters", Tier.MASTER },
            { "Challenger", Tier.MASTER }
        };

        // String -> Role
        private static Dictionary<string, Role> STRING_TO_ROLE = new Dictionary<string, Role>() {
            { "Top", Role.TOP },
            { "Jungle", Role.JNG },
            { "Mid", Role.MID },
            { "ADC", Role.BOT },
            { "Support", Role.SUP },
            { "Fill", Role.FILL }
        };

        // Deep Copy function
        public static T deepClone<T>(T obj) {
            using (var ms = new MemoryStream()) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        // Rolls a uniform random number between min and max
        private static int randomNumber(int min, int max) {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }
        private static int randomNumber(int min, int max, int seedVal) {
            Random rnd = new Random(seedVal);
            return rnd.Next(min, max);
        }

        // Assigning roles from masterList based on primary or secondary
        private static void assignRoles(ref Dictionary<string, Player> masterList,
            ref Dictionary<Role, List<Player>> assignRoleList,
            Dictionary<string, string> duoList,
            int numTeams, int randSeed, bool primaryRole) {
            // Fxn start
            List<string> removeList = new List<string>();
            foreach (string ign in masterList.Keys) {
                Player player = masterList[ign];
                string duoName = duoList[ign];
                Role role = (primaryRole) ? player.primaryRole : player.secondRole;
                if (role != Role.FILL && !containsName(assignRoleList[role], duoName)) {
                    player.assignedRole = role;
                    assignRoleList[role].Add(player);
                    // Remove from masterList
                    // Currently immutabale due to iteration. Remove later
                    removeList.Add(ign);
                }
            }
            foreach (string removeKey in removeList) {
                masterList.Remove(removeKey.ToLower());
            }
            // Check if each Role has more than 8 players.
            // If so, "roleChange" a player
            foreach (List<Player> roleList in assignRoleList.Values) {
                while (roleList.Count > numTeams) {
                    // Used to determine who gets kicked
                    Player changePlayer = roleChange(ROLE_CHANGE_OPTION, roleList, randSeed, primaryRole);
                    roleList.Remove(changePlayer);
                    changePlayer.assignedRole = Role.NONE;
                    masterList.Add(changePlayer.ign.ToLower(), changePlayer);
                }
            }
        }

        // Removes a player from the roleList if > numTeams
        // Based on the option, it determines who gets kicked
        // The Player getting kicked is returned.
        private static Player roleChange(int option, List<Player> roleList, 
            int randSeed, bool primary) {
            // Make a new list of secondaries if primary = false
            List<Player> roleSecList = new List<Player>();
            if (!primary) {
                foreach (Player addPlayer in roleList) {
                    if (addPlayer.isSecondaryAssigned()) {
                        roleSecList.Add(addPlayer);
                    }
                }
            }
            Player changePlayer = new Player();
            if (option == 1) {
                // Method 1: Remove by random
                int index = (primary) ? 
                    randomNumber(0, roleList.Count - 1, randSeed) : 
                    randomNumber(0, roleSecList.Count - 1, randSeed);
                changePlayer = (primary) ? roleList[index] : roleSecList[index];
            }
            else if (option == 2) {
                // Method 2: Remove the higher ranked
                changePlayer = (primary) ? roleList.Max() : roleSecList.Max();
            }
            else if (option == 3) {
                // Method 3: Remove the lower ranked
                changePlayer = (primary) ? roleList.Min() : roleSecList.Min();
            }
            return changePlayer;
        }

        // Figure out which role that's < numTeams has the least
        private static Role roleMinPointsFill(Dictionary<Role, List<Player>> assignRoleList, 
            int numTeams) {
            Role retRole = Role.NONE;
            int smallest = 9000;    // yeah...
            foreach (Role role in assignRoleList.Keys) {
                if (assignRoleList[role].Count < numTeams) {
                    int roleTotalVal = 0;
                    foreach (Player player in assignRoleList[role]) {
                        roleTotalVal += player.rankValue(true);
                    }
                    if (roleTotalVal < smallest) {
                        smallest = roleTotalVal;
                        retRole = role;
                    }
                }
            }
            return retRole;
        }

        private static bool containsName(List<Player> roleList, string name) {
            foreach (Player player in roleList) {
                if (player.ign.ToLower() == name.ToLower()) {
                    return true;
                }
            }
            return false;
        }

        // For securing the Trash
        private static void releaseObject(object obj) {
            try {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex) {
                obj = null;
                throw ex;
            }
            finally {
                GC.Collect();
            }
        }
    }
}
