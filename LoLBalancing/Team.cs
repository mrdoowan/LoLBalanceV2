using System;
using System.Collections.Generic;

namespace LoLBalancing
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
            players.Add(Role.ADC, bot);
            players.Add(Role.SUP, sup);
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
            players[role] = player;
            player.assignedRole = role;
        }

        // Returns true if ign is in the Team
        public bool isNameInTeam(Name ign) {
            foreach (Player player in players.Values) {
                if (player.ign == ign) {
                    return true;
                }
            }
            return false;
        }

        // Returns true if the player's duo is currently in the team
        public bool isDuoInTeam(Player player) {
            if (!player.hasDuo()) { return false; }
            foreach (Player possDuo in players.Values) {
                if (possDuo.ign == player.duo) { return true; }
            }
            return false;
        }

        /*
        // Returns a solo player with preference of primary and secondary
        // in the team
        public Player aSoloPlayerFromTeam(Role primary, Role second) {
            try { if (!players[primary].hasDuo()) { return players[primary]; } } catch { }
            try { if (!players[second].hasDuo()) { return players[second]; } } catch { }
            // The try+catch is for any Fill roles
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
        */
    }
}
