using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace LoLBalanceV2
{
    class Program
    {
        private const int ROLE_CHANGE_OPTION = 1;
        private const int SEED = 11;
        private const int RANDOMIZE = 10;
        private const int RANGE_THRESHOLD = 2;

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
                Dictionary<string, Player> masterList = new Dictionary<string, Player>();
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
                        masterList.Add(playerIGN.ToLower(), new Player(playerName, playerIGN, 
                            playerTier, playerDiv, playerPrimary, playerSecond, playerDuo));
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("ERROR - Parsing Inputs: " + ex.Message + '\n' +
                        "Check Row: " + parseRow);
                    return;
                }
                // ----- Check number of Players
                if (masterList.Count < 40) {
                    Console.WriteLine("ERROR - Total number of Players does not exceed 40");
                    return;
                }
                if (masterList.Count % 5 != 0) {
                    Console.WriteLine("ERROR - Total number of Players is not divisible by 5");
                    return;
                }
                int numTeams = masterList.Count / 5;
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
                        Player origPlayer = masterList[summoner];
                        Player duoPlayer = masterList[duoIGNLower];
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
                            if (randomNumber(1, 2, SEED) == 1) { origPlayer.primaryRole = Role.FILL; }
                            else { duoPlayer.primaryRole = Role.FILL; }
                        }
                    }
                    catch {
                        // Player couldn't be found or incorrect, so we blank out duo
                        Player player = masterList[summoner];
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
                Player lowestPlayer = masterList.Values.ToList().Min();

                // ----- PART 2: ASSIGN ROLES
                Dictionary<Role, List<Player>> assignRoleList = new Dictionary<Role, List<Player>>() {
                    { Role.TOP, new List<Player>() },
                    { Role.JNG, new List<Player>() },
                    { Role.MID, new List<Player>() },
                    { Role.BOT, new List<Player>() },
                    { Role.SUP, new List<Player>() },
                };
                // Assign non-Fill Primary Roles
                try { assignRoles(ref masterList, ref assignRoleList, duoList, numTeams, true); }
                catch (Exception ex) {
                    Console.WriteLine("ERROR - Assigning primary roles\n" + 
                        "Reason: " + ex.Message);
                    return;
                }
                // Assign non-Fill Secondary Roles
                try { assignRoles(ref masterList, ref assignRoleList, duoList, numTeams, false); }
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
                int lowestRankVal = lowestPlayer.rankValue();
                int lowestRange = 0;
                Balance currBalance = new Balance(assignRoleList, duoList, lowestRankVal, out lowestRange);
                Balance bestBalance = deepClone(currBalance);
                int rolesRemain = 5;
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
                // -- Remember to move duos too!
                while (rolesRemain == 0) {
                    Role checkRole = roleQ.Peek();
                    int minCheckRemain = numTeams, maxCheckRemain = numTeams;
                    bool minCheck = true, thresholdReached = false;
                    // Check within each Role
                    while (minCheckRemain > 0 && maxCheckRemain > 0) {

                    }
                    if (thresholdReached) { break; }
                    rolesRemain--;
                    roleQ.Dequeue();
                    roleQ.Enqueue(checkRole);
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
            { "Master", Tier.MASTER },
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
        private static int randomNumber(int min, int max, int seedVal) {
            Random rnd = new Random(seedVal);
            return rnd.Next(min, max);
        }

        // Assigning roles from masterList based on primary or secondary
        private static void assignRoles(ref Dictionary<string, Player> masterList,
            ref Dictionary<Role, List<Player>> assignRoleList,
            Dictionary<string, string> duoList,
            int numTeams, bool primaryRole) {
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
                    Player changePlayer = roleChange(ROLE_CHANGE_OPTION, roleList, primaryRole);
                    roleList.Remove(changePlayer);
                    changePlayer.assignedRole = Role.NONE;
                    masterList.Add(changePlayer.ign.ToLower(), changePlayer);
                }
            }
        }

        // Removes a player from the roleList if > numTeams
        // Based on the option, it determines who gets kicked
        // The Player getting kicked is returned.
        private static Player roleChange(int option, List<Player> roleList, bool primary) {
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
                    randomNumber(0, roleList.Count - 1, SEED) : 
                    randomNumber(0, roleSecList.Count - 1, SEED);
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

        private static bool containsName(List<Player> roleList, string name) {
            foreach (Player player in roleList) {
                if (player.ign.ToLower() == name.ToLower()) {
                    return true;
                }
            }
            return false;
        }
    }
}
