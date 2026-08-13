using Luman.Busines.DTOs.OrderDTO;
using Luman.Busines.Services.ProductService;
using Luman.Busines.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Luman.Api.Controllers
{
    [Route("api/v{version:apiVersion}/Product")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IUserServices _userService;

        public ProductController(IProductService productService, IUserServices userService)
        {
            _productService = productService;
            _userService = userService;
        }

        [HttpGet("GetAllProductForIndex")]
        public IActionResult GetAllProductForIndex()
        {
            return Ok(_productService.GetAllForIndex());
        }

        [HttpGet("GetAllCategoryForIndex")]
        public IActionResult GetAllCategory()
        {
            return Ok(_productService.GetAllCategories());
        }


        [HttpPost("AddFavorites")]
        [Authorize]
        public IActionResult AddFavorites([FromBody] FavoritesDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("شناسه کاربر معتبر نیست.");

            model.UserId = userId;

            if (!_productService.IsExistproduct(model.ProId))
                return NotFound("محصول یافت نشد.");

            if (_productService.existingFavorite(model.ProId, model.UserId))
                return BadRequest("این محصول قبلاً به لیست علاقه‌مندی‌ها اضافه شده است.");

            _productService.AddToFavorite(model.ProId, model.UserId);
            return Ok(new
            {
                Message = "محصول با موفقیت به علاقه‌مندی‌ها افزوده شد",
                UserId = model.UserId,
                ProductId = model.ProId
            });
        }


        [HttpGet("GetUserFavorites")]
        [Authorize]

        public IActionResult GetUserFavorites()
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("شناسه کاربر معتبر نیست.");
            }

            try
            {
                var favorites =  _productService.GetUserFavorites(userId);

                if (favorites == null || !favorites.Any())
                {
                    return NotFound("شما هیچ محصولی در لیست علاقه‌مندی ندارید.");
                }

                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



       
    }
}
