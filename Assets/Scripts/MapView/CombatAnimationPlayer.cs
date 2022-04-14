using System.Collections;
using FireEmblem.CombatView;
using FireEmblem.Domain.Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FireEmblem.MapView
{
    public class CombatAnimationPlayer
    {
        private const string CombatSceneName = "Combat";
        
        public static IEnumerator PerformCombat(CombatForecast combatForecast)
        {
            var combatResult = Combat.ResolveCombat(combatForecast);
            
            yield return SceneManager.LoadSceneAsync(CombatSceneName, LoadSceneMode.Additive);

            var combatAnimationInitiator = Object.FindObjectOfType<CombatAnimationInitiator>();
            yield return combatAnimationInitiator.PlayCombatAnimation(combatForecast, combatResult);

            yield return SceneManager.UnloadSceneAsync(CombatSceneName);
        }
    }
}