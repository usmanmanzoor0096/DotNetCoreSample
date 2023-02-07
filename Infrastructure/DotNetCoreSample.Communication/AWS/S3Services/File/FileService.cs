using Amazon.S3;
using Amazon.S3.Model;
using  AuthService.Common.Extensions;
using  AuthService.Common.GenericResponses;
using  AuthService.Common.Models.AWS.DTOs;
using  AuthService.Common.Models.AWS.RequestModels;
using static  AuthService.Common.Enums.APIEnums;

namespace  AuthService.Common.Communication.AWS.S3Services.File
{
    public class FileService : IFileService
    {
        private readonly IAmazonS3 _s3Client;
        public FileService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        /// <summary>
        /// Delete a file from spcific bucket of S3
        /// </summary>
        /// <param name="aWSS3RequestModel"></param>
        /// <returns></returns>
        public async Task<GenericApiResponse<DeleteObjectResponse>> DeleteFileAsync(AWSS3RequestModel aWSS3RequestModel)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(aWSS3RequestModel.BucketName);
            if (!bucketExists)
                return GenericApiResponse<DeleteObjectResponse>.Failure(MessageResource.GetMessage(APIStatusCodes.S3BucketNotExist), APIStatusCodes.S3BucketNotExist);
            var result = await _s3Client.DeleteObjectAsync(aWSS3RequestModel.BucketName, aWSS3RequestModel.Key);
            return GenericApiResponse<DeleteObjectResponse>.Success(result, "");
        }
        /// <summary>
        /// Get all files from a spcific bucket
        /// </summary>
        /// <param name="aWSS3RequestModel"></param>
        /// <returns></returns>
        public async Task<GenericApiResponse<IEnumerable<S3ObjectDto>>> GetAllFilesAsync(AWSS3RequestModel aWSS3RequestModel)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(aWSS3RequestModel.BucketName);
            if (!bucketExists)
                return GenericApiResponse<IEnumerable<S3ObjectDto>>.Failure(MessageResource.GetMessage(APIStatusCodes.S3BucketNotExist), APIStatusCodes.S3BucketNotExist);
            var request = new ListObjectsV2Request()
            {
                BucketName = aWSS3RequestModel.BucketName,
                Prefix = aWSS3RequestModel.Prefix
            };
            var result = await _s3Client.ListObjectsV2Async(request);
            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = aWSS3RequestModel.BucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(1)
                };
                return new S3ObjectDto()
                {
                    Name = s.Key.ToString(),
                    PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
                };
            });
            return GenericApiResponse<IEnumerable<S3ObjectDto>>.Success(s3Objects, "");
        }
        /// <summary>
        /// Get file by key from S3 Bucket
        /// </summary>
        /// <param name="aWSS3RequestModel"></param>
        /// <returns></returns>
        public async Task<GenericApiResponse<GetObjectResponse>> GetFileByKeyAsync(AWSS3RequestModel aWSS3RequestModel)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(aWSS3RequestModel.BucketName);
            if (!bucketExists)
                return GenericApiResponse<GetObjectResponse>.Failure(MessageResource.GetMessage(APIStatusCodes.S3BucketNotExist), APIStatusCodes.S3BucketNotExist);
            var result = await _s3Client.GetObjectAsync(aWSS3RequestModel.BucketName, aWSS3RequestModel.Key);
            return GenericApiResponse<GetObjectResponse>.Success(result, "");
        }
        /// <summary>
        /// Upload file to a specific bucket of S3
        /// </summary>
        /// <param name="aWSS3RequestModel"></param>
        /// <returns></returns>
        public async Task<GenericApiResponse<PutObjectResponse>> UploadFileAsync(AWSS3RequestModel aWSS3RequestModel)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(aWSS3RequestModel.BucketName);
            if (!bucketExists)
                return GenericApiResponse<PutObjectResponse>.Failure(MessageResource.GetMessage(APIStatusCodes.S3BucketNotExist), APIStatusCodes.S3BucketNotExist);
            var request = new PutObjectRequest()
            {
                BucketName = aWSS3RequestModel.BucketName,
                Key = string.IsNullOrEmpty(aWSS3RequestModel.Prefix) ? aWSS3RequestModel.File.FileName : $"{aWSS3RequestModel.Prefix?.TrimEnd('/')}/{aWSS3RequestModel.File.FileName}",
                InputStream = aWSS3RequestModel.File.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", aWSS3RequestModel.File.ContentType);
            var result = await _s3Client.PutObjectAsync(request);
            return GenericApiResponse<PutObjectResponse>.Success(result, "");
        }
    }
}
