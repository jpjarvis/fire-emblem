namespace FireEmblem.Model.Combat
{
    public class AttackResult
    {
        public AttackResult(Unit attacker, Unit target, int damage, bool isHit, bool isCrit, bool isFatal)
        {
            Attacker = attacker;
            Target = target;
            Damage = damage;
            IsHit = isHit;
            IsCrit = isCrit;
            IsFatal = isFatal;
        }
        
        public Unit Attacker { get; }
        public Unit Target { get; }
        public int Damage { get; }
        public bool IsHit { get; }
        public bool IsCrit { get; }
        public bool IsFatal { get; }
    }
}