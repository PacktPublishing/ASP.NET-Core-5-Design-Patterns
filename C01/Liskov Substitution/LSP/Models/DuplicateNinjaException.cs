using System;

namespace LSP.Models
{
    public class DuplicateNinjaException : Exception
    {
        public DuplicateNinjaException()
            : base("Cannot add the same ninja twice!") { }
    }
}
