using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DeployApi.Tests.Util;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Xunit;

namespace DeployApi.Tests
{
    public class DockerHubTest : IClassFixture<WebApplicationFactory<DeployApi.Startup>>
    {
        private readonly WebApplicationFactory<DeployApi.Startup> _factory;
        private readonly HttpClient _client;

        public DockerHubTest(WebApplicationFactory<DeployApi.Startup> factory) {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task NullBodyBadRequest() {
            var response = await _client.PostAsJsonAsync<String>("/api/docker-hub", null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CanDeployImage() {
            // Verify that image is deployed to a Kubernetes Deployment
            var response = await _client.PostAsJsonAsync<JObject>("/api/docker-hub", TestData.LoadJson("webhook-commit-sha-tag"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task LatestTagNotDeployed() {
            // Verify that image tagged ":latest" is not deployed
            var response = await _client.PostAsJsonAsync<JObject>("/api/docker-hub", TestData.LoadJson("webhook-latest-tag"));
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Fact]
        public async Task InvalidRequestBodyBadRequest() {
            // Verify that an invalid request body results in BadRequest status
            var response = await _client.PostAsJsonAsync<JObject>("/api/docker-hub", TestData.LoadJson("webhook-invalid"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); 
        }
    }
}
