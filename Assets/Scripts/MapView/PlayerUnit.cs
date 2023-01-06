using FireEmblem.Domain.Combat;

namespace FireEmblem.MapView
{
    public class PlayerUnit : BaseUnit
    {
        protected override Allegiance Allegiance => Allegiance.Player;
    }
}