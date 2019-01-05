using System.Threading;
using System.Threading.Tasks;
using DeployApi.Services;

namespace DeployApi.Tests.Util {
    public class MockDockerHubCallbackService : IDockerHubCallbackService
    {
        private static int _sentCount = 0;

        public Task SendCallback(bool isSuccessful, string description, string callbackUrl)
        {
            Interlocked.Increment(ref _sentCount);
            return Task.FromResult(0);
        }

        public static int SentCount
        {
            get { return _sentCount; }
        }
    }
}
