using System;

namespace NinjaEmptyShell
{
    public interface IAttackable { }

    public class Ninja : IAttackable
    {
        public Weapon EquippedWeapon { get; set; }
        public string Name { get; }

        public Ninja(string name)
        {
            Name = name;
        }

        public AttackResult Attack(IAttackable target)
        {
            throw new NotImplementedException();
        }
    }

    public class Weapon { }

    public class Sword : Weapon { }

    public class Shuriken : Weapon { }

    public class AttackResult { }
}
