using System.Collections.Generic;

namespace LoLBalanceV2
{
    class Balance
    {
        public List<Team> teams;
        private int rangeBalance;    // This is the difference between max and min of teamValues
        private int lowestRankVal;
        private int maxTeamIndex;
        private int minTeamIndex;

        // Default ctor
        public Balance() {
            teams = new List<Team>();
            rangeBalance = 0;
            lowestRankVal = 0;
        }

        // Init ctor
        public Balance(Dictionary<Role, List<Player>> roleList, 
            int numTeams, int lowestRank_, out int range) {
            for (int i = 0; i < numTeams; ++i) {
                Player top = roleList[Role.TOP][i];
                Player jng = roleList[Role.JNG][i];
                Player mid = roleList[Role.MID][i];
                Player adc = roleList[Role.BOT][i];
                Player sup = roleList[Role.SUP][i];
                teams.Add(new Team(top, jng, mid, adc, sup));
            }
            lowestRankVal = lowestRank_;
            calcRange();
            range = rangeBalance;
        }

        // Switch player team1 and player team2 based on role
        // Returns the new range
        public int switchPlayer(Role role, int team1, int team2) {
            Player player1 = new Player();
            player1 = teams[team1].getPlayerRole(role);
            Player player2 = new Player();
            player2 = teams[team2].getPlayerRole(role);
            teams[team1].setPlayerRole(role, player2);
            teams[team2].setPlayerRole(role, player1);
            calcRange();
            return rangeBalance;
        }

        // Calculates range and updates it
        private void calcRange() {
            int maxVal = 0, minVal = 9000;
            for (int i = 0; i < teams.Count; ++i) {
                int teamVal = teams[i].calcTeamValue(lowestRankVal);
                if (teamVal > maxVal) {
                    maxVal = teamVal;
                    maxTeamIndex = i;
                }
                if (teamVal < minVal) {
                    minVal = teamVal;
                    minTeamIndex = i;
                }
            }
            rangeBalance = maxVal - minVal;
        }

        // Gets the index of the highest Team Value
        public int getMaxTeamIndex() {
            return maxTeamIndex;
        }

        // Gets the index of the lowest Team Value
        public int getMinTeamIndex() {
            return minTeamIndex;
        }
    }
}
