using LSP.Models;
using System;

namespace LSP.Examples.Update3
{
    public class HallOfHeroes : HallOfFame
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

    public class NinjaAddedEventArgs : EventArgs
    {
        public Ninja AddedNinja { get; }

        public NinjaAddedEventArgs(Ninja ninja)
        {
            AddedNinja = ninja ?? throw new ArgumentNullException(nameof(ninja));
        }
    }
}
