using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public class MovementGenerator
    {
        private readonly MapGrid mapGrid;

        public MovementGenerator(MapGrid mapGrid)
        {
            this.mapGrid = mapGrid;
        }

        public Dictionary<MapPosition, AccessibleTile> GenerateAccessibleTiles(BaseUnit unit, IEnumerable<BaseUnit> enemyUnits)
        {
            var startPosition = unit.Position;
            var maximumMoveDistance = unit.Unit.Stats.Movement;
            var minAttackRange = unit.Unit.Weapon.Data.MinRange;
            var maxAttackRange = unit.Unit.Weapon.Data.MaxRange;

            var workingTiles = new HashSet<MapPosition> { startPosition };
            var moveableTiles = new HashSet<MapPosition>();
            var movementSourceTiles = new Dictionary<MapPosition, MapPosition>();

            var spacesMoved = 0;

            while (spacesMoved < maximumMoveDistance && workingTiles.Any())
            {
                var newWorkingTiles = new HashSet<MapPosition>();
                foreach (var sourceTile in workingTiles)
                {
                    foreach (var tile in TilesInRange(sourceTile, 1, 1).Where(tileToCheck =>
                                 CanMoveThrough(tileToCheck, enemyUnits) && tileToCheck != startPosition))
                    {
                        moveableTiles.Add(tile);
                        newWorkingTiles.Add(tile);
                        if (!movementSourceTiles.ContainsKey(tile))
                        {
                            movementSourceTiles.Add(tile, sourceTile);
                        }
                    }
                    
                }

                workingTiles = newWorkingTiles;
                spacesMoved++;
            }

            var attackableTiles = new HashSet<MapPosition>();
            var attackSourceTiles = new Dictionary<MapPosition, List<MapPosition>>();

            foreach (var sourceTile in moveableTiles.Concat(new[] { unit.Position }))
            {
                foreach (var attackTile in TilesInRange(sourceTile, minAttackRange, maxAttackRange))
                {
                    if (!moveableTiles.Contains(attackTile) && attackTile != startPosition)
                    {
                        attackableTiles.Add(attackTile);
                        if (attackSourceTiles.TryGetValue(attackTile, out var sourceTiles))
                        {
                            sourceTiles.Add(sourceTile);
                        }
                        else
                        {
                            attackSourceTiles.Add(attackTile, new List<MapPosition> {sourceTile});
                        }
                    }
                }
            }

            var moveableTilesDictionary = moveableTiles.ToDictionary(t => t, t => new AccessibleTile(TileAccessibility.CanMoveTo, new []{ movementSourceTiles[t] }));
            var attackableTilesDictionary = attackableTiles.ToDictionary(t => t, t=> new AccessibleTile(TileAccessibility.CanAttack, attackSourceTiles[t]));

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
            return !enemyUnits.Any(unit => unit.Position == mapPosition) &&
                   mapGrid.GetTileAt(mapPosition).IsTraversable;
        }
    }

    public class AccessibleTile
    {
        public AccessibleTile(TileAccessibility tileAccessibility, IEnumerable<MapPosition> sourceTiles)
        {
            Accessibility = tileAccessibility;
            SourceTiles = sourceTiles;
        }
        
        public TileAccessibility Accessibility { get; }
        
        public IEnumerable<MapPosition> SourceTiles { get; }
    }

    public enum TileAccessibility
    {
        Inaccessible,
        CanMoveTo,
        CanAttack
    }
}