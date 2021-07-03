using System;
using System.Collections.Generic;
using FireEmblem.Model.Map;
using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class UnitObjectManager : MonoBehaviour
    {
        private List<GameObject> _activeUnitObjects;
        private IUnitPrefabProvider _unitPrefabProvider;
        private Grid _grid;

        [Inject]
        public void Init(IUnitPrefabProvider unitPrefabProvider, Grid grid)
        {
            _unitPrefabProvider = unitPrefabProvider;
            _grid = grid;
        }

        public void CreatePlayerUnit(MapUnit mapUnit, Action onClick)
        {
            var unitObject = Instantiate(_unitPrefabProvider.GetPlayerUnitPrefab(), transform);
            var playerUnit = unitObject.GetComponent<PlayerUnit>();
            playerUnit.Unit = mapUnit;
            playerUnit.OnClick = onClick;
            MoveObjectToGridPosition(unitObject, mapUnit.Position.X, mapUnit.Position.Y);
        }
        
        public void CreateEnemyUnit(MapUnit mapUnit)
        {
            var enemyUnit = Instantiate(_unitPrefabProvider.GetEnemyUnitPrefab(), transform);
            enemyUnit.GetComponent<EnemyUnit>().Unit = mapUnit;
            MoveObjectToGridPosition(enemyUnit, mapUnit.Position.X, mapUnit.Position.Y);
        }
        
        private void MoveObjectToGridPosition(GameObject obj, int x, int y)
        {
            obj.transform.localPosition = _grid.GetCellCenterLocal(new Vector3Int(x, y, 0));
        }
    }
}