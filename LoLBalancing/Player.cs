using System;
using System.Collections.Generic;

namespace LoLBalancing
{
    [Serializable]
    public class Player : IComparable<Player>
    {
        public string name;         // Real name of player (never use this for code)  
        public string ign;          // Summoner name
        public Tier tier;
        public int division;
        public Role primaryRole;    // Primary Role
        public Role secondRole;     // Secondary Role
        public Role assignedRole;   // The assigned Role when in a team. Initialize to NONE
        public string duo;          // Leave blank if Solo

        // Return boolean getters
        public bool isPrimaryAssigned() {
            return primaryRole == assignedRole;
        }
        public bool isSecondaryAssigned() {
            return secondRole == assignedRole;
        }
        public bool isRoleFilled() {
            return (primaryRole == Role.FILL) ||
                (primaryRole != assignedRole && secondRole == Role.FILL);
        }
        public bool isAutoFilled() {
            return primaryRole != Role.FILL &&
                secondRole != Role.FILL &&
                primaryRole != assignedRole &&
                secondRole != assignedRole;
        }
        // so that string.IsNullorWhite fxn isn't overkilled
        public bool hasDuo() {
            return (!string.IsNullOrWhiteSpace(duo));
        }

        // Default Ctor
        public Player() {
            tier = Tier.BRONZE;
            division = 5;
            primaryRole = Role.NONE;
            secondRole = Role.NONE;
            assignedRole = Role.NONE;
        }

        // Init ctor
        public Player(string name_, string ign_, Tier tier_, int div_,
            Role pri_, Role sec_, string duo_ = "") {
            name = name_;
            ign = ign_;
            tier = tier_;
            division = (tier == Tier.MASTER) ? 5 : div_;
            primaryRole = pri_;
            secondRole = sec_;
            duo = duo_;
            assignedRole = Role.NONE;
        }

        // Calculates the value of a Rank.
        // True takes assignedRole into account, False does not
        // standard y = mx - b, with b = (lowestRank) - 1
        public int rankValue(bool roleAffect) {
            int pts = TIER_TO_PTS[this.tier] * 5 + (6 - this.division);
            if (roleAffect) {
                // Any players filled or get their secondary role will be 10% lower
                if (isRoleFilled() || isSecondaryAssigned()) {
                    pts = pts * 9 / 10;
                }
                // Players that are autofilled will be 30% lower
                else if (isAutoFilled()) {
                    pts = pts * 7 / 10;
                }
            }
            return (pts < 1) ? 1 : pts;
        }

        // -1 if this rank is lower than Other rank
        // 0 if equal ranked
        // 1 if higher ranked
        int IComparable<Player>.CompareTo(Player other) {
            int sumOther = TIER_TO_PTS[other.tier] * 5 + (6 - other.division);
            int sumThis = TIER_TO_PTS[this.tier] * 5 + (6 - this.division);
            if (sumOther > sumThis) { return -1; }
            else if (sumOther == sumThis) { return 0; }
            else { return 1; }
        }

        // Point value from Tier for IComparable
        private Dictionary<Tier, int> TIER_TO_PTS = new Dictionary<Tier, int>() {
            { Tier.BRONZE, 0 },
            { Tier.SILVER, 1 },
            { Tier.GOLD, 2 },
            { Tier.PLATINUM, 3 },
            { Tier.DIAMOND, 4 },
            { Tier.MASTER, 5 }
        };
    }
}

// Enumerations signifying Tiers
namespace LoLBalancing
{
    public enum Tier
    {
        BRONZE,
        SILVER,
        GOLD,
        PLATINUM,
        DIAMOND,
        MASTER
    }

    // Enumerations signifying everyone's roles
    public enum Role
    {
        TOP,
        JNG,
        MID,
        BOT,
        SUP,
        FILL,
        NONE
    }
}
