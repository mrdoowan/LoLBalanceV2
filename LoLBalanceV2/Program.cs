using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LoLBalanceV2
{
    class Program
    {
        private const int ROLE_CHANGE_OPTION = 1;

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
                // ----- Parse Inputs
                int parseRow = 0;
                // IGN -> Player Obj
                Dictionary<string, Player> masterList = new Dictionary<string, Player>();
                // IGN -> duoIGN
                Dictionary<string, string> duoList = new Dictionary<string, string>();
                try {
                    string openPath = ofd.FileName;
                    string csvText = File.ReadAllText(openPath);
                    string[] rowList = csvText.Split('\n');
                    foreach (string rowPlayer in rowList) {
                        string[] rowParts = rowPlayer.Split(',');
                        string playerName = rowParts[0];
                        string playerIGN = rowParts[1];
                        Tier playerTier = STRING_TO_TIER[rowParts[2]];
                        int playerDiv = int.Parse(rowParts[3]);
                        Role playerPrimary = STRING_TO_ROLE[rowParts[4]];
                        Role playerSecond = (playerPrimary == Role.FILL) ? 
                            Role.FILL : STRING_TO_ROLE[rowParts[5]];
                        string playerDuo = rowParts[6];
                        duoList.Add(playerIGN, playerDuo);
                        masterList.Add(playerIGN, new Player(playerName, playerIGN, 
                            playerTier, playerDiv, playerPrimary, playerSecond, playerDuo));
                        parseRow++;
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("ERROR - Parsing Inputs: " + ex.Message + '\n' +
                        "Check Row: " + parseRow);
                    return;
                }
                // ----- Check number of Players
                if (masterList.Count % 5 != 0) {
                    Console.WriteLine("ERROR - Total number of Players is not divisible by 5");
                    return;
                }
                int numTeams = masterList.Count / 5;
                // ----- Validate duos and their Roles
                foreach (string summoner in duoList.Keys) {
                    try {
                        string duoIGN = duoList[summoner];
                        string origIGN = duoList[duoIGN];
                        if (summoner.ToLower() != origIGN.ToLower()) {
                            throw new Exception();
                            // Pretty much being treated as not finding a Key
                            // It's bad practice, but suitable
                        }
                        // At this point, the duos are confirmed.
                        // Now both duos roles for primary and secondary must be different
                        // If not, Fill for both primary and secondary
                        // 1) If Primary/Secondary are the same
                        Player origPlayer = masterList[summoner];
                        Player duoPlayer = masterList[duoIGN];
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
                        Player player = masterList[summoner];
                        player.duo = "";
                    }
                }
                // ----- Assign by 5 Roles
                Dictionary<Role, List<Player>> assignRoleList = new Dictionary<Role, List<Player>>() {
                    { Role.TOP, new List<Player>() },
                    { Role.JNG, new List<Player>() },
                    { Role.MID, new List<Player>() },
                    { Role.BOT, new List<Player>() },
                    { Role.SUP, new List<Player>() },
                };
                // Assign non-Fill Primary Roles
                try { assignRoles(ref masterList, ref assignRoleList, numTeams, true); }
                catch (Exception ex) {
                    Console.WriteLine("ERROR - Assigning primary roles\n" + 
                        "Reason: " + ex.Message);
                    return;
                }
                // Assign non-Fill Secondary Roles
                try { assignRoles(ref masterList, ref assignRoleList, numTeams, false); }
                catch (Exception ex) {
                    Console.WriteLine("ERROR - Assigning secondary roles\n" +
                        "Reason: " + ex.Message);
                    return;
                }
                // For naming purposes, the remaining masterList is now fillList
                assignRoleList.Add(Role.FILL, masterList.Values.ToList());


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

        // Rolls a uniform random number between min and max
        private static int randomNumber(int min, int max) {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        // Assigning roles from masterList based on primary or secondary
        private static void assignRoles(ref Dictionary<string, Player> masterList,
            ref Dictionary<Role, List<Player>> assignRoleList,
            int numTeams, bool primaryRole) {
            List<string> removeList = new List<string>();
            foreach (string ign in masterList.Keys) {
                Player player = masterList[ign];
                Role role = (primaryRole) ? player.primaryRole : player.secondRole;
                if (role != Role.FILL) {
                    assignRoleList[role].Add(player);
                    // Remove from masterList
                    // Currently immutabale due to iteration. Remove later
                    removeList.Add(ign);
                }
            }
            foreach (string removeKey in removeList) {
                masterList.Remove(removeKey);
            }
            // Check if each Role has more than 8 players.
            // If so, "roleChange" a player
            foreach (List<Player> roleList in assignRoleList.Values) {
                while (roleList.Count > numTeams) {
                    // Used to determine who gets kicked
                    Player changePlayer = new Player();
                    if (ROLE_CHANGE_OPTION == 1) {
                        // Method 1: Remove by random
                        int index = randomNumber(0, roleList.Count - 1);
                        changePlayer = roleList[index];
                    }
                    else if (ROLE_CHANGE_OPTION == 2) {
                        // Method 2: Remove the higher ranked
                        changePlayer = roleList.Max();
                    }
                    else if (ROLE_CHANGE_OPTION == 3) {
                        // Method 3: Remove the lower ranked
                        changePlayer = roleList.Min();
                    }
                    roleList.Remove(changePlayer);
                    masterList.Add(changePlayer.ign, changePlayer);
                }
            }
        }
    }
}
