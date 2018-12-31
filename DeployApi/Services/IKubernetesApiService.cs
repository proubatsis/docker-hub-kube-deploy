using System.Threading.Tasks;
using DeployApi.Models.Kubernetes;

namespace DeployApi.Services {
    public interface IKubernetesApiService {
        Task<DeploymentModel> GetDeployment(string deploymentName, string deploymentNamespace);
        Task SetDeploymentImage(string deploymentName, string deploymentNamespace, string containerName, string image);
    }
}
