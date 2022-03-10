using FireEmblem.Model.Combat;
using TMPro;
using UnityEngine;

namespace FireEmblem.MapView.UI
{
    public class UnitStatsDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        
        [Header("Combat stats")]
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI attackSpeedText;
        [SerializeField] private TextMeshProUGUI hitText;
        [SerializeField] private TextMeshProUGUI avoidText;
        [SerializeField] private TextMeshProUGUI critText;
        
        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI maxHpText;
        [SerializeField] private TextMeshProUGUI currentHpText;
        [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private TextMeshProUGUI magicText;
        [SerializeField] private TextMeshProUGUI dexterityText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI defenceText;
        [SerializeField] private TextMeshProUGUI resText;
        [SerializeField] private TextMeshProUGUI luckText;
        [SerializeField] private TextMeshProUGUI moveText;
        
        public void DisplayUnit(Unit unit)
        {
            TrySetText(unitNameText, unit.Name);
            TrySetText(weaponNameText, unit.Weapon.Name);
            
            TrySetText(attackText, unit.Attack);
            TrySetText(attackSpeedText, unit.AttackSpeed);
            TrySetText(hitText, unit.Hit);
            TrySetText(avoidText, unit.Avoid);
            TrySetText(critText, unit.Crit);
            
            TrySetText(currentHpText, unit.Hp);
            TrySetText(maxHpText, unit.MaxHp);
            
            TrySetText(strengthText, unit.Stats.Strength);
            TrySetText(magicText, unit.Stats.Magic);
            TrySetText(dexterityText, unit.Stats.Dexterity);
            TrySetText(speedText, unit.Stats.Speed);
            TrySetText(defenceText, unit.Stats.Defence);
            TrySetText(resText, unit.Stats.Resistance);
            TrySetText(luckText, unit.Stats.Luck);
            TrySetText(moveText, unit.Stats.Movement);
        }

        private void TrySetText(TextMeshProUGUI textObject, object text)
        {
            if (textObject == null) return;
            textObject.text = text.ToString();
        }
    }
}
