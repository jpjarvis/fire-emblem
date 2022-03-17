using FireEmblem.Data;
using FireEmblem.Domain.Combat;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class CombatBootstrapper : MonoBehaviour
    {
        [SerializeField] private UnitData attackerData;
        [SerializeField] private UnitData defenderData;

        [SerializeField] private CombatParticipantDisplay attackerDisplay;
        [SerializeField] private CombatParticipantDisplay defenderDisplay;
        
        private void Start()
        {
            var attacker = Unit.Create(attackerData);
            var defender = Unit.Create(defenderData);
            
            var combatForecast = CombatForecast.Create(attacker, defender, 1);

            attackerDisplay.Display(combatForecast.Attacker);
            defenderDisplay.Display(combatForecast.Defender);
        }
    }
}