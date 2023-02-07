using Amazon.S3.Model;
using  AuthService.Common.GenericResponses;

namespace  AuthService.Common.Communication.AWS.S3Services.Bucket
{
    public interface IBucketService
    {
        Task<GenericApiResponse<PutBucketResponse>> CreateBucketAsync(string bucketName);
        Task<GenericApiResponse<ListBucketsResponse>> GetAllBucketAsync();
        Task<GenericApiResponse<DeleteBucketResponse>> DeleteBucketAsync(string bucketName);
        Task<GenericApiResponse<S3Bucket>> GetBucketByNameAsync(string bucketName);
    }
}
