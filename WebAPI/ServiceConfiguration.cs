using Hangfire;
using Infrastructure.Context;
using Infrastructure.Repository.Implement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Repository.Abstract;
using Repository.Implement;
using Service.Abstract;
using Service.Implement;
using System.Text;
using System.Text.Json.Serialization;
using WebAPI.Middlewares;

namespace WebAPI
{
    public static class ServiceConfiguration
    {
        private const string CONNECTION_STRING_MODULE_CARD = "CardModule-dev";
        public static void ConfigurateMSSQLContext(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ModuleCardDbContext>(
                opt => opt.UseSqlServer(configuration.GetConnectionString(CONNECTION_STRING_MODULE_CARD)));
        }

        public static void RegisterDI(this IServiceCollection service)
        {
            service.AddScoped<ICategoryRepository, CategoryRepository>();
            service.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            service.AddScoped<ISampleGreetingRepository, SampleGreetingRepository>();
            service.AddScoped<ITemplateCardRepository, TemplateCardRepository>();
            service.AddScoped<IUserCardRepository, UserCardRepository>();
            service.AddScoped<IUserInfoRepository, UserInfoRepository>();
            service.AddScoped<IUserUploadRepository, UserUploadRepository>();
            service.AddScoped<ITagRepository, TagRepository>();

            service.AddScoped<ICategoryService, CategoryService>();
            service.AddScoped<IRefreshTokenService, RefreshTokenService>();
            service.AddScoped<ISampleGreetingService, SampleGreetingService>();
            service.AddScoped<ITemplateCardService, TemplateCardService>();
            service.AddScoped<IUserCardService, UserCardService>();
            service.AddScoped<IUserInfoService, UserInfoService>();
            service.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            service.AddScoped<IUserUploadService, UserUploadService>();
            service.AddTransient<AdminAuthorizationFilter>();
            service.AddTransient<AuthenticationFilter>();
        }

        public static void ConfigureAuthentication(this IServiceCollection service, IConfiguration configuration)
        {
            var secretKey = configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey!);

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }

        public static void ConfigureNewtonsoftJson(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllers().AddJsonOptions(x =>
                            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        }

        public static void ConfigureHangFire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString(CONNECTION_STRING_MODULE_CARD)));

            services.AddHangfireServer();
        }

        public static void RegisterCORS(this IServiceCollection service)
        {
            service.AddCors(p => p.AddPolicy("MyCors", build =>
            {
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));
        }

    }
}
