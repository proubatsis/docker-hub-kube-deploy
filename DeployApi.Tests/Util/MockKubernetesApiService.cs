using System.Threading.Tasks;
using DeployApi.Models.Kubernetes;
using DeployApi.Services;

namespace DeployApi.Tests.Util {
    public class MockKubernetesApiService : IKubernetesApiService
    {
        public Task<DeploymentModel> SetDeploymentImage(string deploymentName, string deploymentNamespace, string imageName, string imageTag)
        {
            return Task.FromResult(new DeploymentModel {
                Name = deploymentName,
                ContainerName = $"{deploymentName}-{deploymentNamespace}-{imageName}-{imageTag}"
            });
        }
    }
}
