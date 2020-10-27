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
        /// Edit Restaurant
        /// </summary>
        /// <param name="registerRequest">Register data</param>
        /// <returns></returns>
        // POST api/restaurant/edit
        [HttpPost("edit")]
        public async Task<IActionResult> Edit(RestaurantRegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var id = _caller.Claims.Single(c => c.Type == "id");
            var rest = _userService.GetResturantById(id.Value).Result;
            rest.Desc = registerRequest.Desc;
            rest.RestaurantName = registerRequest.RestaurantName;
            rest.Address = registerRequest.Address;


            var updateResult = _userService.UpdateResturantById(rest);
            if (!updateResult)
                return new BadRequestObjectResult(Errors.AddErrorsToModelState(null, ModelState));
            return new JsonResult(new
            {
                Response = "Account edited"
            });
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
        public async Task<IActionResult> Orders(string status)
        {
            var id = _caller.Claims.Single(c => c.Type == "id");
            var res = _userService.ListOrders(id.Value).ToList();
            res = res.Where(item => (!item.approved.HasValue || item.approved == true) && item.Status == status).ToList();
            for(int i = 0; i < res.Count; i++)
            {
                res[i].OrderItems = res[i].OrderItems.ToList();
                var r1 = res[i].OrderItems.ToList();
            }
            return new JsonResult(res.Where(item=>item.OrderItems!=null && item.OrderItems.Count>0).ToList().Select(item => new
            {
               item.Id,
               items=item.OrderItems.ToList().Select(i=>new
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
            var order = _userService.ListOrders(id.Value).FirstOrDefault(item => item.Id == orderId);
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
            var menu = _userService.GetMenu(id.Value).ToList().FirstOrDefault(item=>item.Id==menuId);
            return new JsonResult(new
            {
                Done = _userService.DeleteMenu(menu.Id)
            }) ;
        }

        /// <summary>
        /// Add new menu item
        /// </summary>
        /// <param name="newMenuItem">Menu item</param>
        /// <returns></returns>
        // POST api/restaurant/addMenu
        [HttpPost("editMenu")]
        public async Task<IActionResult> EditMenu([FromBody]NewMenuItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = _caller.Claims.Single(c => c.Type == "id");
            var menu = _userService.GetMenu(id.Value);
            var menuitem = new Model.Entities.MenuItem
            {
                Id= newItem.Id,
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
    }
}
