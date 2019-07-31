using LSP.Models;
using System;

namespace LSP.Examples.Update3
{
    public class HallOfHeroes : HallOfFame
    {
        public event EventHandler<AddingDuplicatedNinjaEventArgs> AddingDuplicatedNinja;

        public override void Add(Ninja ninja)
        {
            if (InternalMembers.Contains(ninja))
            {
                OnAddingDuplicatedNinja(new AddingDuplicatedNinjaEventArgs(ninja));
                return;
            }
            InternalMembers.Add(ninja);
        }

        protected virtual void OnAddingDuplicatedNinja(AddingDuplicatedNinjaEventArgs e)
        {
            AddingDuplicatedNinja?.Invoke(this, e);
        }
    }

    public class AddingDuplicatedNinjaEventArgs : EventArgs
    {
        public Ninja DuplicatedNinja { get; }

        public AddingDuplicatedNinjaEventArgs(Ninja ninja)
        {
            DuplicatedNinja = ninja ?? throw new ArgumentNullException(nameof(ninja));
        }
    }
}
