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
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<CustomerUser> userManager;
        private readonly ClaimsPrincipal _caller;
        private readonly IResturantService _userService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        public CustomerController(UserManager<CustomerUser> userManager, IHttpContextAccessor httpContextAccessor, IResturantService userService, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.userManager = userManager;
            _caller = httpContextAccessor.HttpContext.User;
            _userService = userService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }
        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="newOrder">order</param>
        /// <returns></returns>
        // POST api/restaurant/addOrder
        [HttpPost("addOrder")]
        public async Task<IActionResult> AddOrder([FromBody] BonAppetit.Web.Models.Order newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var id = _caller.Claims.Single(c => c.Type == "id");
            Model.Entities.Order order = new Model.Entities.Order() { 
                CustomerUserId=id.Value,
                Status="Pending",
                
            };
            order.OrderItems = new List<OrderItem>();
            var rest=_userService.GetResturantById(newOrder.ResturantId).Result;
            if (!rest.IsOpen)
            {
                return new JsonResult(new
                {
                    error="Is not open"
                });
            }
            var menu = _userService.GetMenu(newOrder.ResturantId);
            for (int i = 0; i < newOrder.OrderItems.Count(); i++)
            {
                if (newOrder.OrderItems[i].Num <= 0)
                {
                    continue;
                }
                var item = menu.FirstOrDefault(r => r.Id == newOrder.OrderItems[i].Id && newOrder.OrderItems[i].Num<=r.Num);
                order.OrderItems.Add(new Model.Entities.OrderItem
                {
                    MenuItem=item,
                    MenuItemId = newOrder.OrderItems[i].Id,
                    Count = newOrder.OrderItems[i].Num
                });
                item.Num = item.Num - newOrder.OrderItems[i].Num;
                _userService.EditMenuCount(item);
            }
            var done = _userService.AddOrder(order);
            return new JsonResult(new
            {
                Done = done
            });
        }
    }
}
