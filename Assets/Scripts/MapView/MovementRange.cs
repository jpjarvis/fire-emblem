using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using JetBrains.Annotations;

namespace FireEmblem.MapView
{
    public class MovementRange
    {
        private readonly MapPosition sourcePosition;
        private readonly Dictionary<MapPosition, AccessibleTile> accessibleTiles;

        private MovementRange(MapPosition sourcePosition, IEnumerable<AccessibleTile> accessibleTiles)
        {
            this.sourcePosition = sourcePosition;
            this.accessibleTiles = accessibleTiles.ToDictionary(x => x.Position, x => x);
        }

        [CanBeNull]
        public AccessibleTile GetAccessibleTile(MapPosition mapPosition)
        {
            return accessibleTiles.GetValueOrDefault(mapPosition);
        }
        
        public IEnumerable<AccessibleTile> GetAllAccessibleTiles()
        {
            return accessibleTiles.Values;
        }

        public IEnumerable<MapPosition> GetPathTo(MapPosition position)
        {
            var currentPosition = position;
            var targetTile = GetAccessibleTile(position);
            if (targetTile?.Accessibility == TileAccessibility.CanAttack)
            {
                currentPosition = targetTile.SourceTiles.FirstOrDefault();
            }

            var tilesInPath = new List<MapPosition>();

            while (currentPosition != null && currentPosition != sourcePosition)
            {
                tilesInPath.Add(currentPosition);
                currentPosition = accessibleTiles[currentPosition].SourceTiles.FirstOrDefault();
            }

            tilesInPath.Reverse();
            return tilesInPath;
        }
        
        public static MovementRange Generate(Unit unit, Map map)
        {
            var startPosition = map.GetPositionOfUnit(unit);
            if (startPosition == null)
            {
                return new MovementRange(null, new List<AccessibleTile>());
            }

            var enemyUnitPositions = map.Units
                .Where(x => x.Allegiance != unit.Allegiance)
                .Select(map.GetPositionOfUnit);

            var maximumMoveDistance = unit.Stats.Movement;
            var minAttackRange = unit.Weapon.Data.MinRange;
            var maxAttackRange = unit.Weapon.Data.MaxRange;

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
                                 CanMoveThrough(map, tileToCheck, enemyUnitPositions) && tileToCheck != startPosition))
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

            foreach (var sourceTile in moveableTiles.Concat(new[] { startPosition }))
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
                            attackSourceTiles.Add(attackTile, new List<MapPosition> { sourceTile });
                        }
                    }
                }
            }

            var accessibleTiles = moveableTiles.Select(
                    t => new AccessibleTile(t, TileAccessibility.CanMoveTo, new[] { movementSourceTiles[t] }))
                .Concat(attackableTiles.Select(t =>
                    new AccessibleTile(t, TileAccessibility.CanAttack, attackSourceTiles[t])));
            return new MovementRange(startPosition, accessibleTiles);
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

        private static bool CanMoveThrough(Map map, MapPosition mapPosition, IEnumerable<MapPosition> enemyUnitPositions)
        {
            return !enemyUnitPositions.Contains(mapPosition) &&
                   map.GetTileAt(mapPosition).IsTraversable;
        }
    }
}