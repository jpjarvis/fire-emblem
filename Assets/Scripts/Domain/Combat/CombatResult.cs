using System.Collections.Generic;

namespace FireEmblem.Domain.Combat
{
    public class CombatResult
    {
        public CombatResult(Unit attacker, Unit defender, IEnumerable<AttackResult> attackResults)
        {
            Attacker = attacker;
            Defender = defender;
            AttackResults = attackResults;
        }

        public Unit Attacker { get; }
        public Unit Defender { get; }
        
        public IEnumerable<AttackResult> AttackResults { get; }
    }
}