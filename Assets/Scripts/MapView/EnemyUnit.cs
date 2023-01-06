using FireEmblem.Domain.Combat;

namespace FireEmblem.MapView
{
    public class EnemyUnit : BaseUnit
    {
        protected override Allegiance Allegiance => Allegiance.Enemy;
    }
}