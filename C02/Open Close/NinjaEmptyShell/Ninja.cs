using System;

namespace NinjaEmptyShell
{
    public class Human
    {
    }

    public class Ninja : Human
    {
        private readonly Weapon _equipedWeapon;

        public AttackResult Attack(Human target)
        {
            throw new NotImplementedException();
        }
    }

    public class Weapon { }

    public class Sword : Weapon { }

    public class Shuriken : Weapon { }

    public class AttackResult { }
}
