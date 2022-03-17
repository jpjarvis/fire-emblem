using FireEmblem.Domain.Data;

namespace FireEmblem.Domain.Combat
{
    public class CombatForecast
    {
        private CombatForecast(CombatForecastParticipant attacker, CombatForecastParticipant defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public CombatForecastParticipant Attacker { get; }
        public CombatForecastParticipant Defender { get; }

        public static CombatForecast Create(Unit attacker, Unit defender, int distanceBetween)
        {
            var attackerCanAttack = attacker.Weapon.Data.IsInRange(distanceBetween);
            var defenderCanAttack = defender.Weapon.Data.IsInRange(distanceBetween);

            var numberOfAttackerAttacks = attackerCanAttack ? attacker.CanFollowUp(defender) ? 2 : 1 : 0;
            var numberOfDefenderAttacks = defenderCanAttack ? defender.CanFollowUp(attacker) ? 2 : 1 : 0;

            return new CombatForecast(
                new CombatForecastParticipant(
                    attacker,
                    CombatForecastAttack.Create(attacker, defender),
                    numberOfAttackerAttacks
                    ),
                new CombatForecastParticipant(
                    defender,
                    CombatForecastAttack.Create(defender, attacker),
                    numberOfDefenderAttacks
                )
            );
        }
    }
}