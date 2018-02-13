﻿using System;
using System.Collections.Generic;

namespace LoLBalancing
{
    [Serializable]
    public class Player : IComparable<Player>
    {
        // Values that are dependent on MainForm
        private int secondPctDrop;
        private int fillPctDrop;

        // Public member functions
        public string name;         // Real name of player (never use this for code)  
        public Name ign;            // Summoner name
        public Tier tier;
        public int division;
        public Role primaryRole;        // Primary Role
        public List<Role> secondRoles;  // Secondary Role
        public Role assignedRole;       // The assigned Role when in a team. Initialize to NONE
        public Name duo;                // Leave blank if Solo

        // Return boolean getters
        public bool isPrimaryAssigned() {
            return primaryRole == assignedRole;
        }
        public bool isSecondaryAssigned() {
            return secondRoles.Contains(assignedRole);
        }
        public bool isRoleFilled() {
            return (primaryRole == Role.FILL) ||
                (primaryRole != assignedRole && secondRoles.Contains(Role.FILL));
        }
        public bool isAutoFilled() {
            return primaryRole != Role.FILL && !secondRoles.Contains(Role.FILL) &&
                primaryRole != assignedRole && !secondRoles.Contains(assignedRole);
        }
        // so that string.IsNullorWhite fxn isn't overkilled
        public bool hasDuo() {
            return (!string.IsNullOrWhiteSpace(duo.name));
        }
        public string rankString() {
            string str = TIER_TO_STRING[tier];
            return (tier == Tier.MASTER || tier == Tier.CHALLENGER) ? str : str + " " + division.ToString();
        }

        // Default Ctor
        public Player() {
            tier = Tier.BRONZE;
            division = 5;
            primaryRole = Role.NONE;
            secondRoles = new List<Role>();
            assignedRole = Role.NONE;
            secondPctDrop = 90; // Default
            fillPctDrop = 70;   // Default
        }

        // Init ctor
        public Player(string name_, Name ign_, Tier tier_, int div_,
            Role pri_, List<Role> sec_, Name duo_, int secDrop_, int fillDrop_) {
            name = name_;
            ign = ign_;
            tier = tier_;
            division = (tier == Tier.MASTER) ? 5 : div_;
            primaryRole = pri_;
            secondRoles = sec_;
            duo = duo_;
            assignedRole = Role.NONE;
            secondPctDrop = secDrop_;
            fillPctDrop = fillDrop_;
        }

        // Calculates the value of a Rank.
        // True takes assignedRole into account, False does not
        // standard y = mx - b, with b = (lowestRank) - 1
        public int rankValue() {
            int pts = MainForm.currRank2Pts[this.rankString()];
            // Any players filled or get their secondary role will be slightly lower
            if (isRoleFilled() || isSecondaryAssigned()) {
                pts = pts * secondPctDrop / 100;
            }
            // Players that are autofilled will be slightly lower
            else if (isAutoFilled()) {
                pts = pts * fillPctDrop / 100;
            }
            return (pts < 1) ? 1 : pts;
        }

        // -1 if this rank is lower than Other rank
        // 0 if equal ranked
        // 1 if higher ranked
        int IComparable<Player>.CompareTo(Player other) {
            int sumOther = MainForm.currRank2Pts[other.rankString()];
            int sumThis = MainForm.currRank2Pts[this.rankString()];
            if (sumOther > sumThis) { return -1; }
            else if (sumOther == sumThis) { return 0; }
            else { return 1; }
        }

        // Tier to string
        private Dictionary<Tier, string> TIER_TO_STRING = new Dictionary<Tier, string>() {
            { Tier.BRONZE, "Bronze" },
            { Tier.SILVER, "Silver" },
            { Tier.GOLD, "Gold" },
            { Tier.PLATINUM, "Platinum" },
            { Tier.DIAMOND, "Diamond" },
            { Tier.MASTER, "Masters" },
            { Tier.CHALLENGER, "Challenger" }
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
        MASTER,
        CHALLENGER
    }

    // Enumerations signifying everyone's roles
    public enum Role
    {
        TOP,
        JNG,
        MID,
        ADC,
        SUP,
        FILL,
        NONE
    }
}
