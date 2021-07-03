using FireEmblem.Model.Combat;
using FireEmblem.Model.Data;

namespace FireEmblem.Model.Map
{
    public class MapUnit
    {
        public MapPosition Position { get; set; }
        public Unit Unit { get; }
            
        public MapUnit(MapPosition position, Unit unit)
        {
            Position = position;
            Unit = unit;
        }
    }
}