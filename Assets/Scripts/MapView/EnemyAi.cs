using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public static class EnemyAi
    {
        public static MapPosition GetMoveDestination(Unit unit, Map map)
        {
            var movementGenerator = new MovementGenerator(map);
            var attackTiles = movementGenerator.GenerateAccessibleTiles(unit)
                .Where(x => x.Value.Accessibility == TileAccessibility.CanAttack);

            var tileToAttack = attackTiles.FirstOrDefault(x =>
            {
                var targetUnit = map.GetUnitAt(x.Key);

                if (targetUnit != null)
                {
                    return targetUnit.Allegiance != unit.Allegiance;
                }

                return false;
            });
            return tileToAttack.Value?.SourceTiles.FirstOrDefault () ?? map.GetPositionOfUnit(unit);
        }
    }
}