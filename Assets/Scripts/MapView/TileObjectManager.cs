using System.Collections.Generic;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class TileObjectManager : MonoBehaviour
    {
        private readonly List<GameObject> _activeTileObjects = new List<GameObject>();
        
        [SerializeField] private GameObject moveTilePrefab;
        [SerializeField] private GameObject attackTilePrefab;
        [SerializeField] private MapGrid grid;

        public void CreateMoveTile(MapPosition mapPosition)
        {
            CreateTile(moveTilePrefab, mapPosition);
        }

        public void CreateAttackTile(MapPosition mapPosition)
        {
            CreateTile(attackTilePrefab, mapPosition);
        }

        private void CreateTile(GameObject prefab, MapPosition mapPosition)
        {
            var tileObject = grid.InstantiateAtGridPosition(prefab, mapPosition);
            _activeTileObjects.Add(tileObject);
        }

        public void DestroyAll()
        {
            _activeTileObjects.ForEach(Destroy);
            _activeTileObjects.Clear();
        }
    }
}