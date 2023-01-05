using FireEmblem.Domain.Data;

namespace FireEmblem.MapView
{
    public class EnemyUnit : BaseUnit
    {
        public MapPosition GetMoveDestination(MapGrid mapGrid)
        {
            var movementGenerator = new MovementGenerator(mapGrid);
            var destinations = movementGenerator.GenerateAccessibleTiles();
        }
    }
}