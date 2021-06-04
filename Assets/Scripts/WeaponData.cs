﻿﻿using System;

namespace FireEmblem
{
    public class WeaponData
    {
        public int Might { get; set; }
        
        public int Hit { get; set; }
        
        public int Crit { get; set; }
        
        public int MinRange { get; set; }
        
        public int MaxRange { get; set; }
        
        public Stat AttackStat { get; set; }

        public static readonly WeaponData Empty = new WeaponData
        {
            AttackStat = Stat.Strength,
            Might = 0,
            Crit = 0,
            Hit = 0,
            MinRange = 1,
            MaxRange = 1
        };

        public bool IsInRange(int range)
        {
            return range >= MinRange && range <= MaxRange;
        }
    }
}