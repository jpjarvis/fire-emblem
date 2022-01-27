using System.Collections.Generic;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class TileObjectManager : MonoBehaviour
    {
        private readonly List<GameObject> _activeTileObjects = new List<GameObject>();
        
        [SerializeField] private GameObject moveTilePrefab;
        [SerializeField] private MapGrid grid;

        public void CreateMoveTile(MapPosition mapPosition)
        {
            var tileObject = grid.InstantiateAtGridPosition(moveTilePrefab, mapPosition);
            _activeTileObjects.Add(tileObject);
        }

        public void DestroyAll()
        {
            _activeTileObjects.ForEach(Destroy);
            _activeTileObjects.Clear();
        }
    }
}