using System.Collections;
using FireEmblem.Domain.Combat;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class CombatAnimationInitiator : MonoBehaviour
    {
        [SerializeField] private CombatAnimation combatAnimation;
        
        [SerializeField] private CombatParticipantDisplay attackerDisplay;
        [SerializeField] private CombatParticipantDisplay defenderDisplay;

        public IEnumerator PlayCombatAnimation(CombatForecast combatForecast, CombatResult combatResult)
        {
            attackerDisplay.Display(combatForecast.Attacker);
            defenderDisplay.Display(combatForecast.Defender);

            yield return combatAnimation.AnimateCombatResult(combatResult);
        }
    }
}