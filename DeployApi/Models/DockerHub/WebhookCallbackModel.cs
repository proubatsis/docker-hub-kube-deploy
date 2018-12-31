using System.ComponentModel.DataAnnotations;

namespace DeployApi.Models.DockerHub {
    public class WebhookCallbackModel {
        [Required]
        public string State { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string Context { get; set; }
        public string TargetUrl { get; set; }
    }
}
