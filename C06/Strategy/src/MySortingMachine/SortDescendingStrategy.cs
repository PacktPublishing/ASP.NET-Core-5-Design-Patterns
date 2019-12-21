using System.Collections.Generic;
using System.Linq;

namespace MySortingMachine
{
    public class SortDescendingStrategy : ISortStrategy
    {
        public IEnumerable<string> Sort(IEnumerable<string> input) =>
            input.OrderByDescending(x => x);
    }
}
