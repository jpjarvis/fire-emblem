using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class TileObjectManager : MonoBehaviour
    {
        private readonly List<GameObject> _activeTileObjects = new List<GameObject>();
        private ITilePrefabProvider _tilePrefabProvider;
        private Grid _grid;

        [Inject]
        public void Init(ITilePrefabProvider tilePrefabProvider, Grid grid)
        {
            _tilePrefabProvider = tilePrefabProvider;
            _grid = grid;
        }

        public void CreateMoveTile(int x, int y, Action onClick)
        {
            var tileObject = Instantiate(_tilePrefabProvider.GetMoveTilePrefab(), transform);
            tileObject.transform.position =
                _grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            tileObject.GetComponent<MouseHandler>().ActionOnClick = onClick;
            _activeTileObjects.Add(tileObject);
        }

        public void DestroyAll()
        {
            _activeTileObjects.ForEach(Destroy);
            _activeTileObjects.Clear();
        }
    }
}