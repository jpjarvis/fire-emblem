using System;
using System.Collections;
using System.Collections.Generic;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class UnitObjectManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerUnitPrefab;
        [SerializeField] private GameObject enemyUnitPrefab;
        [SerializeField] private Map map;
        [SerializeField] private MapGrid mapGrid;
        
        private readonly Dictionary<Guid, GameObject> unitObjects = new();

        private void Start()
        {
            foreach (var unit in map.Units)
            {
                CreateUnitObject(unit, map.GetPositionOfUnit(unit));
            }
        }

        public void CreateUnitObject(Unit unit, MapPosition mapPosition)
        {
            unitObjects.Add(unit.Id, InstantiateUnitObject(unit, mapPosition));
        }

        public IEnumerator MoveUnitObject(Unit unit, IEnumerable<MapPosition> path)
        {
            if (unitObjects.TryGetValue(unit.Id, out var unitObject))
            {
                foreach (var position in path)
                {
                    var destination = mapGrid.GetCoordinatesOfMapPosition(position);
                    yield return MoveTo(unitObject, destination);
                }
            }

            yield return null;
        }

        private static IEnumerator MoveTo(GameObject objectToMove, Vector3 destination)
        {
            var t = 0f;
            var startPosition = objectToMove.transform.position;
            var duration = 0.2f;
            while (t < duration)
            {
                objectToMove.transform.position = Vector3.Lerp(startPosition, destination, t / duration);
                t += Time.deltaTime;
                yield return null;
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