using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireEmblem.MapView
{
    [CreateAssetMenu(fileName = "NewMapTile", menuName = "Fire Emblem/MapTile", order = 1)]
    public class MapTile : TileBase
    {
        [SerializeField] private bool traversable;
        [SerializeField] private Sprite sprite;

        public bool IsTraversable => traversable;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = sprite;
            base.GetTileData(position, tilemap, ref tileData);
        }
    }
}