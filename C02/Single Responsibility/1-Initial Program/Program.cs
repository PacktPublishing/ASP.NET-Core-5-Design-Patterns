using System;

namespace C02.SRP
{
    class Program
    {
        static void Main(string[] args)
        {
            var run = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Choices:");
                Console.WriteLine("1: Fetch and display book id 1");
                Console.WriteLine("2: Fail to fetch a book");
                Console.WriteLine("3: Book does not exist");
                Console.WriteLine("4: Create an out of order book");
                Console.WriteLine("5: Display a book somewhere else");
                //...
                Console.WriteLine("0: Exit");

                var input = Console.ReadLine();
                Console.Clear();
                try
                {
                    switch (input)
                    {
                        case "1":
                            FetchAnDisplayBook();
                            break;
                        case "2":
                            FailToFetchBook();
                            break;
                        case "3":
                            BookDoesNotExist();
                            break;
                        case "4":
                            CreateOutOfOrderBook();
                            break;
                        case "5":
                            DisplayTheBookSomewhereElse();
                            break;
                        case "0":
                            run = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }
                    Console.WriteLine("Press enter to go back to the main menu.");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The following exception occured, press enter to continue:");
                    Console.WriteLine(ex);
                    Console.ReadLine();
                }    
            } while (run);
        }

        private static void FetchAnDisplayBook()
        {
            var book = new Book(id: 1);
            book.Load();
            book.Display();
        }

        private static void FailToFetchBook()
        {
            var book = new Book();
            book.Load(); // Exception: You must set the Id to the Book Id you want to load.
            book.Display();
        }

        private static void BookDoesNotExist()
        {
            var book = new Book(id: 2);
            book.Load();
            book.Display();
        }

        private static void CreateOutOfOrderBook()
        {
            var book = new Book
            {
                Id = 999, // this value is not enforced by anything
                Title = "Some out of order book"
            };
            book.Save();
            book.Display();
        }

        private static void DisplayTheBookSomewhereElse()
        {
            Console.WriteLine("Oups! Can't do that, the Display method only write to the \"Console\".");
        }
    }
}
