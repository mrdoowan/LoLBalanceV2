using System;
using System.Collections.Generic;

namespace LoLBalanceV2
{
    [Serializable]
    class Team
    {
        private Dictionary<Role, Player> players;   // Players in this team. Size strictly 5
        private int teamValue;                      // Combined rank value

        // Default ctor
        public Team() {
            players = new Dictionary<Role, Player>();
            teamValue = 0;
        }

        // Init ctor
        public Team(Player top, Player jng, Player mid,
            Player bot, Player sup) {
            players = new Dictionary<Role, Player>();
            players.Add(Role.TOP, top);
            players.Add(Role.JNG, jng);
            players.Add(Role.MID, mid);
            players.Add(Role.BOT, bot);
            players.Add(Role.SUP, sup);
        }

        // Return and set team's combined rankValues w/ lowestRank
        public int calcTeamValue(int lowestRank) {
            int combinedValue = 0;
            foreach (Player player in players.Values) {
                combinedValue += player.rankValue(lowestRank);
            }
            teamValue = combinedValue;
            return combinedValue;
        }

        // Assume TOP, JNG, MID, BOT, SUP as valid inputs
        // Returns the players in that role
        public Player getPlayerRole(Role role) {
            return players[role];
        }

        // Assume TOP, JNG, MID, BOT, SUP as valid inputs
        // Sets the players in that role
        public void setPlayerRole(Role role, Player player) {
            players[role] = player;
        }

        // Returns true if ign is in the Team
        public bool isNameInTeam(string ign) {
            foreach (Player player in players.Values) {
                if (player.ign.ToLower() == ign.ToLower()) {
                    return true;
                }
            }
            return false;
        }
        
        // Returns a solo player with preference of primary and secondary
        // in the team
        public Player aSoloPlayerFromTeam(Role primary, Role second) {
            if (!players[primary].hasDuo()) { return players[primary]; }
            if (!players[second].hasDuo()) { return players[second]; }
            List<Player> soloPlayers = new List<Player>();
            foreach (Player player in players.Values) {
                if (!player.hasDuo()) {
                    soloPlayers.Add(player);
                }
            }
            if (soloPlayers.Count == 0) {
                Console.WriteLine("ERROR - An entire team has no solo?");
                throw new Exception();
            }
            Random rnd = new Random();
            return soloPlayers[rnd.Next(0, soloPlayers.Count - 1)];
        }

        // Returns the number of solo players the team has
        public int numSoloPlayers() {
            int solo = 0;
            foreach (Player player in players.Values) {
                if (!player.hasDuo()) {
                    solo++;
                }
            }
            return solo;
        }
    }
}
