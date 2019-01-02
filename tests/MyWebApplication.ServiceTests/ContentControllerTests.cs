using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using MyWebApplication.Data;
using MyWebApplication.Models;
using Newtonsoft.Json;
using Xunit;

namespace MyWebApplication.ServiceTests
{
    public class ContentControllerTests : IDisposable
    {
        public HttpClient Client { get; }

        public TestServer Server { get; }

        public ContentControllerTests()
        {
            IWebHostBuilder webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(Configuration);

            Server = new TestServer(webHostBuilder);
            Client = Server.CreateClient();

            if (NameRepository.NameCollection.Count != 0) return;
            NameRepository.NameCollection.Add("id1");
            NameRepository.NameCollection.Add("id2");

        }

        [Theory]
        [InlineData("id1", HttpStatusCode.OK)]
        [InlineData("test", HttpStatusCode.NotFound)]
        public async Task ContentController_GetName_Returns_User_With_Specified_Name(string name, HttpStatusCode expectedStatusCode)
        {
            //Arrange & Act
            HttpResponseMessage getNameMessage = await Client.GetAsync($"/content/{name}");

            //Assert
            Assert.Equal(expectedStatusCode, getNameMessage.StatusCode);


        }

        [Theory]
        [InlineData("id3", HttpStatusCode.Created)]
        [InlineData("id1", HttpStatusCode.Conflict)]
        public async Task ContentController_AddUser_Adds_Specified_User(string name, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var nameToBeAdded = new NameApiModel
            {
                Name = name
            };

            //Act
            HttpResponseMessage addName = await Client.PostAsync("/add", new StringContent(
                JsonConvert.SerializeObject(nameToBeAdded), Encoding.UTF8, "application/json"));

            //Assert
            Assert.Equal(expectedStatusCode, addName.StatusCode);

        }
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json", false, true)
            .AddEnvironmentVariables()
            .Build();

        #region IDisposable

        /// <summary>
        ///     This method is called to dispose the resources used in the Tests extending this class
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            Client?.Dispose();
            Server?.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
