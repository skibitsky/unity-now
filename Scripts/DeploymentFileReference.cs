using Newtonsoft.Json;

namespace com.skibitsky.UnityNow
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