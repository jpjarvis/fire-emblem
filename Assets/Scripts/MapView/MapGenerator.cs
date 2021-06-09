using System.Linq;
using FireEmblem.Model.Combat;
using FireEmblem.Model.Data.Serialization;
using FireEmblem.Model.Map;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerUnitPrefab;
        
        [SerializeField]
        private GameObject enemyUnitPrefab;
        
        private Grid _grid;

        private void Awake()
        {
            _grid = GetComponent<Grid>();
        }

        public void GenerateMap(string mapId)
        {
            var mapData = MapLoader.LoadMap(mapId);
            var map = Map.Create(mapData);
            var playerUnits = PlayerUnitLoader.LoadPlayerUnits();

            foreach (var playerUnit in map.PlayerUnits)
            {
                CreatePlayerUnit(playerUnit);
            }

            foreach (var enemyUnit in map.EnemyUnits)
            {
                CreateEnemyUnit(enemyUnit);
            }
        }

        private void MoveObjectToGridPosition(GameObject obj, int x, int y)
        {
            obj.transform.localPosition = _grid.GetCellCenterLocal(new Vector3Int(x - 10, y - 10, 0));
        }
        
        private void CreatePlayerUnit(MapUnit mapUnit)
        {
            var playerUnit = Instantiate(playerUnitPrefab, transform);
            playerUnit.GetComponent<PlayerUnit>().Unit = mapUnit;
            MoveObjectToGridPosition(playerUnit, mapUnit.Position.X, mapUnit.Position.Y);
        }
        
        private void CreateEnemyUnit(MapUnit mapUnit)
        {
            var enemyUnit = Instantiate(enemyUnitPrefab, transform);
            enemyUnit.GetComponent<EnemyUnit>().Unit = mapUnit;
            MoveObjectToGridPosition(enemyUnit, mapUnit.Position.X, mapUnit.Position.Y);
        }
    }
}