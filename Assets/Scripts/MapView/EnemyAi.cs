using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public static class EnemyAi
    {
        public static IEnemyAction GetAction(Unit unit, Map map)
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

            if (tileToAttack.Value != null)
            {
                return new MoveAndAttackAction(tileToAttack.Value.SourceTiles.FirstOrDefault(), map.GetUnitAt(tileToAttack.Key));
            }

            return new NoAction();
        }

        
    }
    
    public interface IEnemyAction
    {
    }

    public record MoveAndAttackAction(MapPosition PositionToMoveTo, Unit UnitToAttack) : IEnemyAction;

    public record NoAction : IEnemyAction;
}