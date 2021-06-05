using FireEmblem.Model.Data;

namespace FireEmblem.Model.Combat
{
    public class PlayerUnit : Unit
    {
        private readonly PlayerUnitData _playerUnitData;
        public override string Name => _playerUnitData.Name;
        public override IStatBlock Stats => _playerUnitData.BaseStats;

        public PlayerUnit(PlayerUnitData playerUnitData)
        {
            _playerUnitData = playerUnitData;
        }
    }
}