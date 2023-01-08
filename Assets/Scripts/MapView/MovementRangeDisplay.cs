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
        [SerializeField] private MapGrid grid;
        
        private readonly List<GameObject> activeTileObjects = new();
        private readonly List<GameObject> activeMovePathTiles = new();
        [CanBeNull] private MovementRange shownMovementRange;
        private IEnumerable<MapPosition> shownMovePath;
        
        private void Awake()
        {
            shownMovePath = new List<MapPosition>();
        }

        public void ShowMovementRange(MovementRange movementRange)
        {
            foreach (var tile in movementRange.GetAllAccessibleTiles())
            {
                switch (tile.Accessibility)
                {
                    case TileAccessibility.CanMoveTo:
                        CreateMoveTile(tile.Position);
                        break;
                    case TileAccessibility.CanAttack:
                        CreateAttackTile(tile.Position);
                        break;
                }
            }

            shownMovementRange = movementRange;
        }

        public void ShowPathTo(MapPosition targetPosition)
        {
            var accessibleTile = shownMovementRange?.GetAccessibleTile(targetPosition);
            if (accessibleTile == null)
            {
                return;
            }
            
            var tilesInPath = shownMovementRange.GetPathTo(targetPosition).ToList();

            foreach (var tile in tilesInPath)
            {
                CreateMovePathTile(tile);
            }
            
            shownMovePath = tilesInPath;
        }
        
        [CanBeNull]
        public AccessibleTile GetAccessibleTile(MapPosition position)
        {
            return shownMovementRange?.GetAccessibleTile(position);
        }

        public IEnumerable<MapPosition> GetMovePath()
        {
            return shownMovePath;
        }

        public void Clear()
        {
            ClearMovePath();
            activeTileObjects.ForEach(Destroy);
            activeTileObjects.Clear();
            shownMovementRange = null;
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