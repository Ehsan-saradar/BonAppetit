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
    }
}
