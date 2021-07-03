using System;
using System.Collections.Generic;
using FireEmblem.Common;
using FireEmblem.Model.Map;
using UnityEngine;
using Zenject;

namespace FireEmblem.MapView
{
    public class MapManager : Singleton<MapManager>
    {
        private TileObjectManager _tileObjectManager;
        private Map _map;
        private IMapGenerator _mapGenerator;

        private MapUnit _selectedUnit = null;
        
        [Inject]
        public void Init(TileObjectManager tileObjectManager, Map map, IMapGenerator mapGenerator)
        {
            _tileObjectManager = tileObjectManager;
            _map = map;
            _mapGenerator = mapGenerator;
        }

        private void Start()
        {
            _mapGenerator.GenerateMap(_map);
        }

        public void SelectUnit(MapUnit mapUnit)
        {
            _selectedUnit = mapUnit;
            _tileObjectManager.DestroyAll();
            ShowMovementRange(mapUnit);
        }
        
        private void ShowMovementRange(MapUnit mapUnit)
        {
            var tiles = MovementGenerator.GenerateAccessibleTiles(_map, mapUnit);

            foreach (var tile in tiles)
            {
                _tileObjectManager.CreateMoveTile(tile.Position.X, tile.Position.Y);
            }
        }
    }
}