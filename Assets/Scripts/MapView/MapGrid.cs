using FireEmblem.Domain.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public abstract class MapGrid : MonoBehaviour
    {
        public abstract IMapTile GetTileAt(MapPosition mapPosition);

        public abstract void MoveObjectToGridPosition(GameObject objectToMove, MapPosition position);

        public abstract GameObject InstantiateAtGridPosition(GameObject prefab, MapPosition position);
    }
}