using Amazon.Runtime;
using Amazon.SecretsManager;
using AuthService.Common.HttpClient;
using AuthService.Common.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using static AuthService.Common.Enums.APIEnums;
using Amazon.Extensions.NETCore.Setup;
using Amazon;
using System.Reflection;
using AuthService.Common.Operation;
using AuthService.Common.Cache;
using AuthService.Common.Models.ConfigModels;
using AuthService.Common.Communication.EmailServices;
using AuthService.Common.Communication.AWS.S3Services.Bucket;
using Transaction.Infrastructure.Communication.AWS.S3Services.Bucket;
using AuthService.Common.Communication.AWS.S3Services.File;
using AuthService.Data.Context;
using Microsoft.AspNetCore.Identity;
using Amazon.S3;
using AuthService.Models.DBModels;
using System.Text.Json.Serialization;
using AuthService.Services.Interfaces;
using AuthService.Services.Services;
using AuthService.Data.IRepositories;
using AuthService.Data.Repositories;
using AuthService.Models.Config;
using AuthService.Business.Services;
using Newtonsoft.Json.Converters;
using AuthService.Communication.SQService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.API.Extensions
{
    public static class RegisterDISerices
    {
        public async static Task RegisterAllDIServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(
            options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.Converters.Add(new StringEnumConverter());
            });


            #region Identity 
            builder.Configuration.AddEnvironmentVariables();
            DotNetEnv.Env.TraversePath().Load(".env");
            string ConnectionString = String.Empty;
            string AuthServerUri = string.Empty;
            string BaseUri = string.Empty;
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (environment == "DEVELOPMENT")
            {
                ConnectionString = Environment.GetEnvironmentVariable("DEFAULTCONNECTION_AUTH");
                AuthServerUri = builder.Configuration["AppConfigs:AuthServer_Dev"];
                BaseUri = builder.Configuration["AppConfigs:BaseUri_Dev"];
            }
            else if (environment == "STAGING")
            {
                ConnectionString = Environment.GetEnvironmentVariable("STAGINGCONNECTION_AUTH");
                AuthServerUri = builder.Configuration["AppConfigs:AuthServer_Stage"];
                BaseUri = builder.Configuration["AppConfigs:BaseUri_Stage"];

            }
            else if (environment == "PROD")
            {
                ConnectionString = Environment.GetEnvironmentVariable("PRODCONNECTION_AUTH");
                AuthServerUri = builder.Configuration["AppConfigs:AuthServer_Prod"];
                BaseUri = builder.Configuration["AppConfigs:BaseUri_Prod"];

            }
            AppConfig appConfig = new AppConfig
            {
                BaseUri = BaseUri
            };
            builder.Services.AddSingleton(appConfig);

            //string ConnectionString = builder.Configuration.GetConnectionString("Default");

            // For Entity Framework  
            builder.Services.AddDbContextPool<SqlDBContext>(options => options.UseLazyLoadingProxies().UseSqlServer(ConnectionString));
            // For Identity  
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<SqlDBContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<RoleManager<IdentityRole>>();
            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            #endregion Identity

            // Adding Authentication  
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = builder.Configuration["JwtConfig:Audience"],
                    ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });




            #region Inject Configurations
            builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
            builder.Services.Configure<SendGridConfig>(builder.Configuration.GetSection("SendGridConfig"));
            builder.Services.Configure<TwilioConfig>(builder.Configuration.GetSection("TwilioConfig"));
            builder.Services.Configure<CircleConfig>(builder.Configuration.GetSection("CircleConfig"));
            //builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfigs"));
            builder.Services.Configure<GeneralConfig>(builder.Configuration.GetSection("GeneralConfig"));
            builder.Services.Configure<BlockChianConfig>(builder.Configuration.GetSection("BlockChianConfig"));
            builder.Services.Configure<AWSecretManagerConfig>(builder.Configuration.GetSection("AWSecretManagerConfig"));
            builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthOptions"));
            builder.Services.Configure<ClientAppOptions>(builder.Configuration.GetSection("ClientAppOptions"));
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(30));
            builder.Services.Configure<AWSQSConfigs>(builder.Configuration.GetSection("AWSQSConfigs"));
            #endregion Inject Configurations

            #region Inject Communication Services


            #region Inject Business Services 
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserAuthService, UserAuthService>();

            #endregion Inject Business Services 

            #region Inject Data Services
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            #endregion Inject Data Services



            //builder.Services.AddScoped<IHttpCall, HttpCall>();
            //builder.Services.AddScoped<ICirclePaymentService, CirclePaymentService>();

            builder.Services.AddScoped<IEmailSendGrid, EmailSendGrid>();
            builder.Services.AddScoped<IBucketService, BucketService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IUtilService, UtilService>();
            builder.Services.AddScoped<IHttpCall, HttpCall>();
            builder.Services.AddScoped<ISafeOperationExecutor, SafeOperationExecutor>();
            builder.Services.AddScoped<IDBOperationExecutor, DBOperationExecutor>();
            builder.Services.AddScoped<ICache, AzureRedisCache>();
            builder.Services.AddScoped<ISQSEmailService, SQSEmailService>();
            #endregion Inject Communication Services

            #region Swagger/SEQ Logging/HTTP Client and Others



            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(builder.Configuration.GetValue<string>("Swagger:Version"), new OpenApiInfo
                {
                    Version = builder.Configuration.GetValue<string>("Swagger:Version"),
                    Title = builder.Configuration.GetValue<string>("Swagger:Title"),
                    Description = builder.Configuration.GetValue<string>("Swagger:Description"),
                    TermsOfService = new Uri(builder.Configuration.GetValue<string>("Swagger:TermsOfService")),
                    Contact = new OpenApiContact
                    {
                        Name = builder.Configuration.GetValue<string>("Swagger:ContactName"),
                        Url = new Uri(builder.Configuration.GetValue<string>("Swagger:ContactUrl"))
                    },
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Example License",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            builder.Services.AddSwaggerGenNewtonsoftSupport(); // resolves slowness of swagger 
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
            });
            builder.Services.AddHttpClient(ClientType.LicenceNodeAPI.ToString(), httpClient =>
            {
                httpClient.BaseAddress = new Uri("SEQ - URL ");
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/json");
            });


            #region DotNetCoreSample API resolver 
            builder.Services.AddHttpClient(ClientType.DotNetCoreSample.ToString(), httpClient =>
            {
                httpClient.BaseAddress = new Uri(AuthServerUri);
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/json");
            });
            #endregion


            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();

            AWSOptions awsOptions = new AWSOptions
            {
                Credentials = new BasicAWSCredentials(builder.Configuration.GetValue<string>("AWSecretManagerConfig:AccessKey"), builder.Configuration.GetValue<string>("AWSecretManagerConfig:SecretAccessKey")),
                Region = RegionEndpoint.GetBySystemName(builder.Configuration.GetValue<string>("AWSecretManagerConfig:Region")),
            };
            builder.Services.AddDefaultAWSOptions(awsOptions);
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddAWSService<IAmazonSecretsManager>();

            #endregion Swagger/SEQ Logging/HTTP Client and Others
        }
    }
}
