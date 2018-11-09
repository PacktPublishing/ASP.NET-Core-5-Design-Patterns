using System;

namespace LSP.Models
{
    public class NinjaAddedEventArgs : EventArgs
    {
        public Ninja AddedNinja { get; }

        public NinjaAddedEventArgs(Ninja ninja)
        {
            AddedNinja = ninja ?? throw new ArgumentNullException(nameof(ninja));
        }
    }
}
