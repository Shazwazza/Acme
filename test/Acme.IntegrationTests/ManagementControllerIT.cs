using System.Net;
using System.Threading.Tasks;
using Acme.Presentation.Website;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Acme.IntegrationTests
{
    [TestFixture]
    public class ManagementControllerIT
    {
        [Test]
        [TestCase(arg: "/Submission/List")]
        [TestCase(arg: "/SerialNumber/List")]
        public async Task protected_endpoints_must_show_login(string url)
        {
            // Arranges
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(content.Contains("Login"));
        }

        [Test]
        [TestCase(arg: "/")]
        [TestCase(arg: "/Identity/Account/Login")]
        [TestCase(arg: "/Identity/Account/Register")]
        public async Task public_endpoints_must_return_success(string url)
        {
            // Arranges
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}