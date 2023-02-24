using System;
using TMPro;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private HealthBar healthBar;
        
        private int _hp;
        private int _maxHp;
        
        public void Initialise(int currentHp, int maxHp)
        {
            _hp = currentHp;
            _maxHp = maxHp;
            hpText.text = currentHp.ToString();
            UpdateHpText();
            UpdateHealthBar();
        }

        public void TakeDamage(int damage)
        {
            _hp = Math.Max(_hp - damage, 0);
            UpdateHpText();
            UpdateHealthBar();
        }

        private void UpdateHpText()
        {
            hpText.text = _hp.ToString();
        }

        private void UpdateHealthBar()
        {
            healthBar.SetHealthProportion((float) _hp / _maxHp);
        }
    }
}