namespace FireEmblem.Domain.Combat
{
    public class AttackResult
    {
        public AttackResult(bool isFromAttacker, int damage, bool isHit, bool isCrit, bool isFatal)
        {
            IsFromAttacker = isFromAttacker;
            Damage = damage;
            IsHit = isHit;
            IsCrit = isCrit;
            IsFatal = isFatal;
        }
        
        public bool IsFromAttacker { get; }
        public int Damage { get; }
        public bool IsHit { get; }
        public bool IsCrit { get; }
        public bool IsFatal { get; }
    }
}