using System;
using UnityEngine;
using UnityEngine.UI;

namespace FireEmblem.CombatView
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image barImage;
        [SerializeField] private Color lowHealthColor;

        private Color _normalColor;
        
        private void Awake()
        {
            _normalColor = barImage.color;
        }

        public void SetHealthProportion(float proportion)
        {
            barImage.fillAmount = proportion;
            barImage.color = proportion < 0.2 ? lowHealthColor : _normalColor;
        }
    }
}