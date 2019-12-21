using System.Collections.Generic;

namespace MySortingMachine
{
    public interface ISortStrategy
    {
        IEnumerable<string> Sort(IEnumerable<string> input);
    }
}
