using System.Collections.Generic;
using FireEmblem.Domain.Data;

namespace FireEmblem.Domain.Map
{
    public class AccessibleTile
    {
        public AccessibleTile(MapPosition position, TileAccessibility tileAccessibility,
            IEnumerable<MapPosition> sourceTiles)
        {
            Position = position;
            Accessibility = tileAccessibility;
            SourceTiles = sourceTiles;
        }

        public MapPosition Position { get; }
        public TileAccessibility Accessibility { get; }

        public IEnumerable<MapPosition> SourceTiles { get; }
    }
}