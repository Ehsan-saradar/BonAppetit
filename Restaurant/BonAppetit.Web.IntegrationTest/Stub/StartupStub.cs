using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BonAppetit.Model.Entities;
using BonAppetit.Repository.Contracts;
using BonAppetit.Repository.Implementations;
using BonAppetit.Service.Contracts;
using BonAppetit.Service.Implementations;
using BonAppetit.Web.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace BonAppetit.Web.IntegrationTest.Stub
{
    public class StartupStub
    {
        private const string SecretKey = "JWcsAowAoat5UWj80vkUsqskKUcudTnyiOM4LLTB";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        static StartupStub()
        {
            Configuration = GetConfiguration();
        }

        protected static IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<BonAppetitDbContext>(
                options => options.UseSqlServer(Configuration["DbConnection"]),
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton);

            var builder = services.AddIdentityCore<CustomerUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<BonAppetitDbContext>();

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);

            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });



            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Web.Helpers.Defaults.Strings.JwtClaimIdentifiers.Rol, Web.Helpers.Defaults.Strings.JwtClaims.ApiAccess));
            });


            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IRestaurantUserRepository, RestaurantUserRepository>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IResturantService, ResturantService>();

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, BonAppetitDbContext dbContext)
        {
            dbContext.Database.Migrate();            

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
          
        }

        private static IConfiguration GetConfiguration()
            => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    }

}