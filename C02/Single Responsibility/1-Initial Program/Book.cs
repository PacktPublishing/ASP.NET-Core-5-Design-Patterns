using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C02.SRP
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Book(int? id = null)
        {
            Id = id ?? default(int);
        }

        public void Save()
        {
            // Create the book is it does not exist, 
            // otherwise, find its index and replace it 
            // by the current object.
            if (BookStore.Books.Any(x => x.Id == Id))
            {
                var index = BookStore.Books.FindIndex(x => x.Id == Id);
                BookStore.Books[index] = this;
            }
            else
            {
                BookStore.Books.Add(this);
            }
        }

        public void Load()
        {
            // Validate that an Id is set
            if (Id == default(int))
            {
                throw new Exception("You must set the Id to the Book Id you want to load.");
            }

            // Get the book
            var book = BookStore.Books.FirstOrDefault(x => x.Id == Id);

            // Make sure it exist
            if (book == null)
            {
                throw new Exception("This book does not exist in the bookstore.");
            }

            // Copy the book properties to the current object
            Id = book.Id; // this should already be set
            Title = book.Title;
        }

        public void Display()
        {
            Console.WriteLine($"Book: {Title} ({Id})");
        }
    }
}
