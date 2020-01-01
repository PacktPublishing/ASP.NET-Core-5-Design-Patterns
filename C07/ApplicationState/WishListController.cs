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

        public InMemoryWishList(IOptions<InMemoryWishListOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public Task<WishListItem> AddOrRefreshAsync(string itemName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WishListItem>> AllAsync()
        {
            throw new NotImplementedException();
        }

        private class InternalItem
        {
            public int Count { get; set; }
            public DateTimeOffset Expiration { get; set; }
        }
    }
}
