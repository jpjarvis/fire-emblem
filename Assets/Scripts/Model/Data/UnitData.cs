using System;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class UnitData
    {
        public string Name { get; set; }
        public StatBlock Stats { get; set; }
    }
}