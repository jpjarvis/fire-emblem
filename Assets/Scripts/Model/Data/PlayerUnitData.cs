using System;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class PlayerUnitData
    {
        public string Name { get; set; }
        public StatBlock BaseStats { get; set; }
        public StatBlock Growths { get; set; }
    }
}