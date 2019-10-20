namespace com.skibitsky.UnityNow
{
    public class DeploymentFile
    {
        public long FileSize { get; set; }

        public string RelativePath { get; set; }

        public string Checksum { get; set; }

        public byte[] Data { get; set; }
    }
}