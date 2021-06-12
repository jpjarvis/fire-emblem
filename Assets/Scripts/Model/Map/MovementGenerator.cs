using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;

namespace FireEmblem.Model.Map
{
    public class MovementGenerator
    {
        public static List<AccessibleTile> GenerateAccessibleTiles(Map map, MapUnit unit)
        {
            var startPosition = unit.Position;
            var maximumMoveDistance = unit.Unit.Stats.Movement;
            var minAttackRange = unit.Unit.Weapon.Data.MinRange;
            var maxAttackRange = unit.Unit.Weapon.Data.MaxRange;

            var workingTiles = new List<MapPosition> {startPosition};
            var accessibleTiles = new List<AccessibleTile>();

            var spacesMoved = 0;
            
            while (spacesMoved < maximumMoveDistance && workingTiles.Any())
            {
                var newWorkingTiles = new List<MapPosition>();
                foreach (var tile in workingTiles)
                {
                    var tilesToCheck = new List<MapPosition>
                    {
                        new MapPosition
                        {
                            X = tile.X + 1,
                            Y = tile.Y
                        },
                        new MapPosition
                        {
                            X = tile.X - 1,
                            Y = tile.Y
                        },
                        new MapPosition
                        {
                            X = tile.X,
                            Y = tile.Y + 1
                        },
                        new MapPosition
                        {
                            X = tile.X,
                            Y = tile.Y - 1
                        },
                    };

                    foreach (var tileToCheck in tilesToCheck)
                    {
                        if (map.CanMoveThrough(tileToCheck) 
                            && !accessibleTiles.Any(t => t.Position.Equals(tileToCheck)) 
                            && !newWorkingTiles.Any(t => t.Equals(tileToCheck))
                            && !tileToCheck.Equals(startPosition))
                        {
                            accessibleTiles.Add(new AccessibleTile
                            {
                                Position = tileToCheck,
                                Accessibility = TileAccessibility.CanMoveTo
                            });
                            newWorkingTiles.Add(tileToCheck);
                        }
                    }
                }

                workingTiles = newWorkingTiles;
                spacesMoved++;
            }
            
            return accessibleTiles;
        }
    }

    public class AccessibleTile
    {
        public MapPosition Position { get; set; }
        public TileAccessibility Accessibility { get; set; }
    }

    public enum TileAccessibility
    {
        Inaccessible,
        CanMoveTo,
        CanAttack
    }
}