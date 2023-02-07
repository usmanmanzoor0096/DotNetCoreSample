namespace  AuthService.Common.Models.ConfigModels
{
    public class BlobStorageConfig
    {
        public string BlobStorageConnectionString { get; set; }
        public string OriginalImagesContainer { get; set; }
        public string ReducedQualityImagesContainerName { get; set; }
        public string ThumbnailImagesContainerName { get; set; }
        public string CroppedImagesContainerName { get; set; }
        public string ProfileImagesContainerName { get; set; }
        public string ProfileBannersContainerName { get; set; }
        public string VideoContainerName { get; set; }
        public string AudioContainerName { get; set; }
        public string CdnUrl { get; set; }
        public string CategoryIconContainer { get; set; }
    }
}
