using Hangfire;
using Microsoft.OpenApi.Models;
using WebAPI;
using WebAPI.HangFireJob;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new ResponseFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement

                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
    option.EnableAnnotations();
});


ServiceConfiguration.ConfigurateMSSQLContext(builder.Services, builder.Configuration);
ServiceConfiguration.RegisterDI(builder.Services);
ServiceConfiguration.ConfigureAuthentication(builder.Services, builder.Configuration);
ServiceConfiguration.ConfigureNewtonsoftJson(builder.Services);
ServiceConfiguration.ConfigureHangFire(builder.Services, builder.Configuration);
ServiceConfiguration.RegisterCORS(builder.Services);

var app = builder.Build();

app.UseHangfireDashboard();
// "0 0 0 * * *" la bieu thuc Cron
RecurringJob.AddOrUpdate<RefreshTokenCleanupJob>("cleanRefreshTokenJob", x => x.CleanupExpriredRefreshToken(), "0 0 0 * * *");

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (true)//app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("MyCors");

app.Run();
