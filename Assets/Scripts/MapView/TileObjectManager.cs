using System.Collections.Generic;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class TileObjectManager : MonoBehaviour
    {
        private readonly List<GameObject> _activeTileObjects = new List<GameObject>();

        private readonly List<GameObject> _activeMovePathTiles = new List<GameObject>();
        
        [SerializeField] private GameObject moveTilePrefab;
        [SerializeField] private GameObject attackTilePrefab;
        [SerializeField] private GameObject movePathTilePrefab;
        [SerializeField] private MapGrid grid;

        public void CreateMoveTile(MapPosition mapPosition)
        {
            CreateTile(moveTilePrefab, mapPosition);
        }

        public void CreateAttackTile(MapPosition mapPosition)
        {
            CreateTile(attackTilePrefab, mapPosition);
        }
        
        public void CreateMovePathTile(MapPosition mapPosition)
        {
            var tileObject = grid.InstantiateAtGridPosition(movePathTilePrefab, mapPosition);
            _activeMovePathTiles.Add(tileObject);
        }

        private void CreateTile(GameObject prefab, MapPosition mapPosition)
        {
            var tileObject = grid.InstantiateAtGridPosition(prefab, mapPosition);
            _activeTileObjects.Add(tileObject);
        }

        public void DestroyAll()
        {
            ClearMovePath();
            _activeTileObjects.ForEach(Destroy);
            _activeTileObjects.Clear();
        }

        public void ClearMovePath()
        {
            _activeMovePathTiles.ForEach(Destroy);
            _activeMovePathTiles.Clear();
        }
    }
}