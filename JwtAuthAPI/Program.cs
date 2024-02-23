using System.Text;
using JwtAuthAPI.Data;
using JwtAuthAPI.Data.Interfaces;
using JwtAuthAPI.Services;
using JwtAuthAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// Builder
var builder = WebApplication.CreateBuilder(args);

// Database settings (SqLite)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);

// Scopes settings
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Controllers settings
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JSON Web Tokens settings (JWT)
builder.Services.AddAuthorization();
builder
    .Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        string? SecretKey = builder.Configuration["Jwt:SecretKey"];

        if (string.IsNullOrEmpty(SecretKey))
        {
            string message = "JWT SecretKey is not set correctly.";
            throw new InvalidOperationException(message);
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var signingCredentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256Signature
        );

        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = signingKey,
        };
    });

// Build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
