using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using FireEmblem.Model.Data;
using Unity.VisualScripting;
using UnityEditor.Rendering;

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

            var workingTiles = new HashSet<MapPosition> { startPosition };
            var moveableTiles = new HashSet<MapPosition>();

            var spacesMoved = 0;

            while (spacesMoved < maximumMoveDistance && workingTiles.Any())
            {
                var newWorkingTiles = new HashSet<MapPosition>();
                foreach (var tile in workingTiles)
                {
                    var tilesToCheck = TilesInRange(tile, 1, 1);

                    foreach (var tileToCheck in tilesToCheck.Where(tileToCheck => CanMoveThrough(tileToCheck, enemyUnits)
                                 && !tileToCheck.Equals(startPosition)))
                    {
                        moveableTiles.Add(tileToCheck);
                        newWorkingTiles.Add(tileToCheck);
                    }
                }

                workingTiles = newWorkingTiles;
                spacesMoved++;
            }

            var attackableTiles = new HashSet<MapPosition>();
            
            foreach (var tile in moveableTiles.Concat(new [] {unit.Position}))
            {
                foreach (var attackTile in TilesInRange(tile, minAttackRange, maxAttackRange))
                {
                    if (!moveableTiles.Contains(attackTile) && !attackTile.Equals(startPosition))
                    {
                        attackableTiles.Add(attackTile);
                    }
                }
            }

            return moveableTiles.Select(t => new AccessibleTile(t, TileAccessibility.CanMoveTo))
                .Concat(attackableTiles.Select(t => new AccessibleTile(t, TileAccessibility.CanAttack)));
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

            var tiles = new HashSet<MapPosition>();

            combinations.ForEach(combination =>
            {
                var (x, y) = combination;
                tiles.Add(new MapPosition(tile.X + x, tile.Y + y));
                tiles.Add(new MapPosition(tile.X + x, tile.Y - y));
                tiles.Add(new MapPosition(tile.X - x, tile.Y + y));
                tiles.Add(new MapPosition(tile.X - x, tile.Y - y));
            });

            return tiles;
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