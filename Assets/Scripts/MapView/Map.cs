using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Data;
using FireEmblem.MapView.UI;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private TileObjectManager tileObjectManager;
        [SerializeField] private MapGrid mapGrid;
        [SerializeField] private UnitStatsDisplay unitStatsDisplay;

        private PlayerUnit selectedUnit;
        private Dictionary<MapPosition, AccessibleTile> accessibleTiles;
        private MovementGenerator movementGenerator;

        private void Awake()
        {
            movementGenerator = new MovementGenerator(mapGrid);
        }
        
        private void SelectUnit(PlayerUnit unit)
        {
            selectedUnit = unit;

            if (unitStatsDisplay != null)
            {
                unitStatsDisplay.DisplayUnit(unit.Unit);
            }
            
            tileObjectManager.DestroyAll();
            
            accessibleTiles = movementGenerator.GenerateAccessibleTiles(unit);
            ShowAccessibleTiles(accessibleTiles);
        }

        public void SelectCell(MapPosition position)
        {
            if (selectedUnit != null)
            {
                var tileIsAccessible = accessibleTiles.TryGetValue(position, out var tile);
                if (tileIsAccessible && tile is { Accessibility: TileAccessibility.CanMoveTo })
                {
                    mapGrid.MoveObjectToGridPosition(selectedUnit.gameObject, position);
                    selectedUnit.HasActed = true;
                    if (mapGrid.Units.OfType<PlayerUnit>().All(x => x.HasActed))
                    {
                        TakeEnemyTurn();
                        StartPlayerTurn();
                    }
                }

                tileObjectManager.DestroyAll();
                selectedUnit = null;
                return;
            }

            var selectedPlayerUnit = mapGrid.Units.OfType<PlayerUnit>().FirstOrDefault(u => u.Position == position);
            if (selectedPlayerUnit && !selectedPlayerUnit.HasActed)
            {
                SelectUnit(selectedPlayerUnit);
            }
        }

        private void StartPlayerTurn()
        {
            foreach (var playerUnit in mapGrid.Units.OfType<PlayerUnit>())
            {
                playerUnit.HasActed = false;
            }
        }

        private void TakeEnemyTurn()
        {
            foreach (var enemyUnit in mapGrid.Units.OfType<EnemyUnit>())
            {
                var destination = enemyUnit.GetMoveDestination(mapGrid);
                mapGrid.MoveObjectToGridPosition(enemyUnit.gameObject, destination);
            }
        }

        private void ShowAccessibleTiles(Dictionary<MapPosition, AccessibleTile> accessibleTiles)
        {
            foreach (var position in accessibleTiles.Keys)
            {
                var tile = accessibleTiles[position];
                
                switch (tile.Accessibility)
                {
                    case TileAccessibility.CanMoveTo:
                        tileObjectManager.CreateMoveTile(position);
                        break;
                    case TileAccessibility.CanAttack:
                        tileObjectManager.CreateAttackTile(position);
                        break;
                }
            }
        }

        public void HighlightTile(MapPosition tile)
        {
            if (selectedUnit != null)
            {
                tileObjectManager.ClearMovePath();
                if (accessibleTiles.ContainsKey(tile))
                {
                    ShowPathToTile(tile);
                }
            }
        }

        private void ShowPathToTile(MapPosition targetPosition)
        {
            var currentPosition = targetPosition;

            var tilesInPath = new List<MapPosition>();
            
            while (currentPosition != null && currentPosition != selectedUnit.Position)
            {
                tilesInPath.Add(currentPosition);
                currentPosition = accessibleTiles[currentPosition].SourceTiles.FirstOrDefault();
            }

            foreach (var tile in tilesInPath)
            {
                tileObjectManager.CreateMovePathTile(tile);
            }
        }
    }
}