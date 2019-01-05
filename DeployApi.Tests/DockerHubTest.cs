using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DeployApi.Models.DockerHub;
using DeployApi.Models.Kubernetes;
using DeployApi.Services;
using DeployApi.Tests.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace DeployApi.Tests
{
    public class DockerHubTest : IClassFixture<WebApplicationFactory<DeployApi.Startup>>
    {
        private readonly WebApplicationFactory<DeployApi.Startup> _factory;
        private readonly HttpClient _client;

        public DockerHubTest(WebApplicationFactory<DeployApi.Startup> factory) {
            _factory = factory;

            _client = _factory.WithWebHostBuilder(builder => {
                builder.ConfigureTestServices(services => {
                    services.AddScoped<IDockerHubCallbackService, MockDockerHubCallbackService>();
                    services.AddScoped<IKubernetesApiService, MockKubernetesApiService>();
                });
            })
            .CreateClient();
        }

        [Fact]
        public async Task NullBodyBadRequest() {
            var response = await _client.PostAsJsonAsync<String>("/api/docker-hub", null);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CanDeployImage() {
            // Verify that image is deployed to a Kubernetes Deployment
            var data = TestData.LoadJson("webhook-commit-sha-tag");
            var webhook = data.ToObject<WebhookRequestModel>();

            var response = await _client.PostAsJsonAsync<JObject>("/api/docker-hub", TestData.LoadJson("webhook-commit-sha-tag"));
            var deployment = await response.Content.ReadAsAsync<JObject>();
            var x = deployment["name"];

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("my-deployment", deployment["name"].ToString());
            Assert.Equal("my-deployment-test-namespace-proubatsis/blog-site-ff20631c5a9ef0ef040e8b4b3b51fc65abffed96", deployment["container_name"].ToString());
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

        [Fact]
        public async Task CallsDockerHubCallback() {
            // Verify that the dockerhub callback is called
            var before = MockDockerHubCallbackService.SentCount;
            var response = await _client.PostAsJsonAsync<JObject>("/api/docker-hub", TestData.LoadJson("webhook-latest-tag"));
            var after = MockDockerHubCallbackService.SentCount;
            
            Assert.True(after > before, $"{after} should be greater than {before}!");
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }
    }
}
