using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LSP.Models
{
    public class HallOfHeroes
    {
        protected List<Ninja> InternalMembers { get; } = new List<Ninja>();

        public virtual void Add(Ninja ninja)
        {
            if (InternalMembers.Contains(ninja))
            {
                return;
            }
            InternalMembers.Add(ninja);
        }

        public virtual IEnumerable<Ninja> Members
            => new ReadOnlyCollection<Ninja>(
                InternalMembers
                    .OrderByDescending(x => x.Kills)
                    .ToArray()
            );
    }
}
