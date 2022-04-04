using System;
using System.Collections;
using FireEmblem.Domain.Combat;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class CombatAnimator : MonoBehaviour
    {
        [SerializeField] private CombatParticipantAnimation attackerAnimation;
        [SerializeField] private CombatParticipantAnimation defenderAnimation;

        public IEnumerator AnimateCombatResult(CombatResult combatResult)
        {
            foreach (var attackResult in combatResult.AttackResults)
            {
                if (attackResult.IsFromAttacker)
                {
                    yield return attackerAnimation.AttackAnimation();
                }
                else
                {
                    yield return defenderAnimation.AttackAnimation();
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}