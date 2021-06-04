using System;
using System.Collections.Generic;
using System.Transactions;

namespace FireEmblem
{
    public class StatBlock
    {
        private readonly Dictionary<Stat, int> _stats;

        public StatBlock()
        {
            _stats = new Dictionary<Stat, int>();

            foreach (Stat stat in Enum.GetValues(typeof(Stat)))
            {
                _stats[stat] = 0;
            }
        }
        
        public int Get(Stat stat)
        {
            return _stats[stat];
        }

        public void Increment(Stat stat)
        {
            _stats[stat]++;
        }
        
        public void Set(Stat stat, int newValue)
        {
            _stats[stat] = newValue;
        }
    }
}