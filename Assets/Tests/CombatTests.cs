using FireEmblem.Domain.Combat;
using NUnit.Framework;

namespace FireEmblem.Tests
{
    [TestFixture]
    public class CombatTests
    {
        [Test]
        public void CombatBasicallyWorks()
        {
            var attacker = Unit.Create(new TestUnitData(
                "TestUnit1",
                new TestStatBlock(10, 10, 10, 10, 10, 10, 10, 10, 5), 
                new [] {new TestWeaponData("TestWeapon", 10, 100, 0, 5, 1, 1, false)}));
            
            var defender = Unit.Create(new TestUnitData(
                "TestUnit2",
                new TestStatBlock(10, 10, 10, 10, 10, 2, 10, 10, 5), 
                new [] {new TestWeaponData("TestWeapon", 10, 100, 0, 5, 1, 1, false)}));

            var combatForecast = CombatForecast.Create(attacker, defender, 1);

            Assert.That(combatForecast.Attacker.NumberOfAttacks, Is.GreaterThan(1));
            Assert.That(combatForecast.Attacker.Attack.Damage, Is.EqualTo(10));
            
            var result = Combat.ResolveCombat(combatForecast);
            Assert.That(result.AttackResults, Has.Count.EqualTo(1));
            Assert.That(result.Defender.IsDead());
        }
    }
}