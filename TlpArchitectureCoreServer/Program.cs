using TlpArchitectureCore.Extensions;
using TlpArchitectureCore.Options;
using TlpArchitectureCore.Services;
using TlpArchitectureCoreServer.Options;
using TlpArchitectureCoreServer.Services;

var builder = WebApplication.CreateBuilder(args);

var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb") ??
    throw new NullReferenceException("MongoDb connection string is not set");

var authSection = builder.Configuration.GetSection("AuthOptions");
var authOptions = authSection.Get<AuthOptions>() ??
    throw new NullReferenceException("AuthOptions is not set");

var hostingOptionsSection = builder.Configuration.GetSection(nameof(HostingOptions));

builder.Services.Configure<HostingOptions>(hostingOptionsSection);

builder.Services.AddMongoDb(mongoDbConnectionString);
builder.Services.AddRabbitMqConnection();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication();
builder.Services.AddAuthentication()
    .AddJwtBearer(jwtOptions =>
    {
        jwtOptions.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = authOptions.SymmetricSecurityKey,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.Configure<AuthOptions>(authSection);

builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<IPasswordHasher,PasswordHasher>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectRequestService, ProjectRequestService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
