using System;
using System.Collections.Generic;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class UnitData
    {
        public string Name { get; set; }
        public StatBlock Stats { get; set; }
        
        public List<WeaponItemData> Inventory { get; set; } = new List<WeaponItemData>();
    }
}