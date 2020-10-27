using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BonAppetit.Model.Entities;

namespace BonAppetit.Web.IntegrationTest.Fixture
{
    public class Fixture
    {
        static Fixture()
        {
            Configuration = GetConfiguration();
            CreateDatabase();
        }

        private static IConfiguration GetConfiguration()
            => new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static void CreateDatabase()
        {
            var options = new DbContextOptionsBuilder<BonAppetitDbContext>()
                .UseSqlServer(Configuration["DbConnection"])
                .EnableSensitiveDataLogging()
                //.UseInMemoryDatabase(databaseName: "BonAppetitList")
                .Options;

            new BonAppetitDbContext(options).Database.Migrate();
        }

        protected static IConfiguration Configuration { get; }
    }
}