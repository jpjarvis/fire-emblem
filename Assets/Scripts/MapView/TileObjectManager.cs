using System.Collections.Generic;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class TileObjectManager : MonoBehaviour
    {
        private readonly List<GameObject> _activeTileObjects = new List<GameObject>();
        
        [SerializeField] private GameObject moveTilePrefab;
        [SerializeField] private Grid grid;

        public void CreateMoveTile(int x, int y)
        {
            var tileObject = Instantiate(moveTilePrefab, transform);
            tileObject.transform.position =
                grid.GetCellCenterWorld(new Vector3Int(x, y, 0));
            _activeTileObjects.Add(tileObject);
        }

        public void DestroyAll()
        {
            _activeTileObjects.ForEach(Destroy);
            _activeTileObjects.Clear();
        }
    }
}