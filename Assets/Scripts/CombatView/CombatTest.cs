using FireEmblem.Model.Combat;
using FireEmblem.Model.Data;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class CombatTest : MonoBehaviour
    {
        [SerializeField] private UnitData attackerData;
        [SerializeField] private UnitData defenderData;
        private void Start()
        {
            var combat = new Combat();
            var attacker = Unit.Create(attackerData);
            var defender = Unit.Create(defenderData);

            var combatForecast = CombatForecast.Create(attacker, defender, 1);
            
            Debug.Log($"{combatForecast.Attacker.Unit.Name} vs {combatForecast.Defender.Unit.Name}");
            
            var combatResult = combat.ResolveCombat(combatForecast);

            foreach (var attack in combatResult.AttackResults)
            {
                Debug.Log($"{attack.Attacker.Name} attacks!");
                if (attack.IsHit)
                {
                    Debug.Log($"{attack.Damage} damage!");
                }
                else
                {
                    Debug.Log($"Missed!");
                }
                
            }
        }
    }
}