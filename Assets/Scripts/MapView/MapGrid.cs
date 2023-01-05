using FireEmblem.Domain.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireEmblem.MapView
{
    [RequireComponent(typeof(Tilemap))]
    [RequireComponent(typeof(Grid))]
    public class MapGrid : MonoBehaviour
    {
        private Tilemap _tilemap;
        private Grid _grid;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            _grid = GetComponent<Grid>();
        }

        public IMapTile GetTileAt(MapPosition mapPosition)
        {
            var tile = _tilemap.GetTile<MapTile>(new Vector3Int(mapPosition.X, mapPosition.Y, 0));

            if (tile)
            {
                return tile;
            }

            return new EmptyMapTile();
        }

        public void MoveObjectToGridPosition(GameObject objectToMove, MapPosition position)
        {
            objectToMove.transform.position =
                _grid.GetCellCenterLocal(new Vector3Int(position.X, position.Y, 0));
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