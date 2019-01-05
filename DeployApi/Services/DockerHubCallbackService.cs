using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DeployApi.Models.DockerHub;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeployApi.Services {
    public class DockerHubCallbackService : IDockerHubCallbackService
    {
        private const string SUCCESS_STATE = "success";
        private const string FAILURE_STATE = "failure";
        private const string CONTEXT = "Docker Hub Kubernetes Deployer";

        private readonly HttpClient _client;
        private readonly ILogger<DockerHubCallbackService> _logger;
    
        public DockerHubCallbackService(IHttpClientFactory factory, ILogger<DockerHubCallbackService> logger) {
            _client = factory.CreateClient();
            _logger = logger;
        }

        public async Task SendCallback(bool isSuccessful, string description, string callbackUrl)
        {
            var model = new WebhookCallbackModel {
                State = isSuccessful ? SUCCESS_STATE : FAILURE_STATE,
                Description = description,
                Context = CONTEXT
            };

            var serialized = JsonConvert.SerializeObject(model);
            _logger.LogDebug($"\n\nSERIALIZED CALLBACK =>\n{serialized}\n\n");

            await _client.PostAsync(
                callbackUrl,
                new StringContent(serialized, Encoding.UTF8, "application/json"));
        }
    }
}
