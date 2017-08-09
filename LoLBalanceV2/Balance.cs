using System.Collections.Generic;
using System;

namespace LoLBalanceV2
{
    [Serializable]
    class Balance
    {
        public List<Team> teams;
        private Dictionary<string, Player> roster;    // Complete roster loaded here
        private Dictionary<string, string> duoList;
        private int rangeBalance;       // This is the difference between max and min of teamValues
        private int lowestRankVal;
        private int maxTeamIndex;
        private int minTeamIndex;

        // Default ctor
        public Balance() {
            teams = new List<Team>();
            roster = new Dictionary<string, Player>();
            rangeBalance = 0;
            lowestRankVal = 0;
            maxTeamIndex = 0;
            minTeamIndex = 0;
        }

        // Init ctor
        public Balance(Dictionary<Role, List<Player>> roleList, 
            Dictionary<string, string> duoList_,
            int lowestRank_, out int range) {
            // Fxn start
            teams = new List<Team>();
            roster = new Dictionary<string, Player>();
            duoList = duoList_;
            int numTeams = roleList[Role.TOP].Count;
            // Also check if Duos are in the same Role
            for (int i = 0; i < numTeams; ++i) {
                Player top = roleList[Role.TOP][i];
                Player jng = roleList[Role.JNG][i];
                Player mid = roleList[Role.MID][i];
                Player adc = roleList[Role.BOT][i];
                Player sup = roleList[Role.SUP][i];
                teams.Add(new Team(top, jng, mid, adc, sup));
                roster.Add(top.ign.ToLower(), top);
                roster.Add(jng.ign.ToLower(), jng);
                roster.Add(mid.ign.ToLower(), mid);
                roster.Add(adc.ign.ToLower(), adc);
                roster.Add(sup.ign.ToLower(), sup);
            }
            // Align duos into the same teams. i == Team #
            alignDuoRole(Role.TOP);
            alignDuoRole(Role.JNG);
            alignDuoRole(Role.MID);
            alignDuoRole(Role.BOT);
            // We can assume Support is already aligned
            lowestRankVal = lowestRank_;
            calcRange();
            range = rangeBalance;
        }

        // Switch player team1 and player team2 based on role
        // Returns the new range
        public int switchPlayer(Role role, int team1, int team2, bool calc = true) {
            Player player1 = new Player();
            player1 = teams[team1].getPlayerRole(role);
            Player player2 = new Player();
            player2 = teams[team2].getPlayerRole(role);
            teams[team1].setPlayerRole(role, player2);
            teams[team2].setPlayerRole(role, player1);
            if (calc) { calcRange(); }
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

        // Going through by every player in that Role, align duos
        private void alignDuoRole(Role role) {
            // i == team #
            for (int i = 0; i < teams.Count; ++i) {
                Player ogPlayer = teams[i].getPlayerRole(role);
                string duoName = ogPlayer.duo;
                if (!string.IsNullOrWhiteSpace(duoName)) {
                    Player duoPlayer = roster[duoName.ToLower()];
                    Role duoRole = duoPlayer.assignedRole;
                    // j == duoTeam #
                    for (int j = 0; j < teams.Count; ++j) {
                        if (teams[j].getPlayerRole(duoRole).ign.ToLower() 
                            == duoName.ToLower()) {
                            // Now need to check if teams[i].Player(duoRole) has a duo
                            // AND if that duo is in teams[i] (save that person's role)
                            // If this condition is true, find two solos within those roles
                            // This is one of the worst corner cases ever
                            switchPlayer(duoRole, i, j, false);
                            break;
                        }
                    }
                }
            }
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
