using FireEmblem.Common;
using FireEmblem.Model.Map;
using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class MapManager : MonoBehaviour
    {
        private TileObjectManager _tileObjectManager;
        private UnitObjectManager _unitObjectManager;
        private Map _map;

        private MapUnit _selectedUnit = null;

        [Inject]
        public void Init(TileObjectManager tileObjectManager, UnitObjectManager unitObjectManager, Map map)
        {
            _tileObjectManager = tileObjectManager;
            _unitObjectManager = unitObjectManager;
            _map = map;
        }

        private void Start()
        {
            GenerateMap(_map);
        }

        private void GenerateMap(Map map)
        {
            foreach (var playerUnit in map.PlayerUnits)
            {
                _unitObjectManager.CreatePlayerUnit(playerUnit, () => {SelectUnit(playerUnit);});
            }

            foreach (var enemyUnit in map.EnemyUnits)
            {
                _unitObjectManager.CreateEnemyUnit(enemyUnit);
            }
        }
        
        public void SelectUnit(MapUnit mapUnit)
        {
            _selectedUnit = mapUnit;
            _tileObjectManager.DestroyAll();
            ShowMovementRange(mapUnit);
        }
        
        private void ShowMovementRange(MapUnit mapUnit)
        {
            var tiles = MovementGenerator.GenerateAccessibleTiles(_map, mapUnit);

            foreach (var tile in tiles)
            {
                _tileObjectManager.CreateMoveTile(tile.Position.X, tile.Position.Y);
            }
        }
    }
}