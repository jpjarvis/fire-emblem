using System;
using System.Collections.Generic;

namespace FireEmblem.Model.Data
{
    [Serializable]
    public class MapData
    {
        public string Id { get; set; }
        public List<MapPosition> PlayerUnitPositions { get; set; }
        public List<MapUnitData> EnemyUnits { get; set; }
        public Dictionary<string, UnitData> UnitData { get; set; }
    }
}