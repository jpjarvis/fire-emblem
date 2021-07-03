using FireEmblem.Model.Map;

namespace FireEmblem.MapView
{
    public class MapGenerator : IMapGenerator
    {
        private readonly UnitObjectManager _unitObjectManager;
        
        public MapGenerator(UnitObjectManager unitObjectManager)
        {
            _unitObjectManager = unitObjectManager;
        }

        public void GenerateMap(Map map)
        {
            foreach (var playerUnit in map.PlayerUnits)
            {
                _unitObjectManager.CreatePlayerUnit(playerUnit);
            }

            foreach (var enemyUnit in map.EnemyUnits)
            {
                _unitObjectManager.CreateEnemyUnit(enemyUnit);
            }
        }
    }
}