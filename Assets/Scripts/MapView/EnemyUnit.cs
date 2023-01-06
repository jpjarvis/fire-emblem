using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public class EnemyUnit : BaseUnit
    {
        public MapPosition GetMoveDestination(IEnumerable<BaseUnit> playerUnits, MovementGenerator movementGenerator)
        {
            var playerUnitsDictionary = playerUnits.ToDictionary(x => x.Position, x => x);
            var attackTiles = movementGenerator.GenerateAccessibleTiles(this, playerUnitsDictionary.Values)
                .Where(x => x.Value.Accessibility == TileAccessibility.CanAttack);

            var tileToAttack = attackTiles.FirstOrDefault(x => playerUnitsDictionary.ContainsKey(x.Key));
            return tileToAttack.Value?.SourceTiles.FirstOrDefault () ?? Position;
        }
    }
}