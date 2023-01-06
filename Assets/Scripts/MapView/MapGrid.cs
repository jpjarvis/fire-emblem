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

        public IEnumerable<BaseUnit> Units { get; private set; }
        
        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
            grid = GetComponent<Grid>();
            
            Units = GetComponentsInChildren<BaseUnit>().ToList();
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
            return Units.FirstOrDefault(x => x.Position == mapPosition)?.Unit;
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