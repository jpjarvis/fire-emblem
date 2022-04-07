using System.Collections;
using FireEmblem.Domain.Combat;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class CombatAnimator : MonoBehaviour
    {
        [SerializeField] private CombatParticipantAnimation attackerAnimation;
        [SerializeField] private CombatParticipantAnimation defenderAnimation;

        [SerializeField] private HealthDisplay attackerHealthDisplay;
        [SerializeField] private HealthDisplay defenderHealthDisplay;

        public IEnumerator AnimateCombatResult(CombatResult combatResult)
        {
            foreach (var attackResult in combatResult.AttackResults)
            {
                if (attackResult.IsFromAttacker)
                {
                    yield return attackerAnimation.AttackAnimation(() =>
                        defenderHealthDisplay.TakeDamage(attackResult.Damage));
                }
                else
                {
                    yield return defenderAnimation.AttackAnimation(() =>
                        attackerHealthDisplay.TakeDamage(attackResult.Damage));
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}