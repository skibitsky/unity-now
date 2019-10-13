using Newtonsoft.Json;

namespace com.skibitsky.UnityNow
{
    public static partial class DeployNow
    {
        private class DeploymentContent
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public int Version { get; set; }
            
            [JsonProperty("files")]
            public DeploymentFileReference[] Files { get; set; }
        }
    }
}