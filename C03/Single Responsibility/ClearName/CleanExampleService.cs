using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace ClearName
{
    public class CleanExampleService : IExampleService
    {
        private readonly IEnumerable<string> _data;
        private static readonly Random _random = new Random();

        public CleanExampleService(IEnumerable<string> data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            if (_data.Count() > byte.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(data), 
                    $"The number of elements must be lower or equal to '{byte.MaxValue}'."
                );
            }
        }

        public RandomResult RandomizeOneString()
        {
            var upperBound = _data.Count();
            var index = _random.Next(0, upperBound);
            var shuffledData = ShuffleData();
            var randomString = shuffledData.ElementAt(index);
            return new RandomResult(randomString, index, shuffledData);
        }

        private IEnumerable<string> ShuffleData()
        {
            // The shuffle algorithm is based on the Fisher–Yates shuffle, 
            // implementation taken from https://stackoverflow.com/a/1262619/8339553
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
            return shuffledList;
        }
    }
}
