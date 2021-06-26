using System;
using System.Collections.Generic;
using FireEmblem.Common;
using FireEmblem.Model.Map;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField] private GameObject movementRangeTilePrefab;
        
        private Map _map;

        private Grid _grid;

        private readonly List<GameObject> _highlightedTiles = new List<GameObject>();
        
        private void Awake()
        {
            _grid = GetComponent<Grid>();
        }

        public void SetMap(Map map)
        {
            _map = map;
        }

        public void SelectUnit(MapUnit mapUnit)
        {
            _selectedUnit = mapUnit;
            ClearMovementRange();
            ShowMovementRange(mapUnit);
        }
        
        private void ShowMovementRange(MapUnit mapUnit)
        {
            var tiles = MovementGenerator.GenerateAccessibleTiles(_map, mapUnit);

            foreach (var tile in tiles)
            {
                var tileObject = Instantiate(movementRangeTilePrefab, transform);
                tileObject.transform.position =
                    _grid.GetCellCenterWorld(new Vector3Int(tile.Position.X, tile.Position.Y, 0));
                _highlightedTiles.Add(tileObject);
            }
        }

        public void ClearMovementRange()
        {
            _highlightedTiles.ForEach(Destroy);
            _highlightedTiles.Clear();
        }
    }
}