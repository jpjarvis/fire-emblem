using FireEmblem.Domain.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireEmblem.MapView
{
    [CreateAssetMenu(fileName = "NewMapTile", menuName = "Fire Emblem/MapTile", order = 1)]
    public class MapTile : TileBase, IMapTile
    {
        [SerializeField] private bool traversable;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject gameObject;

        public bool IsTraversable => traversable;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (sprite != null)
            {
                tileData.sprite = sprite;
            }

            if (gameObject != null)
            {
                tileData.gameObject = gameObject;
            }
            
            base.GetTileData(position, tilemap, ref tileData);
        }
    }
}