using System.Collections.Generic;
using System.Linq;
using FireEmblem.Domain.Combat;
using FireEmblem.Domain.Data;
using FireEmblem.MapView.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace FireEmblem.MapView
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private MovementRangeDisplay movementRangeDisplay;
        [SerializeField] private Map map;
        [SerializeField] private UnitStatsDisplay unitStatsDisplay;

        [CanBeNull] private Unit selectedUnit;
        private Dictionary<MapPosition, AccessibleTile> accessibleTiles;
        private MovementGenerator movementGenerator;

        private List<Unit> unitsToMove;
        
        private void Awake()
        {
            movementGenerator = new MovementGenerator(map);
            unitsToMove = new List<Unit>();
        }

        private void Start()
        {
            StartPlayerTurn();
        }

        public void SelectCell(MapPosition position)
        {
            if (selectedUnit != null)
            {
                var tileIsAccessible = accessibleTiles.TryGetValue(position, out var tile);
                if (tileIsAccessible && tile is { Accessibility: TileAccessibility.CanMoveTo })
                {
                    map.MoveUnit(selectedUnit, position);
                    unitsToMove.Remove(selectedUnit);
                    if (!unitsToMove.Any())
                    {
                        TakeEnemyTurn();
                        StartPlayerTurn();
                    }
                }

                movementRangeDisplay.Clear();
                selectedUnit = null;
                return;
            }

            var selectedPlayerUnit = map.GetUnitAt(position);
            if (selectedPlayerUnit != null && unitsToMove.Contains(selectedPlayerUnit))
            {
                SelectUnit(selectedPlayerUnit);
            }
        }
        
        public void HighlightCell(MapPosition position)
        {
            if (selectedUnit != null)
            {
                movementRangeDisplay.ClearMovePath();
                if (accessibleTiles.ContainsKey(position))
                {
                    movementRangeDisplay.ShowPathTo(position);
                }
            }
        }
        
        private void SelectUnit(Unit unit)
        {
            if (unit.Allegiance != Allegiance.Player)
            {
                return;
            }
            
            selectedUnit = unit;

            if (unitStatsDisplay != null)
            {
                unitStatsDisplay.DisplayUnit(unit);
            }
            
            movementRangeDisplay.Clear();
            
            accessibleTiles = movementGenerator.GenerateAccessibleTiles(unit);
            movementRangeDisplay.ShowMovementRange(map.GetPositionOfUnit(unit), accessibleTiles);
        }

        private void StartPlayerTurn()
        {
            foreach (var playerUnit in map.Units.Where(x => x.Allegiance == Allegiance.Player))
            {
                unitsToMove.Add(playerUnit);
            }
        }

        private void TakeEnemyTurn()
        {
            foreach (var enemyUnit in map.Units.Where(x => x.Allegiance == Allegiance.Enemy))
            {
                var destination = EnemyAi.GetMoveDestination(enemyUnit, map);
                map.MoveUnit(enemyUnit, destination);
            }
        }
    }
}