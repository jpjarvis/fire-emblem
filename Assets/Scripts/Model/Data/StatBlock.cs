using System;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class StatBlock : IStatBlock
    {
        public int Hp { get; set; }
        public int Strength { get; set; }
        public int Magic { get; set; }
        public int Dexterity { get; set; }
        public int Luck { get; set; }
        public int Speed { get; set; }
        public int Defence { get; set; }
        public int Resistance { get; set; }
    }
}