using System;

namespace NinjaEmptyShell
{
    public interface IAttackable { }

    public class Ninja : IAttackable
    {
        private readonly Weapon _equippedWeapon;

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
