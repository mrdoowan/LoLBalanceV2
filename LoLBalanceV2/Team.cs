using System.Collections.Generic;

namespace LoLBalanceV2
{
    class Team
    {
        private Dictionary<Role, Player> players;   // Players in this team. Size strictly 5
        public int teamValue;                       // Combined rank value

        // Default ctor
        public Team() {
            players = new Dictionary<Role, Player>();
            teamValue = 0;
        }

        // Init ctor
        public Team(Player top, Player jng, Player mid,
            Player bot, Player sup) {
            players.Add(Role.TOP, top);
            players.Add(Role.JNG, jng);
            players.Add(Role.MID, mid);
            players.Add(Role.BOT, bot);
            players.Add(Role.SUP, sup);
            calcTeamValue();
        }

        // Return and set team's combined rankValues w/ lowestRank
        public int calcTeamValue(int lowestRank = 1) {
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
    }
}
