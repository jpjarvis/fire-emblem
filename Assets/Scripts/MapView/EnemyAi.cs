using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public static class EnemyAi
    {
        public static MapPosition GetMoveDestination(Unit unit, MapGrid mapGrid)
        {
            var movementGenerator = new MovementGenerator(mapGrid);
            var attackTiles = movementGenerator.GenerateAccessibleTiles(unit)
                .Where(x => x.Value.Accessibility == TileAccessibility.CanAttack);

            var tileToAttack = attackTiles.FirstOrDefault(x =>
            {
                var targetUnit = mapGrid.GetUnitAt(x.Key);

                if (targetUnit != null)
                {
                    return targetUnit.Allegiance != unit.Allegiance;
                }

                return false;
            });
            return tileToAttack.Value?.SourceTiles.FirstOrDefault () ?? mapGrid.GetPositionOfUnit(unit);
        }
    }
}