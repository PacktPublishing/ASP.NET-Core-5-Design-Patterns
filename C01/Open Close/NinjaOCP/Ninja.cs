using System;

namespace NinjaOCP
{
    public interface IAttackable
    {

    }

    public class Ninja : IAttackable
    {
        private readonly Sword _sword;
        private readonly Shuriken _shuriken;
        public Weapon EquippedWeapon { get; private set; }

        public Ninja()
        {
            _sword = new Sword();
            _shuriken = new Shuriken();
            EquippedWeapon = _sword;
        }

        public AttackResult Attack(IAttackable target)
        {
            return new AttackResult(EquippedWeapon, this, target);
        }

        public void UseShuriken()
        {
            EquippedWeapon = _shuriken;
        }

        public void UseSword()
        {
            EquippedWeapon = _sword;
        }

        public override string ToString() => this.GetType().Name;
    }

    public class Weapon
    {
        public override string ToString()
        {
            return $"Weapon: {this.GetType().Name}";
        }
    }

    public class Sword : Weapon { }

    public class Shuriken : Weapon { }

    public class AttackResult
    {
        public Weapon Weapon { get; }
        public IAttackable Attacker { get; }
        public IAttackable Target { get; }
        public AttackResult(Weapon weapon, IAttackable attacker, IAttackable target)
        {
            Weapon = weapon;
            Attacker = attacker;
            Target = target;
        }
    }

    public class Client
    {
        public void Main()
        {
            // Arrange
            var target = new Ninja();
            var ninja = new Ninja();

            // First attack (default: Sword)
            var result = ninja.Attack(target);
            PrintAttackResult(result);

            // Second attack (Shuriken)
            ninja.UseShuriken();
            var result2 = ninja.Attack(target);
            PrintAttackResult(result2);
        }

        private static void PrintAttackResult(AttackResult result)
        {
            Console.WriteLine($"{result.Attacker} attacked {result.Target} using {result.Weapon}!");
        }
    }
}
