using System.Threading.Tasks;

namespace DeployApi.Services {
    public interface IDockerHubCallbackService {
        Task SendCallback(bool isSuccessful, string description, string callbackUrl);
    }
}
