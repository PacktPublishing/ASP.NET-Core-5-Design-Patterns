using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationState
{
    [Route("/")]
    public class WishListController : ControllerBase
    {
        private readonly IWishList _wishList;

        public WishListController(IWishList wishList)
        {
            _wishList = wishList ?? throw new ArgumentNullException(nameof(wishList));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _wishList.AllAsync();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(string itemName)
        {
            var result = await _wishList.AddOrRefreshAsync(itemName);
            return CreatedAtAction(nameof(GetAsync), result);
        }
    }

    public class WishListItem
    {
        public int Count { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string Name { get; set; }
    }

    public interface IWishList
    {
        Task<WishListItem> AddOrRefreshAsync(string itemName);
        Task<IEnumerable<WishListItem>> AllAsync();
    }

    public class InMemoryWishList : IWishList
    {
        private readonly InMemoryWishListOptions _options;
        private readonly Dictionary<string, InternalItem> _items;

        public InMemoryWishList(IOptions<InMemoryWishListOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            _items = new Dictionary<string, InternalItem>();
        }

        public Task<WishListItem> AddOrRefreshAsync(string itemName)
        {
            var expirationTime = _options.SystemClock.UtcNow.AddSeconds(_options.ExpirationInSeconds);
            if (_items.ContainsKey(itemName))
            {
                var item = _items[itemName];
                item.Count++;
                var wishlistItem = new WishListItem
                {
                    Name = itemName,
                    Count = item.Count,
                    Expiration = item.Expiration
                };
                return Task.FromResult(wishlistItem);
            }
            else
            {
                var item = new InternalItem
                {
                    Count = 1,
                    Expiration = expirationTime
                };
                _items.Add(itemName, item);
                var wishlistItem = new WishListItem
                {
                    Name = itemName,
                    Count = item.Count,
                    Expiration = item.Expiration
                };
                return Task.FromResult(wishlistItem);
            }
        }

        public Task<IEnumerable<WishListItem>> AllAsync()
        {
            var items = _items.Select(x => new WishListItem
            {
                Name = x.Key,
                Count = x.Value.Count,
                Expiration = x.Value.Expiration
            });
            return Task.FromResult(items);
        }

        private class InternalItem
        {
            public int Count { get; set; }
            public DateTimeOffset Expiration { get; set; }
        }
    }
}
