using System;

namespace LSP.Models
{
    public class HallOfHeroesV2Fixed : HallOfHeroes
    {
        public event EventHandler<NinjaAddedEventArgs> NinjaAdded;

        public override void Add(Ninja ninja)
        {
            if (InternalMembers.Contains(ninja))
            {
                return;
            }
            InternalMembers.Add(ninja);
            OnNinjaAdded(new NinjaAddedEventArgs(ninja));
        }

        protected virtual void OnNinjaAdded(NinjaAddedEventArgs e)
        {
            NinjaAdded?.Invoke(this, e);
        }
    }
}
