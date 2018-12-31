using System.ComponentModel.DataAnnotations;

namespace DeployApi.Models.DockerHub {
    public class WebhookRequestModel {
        [Required]
        public string CallbackUrl { get; set; }
        
        [Required]
        public PushDataModel PushData { get; set; }

        [Required]
        public RepositoryModel Repository { get; set; }
    }
}
