using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeployApi.Models.DockerHub;
using System.Net;
using DeployApi.Services;
using DeployApi.Models.Kubernetes;
using Microsoft.Extensions.Configuration;

namespace DeployApi.Controllers
{
    [Route("api/docker-hub")]
    [ApiController]
    public class DockerHubController : ControllerBase
    {
        private const string LATEST_TAG = "latest";
        private const string DEFAULT_NAMESPACE = "default";

        private readonly IKubernetesApiService _kubernetesService;
        private readonly IDeploymentMappingService _mappingService;
        private readonly IDockerHubCallbackService _callbackService;
        private readonly string _kubernetesNamespace;

        public DockerHubController(
            IKubernetesApiService kubernetesService,
            IDeploymentMappingService mappingService,
            IDockerHubCallbackService callbackService,
            IConfiguration configuration) {
            _kubernetesService = kubernetesService;
            _mappingService = mappingService;
            _callbackService = callbackService;
            _kubernetesNamespace = configuration.GetValue("KubernetesNamespace", DEFAULT_NAMESPACE);
        }

        [HttpPost]
        public async Task<ActionResult<DeploymentModel>> Post([FromBody] WebhookRequestModel webhook)
        {
            var imageName = string.Format("{0}/{1}", webhook.Repository.Namespace, webhook.Repository.Name);
            var callbackUrl = webhook.CallbackUrl;

            if (webhook.PushData.Tag == LATEST_TAG) {
                await _callbackService.SendCallback(true, $"Did not deploy {imageName} since it is using the 'latest' tag.", callbackUrl);
                return Accepted();
            }

            var deploymentName = await _mappingService.GetDeploymentNameFromImage(imageName);
            var deployment = await _kubernetesService.SetDeploymentImage(deploymentName, _kubernetesNamespace, imageName, webhook.PushData.Tag);

            await _callbackService.SendCallback(true, $"Deployed {imageName} to {deployment.Name}", callbackUrl);
            return Ok(deployment);
        }
    }
}
