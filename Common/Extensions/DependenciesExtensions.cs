using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Ressource_API.Common.BlobStorage.Cloudflare;
using Ressource_API.Common.Data;
using Ressource_API.Common.Filters;
using Ressource_API.Common.Services;
using Ressource_API.Common.Services.EmailService;
using Ressource_API.Common.Utils;
using Ressource_API.Features.Addresses.Repositories;
using Ressource_API.Features.Addresses.Factories;
using Ressource_API.Features.Articles.Repositories;
using Ressource_API.Features.Articles.Services;
using Ressource_API.Features.Authentifications.Services;
using Ressource_API.Features.BackofficeLogLevels.Repositories;
using Ressource_API.Features.BackofficeLogLevels.Services;
using Ressource_API.Features.BackofficeLogLevels.Factories;
using Ressource_API.Features.BackofficeLogs.Repositories;
using Ressource_API.Features.BackofficeLogs.Services;
using Ressource_API.Features.BackofficeLogs.Factories;
using Ressource_API.Features.BackofficeOperationTypes.Repositories;
using Ressource_API.Features.BackofficeOperationTypes.Services;
using Ressource_API.Features.BackofficeOperationTypes.Factories;
using Ressource_API.Features.Cities.Repositories;
using Ressource_API.Features.Cities.Factories;
using Ressource_API.Features.Comments.Repositories;
using Ressource_API.Features.Comments.Services;
using Ressource_API.Features.Comments.Factories;
using Ressource_API.Features.Departments.Repositories;
using Ressource_API.Features.Departments.Services;
using Ressource_API.Features.Departments.Factories;
using Ressource_API.Features.EmailLogs.Repositories;
using Ressource_API.Features.EmailLogs.Services;
using Ressource_API.Features.EmailLogs.Factories;
using Ressource_API.Features.Events.Repositories;
using Ressource_API.Features.Events.Services;
using Ressource_API.Features.FriendsRequests.Repositories;
using Ressource_API.Features.FriendsRequests.Services;
using Ressource_API.Features.FriendsRequests.Factories;
using Ressource_API.Features.HealthChecks.Services;
using Ressource_API.Features.Logins.Repositories;
using Ressource_API.Features.Logins.Services;
using Ressource_API.Features.Logins.Factories;
using Ressource_API.Features.Notifications.Repositories;
using Ressource_API.Features.Notifications.Services;
using Ressource_API.Features.Notifications.Factories;
using Ressource_API.Features.PasswordHistories.Repositories;
using Ressource_API.Features.PasswordHistories.Services;
using Ressource_API.Features.PasswordInfos.Factories;
using Ressource_API.Features.PasswordInfos.Repositories;
using Ressource_API.Features.PollOptions.Repositories;
using Ressource_API.Features.PollOptions.Services;
using Ressource_API.Features.PollOptions.Factories;
using Ressource_API.Features.Polls.Repositories;
using Ressource_API.Features.Polls.Services;
using Ressource_API.Features.Polls.Factories;
using Ressource_API.Features.ProfilePictures.Repositories;
using Ressource_API.Features.ProfilePictures.Services;
using Ressource_API.Features.ProfilePictures.Factories;
using Ressource_API.Features.Quizzes.Repositories;
using Ressource_API.Features.Quizzes.Factories;
using Ressource_API.Features.QuizzQuestions.Factories;
using Ressource_API.Features.QuizzQuestions.Repositories;
using Ressource_API.Features.QuizzQuestions.Services;
using Ressource_API.Features.RefreshTokens.Repositories;
using Ressource_API.Features.RefreshTokens.Services;
using Ressource_API.Features.RefreshTokens.Factories;
using Ressource_API.Features.Regions.Repositories;
using Ressource_API.Features.Regions.Services;
using Ressource_API.Features.Regions.Factories;
using Ressource_API.Features.Reports.Repositories;
using Ressource_API.Features.Reports.Services;
using Ressource_API.Features.Reports.Factories;
using Ressource_API.Features.ReportTypes.Repositories;
using Ressource_API.Features.ReportTypes.Services;
using Ressource_API.Features.RessourceConfidentialityTypes.Repositories;
using Ressource_API.Features.RessourceConfidentialityTypes.Services;
using Ressource_API.Features.RessourceMedias.Repositories;
using Ressource_API.Features.RessourceMedias.Services;
using Ressource_API.Features.RessourceProgressions.Repositories;
using Ressource_API.Features.RessourceProgressions.Services;
using Ressource_API.Features.RessourceProgressions.Factories;
using Ressource_API.Features.Ressources.Repositories;
using Ressource_API.Features.Ressources.Services;
using Ressource_API.Features.RessourceStatuses.Repositories;
using Ressource_API.Features.RessourceStatuses.Services;
using Ressource_API.Features.RessourceStatuses.Factories;
using Ressource_API.Features.RessourceTypes.Repositories;
using Ressource_API.Features.RessourceTypes.Services;
using Ressource_API.Features.SessionMessages.Repositories;
using Ressource_API.Features.SessionMessages.Services;
using Ressource_API.Features.SessionMessages.Factories;
using Ressource_API.Features.Sessions.Repositories;
using Ressource_API.Features.Sessions.Services;
using Ressource_API.Features.Sessions.Factories;
using Ressource_API.Features.Tags.Repositories;
using Ressource_API.Features.Tags.Services;
using Ressource_API.Features.Tags.Factories;
using Ressource_API.Features.UserRoles.Repositories;
using Ressource_API.Features.UserRoles.Services;
using Ressource_API.Features.UserRoles.Factories;
using Ressource_API.Features.Users.Repositories;
using Ressource_API.Features.Users.Services;
using Ressource_API.Features.Users.Factories;
using Simply.Auth.Argon2.Configuration;
using Simply.Auth.Argon2.Services;
using Simply.Auth.AspNetCore.Extensions;
using Simply.Auth.Core.Abstractions;

