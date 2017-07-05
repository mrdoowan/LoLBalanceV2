using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// In this Class, the teams will be internally be balancing
// It will only be driven by public functions

namespace LoLBalanceV2
{
    public class Balance
    {
        private List<Team> teams;    // This will contain all Players in Teams
        private int maxTeamNum;         // Team that has the highest teamValue
        private int maxTeamValue;       // teamValue in that maxTeam
        private int minTeamNum;         // Team that has the lowest teamValue
        private int minTeamValue;       // teamValue in that minTeam
        public int rangeTeamVal;        // = maxTeamValue - minTeamValue
                                        // It also determines the value of the Balance
                                        // The lower the better
        private const int CUTOFF_RANGE = 2;

        // Default ctor
        public Balance() {
            teams = new List<Team>();
            maxTeamNum = 0;
            maxTeamValue = 0;
            minTeamNum = 0;
            minTeamValue = 0;
            rangeTeamVal = 0;
        }

        // Init ctor
        // Implement greedy approach later if you want a faster algorithm
        public Balance(Dictionary<Role, List<Player>> roleList,
            Dictionary<string, Player> roster, int numTeams) {
            foreach (Role role in roleList.Keys) {
                for (int i = 0; i < numTeams; ++i) {
                    if (!teams[i].containsRole(role)) {
                        
                    }
                }
            }
        }
    }
}
