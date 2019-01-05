using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeployApi.Models.DockerHub {
    public class WebhookCallbackModel {
        [Required]
        [JsonProperty("state")]
        public string State { get; set; }

        [MaxLength(255)]
        [JsonProperty("description")]
        public string Description { get; set; }

        [MaxLength(100)]
        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("target_url")]
        public string TargetUrl { get; set; }
    }
}
