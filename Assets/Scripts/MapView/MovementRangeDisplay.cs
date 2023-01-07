using System;
using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using JetBrains.Annotations;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MovementRangeDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject moveTilePrefab;
        [SerializeField] private GameObject attackTilePrefab;
        [SerializeField] private GameObject movePathTilePrefab;
        [SerializeField] private Map map;
        [SerializeField] private MapGrid grid;
        
        private readonly List<GameObject> activeTileObjects = new();
        private readonly List<GameObject> activeMovePathTiles = new();
        [CanBeNull] private MapPosition shownSourceTile;
        private Dictionary<MapPosition, AccessibleTile> shownAccessibleTiles;

        private MovementGenerator movementGenerator;
        
        private void Awake()
        {
            movementGenerator = new MovementGenerator(map);
            shownAccessibleTiles = new Dictionary<MapPosition, AccessibleTile>();
        }

        public void ShowMovementRange(Unit unit)
        {
            shownSourceTile = map.GetPositionOfUnit(unit);
            var accessibleTiles = movementGenerator.GenerateAccessibleTiles(unit);
            
            foreach (var position in accessibleTiles.Keys)
            {
                var tile = accessibleTiles[position];
                
                switch (tile.Accessibility)
                {
                    case TileAccessibility.CanMoveTo:
                        CreateMoveTile(position);
                        break;
                    case TileAccessibility.CanAttack:
                        CreateAttackTile(position);
                        break;
                }
            }

            shownAccessibleTiles = accessibleTiles;
        }

        public void ShowPathTo(MapPosition targetPosition)
        {
            if (!shownAccessibleTiles.TryGetValue(targetPosition, out _))
            {
                return;
            }
            
            var currentPosition = targetPosition;

            var tilesInPath = new List<MapPosition>();
            
            while (currentPosition != null && currentPosition != shownSourceTile)
            {
                tilesInPath.Add(currentPosition);
                currentPosition = shownAccessibleTiles[currentPosition].SourceTiles.FirstOrDefault();
            }

            foreach (var tile in tilesInPath)
            {
                CreateMovePathTile(tile);
            }
        }

        [CanBeNull]
        public AccessibleTile GetAccessibleTile(MapPosition position)
        {
            return shownAccessibleTiles?.GetValueOrDefault(position);
        }
        
        public void Clear()
        {
            ClearMovePath();
            activeTileObjects.ForEach(Destroy);
            activeTileObjects.Clear();
            shownSourceTile = null;
            shownAccessibleTiles.Clear();
        }

        public void ClearMovePath()
        {
            activeMovePathTiles.ForEach(Destroy);
            activeMovePathTiles.Clear();
        }

        private void CreateMoveTile(MapPosition mapPosition)
        {
            CreateTile(moveTilePrefab, mapPosition);
        }

        private void CreateAttackTile(MapPosition mapPosition)
        {
            CreateTile(attackTilePrefab, mapPosition);
        }

        private void CreateMovePathTile(MapPosition mapPosition)
        {
            var tileObject = grid.InstantiateAtGridPosition(movePathTilePrefab, mapPosition);
            activeMovePathTiles.Add(tileObject);
        }

        private void CreateTile(GameObject prefab, MapPosition mapPosition)
        {
            var tileObject = grid.InstantiateAtGridPosition(prefab, mapPosition);
            activeTileObjects.Add(tileObject);
        }
    }
}