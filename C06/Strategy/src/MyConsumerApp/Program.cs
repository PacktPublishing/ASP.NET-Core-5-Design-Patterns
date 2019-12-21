using MySortingMachine;
using System;
using System.Collections.Generic;

namespace MyConsumerApp
{
    public class Program
    {
        private static readonly SortableCollection _data = new SortableCollection(new[] { "Lorem", "ipsum", "dolor", "sit", "amet." });

        public static void Main(string[] args)
        {
            var input = "";
            do
            {
                Console.Clear();
                Console.WriteLine("Options:");
                Console.WriteLine("1: Display the items");
                Console.WriteLine("2: Sort the collection");
                Console.WriteLine("3: Select the sort ascending strategy");
                Console.WriteLine("4: Select the sort descending strategy");
                Console.WriteLine("0: Exit");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Please make a selection: ");
                input = Console.ReadLine();

                Console.Clear();
                switch (input)
                {
                    case "1":
                        PrintCollection();
                        break;
                    case "2":
                        SortData();
                        break;
                    case "3":
                        SetSortAsc();
                        break;
                    case "4":
                        SetSortDesc();
                        break;
                    case "0":
                        break;
                    default:
                        Console.WriteLine("Invalid input!");
                        break;
                }
                Console.WriteLine("Press **enter** to continue.");
                Console.ReadLine();
            } while (input != "0");
        }

        private static void SetSortAsc()
        {
            _data.SortStrategy = new SortAscendingStrategy();
            Console.WriteLine("The sort strategy is now Ascending!");
        }

        private static void SetSortDesc()
        {
            _data.SortStrategy = new SortDescendingStrategy();
            Console.WriteLine("The sort strategy is now Descending!");
        }

        private static void SortData()
        {
            try
            {
                _data.Sort();
                Console.WriteLine("Data sorted!");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void PrintCollection()
        {
            foreach (var item in _data.Items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
