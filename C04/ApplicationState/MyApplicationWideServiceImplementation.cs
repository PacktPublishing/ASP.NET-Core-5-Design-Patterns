using System;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationState
{
    public class MyApplicationWideServiceImplementation : IMyApplicationWideService
    {
        private readonly IMemoryCache _memoryCache;

        public MyApplicationWideServiceImplementation(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public TItem Get<TItem>(string key)
        {
            return _memoryCache.Get<TItem>(key);
        }

        public bool Has<TItem>(string key)
        {
            return _memoryCache.TryGetValue<TItem>(key, out _);
        }
    }
}
