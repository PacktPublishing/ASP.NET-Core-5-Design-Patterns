using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ClearName
{
    public class OneMethodExampleService : IExampleService
    {
        private readonly IEnumerable<string> _data;
        private static readonly Random _random = new Random();

        public OneMethodExampleService(IEnumerable<string> data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public RandomResult RandomizeOneString()
        {
            // Find the upper bound
            var upperBound = _data.Count();

            // Randomly select the index of the string to return
            var index = _random.Next(0, upperBound);

            // Shuffle the elements to add more randomness
            // The shuffle algorithm is based on 
            // https://stackoverflow.com/a/1262619/8339553
            var shuffledList = _data.ToArray();
            var rng = new RNGCryptoServiceProvider();
            var n = shuffledList.Count();
            while (n > 1)
            {
                var box = new byte[1];
                do rng.GetBytes(box);
                while (!(box[0] < n * (byte.MaxValue / n)));
                var k = (box[0] % n);
                n--;
                var value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }

            // Return the randomly selected element
            var randomString = shuffledList.ElementAt(index);
            return new RandomResult(randomString, index, shuffledList);
        }
    }
}
