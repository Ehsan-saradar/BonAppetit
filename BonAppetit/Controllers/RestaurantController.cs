using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;
using BonAppetit.Repository.Implementations;
using BonAppetit.Service.Contracts;
using BonAppetit.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BonAppetit.Web.Helpers;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace BonAppetit.Web.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly UserManager<RestaurantUser> userManager;
        private readonly ClaimsPrincipal _caller;
        private readonly IResturantService _userService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        public RestaurantController(UserManager<RestaurantUser> userManager, IHttpContextAccessor httpContextAccessor, IResturantService userService, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.userManager = userManager;
            _caller = httpContextAccessor.HttpContext.User;
            _userService = userService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }
        /// <summary>
        /// Register New Restaurant
        /// </summary>
        /// <param name="registerRequest">Register data</param>
        /// <returns></returns>
        // POST api/restaurant/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RestaurantRegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new RestaurantUser()
            {
                UserName = "res"+ registerRequest.Mobile,
                Mobile = registerRequest.Mobile,
                Desc=registerRequest.Desc,
                RestaurantName=registerRequest.RestaurantName,
                Address=registerRequest.Address
            };
            var registerResult = await userManager.CreateAsync(user, registerRequest.Password);
            if (!registerResult.Succeeded)
                return new BadRequestObjectResult(Errors.AddErrorsToModelState(registerResult, ModelState));
            return new JsonResult(new
            {
                Response = "Account created"
            });
        }

        /// <summary>
        /// Login for restaurant
        /// </summary>
        /// <param name="loginRequest">Login data</param>
        /// <returns></returns>
        // POST api/restaurant/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity("res" + loginRequest.Mobile, loginRequest.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, "res" + loginRequest.Mobile, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return Content(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the loginRequest
            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, "req"));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        /// <summary>
        /// Open/Close
        /// </summary>
        /// <param name="restaurantStatus">status</param>
        /// <returns></returns>
        // POST api/restaurant/changeOpen
        [HttpPost("changeOpen")]
        public async Task<IActionResult> ChangeOpen([FromBody]RestaurantStatus status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = _caller.Claims.Single(c => c.Type == "id");
            return new JsonResult(new
            {
                Done = _userService.ChangeOpen(id.Value, status.IsOpen)
            });
        }

        /// <summary>
        /// Add new menu item
        /// </summary>
        /// <param name="newMenuItem">Menu item</param>
        /// <returns></returns>
        // POST api/restaurant/addMenu
        [HttpPost("addMenu")]
        public async Task<IActionResult> AddMenu([FromBody]NewMenuItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = _caller.Claims.Single(c => c.Type == "id");
            var menuitem = new Model.Entities.MenuItem
            {
                Description = newItem.Description,
                RestaurantUserId = id.Value,
                Price = newItem.Price,
                Num = newItem.Num,
                Name = newItem.Name,
            };
            return new JsonResult(new
            {
                Done = _userService.AddMenu(menuitem)
            });
        }

        /// <summary>
        /// list orders
        /// </summary>
        /// <returns></returns>
        // Get api/restaurant/orders
        [HttpGet("orders")]
        public async Task<IActionResult> Orders()
        {
            var id = _caller.Claims.Single(c => c.Type == "id");
            return new JsonResult(_userService.ListOrders(id.Value).Where(item=>!item.approved.HasValue).Select(item => new
            {
               item.Id,
               items=item.OrderItems.Select(i=>new
               {
                   i.Count,
                   i.MenuItem.Id,
                   i.MenuItem.Name,
                   i.MenuItem.Description
               }),
               item.Status,
               item.CustomerUser.Mobile,
               item.CustomerUser.Address
            }));
        }

        /// <summary>
        /// Approve Order
        /// </summary>
        /// <returns></returns>
        // Get api/restaurant/approve
        [HttpGet("approve")]
        public async Task<IActionResult> Approve(long orderId)
        {
            var id = _caller.Claims.Single(c => c.Type == "id");
            var order = _userService.ListOrders(id.Value).FirstOrDefault(item => !item.approved.HasValue && item.Id == orderId);
            return new JsonResult(new
            {
                Done = _userService.EditOrder(order.Id, true, "")
            });
        }

        /// <summary>
        /// Deny Order
        /// </summary>
        /// <returns></returns>
        // Get api/restaurant/deny
        [HttpGet("deny")]
        public async Task<IActionResult> Deny(long orderId)
        {
            var id = _caller.Claims.Single(c => c.Type == "id");
            var order = _userService.ListOrders(id.Value).FirstOrDefault(item => !item.approved.HasValue && item.Id == orderId);
            return new JsonResult(new
            {
                Done = _userService.EditOrder(order.Id, false, "")
            });
        }

        /// <summary>
        /// Set status of offer
        /// </summary>
        /// <returns></returns>
        // Get api/restaurant/status
        [HttpGet("status")]
        public async Task<IActionResult> Status(long orderId,string status)
        {
            var id = _caller.Claims.Single(c => c.Type == "id");
            var order = _userService.ListOrders(id.Value).FirstOrDefault(item => !item.approved.HasValue && item.Id == orderId);
            return new JsonResult(new
            {
                Done = _userService.EditOrder(order.Id, null, status)
            });
        }


        /// <summary>
        /// Delete menu
        /// </summary>
        /// <returns></returns>
        // Get api/restaurant/deleteMenu
        [HttpGet("deleteMenu")]
        public async Task<IActionResult> DeleteMenu(long menuId)
        {
            var id = _caller.Claims.Single(c => c.Type == "id");
            var menu=_userService.GetResturantById(id.Value).Result.MenuItems.First(item => item.Id == menuId);
            return new JsonResult(new
            {
                Done = _userService.DeleteMenu(menu.Id)
            });
        }
    }
}
