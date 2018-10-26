using System.Collections.Generic;
using System.Linq;

namespace C02.SRP
{
    public static class BookStore
    {
        private static int _lastId = 0;

        public static List<Book> Books { get; }
        public static int NextId => ++_lastId;

        static BookStore()
        {
            Books = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Some cool computer book"
                }
            };
        }
    }
}
