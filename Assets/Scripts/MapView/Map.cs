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
        private List<PlayerUnit> PlayerUnits { get; set; } = new();
        private List<EnemyUnit> EnemyUnits { get; set; } = new();

        private PlayerUnit selectedUnit;
        private Dictionary<MapPosition, AccessibleTile> accessibleTiles;
        private MovementGenerator movementGenerator;

        private void Awake()
        {
            PlayerUnits = GetComponentsInChildren<PlayerUnit>().ToList();
            EnemyUnits = GetComponentsInChildren<EnemyUnit>().ToList();

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
            
            accessibleTiles = movementGenerator.GenerateAccessibleTiles(unit, EnemyUnits);
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
                    if (PlayerUnits.All(x => x.HasActed))
                    {
                        foreach (var enemyUnit in EnemyUnits)
                        {
                            var destination = enemyUnit.GetMoveDestination(enemyUnit, PlayerUnits, movementGenerator);
                            mapGrid.MoveObjectToGridPosition(enemyUnit.gameObject, destination);
                            
                        }
                        
                        PlayerUnits.ForEach(x => x.HasActed = false);
                    }
                }

                tileObjectManager.DestroyAll();
                selectedUnit = null;
                return;
            }

            var selectedPlayerUnit = PlayerUnits.FirstOrDefault(u => u.Position == position);
            if (selectedPlayerUnit && !selectedPlayerUnit.HasActed)
            {
                SelectUnit(selectedPlayerUnit);
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