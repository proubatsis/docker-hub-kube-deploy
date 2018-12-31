using System.Collections.Generic;

namespace DeployApi.Models.DockerHub {
    public class PushDataModel {
        public List<string> images { get; set; }
        public string PushedAt { get; set; }
        public string Pusher { get; set; }
        public string Tag { get; set; }
    }
}
