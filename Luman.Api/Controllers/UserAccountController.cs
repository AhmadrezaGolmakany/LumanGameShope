using AutoMapper;
using Luman.Busines.DTOs.UserDTO;
using Luman.Busines.Services.PermissionService;
using Luman.Busines.Services.UserService;
using Luman.Busines.Utility;
using Luman.DataLayer.EntityModel.User;
using Luman.DataLayer.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Security.Claims;

namespace Luman.Api.Controllers
{
    [Route("api/v{version:apiVersion}/Account")]
    //[Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class UserAccountController : ControllerBase
    {
        private readonly IUserServices _services;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;

        public UserAccountController(IUserServices services, IPermissionService permissionService, IMapper mapper)
        {
            _services = services;
            _permissionService = permissionService;
            _mapper = mapper;
        }

        #region Account


        /// <summary>
        /// ثبت نام کاربر
        /// </summary>
        /// <param name="model">مدل ورودی جهت ثبت نام </param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [HttpPost("Register")]
        public IActionResult Register(RegisterUser model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            if (_services.IsExsitEmail(model.Email)) return BadRequest(model);

            if (_services.IsExsitUserName(model.UserName)) return BadRequest(model);

            User user = new()
            {
                Name = model.Name,
                Email = model.Email,
                Family = model.Family,
                Password = PasswordHelper.HashPassword(model.Password),
                UserName = model.UserName,
                CreateDate = DateTime.Now,
                IsDelete = false,

            };



            if (_services.CreateUser(user)) _services.KarbareAdi(user);


            return Ok(model);


        }

        /// <summary>
        /// ورود کابر با سیستم jwt
        /// </summary>
        /// <param name="model">مدل ورودی جهت ورود کاربر </param>
        /// <returns>یک کد jwt را میدهد</returns> 
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            if (_services.IsCorrectpass(model.UserName, model.Password))
            {
                return BadRequest(new { error = "کاربر یافت نشد." });

            }

            var user = await _services.LoginUser(model.UserName, model.Password);
            if (user == null)
            {
                return NotFound(new { error = "کاربر یافت نشد", ErrorCodes = 404 });
            }
            return Ok(new
            {
                Message = "با موفقیت وارد شدید",
                UserId = user.UserId,
                UserName = user.UserName,
                JwtToken = user.JwtSecret
            });
        }

        /// <summary>
        /// ویرایش کاربر از طریق پنل کاربری
        /// </summary>
        /// <param name="model">مدل ورودی جهت ویرایش حساب کاربری</param>
        /// <param name="userid">ایدی کاربر اجاری است</param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [HttpPatch("Edite/{userid:int}")]
        [Authorize]
        public IActionResult EditeUser([FromBody] EditeUserModel model, int userid)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userid.ToString()) 
                return Forbid();    
            
            if (userid != model.UserId) return NotFound(model);

            if (!ModelState.IsValid) return BadRequest(model);

            var user = _services.GetUserById(userid);
            user.Family = model.Family;
            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.UserName;


            if (_services.UpdateUser(user)) return NoContent();

            ModelState.AddModelError("", "مشکلی از سمت سرور پیش آمره لطفا مجددا تلاش کنید.");

            return StatusCode(500, ModelState);
        }

        /// <summary>
        /// تغیر رمز عبور از طریق حساب کاربری 
        /// </summary>
        /// <param name="model">مدل ورودی جهت تغیر رمز عبور توجه داشته باشید وارد کردن username اجباری است </param>
        /// <param name="userid">ایدی کاربر اجباری است</param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [HttpPatch("changepassword/{userid:int}")]
        [Authorize]
        public IActionResult ChnagePassWord([FromBody] ChangePassword model, int userid)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userid.ToString())
                return Forbid();

            if (_services.ComparePassword(model.UserName, model.OldPassword))
            {
                _services.ChangePassword(model.UserName, model.NewPassword);
            }
            return Ok();
        }


        /// <summary>
        /// گرفتن اطلاعات برای حساب کاربری
        /// </summary>
        /// <param name="username">یوزرنیم کاربر </param>
        /// <returns></returns>  
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [HttpGet("UserPanel/{username}")]
        [Authorize]
        public IActionResult GetInfoUser(string username)
        {
            if (User.Identity.Name != username)
            {
                return Forbid();

            }

            var user = _services.GetUserByUsername(username);

            if (user == null) return NotFound();

            var model = _mapper.Map<User>(user);

            var panel = _services.GetUserInformation(username);

            return Ok(panel);
        }

        #endregion
        [HttpGet("TestException")]
        public IActionResult TestException()
        {
            throw new Exception("test");
        }
    }
}
