using System;
using TMPro;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpText;

        private int _hp;
        private int _maxHp;
        
        public void Initialise(int currentHp, int maxHp)
        {
            _hp = currentHp;
            _maxHp = maxHp;
            hpText.text = currentHp.ToString();
        }

        public void TakeDamage(int damage)
        {
            _hp = Math.Max(_hp - damage, 0);
            UpdateHpText();
        }

        private void UpdateHpText()
        {
            hpText.text = _hp.ToString();
        }
    }
}