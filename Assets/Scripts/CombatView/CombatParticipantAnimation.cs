using System;
using System.Collections;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class CombatParticipantAnimation : MonoBehaviour
    {
        [SerializeField] private bool faceLeft;
        
        private IEnumerator MoveToSmooth(Vector3 position, float durationInSeconds)
        {
            var t = 0.0f;

            var startPosition = transform.localPosition;
            
            while (t < durationInSeconds)
            {
                t += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startPosition, position, Mathf.SmoothStep(0, 1, t / durationInSeconds));
                yield return null;
            }
        }

        public IEnumerator AttackAnimation()
        {
            var direction = faceLeft ? -1 : 1;
            yield return MoveToSmooth(new Vector2(direction * 100, 0), 0.2f);
            yield return MoveToSmooth(new Vector2(0, 0), 0.2f);
        }
    }
}