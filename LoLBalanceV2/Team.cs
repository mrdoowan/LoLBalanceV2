using System.Collections.Generic;

namespace LoLBalanceV2
{
    public class Team
    {
        private Dictionary<Role, Player> players;   // Players in this team. Size strictly 5
        public int teamValue;                       // Combined rank value
        public int numFill;                         // Number of players filled

        // Default ctor
        public Team() {
            players = new Dictionary<Role, Player>();
            teamValue = 0;
            numFill = 0;
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
        public int calcTeamValue() {
            int combinedValue = 0;
            foreach (Player player in players.Values) {
                combinedValue += player.rankValue();
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
            if (players.ContainsKey(role)) {
                players[role] = player;     // Sets
            }
            else {
                players.Add(role, player);  // Adds
            }
            calcTeamValue();
        }
        
        // Returns true if team has the role, otherwise false
        public bool containsRole(Role role) {
            return players.ContainsKey(role);
        }
    }
}
