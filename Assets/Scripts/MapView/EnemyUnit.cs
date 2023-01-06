using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public class EnemyUnit : BaseUnit
    {
        protected override Allegiance Allegiance => Allegiance.Enemy;
        
        public MapPosition GetMoveDestination(MapGrid mapGrid)
        {
            var movementGenerator = new MovementGenerator(mapGrid);
            var attackTiles = movementGenerator.GenerateAccessibleTiles(this)
                .Where(x => x.Value.Accessibility == TileAccessibility.CanAttack);

            var tileToAttack = attackTiles.FirstOrDefault(x =>
            {
                var unit = mapGrid.GetUnitAt(x.Key);

                if (unit != null)
                {
                    return unit.Allegiance != Allegiance;
                }

                return false;
            });
            return tileToAttack.Value?.SourceTiles.FirstOrDefault () ?? Position;
        }
    }
}