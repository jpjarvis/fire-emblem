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
    [RequireComponent(typeof(Grid))]
    public class MapGrid : MonoBehaviour
    {
        private Tilemap tilemap;
        private Grid grid;

        private Dictionary<MapPosition, BaseUnit> positionsToUnitObject;
        private Dictionary<Unit, MapPosition> unitsToPosition;

        public IReadOnlyCollection<Unit> Units
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
            grid = GetComponent<Grid>();
        }

        private void Start()
        {
            EnsureUnitDictionariesAreInitialised();
        }

        private void EnsureUnitDictionariesAreInitialised()
        {
            if (positionsToUnitObject != null && unitsToPosition != null)
            {
                return;
            }
            
            var unitObjects = GetComponentsInChildren<BaseUnit>();
            positionsToUnitObject = unitObjects.ToDictionary(x => x.Position, x => x);
            unitsToPosition = unitObjects.ToDictionary(x => x.Unit, x => x.Position);
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
            return unitsToPosition.GetValueOrDefault(unit);
        }

        public void MoveUnit(Unit unit, MapPosition destination)
        {
            unitsToPosition.Remove(unit, out var startPosition);
            unitsToPosition.Add(unit, destination);

            positionsToUnitObject.Remove(startPosition, out var unitObject);
            positionsToUnitObject.Add(destination, unitObject);
            
            MoveObjectToGridPosition(unitObject.gameObject, destination);
        }
        
        public void MoveObjectToGridPosition(GameObject objectToMove, MapPosition position)
        {
            objectToMove.transform.position =
                grid.GetCellCenterLocal(new Vector3Int(position.X, position.Y, 0));
        }

        public GameObject InstantiateAtGridPosition(GameObject prefab, MapPosition position)
        {
            var instantiatedObject = Instantiate(prefab, transform);
            MoveObjectToGridPosition(instantiatedObject, position);
            return instantiatedObject;
        }

        private class EmptyMapTile : IMapTile
        {
            public bool IsTraversable => true;
        }
    }
}