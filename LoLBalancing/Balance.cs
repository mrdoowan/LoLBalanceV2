using System.Collections.Generic;
using System;

namespace LoLBalancing
{
    [Serializable]
    class Balance
    {
        public List<Team> teams;
        private Dictionary<string, Player> roster;    // Complete roster loaded here
        private int rangeBalance;       // This is the difference between max and min of teamValues
        private int maxTeamIndex;
        private int minTeamIndex;

        // Default ctor
        public Balance() {
            teams = new List<Team>();
            roster = new Dictionary<string, Player>();
            rangeBalance = 0;
            maxTeamIndex = 0;
            minTeamIndex = 0;
        }

        // Init ctor
        public Balance(Dictionary<Role, List<Player>> roleList, out int range) {
            // Fxn start
            teams = new List<Team>();
            roster = new Dictionary<string, Player>();
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
            alignDuoRole(Role.SUP);
            calcRange();
            range = rangeBalance;
        }

        // Switch player team1 and player team2 based on role
        // team1 is the original player being examined.
        // Returns the new range. Also switches duos
        public int switchPlayer(Role role, int team1, int team2, bool withDuo = true) {
            // Figure out how many Roles need to be switched based on duos
            HashSet<Role> roleList = new HashSet<Role>();
            roleList.Add(role);
            Player player1 = teams[team1].getPlayerRole(role);
            Player player2 = teams[team2].getPlayerRole(role);
            if (withDuo) {
                duoChainHelper(player1.ign, player1, team1, team2, ref roleList);
                duoChainHelper(player2.ign, player2, team2, team1, ref roleList);
            }
            // With the Roles needed to switch, conduct the switch.
            foreach (Role roleLoop in roleList) {
                player1 = teams[team1].getPlayerRole(roleLoop);
                player2 = teams[team2].getPlayerRole(roleLoop);
                teams[team1].setPlayerRole(roleLoop, player2);
                teams[team2].setPlayerRole(roleLoop, player1);
            }
            if (withDuo) { calcRange(); }
            return rangeBalance;
        }

        // Helper recursive function for the above in conjunction with duos
        private void duoChainHelper(string ogPlayerName, Player passedPlayer,
            int ogTeam, int swapTeam, ref HashSet<Role> roleList, bool firstFlag = true) {
            // The below condition is to make sure we do not hit an infinite loop
            if (ogPlayerName == passedPlayer.ign && !firstFlag) { return; }
            if (passedPlayer.hasDuo()) {
                string duoPlayer1name = passedPlayer.duo;
                Player duoPlayer1 = roster[duoPlayer1name.ToLower()];
                Role duoRole = duoPlayer1.assignedRole;
                roleList.Add(duoRole);
                Player swapPlayer = teams[swapTeam].getPlayerRole(duoRole);
                duoChainHelper(ogPlayerName, swapPlayer, swapTeam, ogTeam, ref roleList, false);
            }
        }

        // Calculates range and updates it
        private void calcRange() {
            int maxVal = 0, minVal = 9000;
            for (int i = 0; i < teams.Count; ++i) {
                int teamVal = teams[i].calcTeamValue();
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

        // Going through by every player in that Role 
        // and align duos by putting them in the same team by swapping
        // Very messy and complex function trying to tackle a small corner case T_T
        private void alignDuoRole(Role ogRole) {
            // i == ogTeam #
            for (int i = 0; i < teams.Count; ++i) {
                Player ogPlayer = teams[i].getPlayerRole(ogRole);
                string duoName = ogPlayer.duo;
                if (ogPlayer.hasDuo()) {
                    Player duoPlayer = roster[duoName.ToLower()];
                    Role duoRole = duoPlayer.assignedRole;
                    // j == duoTeam #
                    // CORNER CASE 1: If the player and the duo has the same Role
                    if (ogRole == duoRole) {
                        // This only happens if the duo was suddenly autoFilled into the same role as his partner
                        // Just grab a solo from the ogTeam and force swap with duoTeam
                        Player soloPlayer = teams[i].aSoloPlayerFromTeam(duoPlayer.primaryRole, duoPlayer.secondRole);
                        for (int j = 0; j < teams.Count; ++j) {
                            Player possDuo = teams[j].getPlayerRole(ogRole);
                            if (possDuo.ign.ToLower() == duoName.ToLower()) {
                                teams[i].setPlayerRole(soloPlayer.assignedRole, possDuo);
                                teams[j].setPlayerRole(ogRole, soloPlayer);
                                break;
                            }
                        }
                    }
                    // CONDITION 2: Both player and the duo have different roles
                    for (int j = 0; j < teams.Count; ++j) {
                        if (teams[j].getPlayerRole(duoRole).ign.ToLower() == duoName.ToLower()) {
                            // Now need to check if teams[i].Player(duoRole) has a duo
                            // AND if that duo is in teams[i] (save that person's role)
                            Player swapPlayer = teams[i].getPlayerRole(duoRole);
                            if (swapPlayer.hasDuo()) {
                                Player swapPlayerDuo = roster[swapPlayer.duo.ToLower()];
                                if (teams[i].isNameInTeam(swapPlayerDuo.ign)) {
                                    // If this condition is true, find two solos within those roles
                                    // Swap those two with ogTeam. 
                                    // This is one of the worst corner cases ever
                                    Role swapPlayerDuoRole = swapPlayerDuo.assignedRole;
                                    for (int k = 0; k < teams.Count; ++k) {
                                        if (!teams[k].getPlayerRole(duoRole).hasDuo() &&
                                            !teams[k].getPlayerRole(swapPlayerDuoRole).hasDuo()) {
                                            switchPlayer(duoRole, i, k, false);
                                            switchPlayer(swapPlayerDuoRole, i, k, false);
                                            break;
                                        }
                                    }
                                }
                            }
                            // Now switch between ogTeam and duoTeam
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
