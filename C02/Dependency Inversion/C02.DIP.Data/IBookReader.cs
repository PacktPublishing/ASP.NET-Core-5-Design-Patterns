using System.Collections.Generic;

namespace C02.DIP.Data
{
    public interface IBookReader
    {
        IEnumerable<Book> Books { get; }
        Book Find(int bookId);
    }
}
