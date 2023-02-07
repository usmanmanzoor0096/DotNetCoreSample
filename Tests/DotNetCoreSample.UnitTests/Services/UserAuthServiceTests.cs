using Microsoft.Extensions.Options;
using Moq;
using AuthService.Models.Config;
using AuthService.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AuthService.Business.Services;
using AuthService.Data.IRepositories;
using AuthService.Services.Interfaces; 
using System.Transactions; 
using AuthService.Models.RequestModels; 
using Xunit;

namespace Transaction.UnitTests.Services
{
    //[ExcludeFromCodeCoverage]
    //[TestClass()]
    public class UserAuthServiceTests
    {
        private Mock<ILogger<UserAuthService>> _logger;
        private Mock<IOptions<AuthOptions>> _options; 
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<IUserRepository> _userRepository;
        private Mock<IRefreshTokenRepository> _refreshTokenRepository;
        private Mock<IEmailService> _emailService;
        private UserAuthService _userAuthService;
         

        public UserAuthServiceTests()
        {
            _options = new Mock<IOptions<AuthOptions>>();
            _logger = new Mock<ILogger<UserAuthService>>();
            _userManager = new Mock<UserManager<ApplicationUser>>();
            _userRepository = new Mock<IUserRepository>();
            _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _emailService = new Mock<IEmailService>();
            _userAuthService = new UserAuthService(_logger.Object, _options.Object,_userManager.Object,_userRepository.Object,_refreshTokenRepository.Object,_emailService.Object);
        }

        //[TestInitialize]
        //public void TestInitialize()
        //{
        //    try
        //    {
        //        _options = new Mock<IOptions<AuthOptions>>();
        //        _logger = new Mock<ILogger<UserAuthService>>();
        //        //_userManager = new Mock<UserManager<ApplicationUser>>();
        //        _userRepository = new Mock<IUserRepository>();
        //        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        //        _emailService = new Mock<IEmailService>();
        //        _userAuthService = new UserAuthService(_logger.Object, _options.Object, UserManager<ApplicationUser>(), _userRepository.Object, _refreshTokenRepository.Object, _emailService.Object);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        //[TestMethod()]
        [Fact]
        public async Task RegisterUserAsync_Test_WithSuccessResponse()
        {
            using (var scope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions()
            {
                //ENABLE LIKED SERVER READ COMMITT DATA.
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
            }))
            {
                //ARRANGE 
                _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());
                _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());


                //ACT
                var mockResponse = await _userAuthService.RegisterUserAsync(new UserRegisterRequest()
                {
                    ClientId = "1234569564564",
                    FirstName = "Test",
                    LastName = "User",
                    Password = "DotNetCoreSample@123",
                    ConfirmPassword = "DotNetCoreSample@123",
                    UserEmail = "test@test.com",
                    UserRole = AuthService.Models.Enum.Roles.USER,
                    RememberMe = true
                });

                //ASSERT
                Assert.True(mockResponse.Succeeded);
            }
        }  

    }
}
