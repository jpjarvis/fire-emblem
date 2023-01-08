using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using FireEmblem.Domain.Map;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireEmblem.MapView
{
    [RequireComponent(typeof(Tilemap))]
    public class Map : MonoBehaviour, IMap
    {
        private Tilemap tilemap;
        
        private Dictionary<MapPosition, Unit> positionsToUnit;
        private Dictionary<Guid, MapPosition> unitIdsToPosition;

        public IEnumerable<Unit> Units
        {
            get
            {
                EnsureUnitDictionariesAreInitialised();
                return positionsToUnit.Values.ToList();
            }
        }

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        private void Start()
        {
            EnsureUnitDictionariesAreInitialised();
        }

        private void EnsureUnitDictionariesAreInitialised()
        {
            if (positionsToUnit != null && unitIdsToPosition != null)
            {
                return;
            }
            
            var unitObjects = GetComponentsInChildren<UnitObject>();
            positionsToUnit = unitObjects.ToDictionary(x => x.Position, x => x.Unit);
            unitIdsToPosition = unitObjects.ToDictionary(x => x.Unit.Id, x => x.Position);
            foreach (var unitObject in unitObjects)
            {
                Destroy(unitObject.gameObject);
            }
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
            return positionsToUnit.GetValueOrDefault(mapPosition);
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

            positionsToUnit.Remove(startPosition, out _);
            positionsToUnit.Add(destination, unit);
        }

        public void UpdateUnit(Unit unit)
        {
            var unitPosition = unitIdsToPosition.GetValueOrDefault(unit.Id);
            if (unitPosition == null)
            {
                throw new ArgumentException($"The unit ${unit.Name} with id {unit.Id} does not exist on the map.");
            }

            positionsToUnit[unitPosition] = unit;
        }
        
        public void RemoveUnit(Unit unit)
        {
            unitIdsToPosition.Remove(unit.Id, out var unitPosition);
            positionsToUnit.Remove(unitPosition, out _);
        }

        private class EmptyMapTile : IMapTile
        {
            public bool IsTraversable => true;
        }
    }
}