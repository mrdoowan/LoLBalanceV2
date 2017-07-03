﻿using System;
using System.Collections.Generic;

namespace LoLBalanceV2
{
    public class Player : IComparable<Player>
    {
        public string name;        // Real name of player (never use this for code)  
        public string ign;         // Summoner name
        public Tier tier;
        public int division;
        public Role primaryRole;   // Primary Role
        public Role secondRole;    // Secondary Role
        public Role assignRole;    // The assigned Role when in a team. Initialize to NONE
        public string duo;         // Leave blank if Solo
        
        // Default Ctor
        public Player() {
            tier = Tier.BRONZE;
            division = 5;
            primaryRole = Role.NONE;
            secondRole = Role.NONE;
            assignRole = Role.NONE;
        }

        // Init ctor
        public Player(string name_, string ign_, Tier tier_, int div_,
            Role pri_, Role sec_, string duo_ = "") {
            name = name_;
            ign = ign_;
            tier = tier_;
            if (tier == Tier.MASTER || tier == Tier.CHALLENGER) { division = 1; }
            else { division = div_; }
            primaryRole = pri_;
            secondRole = sec_;
            assignRole = Role.NONE;
            duo = duo_;
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
            { Tier.MASTER, 5 },
            { Tier.CHALLENGER, 6 }
        };
    }
}

// Enumerations signifying Tiers
namespace LoLBalanceV2
{
    public enum Tier
    {
        BRONZE,
        SILVER,
        GOLD,
        PLATINUM,
        DIAMOND,
        MASTER,
        CHALLENGER,
    }
}

// Enumerations signifying everyone's roles
namespace LoLBalanceV2
{
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