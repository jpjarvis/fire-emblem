using FireEmblem.Domain.Combat;
using TMPro;
using UnityEngine;

namespace FireEmblem.CombatView
{
    public class CombatParticipantDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private TextMeshProUGUI mightText;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI hitText;
        [SerializeField] private TextMeshProUGUI critText;

        [SerializeField] private GameObject doubleIcon;

        private void Awake()
        {
            doubleIcon.SetActive(false);
        }

        public void Display(CombatForecastParticipant combatForecastParticipant)
        {
            unitNameText.text = combatForecastParticipant.Unit.Name;
            mightText.text = combatForecastParticipant.Attack.Damage.ToString();
            hpText.text = combatForecastParticipant.Unit.Hp.ToString();
            hitText.text = combatForecastParticipant.Attack.HitChance.ToString();
            critText.text = combatForecastParticipant.Attack.CritChance.ToString();

            if (combatForecastParticipant.NumberOfAttacks > 1)
            {
                doubleIcon.SetActive(true);
            }
        }
    }
}
