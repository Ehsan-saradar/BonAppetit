using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using BonAppetit.Model.Entities;
using BonAppetit.Web.IntegrationTest.Fixture;
using BonAppetit.Web.IntegrationTest.Stub;
using BonAppetit.Web.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BonAppetit.Web.IntegrationTest
{
    public class AccountControllerTests : IClassFixture<WebFixture<StartupStub>>
    {
        private readonly WebFixture<StartupStub> _fixture;

        public AccountControllerTests(WebFixture<StartupStub> fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public async Task Register_AsyncCall_ResponseReturnsSuccessStatusCode()
        {
            var formData = new Dictionary<string, string>
            {
                { nameof(RegisterRequest.Email), "ali@gmail.com" },
                { nameof(RegisterRequest.Password), "Pass123@" },
            };

            //Act
            var response = await _fixture.Client.PostAsync(
                "/api/account/register",
                new FormUrlEncodedContent(formData));

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
