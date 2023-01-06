using System.Collections.Generic;
using System.Linq;
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
        
        private readonly List<GameObject> activeTileObjects = new();
        private readonly List<GameObject> activeMovePathTiles = new();
        [CanBeNull] private MapPosition shownSourceTile;
        [CanBeNull] private Dictionary<MapPosition, AccessibleTile> shownAccessibleTiles;
        
        public void ShowMovementRange(MapPosition sourceTile, Dictionary<MapPosition, AccessibleTile> accessibleTiles)
        {
            shownSourceTile = sourceTile;
            shownAccessibleTiles = accessibleTiles;
            
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
        }

        public void ShowPathTo(MapPosition targetPosition)
        {
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
        
        public void Clear()
        {
            ClearMovePath();
            activeTileObjects.ForEach(Destroy);
            activeTileObjects.Clear();
            shownSourceTile = null;
            shownAccessibleTiles = null;
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
            var tileObject = map.InstantiateAtGridPosition(movePathTilePrefab, mapPosition);
            activeMovePathTiles.Add(tileObject);
        }

        private void CreateTile(GameObject prefab, MapPosition mapPosition)
        {
            var tileObject = map.InstantiateAtGridPosition(prefab, mapPosition);
            activeTileObjects.Add(tileObject);
        }
    }
}