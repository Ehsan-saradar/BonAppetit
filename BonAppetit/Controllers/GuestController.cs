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
    public class GuestController : ControllerBase
    {
        private readonly IResturantService _userService;
        public GuestController(IResturantService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// list
        /// </summary>
        /// <returns></returns>
        // Get api/guest/list
        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            return new JsonResult(_userService.List().Select(item => new
            {
                item.RestaurantName,
                item.Address,
                item.Desc
            }));
        }


        /// <summary>
        /// get menu
        /// </summary>
        /// <param name="id">Resturant Id</param>
        /// <returns></returns>
        // Get api/guest/getMenu
        [HttpGet("getMenu")]
        public async Task<IActionResult> GetMenu(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return new JsonResult(new
            {
                Menu = _userService.GetMenu(id).Select(item => new
                {
                    Name = item.Name,
                    Price = item.Price,
                    Num = item.Num,
                    Desc = item.Description
                })
            });
        }

    }
}
