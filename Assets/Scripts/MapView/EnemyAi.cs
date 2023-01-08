using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public static class EnemyAi
    {
        public static IEnemyAction GetAction(Unit unit, Map map)
        {
            var movementRange = MovementGenerator.GetMovementRange(unit, map);
            var attackTiles = movementRange
                .GetAllAccessibleTiles()
                .Where(x => x.Accessibility == TileAccessibility.CanAttack);

            var tileToAttack = attackTiles.FirstOrDefault(x =>
            {
                var targetUnit = map.GetUnitAt(x.Position);

                if (targetUnit != null)
                {
                    return targetUnit.Allegiance != unit.Allegiance;
                }

                return false;
            });

            if (tileToAttack != null)
            {
                return new MoveAndAttackAction(
                    movementRange.GetPathTo(tileToAttack.Position), 
                    map.GetUnitAt(tileToAttack.Position));
            }

            return new NoAction();
        }

        
    }
    
    public interface IEnemyAction
    {
    }

    public record MoveAndAttackAction(IEnumerable<MapPosition> MovementPath, Unit UnitToAttack) : IEnemyAction;

    public record NoAction : IEnemyAction;
}