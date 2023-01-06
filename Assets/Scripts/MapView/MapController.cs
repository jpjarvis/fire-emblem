using System;
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
                if (tileIsAccessible)
                {
                    if (tile is { Accessibility: TileAccessibility.CanMoveTo })
                    {
                        map.MoveUnit(selectedUnit, position);
                    }
                    else if (tile is { Accessibility: TileAccessibility.CanAttack })
                    {
                        var targetUnit = map.GetUnitAt(position);
                        if (targetUnit != null && targetUnit.Allegiance != Allegiance.Player)
                        {
                            map.MoveUnit(selectedUnit, tile.SourceTiles.First());
                            InitiateCombat(selectedUnit, targetUnit);
                        }
                    }
                    
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

        private void InitiateCombat(Unit attacker, Unit defender)
        {
            var combatForecast = CombatForecast.Create(attacker, defender, 1);
            var combatResult = Combat.ResolveCombat(combatForecast);
            HandleCombatResult(combatResult);
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

        private void HandleCombatResult(CombatResult combatResult)
        {
            Debug.Log(combatResult.ToText());
            
            if (combatResult.Attacker.IsDead())
            {
                map.RemoveUnit(combatResult.Attacker);
            }
            else
            {
                map.UpdateUnit(combatResult.Attacker);
            }

            if (combatResult.Defender.IsDead())
            {
                map.RemoveUnit(combatResult.Defender);
            }
            else
            {
                map.UpdateUnit(combatResult.Defender);
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
                var enemyAction = EnemyAi.GetAction(enemyUnit, map);

                switch (enemyAction)
                { 
                    case MoveAndAttackAction a:
                        map.MoveUnit(enemyUnit, a.PositionToMoveTo);
                        InitiateCombat(enemyUnit, a.UnitToAttack);
                        break;
                    case NoAction:
                        break;
                }
            }
        }
    }
}