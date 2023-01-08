using System.Collections;
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
        [SerializeField] private UnitObjectManager unitObjectManager;

        [CanBeNull] private Unit selectedUnit;

        private List<Unit> unitsToMove;
        
        private void Awake()
        {
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
                StartCoroutine(MoveUnit(selectedUnit, position));
                selectedUnit = null;
                return;
            }

            var selectedPlayerUnit = map.GetUnitAt(position);
            if (selectedPlayerUnit != null && unitsToMove.Contains(selectedPlayerUnit))
            {
                SelectUnit(selectedPlayerUnit);
            }
        }

        private IEnumerator MoveUnit(Unit unit, MapPosition position)
        {
            var tile = movementRangeDisplay.GetAccessibleTile(position);
            movementRangeDisplay.Clear();
            if (tile != null)
            {
                switch (tile.Accessibility)
                {
                    case TileAccessibility.CanMoveTo:
                        yield return unitObjectManager.MoveUnitObject(unit, movementRangeDisplay.GetMovePath());
                        map.MoveUnit(unit, position);
                        break;
                    case TileAccessibility.CanAttack:
                    {
                        var targetUnit = map.GetUnitAt(position);
                        if (targetUnit != null && targetUnit.Allegiance != Allegiance.Player)
                        {
                            yield return unitObjectManager.MoveUnitObject(unit,
                                movementRangeDisplay.GetMovePath());
                            map.MoveUnit(unit, tile.SourceTiles.First());
                            InitiateCombat(unit, targetUnit);
                        }

                        break;
                    }
                }

                unitsToMove.Remove(unit);
                if (!unitsToMove.Any())
                {
                    yield return TakeEnemyTurn();
                    StartPlayerTurn();
                }
            }
            
            yield return null;
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
                movementRangeDisplay.ShowPathTo(position);
            }
        }

        private void HandleCombatResult(CombatResult combatResult)
        {
            Debug.Log(combatResult.ToText());
            
            if (combatResult.Attacker.IsDead())
            {
                map.RemoveUnit(combatResult.Attacker);
                unitObjectManager.RemoveUnitObject(combatResult.Attacker);
            }
            else
            {
                map.UpdateUnit(combatResult.Attacker);
            }

            if (combatResult.Defender.IsDead())
            {
                map.RemoveUnit(combatResult.Defender);
                unitObjectManager.RemoveUnitObject(combatResult.Defender);
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
            movementRangeDisplay.ShowMovementRange(MovementGenerator.GetMovementRange(unit, map));
        }

        private void StartPlayerTurn()
        {
            foreach (var playerUnit in map.Units.Where(x => x.Allegiance == Allegiance.Player))
            {
                unitsToMove.Add(playerUnit);
            }
        }

        private IEnumerator TakeEnemyTurn()
        {
            foreach (var enemyUnit in map.Units.Where(x => x.Allegiance == Allegiance.Enemy))
            {
                var enemyAction = EnemyAi.GetAction(enemyUnit, map);

                switch (enemyAction)
                { 
                    case MoveAndAttackAction a:
                        yield return unitObjectManager.MoveUnitObject(enemyUnit, a.MovementPath);
                        map.MoveUnit(enemyUnit, a.MovementPath.Last());
                        InitiateCombat(enemyUnit, a.UnitToAttack);
                        break;
                    case NoAction:
                        break;
                }
            }
        }
    }
}