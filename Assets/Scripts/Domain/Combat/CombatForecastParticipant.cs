namespace FireEmblem.Domain.Combat
{
    public class CombatForecastParticipant
    {
        public CombatForecastParticipant(Unit unit, CombatForecastAttack attack, int numberOfAttacks)
        {
            Unit = unit;
            Attack = attack;
            NumberOfAttacks = numberOfAttacks;
        }

        public Unit Unit { get; }
        public CombatForecastAttack Attack { get; }
        public int NumberOfAttacks { get; }
    }
}