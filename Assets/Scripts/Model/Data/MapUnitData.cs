using System;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class MapUnitData
    {
        public MapPosition Position { get; set; }
        public string UnitDataId { get; set; }
    }
}