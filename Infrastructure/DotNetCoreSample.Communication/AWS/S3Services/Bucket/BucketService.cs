using Amazon.S3;
using Amazon.S3.Model;
using AuthService.Common.Communication.AWS.S3Services.Bucket;
using AuthService.Common.Extensions;
using AuthService.Common.GenericResponses;
using static AuthService.Common.Enums.APIEnums;

namespace Transaction.Infrastructure.Communication.AWS.S3Services.Bucket
{
    public class BucketService : IBucketService
    {
        private readonly IAmazonS3 _s3Client;
        public BucketService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        /// <summary>
        /// Create a bucket on S3
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<GenericApiResponse<PutBucketResponse>> CreateBucketAsync(string bucketName)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (bucketExists)
                return GenericApiResponse<PutBucketResponse>.Failure(MessageResource.GetMessage(APIStatusCodes.S3BucketAlreadyExist), APIStatusCodes.S3BucketAlreadyExist);
            var result = await _s3Client.PutBucketAsync(bucketName);
            return GenericApiResponse<PutBucketResponse>.Success(result, "");
        }
        /// <summary>
        /// Delete a bucket fromo S3
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<GenericApiResponse<DeleteBucketResponse>> DeleteBucketAsync(string bucketName)
        {
            var result= await _s3Client.DeleteBucketAsync(bucketName);
            return GenericApiResponse<DeleteBucketResponse>.Success(result, "");
        }
        /// <summary>
        /// Get all buckets from S3
        /// </summary>
        /// <returns></returns>
        public async Task<GenericApiResponse<ListBucketsResponse>> GetAllBucketAsync()
        {
            var result = await _s3Client.ListBucketsAsync();
            return GenericApiResponse<ListBucketsResponse>.Success(result, "");
        }
        /// <summary>
        /// Get Bucket by Name from S3
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<GenericApiResponse<S3Bucket>> GetBucketByNameAsync(string bucketName)
        {
            var data = await _s3Client.ListBucketsAsync();
            var buckets = data.Buckets.FirstOrDefault(x => x.BucketName == bucketName);
            return GenericApiResponse<S3Bucket>.Success(buckets, "");
        }
    }
}
