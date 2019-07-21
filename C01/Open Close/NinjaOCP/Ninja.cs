using System;

namespace NinjaOCP
{
    public interface IAttackable { }

    public class Ninja : IAttackable
    {
        public Weapon EquippedWeapon { get; set; }

        public AttackResult Attack(IAttackable target)
        {
            return new AttackResult(EquippedWeapon, this, target);
        }

        public override string ToString() => this.GetType().Name;
    }

    public class Weapon
    {
        public override string ToString() => this.GetType().Name;
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

            // First attack (Sword)
            ninja.EquippedWeapon = new Sword();
            var result = ninja.Attack(target);
            PrintAttackResult(result);

            // Second attack (Shuriken)
            ninja.EquippedWeapon = new Shuriken();
            var result2 = ninja.Attack(target);
            PrintAttackResult(result2);
        }

        private static void PrintAttackResult(AttackResult result)
        {
            Console.WriteLine($"{result.Attacker} attacked {result.Target} using {result.Weapon}!");
        }
    }
}
