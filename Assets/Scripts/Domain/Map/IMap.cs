using System.Collections.Generic;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;

namespace FireEmblem.Domain.Map
{
    public interface IMap
    {
        IEnumerable<Unit> Units { get; }
        IMapTile GetTileAt(MapPosition mapPosition);
        Unit GetUnitAt(MapPosition mapPosition);
        MapPosition GetPositionOfUnit(Unit unit);
        void MoveUnit(Unit unit, MapPosition destination);
        void UpdateUnit(Unit unit);
        void RemoveUnit(Unit unit);
    }
}