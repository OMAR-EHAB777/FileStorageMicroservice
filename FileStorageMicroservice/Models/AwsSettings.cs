namespace FileStorageMicroservice.Models
{
    public class AwsSettings
    {
        public string Region { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }

}
