using System.Threading.Tasks;

namespace DeployApi.Services {
    public interface IDeploymentMappingService {
        Task<string> GetDeploymentNameFromImage(string imageName);
    }
}
