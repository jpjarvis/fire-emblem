using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;

namespace FireEmblem.MapView
{
    public class MovementGenerator
    { 
        private readonly MapGrid _mapGrid;

        public MovementGenerator(MapGrid mapGrid)
        {
            _mapGrid = mapGrid;
        }
        
        public IEnumerable<AccessibleTile> GenerateAccessibleTiles(BaseUnit unit, IEnumerable<BaseUnit> enemyUnits)
        {
            var startPosition = unit.Position;
            var maximumMoveDistance = unit.Unit.Stats.Movement;
            var minAttackRange = unit.Unit.Weapon.Data.MinRange;
            var maxAttackRange = unit.Unit.Weapon.Data.MaxRange;

            var workingTiles = new List<MapPosition> { startPosition };
            var tileAccessibility = new Dictionary<MapPosition, TileAccessibility>();

            var spacesMoved = 0;

            while (spacesMoved < maximumMoveDistance && workingTiles.Any())
            {
                var newWorkingTiles = new List<MapPosition>();
                foreach (var tile in workingTiles)
                {
                    var tilesToCheck = new List<MapPosition>
                    {
                        new MapPosition(tile.X + 1, tile.Y),
                        new MapPosition(tile.X - 1, tile.Y),
                        new MapPosition(tile.X, tile.Y + 1),
                        new MapPosition(tile.X, tile.Y - 1),
                    };

                    foreach (var tileToCheck in tilesToCheck.Where(tileToCheck => CanMoveThrough(tileToCheck, enemyUnits)
                                 && !tileAccessibility.ContainsKey(tileToCheck)
                                 && !tileToCheck.Equals(startPosition)))
                    {
                        tileAccessibility.Add(tileToCheck, TileAccessibility.CanMoveTo);
                        newWorkingTiles.Add(tileToCheck);
                    }
                }

                workingTiles = newWorkingTiles;
                spacesMoved++;
            }

            return tileAccessibility.Select(pair => new AccessibleTile(pair.Key, pair.Value));
        }

        private bool CanMoveThrough(MapPosition mapPosition, IEnumerable<BaseUnit> enemyUnits)
        {
            return !enemyUnits.Any(unit => unit.Position.Equals(mapPosition)) &&
                   _mapGrid.GetTileAt(mapPosition).IsTraversable;
        }
    }
    
    public class AccessibleTile
    {
        public AccessibleTile(MapPosition position, TileAccessibility tileAccessibility)
        {
            Position = position;
            Accessibility = tileAccessibility;
        }
        public MapPosition Position { get; }
        public TileAccessibility Accessibility { get; }
    }

    public enum TileAccessibility
    {
        Inaccessible,
        CanMoveTo,
        CanAttack
    }
}