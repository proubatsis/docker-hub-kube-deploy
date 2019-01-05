using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DeployApi.Services {
    public class DeploymentMappingService : IDeploymentMappingService
    {
        private readonly IConfiguration _config;

        public DeploymentMappingService(IConfiguration configuration) {
            _config = configuration;
        }

        public Task<string> GetDeploymentNameFromImage(string imageName)
        {
            var mapping = _config.GetSection("ImageToDeploymentMapping");
            var deploymentName = mapping.GetValue<string>(imageName, null);

            if (deploymentName == null) {
                throw new System.Exception(imageName + " not found!");
            }

            return Task.FromResult(deploymentName);
        }
    }
}
