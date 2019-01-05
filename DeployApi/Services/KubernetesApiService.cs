using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeployApi.Models.Kubernetes;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;

namespace DeployApi.Services {
    public class KubernetesApiService : IKubernetesApiService
    {
        private readonly IKubernetes _client;
        private readonly ILogger<KubernetesApiService> _logger;

        private const string CHANGE_CAUSE_ANNOTATION = "kubernetes.io/change-cause";

        public KubernetesApiService(ILogger<KubernetesApiService> logger) {
            // var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            var config = new KubernetesClientConfiguration {
                Host = "http://127.0.0.1:8001"
            };

            _client = new Kubernetes(config);
            _logger = logger;
        }

        public async Task<DeploymentModel> SetDeploymentImage(string deploymentName, string deploymentNamespace, string imageName, string imageTag)
        {
            var image = string.Format("{0}:{1}", imageName, imageTag);
            _logger.LogDebug($"Deploying image '{image}' to deployment '{deploymentName}' in namespace '{deploymentNamespace}'");

            // Get existing annotations in deployment
            var deployments = await _client.ListNamespacedDeploymentAsync(deploymentNamespace);
            var deployment = deployments.Items.Single(d => d.Metadata.Name == deploymentName);

            var updatedAnnotations = new Dictionary<string, string>(deployment.Metadata.Annotations);

            // Display deployed image in rollout history
            string changeCause = $"Deploy {image}";
            if (updatedAnnotations.ContainsKey(CHANGE_CAUSE_ANNOTATION)) {
                updatedAnnotations[CHANGE_CAUSE_ANNOTATION] = changeCause;
            } else {
                updatedAnnotations.Add(CHANGE_CAUSE_ANNOTATION, changeCause);
            }

            // Create patch to update image and annotations
            var patch = new JsonPatchDocument<V1Deployment>();
            patch.Replace(e => e.Spec.Template.Spec.Containers[0].Image, image);
            patch.Replace(e => e.Metadata.Annotations, updatedAnnotations);

            // Update the deployment
            var updatedDeployment = await _client.PatchNamespacedDeploymentAsync(new V1Patch(patch), deploymentName, deploymentNamespace);
            return new DeploymentModel {
                Name = updatedDeployment.Metadata.Name,
                ContainerName = updatedDeployment.Spec.Template.Spec.Containers[0].Name
            };
        }
    }
}
