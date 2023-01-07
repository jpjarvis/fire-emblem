using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireEmblem.MapView
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(MapGrid))]
    public class Map : MonoBehaviour
    {
        private Tilemap tilemap;
        private MapGrid grid;

        private Dictionary<MapPosition, UnitObject> positionsToUnitObject;
        private Dictionary<Guid, MapPosition> unitIdsToPosition;

        public IEnumerable<Unit> Units
        {
            get
            {
                EnsureUnitDictionariesAreInitialised();
                return positionsToUnitObject?.Values.Select(x => x.Unit).ToList();
            }
        }

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            grid = GetComponent<MapGrid>();
        }

        private void Start()
        {
            EnsureUnitDictionariesAreInitialised();
        }

        private void EnsureUnitDictionariesAreInitialised()
        {
            if (positionsToUnitObject != null && unitIdsToPosition != null)
            {
                return;
            }
            
            var unitObjects = GetComponentsInChildren<UnitObject>();
            positionsToUnitObject = unitObjects.ToDictionary(x => x.Position, x => x);
            unitIdsToPosition = unitObjects.ToDictionary(x => x.Unit.Id, x => x.Position);
        }
        
        public IMapTile GetTileAt(MapPosition mapPosition)
        {
            var tile = tilemap.GetTile<MapTile>(new Vector3Int(mapPosition.X, mapPosition.Y, 0));

            if (tile)
            {
                return tile;
            }

            return new EmptyMapTile();
        }

        [CanBeNull]
        public Unit GetUnitAt(MapPosition mapPosition)
        {
            return positionsToUnitObject.GetValueOrDefault(mapPosition)?.Unit;
        }

        [CanBeNull]
        public MapPosition GetPositionOfUnit(Unit unit)
        {
            return unitIdsToPosition.GetValueOrDefault(unit.Id);
        }

        public void MoveUnit(Unit unit, MapPosition destination)
        {
            unitIdsToPosition.Remove(unit.Id, out var startPosition);
            unitIdsToPosition.Add(unit.Id, destination);

            positionsToUnitObject.Remove(startPosition, out var unitObject);
            positionsToUnitObject.Add(destination, unitObject);
            
            grid.MoveObjectToGridPosition(unitObject.gameObject, destination);
        }

        public void UpdateUnit(Unit unit)
        {
            var unitPosition = unitIdsToPosition.GetValueOrDefault(unit.Id);
            if (unitPosition == null)
            {
                throw new ArgumentException($"The unit ${unit.Name} with id {unit.Id} does not exist on the map.");
            }

            var unitObject = positionsToUnitObject.GetValueOrDefault(unitPosition);
            unitObject.UpdateUnit(unit);
        }
        
        public void RemoveUnit(Unit unit)
        {
            unitIdsToPosition.Remove(unit.Id, out var unitPosition);
            positionsToUnitObject.Remove(unitPosition, out var unitObject);
            Destroy(unitObject.gameObject);
        }

        private class EmptyMapTile : IMapTile
        {
            public bool IsTraversable => true;
        }
    }
}