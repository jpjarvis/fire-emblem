using FireEmblem.Domain.Combat;

namespace FireEmblem.MapView
{
    public class PlayerUnit : BaseUnit
    {
        public bool HasActed { get; set; }
        protected override Allegiance Allegiance => Allegiance.Player;
    }
}