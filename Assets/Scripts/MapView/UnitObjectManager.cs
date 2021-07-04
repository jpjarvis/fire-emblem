using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Model.Data;
using FireEmblem.Model.Map;
using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class UnitObjectManager : MonoBehaviour
    {
        private readonly List<ActiveUnitObject> _activeUnitObjects = new List<ActiveUnitObject>();
        private IUnitPrefabProvider _unitPrefabProvider;
        private Grid _grid;

        private class ActiveUnitObject
        {
            public GameObject Object { get; set; }
            public MapPosition Position { get; set; } 
        }
        
        [Inject]
        public void Init(IUnitPrefabProvider unitPrefabProvider, Grid grid)
        {
            _unitPrefabProvider = unitPrefabProvider;
            _grid = grid;
        }

        public void CreatePlayerUnit(MapUnit mapUnit, Action onClick, Action onMouseOver)
        {
            var unitObject = Instantiate(_unitPrefabProvider.GetPlayerUnitPrefab(), transform);
            unitObject.GetComponent<PlayerUnit>().Unit = mapUnit;
            unitObject.GetComponent<MouseHandler>().ActionOnClick = onClick;
            unitObject.GetComponent<MouseHandler>().ActionOnMouseOver = onMouseOver;
            MoveObjectToGridPosition(unitObject, mapUnit.Position);
            _activeUnitObjects.Add(new ActiveUnitObject
            {
                Object = unitObject,
                Position = mapUnit.Position
            });
        }
        
        public void CreateEnemyUnit(MapUnit mapUnit)
        {
            var enemyUnit = Instantiate(_unitPrefabProvider.GetEnemyUnitPrefab(), transform);
            enemyUnit.GetComponent<EnemyUnit>().Unit = mapUnit;
            MoveObjectToGridPosition(enemyUnit, mapUnit.Position);
            _activeUnitObjects.Add(new ActiveUnitObject
            {
                Object = enemyUnit,
                Position = mapUnit.Position
            });
        }

        public void MoveUnit(MapPosition fromPosition, MapPosition toPosition)
        {
            var unitToMove = _activeUnitObjects.First(o => o.Position.Equals(fromPosition));
            unitToMove.Position = toPosition;
            MoveObjectToGridPosition(unitToMove.Object, toPosition);
        }
        
        private void MoveObjectToGridPosition(GameObject obj, MapPosition position)
        {
            obj.transform.localPosition = _grid.GetCellCenterLocal(new Vector3Int(position.X, position.Y, 0));
        }
    }
}