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

        public Dictionary<MapPosition, AccessibleTile> GenerateAccessibleTiles(BaseUnit unit, IEnumerable<BaseUnit> enemyUnits)
        {
            var startPosition = unit.Position;
            var maximumMoveDistance = unit.Unit.Stats.Movement;
            var minAttackRange = unit.Unit.Weapon.Data.MinRange;
            var maxAttackRange = unit.Unit.Weapon.Data.MaxRange;

            var workingTiles = new HashSet<MapPosition> { startPosition };
            var moveableTiles = new HashSet<MapPosition>();

            var spacesMoved = 0;

            while (spacesMoved < maximumMoveDistance && workingTiles.Any())
            {
                var newWorkingTiles = new HashSet<MapPosition>();
                foreach (var tileToCheck in workingTiles.SelectMany(tile => TilesInRange(tile, 1, 1)
                             .Where(tileToCheck =>
                                 CanMoveThrough(tileToCheck, enemyUnits) && !tileToCheck.Equals(startPosition))))
                {
                    moveableTiles.Add(tileToCheck);
                    newWorkingTiles.Add(tileToCheck);
                }

                workingTiles = newWorkingTiles;
                spacesMoved++;
            }

            var attackableTiles = new HashSet<MapPosition>();

            foreach (var attackTile in moveableTiles.Concat(new[] { unit.Position })
                         .SelectMany(tile => TilesInRange(tile, minAttackRange, maxAttackRange)))
            {
                if (!moveableTiles.Contains(attackTile) && !attackTile.Equals(startPosition))
                {
                    attackableTiles.Add(attackTile);
                }
            }

            var moveableTilesDictionary = moveableTiles.ToDictionary(t => t, t => new AccessibleTile(TileAccessibility.CanMoveTo));
            var attackableTilesDictionary = attackableTiles.ToDictionary(t => t, t=> new AccessibleTile(TileAccessibility.CanAttack));

            return moveableTilesDictionary.Concat(attackableTilesDictionary).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        private static IEnumerable<MapPosition> TilesInRange(MapPosition tile, int minRange, int maxRange)
        {
            var combinations = new List<(int, int)>();

            for (var range = minRange; range <= maxRange; range++)
            {
                for (var i = 0; i <= range; i++)
                {
                    combinations.Add((i, range - i));
                }
            }

            foreach (var combination in combinations)
            {
                var (x, y) = combination;
                yield return new MapPosition(tile.X + x, tile.Y + y);
                yield return new MapPosition(tile.X + x, tile.Y - y);
                yield return new MapPosition(tile.X - x, tile.Y + y);
                yield return new MapPosition(tile.X - x, tile.Y - y);
            }
        }

        private bool CanMoveThrough(MapPosition mapPosition, IEnumerable<BaseUnit> enemyUnits)
        {
            return !enemyUnits.Any(unit => unit.Position.Equals(mapPosition)) &&
                   _mapGrid.GetTileAt(mapPosition).IsTraversable;
        }
    }

    public class AccessibleTile
    {
        public AccessibleTile(TileAccessibility tileAccessibility)
        {
            Accessibility = tileAccessibility;
        }
        
        public TileAccessibility Accessibility { get; }
    }

    public enum TileAccessibility
    {
        Inaccessible,
        CanMoveTo,
        CanAttack
    }
}