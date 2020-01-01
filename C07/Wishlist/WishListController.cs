using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Wishlist
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
        public async Task<IActionResult> PostAsync([FromBody,Required]CreateItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _wishList.AddOrRefreshAsync(newItem.Name);
            return Created("/", result);
        }

        public class CreateItem
        {
            [Required]
            public string Name { get; set; }
        }
    }
}
