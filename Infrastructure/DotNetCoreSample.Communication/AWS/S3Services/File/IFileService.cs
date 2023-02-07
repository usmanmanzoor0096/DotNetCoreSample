using Amazon.S3.Model;
using  AuthService.Common.GenericResponses;
using  AuthService.Common.Models.AWS.DTOs;
using  AuthService.Common.Models.AWS.RequestModels;

namespace  AuthService.Common.Communication.AWS.S3Services.File
{
    public interface IFileService
    {
        Task<GenericApiResponse<PutObjectResponse>> UploadFileAsync(AWSS3RequestModel aWSS3RequestModel);
        Task<GenericApiResponse<IEnumerable<S3ObjectDto>>> GetAllFilesAsync(AWSS3RequestModel aWSS3RequestModel);
        Task<GenericApiResponse<GetObjectResponse>> GetFileByKeyAsync(AWSS3RequestModel aWSS3RequestModel);
        Task<GenericApiResponse<DeleteObjectResponse>> DeleteFileAsync(AWSS3RequestModel aWSS3RequestModel);
    }
}
