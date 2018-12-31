using System.Threading.Tasks;
using DeployApi.Models.Kubernetes;

namespace DeployApi.Services {
    public class MockKubernetesApiService : IKubernetesApiService
    {
        public Task<DeploymentModel> GetDeployment(string deploymentName, string deploymentNamespace)
        {
            if (deploymentNamespace == "none") {
                throw new System.Exception(string.Format("Deployment {0} not found in {1}", deploymentName, deploymentNamespace));
            }

            return Task.FromResult(new DeploymentModel {
                Name = deploymentName,
                ContainerName = deploymentName + "-container"
            });
        }

        public Task SetDeploymentImage(string deploymentName, string deploymentNamespace, string containerName, string image)
        {
            if (deploymentNamespace == "none") {
                throw new System.Exception(string.Format("Deployment {0} not found in {1}", deploymentName, deploymentNamespace));
            }

            return Task.FromResult(0);
        }
    }
}
