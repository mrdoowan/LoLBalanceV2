using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Microsoft.CSharp;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace LoLBalancing
{
    public class StartAlgo
    {
        // Constant variables
        private const int MIN_PLAYERS = 10;
        private const int NUM_ROLES = 5;
        public const int MIN_DIV = 1;
        public const int MAX_DIV = 4;

        // Adjustable const numbers - UPDATED IN GUI
        private int threshold;
        private int startSeed;
        private int maxChecks;
        private bool noAutoFill;
        private bool writeRangeConsole;
        private bool bestOutput;
        // Init ctor
        public StartAlgo(int thresh_, int seed_, int checks_, bool noAuto_, bool writeRange_, bool best_) {
            threshold = thresh_;
            startSeed = seed_;
            maxChecks = checks_;
            noAutoFill = noAuto_;
            writeRangeConsole = writeRange_;
            bestOutput = best_;
        }

        // ---- Member functions
        // IGN -> Player Obj.
        private Dictionary<Name, Player> roster = new Dictionary<Name, Player>();
        // IGN -> duoIGN.
        private Dictionary<Name, Name> duoList = new Dictionary<Name, Name>();
        // Rank -> Points
        private Dictionary<string, int> rank2pts = new Dictionary<string, int>();
        private int numTeams;

        // Write in Console log. Error is true if it forces an exit.
        private void write_ConsoleLog(string msg, ref RichTextBox rtb) {
            rtb.AppendText(msg + '\n');
        }

        // String -> Tier
        private static Dictionary<string, Tier> STRING_TO_TIER = new Dictionary<string, Tier>() {
            { MainForm.IRON, Tier.IRON },
            { MainForm.BRONZE, Tier.BRONZE },
            { MainForm.SILVER, Tier.SILVER },
            { MainForm.GOLD, Tier.GOLD },
            { MainForm.PLATINUM, Tier.PLATINUM },
            { MainForm.DIAMOND, Tier.DIAMOND },
            { MainForm.MASTER, Tier.MASTER },
            { MainForm.GRANDMASTER, Tier.GRANDMASTER },
            { MainForm.CHALLENGER, Tier.CHALLENGER }
        };

        // String -> Role
        private const string TOP = "Top";
        private const string JUNGLE = "Jungle";
        private const string MID = "Mid";
        private const string ADC = "ADC";
        private const string SUPPORT = "Support";
        private const string FILL = "Fill";
        private readonly string[] ROLE_LIST =
            { TOP, JUNGLE, MID, ADC, SUPPORT, FILL };
        private static Dictionary<string, Role> STRING_TO_ROLE = new Dictionary<string, Role>() {
            { TOP, Role.TOP },
            { JUNGLE, Role.JNG },
            { MID, Role.MID },
            { ADC, Role.ADC },
            { SUPPORT, Role.SUP },
            { FILL, Role.FILL }
        };

        #region Main Functions

        // Loads DGV of Roster and initializes StartAlgo
        // returns false if it fails
        public bool loadParamsFunction(DataGridView dgv_Roster, int secDrop, int fillDrop, ref RichTextBox rtb) {
            string playerName = "";
            bool inputsPerfect = true;
            try {
                foreach (DataGridViewRow playerRow in dgv_Roster.Rows) {
                    playerName = playerRow.Cells[1].Value.ToString();

                    string ignStr = playerRow.Cells[2].Value.ToString();
                    if (string.IsNullOrWhiteSpace(ignStr)) {
                        string msg = playerName + " should not have a blank IGN.";
                        write_ConsoleLog(msg, ref rtb);
                        inputsPerfect = false;
                    }
                    Name playerIGN = new Name(playerRow.Cells[2].Value.ToString());

                    string tierStr = playerRow.Cells[3].Value.ToString();
                    if (!MainForm.TIER_LIST.Any(tierStr.Contains)) {
                        string msg = playerIGN + " does not have a valid Tier name.";
                        write_ConsoleLog(msg, ref rtb);
                        inputsPerfect = false;
                    }
                    Tier playerTier = STRING_TO_TIER[tierStr.ToUpper()];

                    string divString = playerRow.Cells[4].Value.ToString();
                    int playerDiv = (MainForm.ROMAN_TO_NUMBER.ContainsKey(divString)) ?
                        MainForm.ROMAN_TO_NUMBER[divString] : int.Parse(playerRow.Cells[4].Value.ToString());
                    if (playerDiv < MIN_DIV || playerDiv > MAX_DIV) {
                        string msg = playerIGN + " does not have a division between " + MIN_DIV + "-" + MAX_DIV + ".";
                        write_ConsoleLog(msg, ref rtb);
                        inputsPerfect = false;
                    }

                    string priStr = playerRow.Cells[5].Value.ToString();
                    if (!ROLE_LIST.Any(priStr.Contains)) {
                        string msg = playerIGN + " does not have a valid primary role.";
                        write_ConsoleLog(msg, ref rtb);
                        inputsPerfect = false;
                    }
                    Role playerPri = STRING_TO_ROLE[playerRow.Cells[5].Value.ToString()];

                    string[] secondRoles = playerRow.Cells[6].Value.ToString().Split(',');
                    List<Role> playerSecs = new List<Role>();
                    foreach (string secRole in secondRoles) {
                        string secRoleTrimmed = secRole.Trim(' ');
                        if (!ROLE_LIST.Any(secRoleTrimmed.Contains)) {
                            string msg = playerIGN + " does not have a valid secondary role.";
                            write_ConsoleLog(msg, ref rtb);
                            inputsPerfect = false;
                        }
                        playerSecs.Add(STRING_TO_ROLE[secRoleTrimmed]);
                    }

                    // Necessary checks for secondary Roles:
                    // 1) Ensure Player is fill
                    if (playerPri == Role.FILL || playerSecs.Contains(Role.FILL)) {
                        playerSecs.Clear();
                        playerSecs.Add(Role.FILL);
                    }
                    // 2) The primary role isn't in the secondary role
                    if (playerSecs.Contains(playerPri) && !playerSecs.Contains(Role.FILL)) {
                        playerSecs.Remove(playerPri);
                        if (playerSecs.Count == 0) {
                            playerSecs.Add(Role.FILL);
                            string msg = playerIGN + ": Secondary role is their Primary role. Default to Fill";
                            write_ConsoleLog(msg, ref rtb);
                        }
                    }
                    Name playerDuo = new Name(playerRow.Cells[7].Value.ToString());
                    if (playerDuo == playerIGN) {
                        string msg = playerIGN + ": Wrote themselves as their duo.";
                        write_ConsoleLog(msg, ref rtb);
                        playerDuo.SetName("");
                    }

                    // Add to List
                    duoList.Add(playerIGN, playerDuo);
                    roster.Add(playerIGN, new Player(playerName, playerIGN,
                        playerTier, playerDiv, playerPri, playerSecs, playerDuo,
                        secDrop, fillDrop));
                }
            }
            catch (Exception ex) {
                string msg = "ERROR - Parsing Inputs: " + ex.Message + ". Check Player: " + playerName;
                write_ConsoleLog(msg, ref rtb);
                return false;
            }

            // ----- Check number of Players
            if (roster.Count < MIN_PLAYERS) {
                string msg = "ERROR - Total number of Players needs to be " + MIN_PLAYERS + ".";
                write_ConsoleLog(msg, ref rtb);
                return false;
            }
            if (roster.Count % NUM_ROLES != 0) {
                string msg = "ERROR - Total number of Players is not divisible by 5.";
                write_ConsoleLog(msg, ref rtb);
                return false;
            }
            numTeams = roster.Count / NUM_ROLES;
            // Display MessageBox of errors
            if (!inputsPerfect) {
                MessageBox.Show("An error occurred when trying to balance. Please look at the Console Log.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return inputsPerfect;
        }

        // ----- PART 1: VALIDATE DUOS
        // Validates the Duos requirements
        public bool validateDuosFunction(ref RichTextBox rtb) {

            // ----- Validate duos and their Roles
            List<Name> noDuoList = new List<Name>();
            foreach (Name summonerName in duoList.Keys) {
                try {
                    Name duoIGN = duoList[summonerName];
                    Name ogIGN = duoList[duoIGN];
                    if (summonerName != ogIGN) {
                        throw new Exception();
                        // Pretty much being treated as not finding a Key
                        // It's bad practice, but suitable
                    }
                    // At this point, the duos are confirmed.
                    // Now both duos roles for primary and secondary must be different
                    // If not, Fill for both primary and secondary
                    // 1) If Primary/Secondary are the same
                    Player origPlayer = roster[summonerName];
                    Player duoPlayer = roster[duoIGN];
                    if ((duoPlayer.secondRoles.Contains(origPlayer.primaryRole) &&
                        origPlayer.secondRoles.Contains(duoPlayer.primaryRole) &&
                        duoPlayer.secondRoles.Count == 1 && origPlayer.secondRoles.Count == 1 &&
                        (origPlayer.primaryRole != Role.FILL || duoPlayer.primaryRole != Role.FILL))) {
                        string msg = summonerName + " and " + duoIGN +
                            ": Both duos for primary and secondary were interchangeably the same.";
                        write_ConsoleLog(msg, ref rtb);
                        origPlayer.secondRoles.Clear();
                        origPlayer.secondRoles.Add(Role.FILL);
                        duoPlayer.secondRoles.Clear();
                        duoPlayer.secondRoles.Add(Role.FILL);
                    }
                    // 2) If both Primary is same
                    // Roll a number on who to bump Secondary to Primary, and make Secondary Fill
                    if (origPlayer.primaryRole == duoPlayer.primaryRole) {
                        string msg = summonerName + " and " + duoIGN +
                            ": Both duos had the same primary role.";
                        write_ConsoleLog(msg, ref rtb);
                        Player selPlayer = randomNumber(1, 2, 0) == 1 ? origPlayer : duoPlayer;
                        int selIndex = randomNumber(0, selPlayer.secondRoles.Count, 0);
                        selPlayer.primaryRole = selPlayer.secondRoles[selIndex];
                        selPlayer.secondRoles.RemoveAt(selIndex);
                        if (selPlayer.secondRoles.Count == 0) {
                            selPlayer.secondRoles.Add(Role.FILL);
                        }
                    }
                }
                catch {
                    // Player couldn't be found or incorrect, so we blank out duo
                    Player player = roster[summonerName];
                    if (!string.IsNullOrWhiteSpace(player.duo.ToString())) {
                        string msg = summonerName + ": Duo \"" + player.duo + "\" does not exist.";
                        write_ConsoleLog(msg, ref rtb);
                    }
                    player.duo.SetName("");
                    // Can't modify duoList yet, so store the name
                    noDuoList.Add(summonerName);
                }
            }
            // Blank out duoList
            foreach (Name summNonDuo in noDuoList) {
                duoList[summNonDuo].SetName("");
            }
            // Final count on how many duos
            int numDuos = 0;
            foreach (Name summDuo in duoList.Values) {
                if (!string.IsNullOrWhiteSpace(summDuo.ToString())) { numDuos++; }
            }
            // At maximum, only 60% of the roster should be Duos
            int maxDuos = roster.Count * 6 / 10;
            if (numDuos > maxDuos) {
                string msg = "There are " + numDuos + " valid duos, which is over 60% of the Roster (" + maxDuos + ")";
                write_ConsoleLog(msg, ref rtb);
                return false;
            }
            return true;
        }

        // ----- PART 2: MEGA LOOP
        public bool balance(ref RichTextBox rtb) {
            int randSeed = startSeed - 1, currChecks = 0;
            Balance bestBalanceEver = new Balance();
            Balance bestBalanceSeed = new Balance(); // This will store our bestBalance within that seed

            // MEGALOOP: Number of checks has not exceeded maxChecks
            while (currChecks < maxChecks) {
                int lowestSeedRange = int.MaxValue;
                currChecks++; randSeed++;
                // Preparations: Make a deep copy of roster that can be edited
                Dictionary<Name, Player> masterList = deepClone(roster);
                // Begin
                Dictionary<Role, List<Player>> assignRoleList = new Dictionary<Role, List<Player>>() {
                    { Role.TOP, new List<Player>() },
                    { Role.JNG, new List<Player>() },
                    { Role.MID, new List<Player>() },
                    { Role.ADC, new List<Player>() },
                    { Role.SUP, new List<Player>() },
                };
                // Assign non-Fill Primary Roles
                try {
                    assignRoles(ref masterList, ref assignRoleList, duoList,
                        numTeams, randSeed, true);
                }
                catch (Exception ex) {
                    string msg = "ERROR - Assigning primary roles. " +
                        "Reason: " + ex.Message;
                    write_ConsoleLog(msg, ref rtb);
                    return false;
                }
                // Assign non-Fill Secondary Roles
                try {
                    assignRoles(ref masterList, ref assignRoleList, duoList,
                        numTeams, randSeed, false);
                }
                catch (Exception ex) {
                    string msg = "ERROR - Assigning secondary roles. " +
                        "Reason: " + ex.Message;
                    write_ConsoleLog(msg, ref rtb);
                    return false;
                }

                // The remaining masterList is now fillList.
                // MASTER-LOOP BEGIN
                while (masterList.Count > 0) {
                    // Find role with smallest points, and add a random player
                    Role addFillRole = roleMinPointsFill(assignRoleList, numTeams);
                    Player addPlayer = masterList.Values.ToList().Max(); // Greedy approach
                    addPlayer.assignedRole = addFillRole;
                    assignRoleList[addFillRole].Add(addPlayer);
                    masterList.Remove(addPlayer.ign);
                    // Make sure the player does not have the same role as their duo
                    // If so, randomly switch a solo player who is autoFill -> secondary -> Primary 
                    // from another role
                    if (containsName(assignRoleList[addFillRole], addPlayer.duo)) {
                        Player swapPlayer = aSoloFromRole(assignRoleList, addFillRole, addPlayer, randSeed);
                        // Taking the assumption that the above function also takes into account that the swapped player
                        // is not in the same role as their duo
                        if (swapPlayer.getName() == null) {
                            string msg = "ERROR - " + swapPlayer.getName() + " cannot swap. Too many duos.";
                            write_ConsoleLog(msg, ref rtb);
                            return false;
                        }
                        Role swapRole = swapPlayer.assignedRole;
                        assignRoleList[swapRole].Remove(swapPlayer);
                        assignRoleList[addFillRole].Remove(addPlayer);
                        swapPlayer.assignedRole = addFillRole;
                        assignRoleList[addFillRole].Add(swapPlayer);
                        addPlayer.assignedRole = swapRole;
                        assignRoleList[swapRole].Add(addPlayer);
                    }
                }
                // MASTER-LOOP END

                bool breakLoop = false;
                foreach (List<Player> roleList in assignRoleList.Values) {
                    // If No AutoFill option is selected, check masterList if there is an AutoFill. If so, skip
                    if (noAutoFill) {
                        foreach (Player player in roleList) {
                            if (player.isAutoFilled()) {
                                breakLoop = true;
                                string msg = "Seed " + randSeed + " has an Autofill";
                                write_ConsoleLog(msg, ref rtb);
                            }
                        }
                    }
                    // Validation: Should hopefully never occur
                    if (roleList.Count != numTeams) {
                        string msg = "ERROR - assignRoleList does not have " + numTeams + " teams. It might be impossible to form a Balance.";
                        write_ConsoleLog(msg, ref rtb);
                        return false;
                    }
                }
                if (breakLoop) { continue; }

                // Initialize the Balance abstract
                Balance currBalance = new Balance(assignRoleList, out lowestSeedRange);
                // All queues acting as a clock queue
                int currMaxIndex = currBalance.getMaxTeamIndex();
                int currMinIndex = currBalance.getMinTeamIndex();
                int rolesRemain = NUM_ROLES;
                Queue<Role> roleQ = new Queue<Role>(new Role[] {
                    Role.TOP, Role.JNG, Role.MID, Role.ADC, Role.SUP });
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
                // If lowestRange changes: both checkRema counters set back to numTeams
                // ROLE-LOOP BEGIN
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
                        if (newRange < lowestSeedRange) {
                            // Condition 2: New minRange
                            bestBalanceSeed = deepClone(currBalance);
                            currMinIndex = currBalance.getMinTeamIndex();
                            currMaxIndex = currBalance.getMaxTeamIndex();
                            lowestSeedRange = newRange;
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
                // ROLE-LOOP END

                // An entire "seedCheck" is finished here
                if (writeRangeConsole) {
                    string msg = "Lowest range in Seed " + randSeed + ": " + lowestSeedRange;
                    write_ConsoleLog(msg, ref rtb);
                }
                if (!bestOutput && lowestSeedRange == threshold) {
                    DialogResult resultBox = MessageBox.Show("A balance was found with a Range of " + lowestSeedRange + " at seed " + randSeed +
                        ".\nWould you like to close and save it? Pressing No continues the balance.",
                        "Finished", MessageBoxButtons.YesNoCancel);
                    if (resultBox == DialogResult.Yes) {
                        save_TeamsExcel(bestBalanceSeed);
                        writeTeamPoints(bestBalanceSeed, ref rtb);
                        break;
                    }
                    else if (resultBox == DialogResult.Cancel) {
                        break;
                    }
                }
                else if (bestOutput && lowestSeedRange < bestBalanceEver.getRange()) {
                    bestBalanceEver = deepClone(bestBalanceSeed);
                }
            }
            // MEGALOOP END
            Cursor.Current = Cursors.Default;
            
            // If we got to this point, that means bestBalance has our best teams
            if (!bestOutput && bestBalanceSeed.getRange() > threshold) {
                MessageBox.Show("Could not find a balance with the desired range of " + threshold + ".");
            }
            else if (bestOutput) {
                MessageBox.Show("The best balance possible for this roster is with a Range of " + bestBalanceEver.getRange() +
                    "\nSaving the output.", "Finished", MessageBoxButtons.OK);
                save_TeamsExcel(bestBalanceEver);
                writeTeamPoints(bestBalanceEver, ref rtb);
            }

            return true;
        }

        #endregion

        #region Helper Functions

        // Output point values for each team in the consoleLog
        private void writeTeamPoints(Balance bal, ref RichTextBox rtb) {
            write_ConsoleLog("Team Points Total", ref rtb);
            for (int i = 0; i < numTeams; ++i) {
                string msg = "Team " + (i + 1) + ": " + bal.teams[i].calcTeamValue();
                write_ConsoleLog(msg, ref rtb);
            }
        }

        // Rolls a uniform random number between min and max
        private int randomNumber(int min, int max, int seedVal) {
            Random rnd = new Random(seedVal);
            return rnd.Next(min, max);
        }

        // Deep Copy function
        public T deepClone<T>(T obj) {
            using (var ms = new MemoryStream()) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        // Assigning roles from masterList in the primary or 
        private void assignRoles(ref Dictionary<Name, Player> masterList,
            ref Dictionary<Role, List<Player>> assignRoleList, Dictionary<Name, Name> duoList,
            int numTeams, int randSeed, bool primaryRole) {

            // Fxn start
            List<Name> removeList = new List<Name>();
            foreach (Name ign in masterList.Keys) {
                Player player = masterList[ign];
                Name duoName = duoList[ign];
                // Determine what the role is. 
                Role selRole = (primaryRole) ? player.primaryRole : detSecRole(assignRoleList, player, randSeed);
                // Check if the Role is not Duo and if their duo isn't in that role
                if (selRole != Role.FILL && !containsName(assignRoleList[selRole], duoName)) {
                    player.assignedRole = selRole;
                    assignRoleList[selRole].Add(player);
                    // Remove from masterList
                    // Currently immutabale due to iteration. Remove later
                    removeList.Add(ign);
                }
            }

            foreach (Name removeKey in removeList) {
                masterList.Remove(removeKey);
            }

            // Check if each Role has more than numTeam players.
            // If so, "roleChange" a player
            foreach (List<Player> roleList in assignRoleList.Values) {
                while (roleList.Count > numTeams) {
                    // Used to determine who gets kicked
                    Player changePlayer = roleChange(roleList, randSeed, primaryRole);
                    roleList.Remove(changePlayer);
                    changePlayer.assignedRole = Role.NONE;
                    masterList.Add(changePlayer.ign, changePlayer);
                }
            }
        }

        // Determines the secondary role based on what is available in assignRoleList
        private Role detSecRole(Dictionary<Role, List<Player>> assignRoleList, Player player, int randSeed) {
            if (player.secondRoles.Contains(Role.FILL)) { return Role.FILL; }
            List<int> validInd = new List<int>(); // To store which index in player.secondRoles is valid
            for (int i = 0; i < player.secondRoles.Count; ++i) {
                Role checkRole = player.secondRoles[i];
                // Check if this role is valid: Does that role have more than numTeams?
                if (assignRoleList[checkRole].Count < numTeams) {
                    validInd.Add(i);
                }
            }
            // Pick selSecInd based on if ValidInd has more than 0 indices
            int selSecInd = (validInd.Count > 0) ? validInd[randomNumber(0, validInd.Count - 1, randSeed)] :
                randomNumber(0, player.secondRoles.Count - 1, randSeed);
            return player.secondRoles[selSecInd];
        }

        // Removes a player from the roleList if > numTeams
        // Based on the option, it determines who gets kicked
        // The Player getting kicked is returned.
        private Player roleChange(List<Player> roleList, int randSeed, bool primary) {
            // Make a new list of secondaries if primary == false
            List<Player> roleSecList = new List<Player>();
            if (!primary) {
                foreach (Player addPlayer in roleList) {
                    if (addPlayer.isSecondaryAssigned()) {
                        roleSecList.Add(addPlayer);
                    }
                }
            }
            Player changePlayer = new Player();
            int index = (primary) ?
                randomNumber(0, roleList.Count - 1, randSeed) :
                randomNumber(0, roleSecList.Count - 1, randSeed);
            changePlayer = (primary) ? roleList[index] : roleSecList[index];
            return changePlayer;
        }

        // Figure out which role that's < numTeams has the least
        private Role roleMinPointsFill(Dictionary<Role, List<Player>> assignRoleList,
            int numTeams) {
            Role retRole = Role.NONE;
            int smallest = int.MaxValue;
            foreach (Role role in assignRoleList.Keys) {
                if (assignRoleList[role].Count < numTeams) {
                    int roleTotalVal = 0;
                    foreach (Player player in assignRoleList[role]) {
                        roleTotalVal += player.rankValue();
                    }
                    if (roleTotalVal < smallest) {
                        smallest = roleTotalVal;
                        retRole = role;
                    }
                }
            }
            return retRole;
        }

        // Returns a random autoFill -> Secondary -> Primary player from that role
        private Player aSoloFromRole(Dictionary<Role, List<Player>> assignRoleList,
            Role ignoreRole, Player swappingPlayer, int seedVal) {
            List<Player> playerPool = new List<Player>();
            // AutoFill == 0, Secondary == 1, Primary == 2; Priority list
            for (int i = 0; i <= 2; ++i) {
                foreach (Role role in assignRoleList.Keys) {
                    if (role == ignoreRole) { continue; }
                    List<Player> roleList = assignRoleList[role];
                    foreach (Player player in roleList) {
                        if ((i == 0 && (player.isAutoFilled() || player.isRoleFilled())) ||
                            (i == 1 && player.isSecondaryAssigned()) ||
                            (i == 2 && player.isPrimaryAssigned()) &&
                            !player.hasDuo()) {
                            playerPool.Add(player);
                        }
                    }
                }
                while (playerPool.Count > 0) {
                    // Check here to make sure that selected player (swapping in their role into) is not in the same role as their duo
                    int randomNum = randomNumber(0, playerPool.Count - 1, seedVal);
                    Player selPlayer = playerPool[randomNum];
                    if (!containsName(assignRoleList[swappingPlayer.assignedRole], selPlayer.duo)) {
                        return selPlayer;
                    }
                    else {
                        playerPool.RemoveAt(randomNum);
                    }
                }
            }
            return new Player();
        }

        // Once Teams are completely balanced, output and save them into an Excel sheet.
        private void save_TeamsExcel(Balance outBalance) {
            SaveFileDialog sfd_Excel = new SaveFileDialog();
            sfd_Excel.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
            sfd_Excel.Title = "Save Teams";
            if (sfd_Excel.ShowDialog() == DialogResult.OK) {
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
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
                            Role.TOP, Role.JNG, Role.MID, Role.ADC, Role.SUP });
                Dictionary<Role, string> role2String = new Dictionary<Role, string>() {
                            { Role.TOP, "Top" },
                            { Role.JNG, "Jungle" },
                            { Role.MID, "Mid" },
                            { Role.ADC, "ADC" },
                            { Role.SUP, "Support" },
                            { Role.NONE, "None" }
                        };
                foreach (Team teamSel in outBalance.teams) {
                    xlWorkSheet.Cells[row, 1] = "Team " + team;
                    xlWorkSheet.get_Range("A" + row, "D" + row).Merge();

                    foreach (Role role in roleList) {
                        row++;
                        Player player = teamSel.getPlayerRole(role);
                        xlWorkSheet.Cells[row, 1] = player.name;
                        string ignString = player.ign.ToString();
                        if (player.hasDuo()) { ignString += " (D)"; }
                        xlWorkSheet.Cells[row, 2] = ignString;
                        string roleString = role2String[player.assignedRole];
                        if (player.isAutoFilled()) { roleString += " (Autofilled)"; }
                        else if (player.isRoleFilled()) { roleString += " (Filled)"; }
                        else if (player.isSecondaryAssigned()) { roleString += " (Secondary)"; }
                        xlWorkSheet.Cells[row, 3] = roleString;
                        string ranking = player.getRank();
                        xlWorkSheet.Cells[row, 4] = ranking;
                        xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(MainForm.RankToColor(player.getTier()));
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
                Cursor.Current = Cursors.Default;
            }
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

        // Checks if the name is in the playerList
        private bool containsName(List<Player> roleList, Name ign) {
            foreach (Player player in roleList) {
                if (player.ign == ign) {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
