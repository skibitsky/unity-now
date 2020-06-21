using Newtonsoft.Json;

namespace Skibitsky.Unity.Editor
{
    public class DeploymentContent
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("version")] public int Version { get; set; }

        [JsonProperty("files")] public DeploymentFileReference[] Files { get; set; }
    }
}