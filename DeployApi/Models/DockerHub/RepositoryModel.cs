namespace DeployApi.Models.DockerHub {
    public class RepositoryModel {
        public int CommentCount { get; set; }
        public double DateCreate { get; set; }
        public string Description { get; set; }
        public string Dockerfile { get; set; }
        public string FullDescription { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsTrusted { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Owner { get; set; }
        public string RepoName { get; set; }
        public string RepoUrl { get; set; }
        public int StarCount { get; set; }
        public string Status { get; set; }
    }
}
