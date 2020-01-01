using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
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
}