namespace Ressource_API.Common.Extensions;

public static class DependenciesExtensions
{
    public static void InjectDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
                options.Filters.Add<ValidationFilter>()) //Applique le result pattern pour les exceptions de validators
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            }); // Force le snake cae sur le retour des formats JSON
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter =
                true; // Désactive le comportement par défaut des erreurs de validation
        });
        builder.Services.AddHttpContextAccessor();
        builder.AddSimply(65536, 3, 4, "ressource-api", "ressource-front");
        builder.AddRepositories();
        builder.AddServices();
        builder.AddFactories();
        builder.AddSwagger();
        builder.AddEfCoreConfiguration();
        builder.AddHybridCache();
    }

    private static void AddSimply(this WebApplicationBuilder builder, int memorySize, int iterations,
        int parallelismDegree, string issuer, string audience)
    {
        string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                           ?? throw new Exception("JWT_SECRET not found");


        builder.Services.AddSimplyAuth(
            argon2 =>
            {
                argon2.MemorySize = memorySize;
                argon2.Iterations = iterations;
                argon2.DegreeOfParallelism = parallelismDegree;
            },
            jwt =>
            {
                jwt.SecretKey = jwtSecret;
                jwt.Issuer = issuer;
                jwt.Audience = audience;
            });

        var descriptor = builder.Services.FirstOrDefault(d =>
            d.ServiceType == typeof(ISimplyPasswordHasher));

        if (descriptor != null)
            builder.Services.Remove(descriptor);

        builder.Services.AddSingleton<ISimplyPasswordHasher>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<SimplyArgon2Options>>();
            return new SimplyArgon2Hasher(options);
        });
    }

    private static void AddFactories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAddressFactory, AddressFactory>();
        builder.Services.AddScoped<IBackofficeLogFactory, BackofficeLogFactory>();
        builder.Services.AddScoped<IBackofficeLogLevelFactory, BackofficeLogLevelFactory>();
        builder.Services.AddScoped<IBackofficeOperationTypeFactory, BackofficeOperationTypeFactory>();
        builder.Services.AddScoped<ICityFactory, CityFactory>();
        builder.Services.AddScoped<ICommentFactory, CommentFactory>();
        builder.Services.AddScoped<IDepartmentFactory, DepartmentFactory>();
        builder.Services.AddScoped<IEmailLogFactory, EmailLogFactory>();
        builder.Services.AddScoped<IFriendsRequestFactory, FriendsRequestFactory>();
        builder.Services.AddScoped<ILoginFactory, LoginFactory>();
        builder.Services.AddScoped<INotificationFactory, NotificationFactory>();
        builder.Services.AddScoped<IPollOptionFactory, PollOptionFactory>();
        builder.Services.AddScoped<IPollFactory, PollFactory>();
        builder.Services.AddScoped<IProfilePictureFactory, ProfilePictureFactory>();
        builder.Services.AddScoped<IPasswordInfoFactory, PasswordInfoFactory>();
        builder.Services.AddScoped<IQuizzFactory, QuizzFactory>();
        builder.Services.AddScoped<IQuizzQuestionFactory, QuizzQuestionFactory>();
        builder.Services.AddScoped<IRefreshTokenFactory, RefreshTokenFactory>();
        builder.Services.AddScoped<IRegionFactory, RegionFactory>();
        builder.Services.AddScoped<IReportFactory, ReportFactory>();
        builder.Services.AddScoped<IRessourceProgressionFactory, RessourceProgressionFactory>();
        builder.Services.AddScoped<IRessourceStatusFactory, RessourceStatusFactory>();
        builder.Services.AddScoped<ISessionMessageFactory, SessionMessageFactory>();
        builder.Services.AddScoped<ISessionFactory, SessionFactory>();
        builder.Services.AddScoped<ITagFactory, TagFactory>();
        builder.Services.AddScoped<IUserRoleFactory, UserRoleFactory>();
        builder.Services.AddScoped<IUserFactory, UserFactory>();
    }

    private static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IArticleService, ArticleService>();
        builder.Services.AddScoped<IBackofficeLogService, BackofficeLogService>();
        builder.Services.AddScoped<IBackofficeLogLevelService, BackofficeLogLevelService>();
        builder.Services.AddScoped<IBackofficeOperationTypeService, BackofficeOperationTypeService>();
        builder.Services.AddScoped<ICommentService, CommentService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();
        builder.Services.AddScoped<IEmailLogService, EmailLogService>();
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<IFriendsRequestService, FriendsRequestService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();
        builder.Services.AddScoped<IPasswordHistoryManager, PasswordHistoryManager>();
        builder.Services.AddScoped<IPollOptionService, PollOptionService>();
        builder.Services.AddScoped<IPollService, PollService>();
        builder.Services.AddScoped<IProfilePictureService, ProfilePictureService>();
        builder.Services.AddScoped<IQuizzQuestionService, QuizzQuestionService>();
        builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        builder.Services.AddScoped<IRegionService, RegionService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<IReportTypeService, ReportTypeService>();
        builder.Services.AddScoped<IRessourceConfidentialityTypeService, RessourceConfidentialityTypeService>();
        builder.Services.AddScoped<IRessourceMediaService, RessourceMediaService>();
        builder.Services.AddScoped<IRessourceProgressionService, RessourceProgressionService>();
        builder.Services.AddScoped<IRessourceService, RessourceService>();
        builder.Services.AddScoped<IRessourceStatusService, RessourceStatusService>();
        builder.Services.AddScoped<IRessourceTypeService, RessourceTypeService>();
        builder.Services.AddScoped<ISessionMessageService, SessionMessageService>();
        builder.Services.AddScoped<ISessionService, SessionService>();
        builder.Services.AddScoped<ITagService, TagService>();
        builder.Services.AddScoped<IUserRoleService, UserRoleService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IHealthCheckService, HealthCheckService>();
        builder.Services.AddScoped<IAuthentificationService, AuthentificationService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IEmailSender, EmailSender>();
        builder.Services.AddScoped<ICloudflareClient, CloudflareClient>();
    }

    private static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
        builder.Services.AddScoped<IBackofficeLogRepository, BackofficeLogRepository>();
        builder.Services.AddScoped<IBackofficeLogLevelRepository, BackofficeLogLevelRepository>();
        builder.Services.AddScoped<IBackofficeOperationTypeRepository, BackofficeOperationTypeRepository>();
        builder.Services.AddScoped<ICityRepository, CityRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddScoped<IEmailLogRepository, EmailLogRepository>();
        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<IFriendsRequestRepository, FriendsRequestRepository>();
        builder.Services.AddScoped<ILoginRepository, LoginRepository>();
        builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        builder.Services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>();
        builder.Services.AddScoped<IPasswordInfoRepository, PasswordInfoRepository>();
        builder.Services.AddScoped<IPollOptionRepository, PollOptionRepository>();
        builder.Services.AddScoped<IPollRepository, PollRepository>();
        builder.Services.AddScoped<IProfilePictureRepository, ProfilePictureRepository>();
        builder.Services.AddScoped<IQuizzRepository, QuizzRepository>();
        builder.Services.AddScoped<IQuizzQuestionRepository, QuizzQuestionRepository>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IRegionRepository, RegionRepository>();
        builder.Services.AddScoped<IReportRepository, ReportRepository>();
        builder.Services.AddScoped<IReportTypeRepository, ReportTypeRepository>();
        builder.Services.AddScoped<IRessourceConfidentialityTypeRepository, RessourceConfidentialityTypeRepository>();
        builder.Services.AddScoped<IRessourceMediaRepository, RessourceMediaRepository>();
        builder.Services.AddScoped<IRessourceProgressionRepository, RessourceProgressionRepository>();
        builder.Services.AddScoped<IRessourceRepository, RessourceRepository>();
        builder.Services.AddScoped<IRessourceStatusRepository, RessourceStatusRepository>();
        builder.Services.AddScoped<IRessourceTypeRepository, RessourceTypeRepository>();
        builder.Services.AddScoped<ISessionMessageRepository, SessionMessageRepository>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();
        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddJwt(this WebApplicationBuilder builder)
    {
        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
                        throw new InvalidOperationException("JWT secret 'JWT_SECRET' not found.");

        var key = Encoding.ASCII.GetBytes(jwtSecret);

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // True in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services.AddAuthorization();
    }

    private static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Ressource_relationnelles", Version = "v1" });
            options.AddSecurityDefinition("api-key", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Name = "x-api-key",
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("api-key", document)] = []
            });
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
    }

    private static void AddEfCoreConfiguration(this WebApplicationBuilder builder)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'DATABASE_CONNECTION_STRING' not found.");

        var poolSize = Int32.Parse(Environment.GetEnvironmentVariable("DBCONTEXT_POOL_SIZE") ?? "1024");

        builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    // Optimisation pour les tests de charge
                    npgsqlOptions.EnableRetryOnFailure(5);
                }),
            poolSize: poolSize
        );
    }

    private static void AddCorsConfiguration(this WebApplicationBuilder builder)
    {
        var clientUrl = Environment.GetEnvironmentVariable("URL_CLIENT") ??
                        throw new InvalidOperationException("Client app URL 'URL_CLIENT' not found.");

        var backofficeUrl = Environment.GetEnvironmentVariable("URL_BACKOFFICE") ??
                            throw new InvalidOperationException("Backlog URL 'URL_BACKOFFICE' not found.");

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowClientApp",
                policy =>
                {
                    policy.WithOrigins(clientUrl)
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            options.AddPolicy("AllowBackLog",
                policy =>
                {
                    policy.WithOrigins(backofficeUrl)
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    private static void AddHybridCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions()
            {
                Expiration = TimeSpan.FromMinutes(10),
                LocalCacheExpiration = TimeSpan.FromMinutes(10)
            };
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
            options.Configuration = redisConnectionString;
        });
    }
}