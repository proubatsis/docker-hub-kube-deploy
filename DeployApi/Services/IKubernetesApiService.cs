using System.Threading.Tasks;
using DeployApi.Models.Kubernetes;

namespace DeployApi.Services {
    public interface IKubernetesApiService {
        Task<DeploymentModel> SetDeploymentImage(string deploymentName, string deploymentNamespace, string imageName, string imageTag);
    }
}
