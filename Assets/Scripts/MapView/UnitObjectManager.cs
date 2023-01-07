using System;
using System.Collections.Generic;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class UnitObjectManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerUnitPrefab;
        [SerializeField] private GameObject enemyUnitPrefab;
        [SerializeField] private MapGrid mapGrid;
        
        private readonly Dictionary<Guid, GameObject> unitObjects = new();

        public void CreateUnitObject(Unit unit, MapPosition mapPosition)
        {
            unitObjects.Add(unit.Id, InstantiateUnitObject(unit, mapPosition));
        }

        public void MoveUnitObject(Unit unit, MapPosition mapPosition)
        {
            if (unitObjects.TryGetValue(unit.Id, out var unitObject))
            {
                mapGrid.MoveObjectToGridPosition(unitObject, mapPosition);
            }
        }
        
        public void RemoveUnitObject(Unit unit)
        {
            if (unitObjects.Remove(unit.Id, out var unitObject))
            {
                Destroy(unitObject);
            }
        }

        private GameObject InstantiateUnitObject(Unit unit, MapPosition mapPosition)
        {
            if (unit.Allegiance == Allegiance.Player)
            {
                return mapGrid.InstantiateAtGridPosition(playerUnitPrefab, mapPosition);
            }

            return mapGrid.InstantiateAtGridPosition(enemyUnitPrefab, mapPosition);
        }
    }
}