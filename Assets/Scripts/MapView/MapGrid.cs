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
            return _tilemap.GetTile<MapTile>(new Vector3Int(mapPosition.X, mapPosition.Y, 0));
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
    }
}