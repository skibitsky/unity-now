using Newtonsoft.Json;

namespace Skibitsky.Unity.Editor
{
    public class DeploymentFileReference
    {
        private readonly DeploymentFile _deploymentFile;

        public DeploymentFileReference(DeploymentFile deploymentFile)
        {
            _deploymentFile = deploymentFile;
        }

        [JsonProperty("file")] public string RelativePath => _deploymentFile.RelativePath;

        [JsonProperty("sha")] public string Checksum => _deploymentFile.Checksum;

        [JsonProperty("size")] public int FileSize => (int) _deploymentFile.FileSize;
    }
}