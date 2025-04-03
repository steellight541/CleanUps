using CleanUps.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// **** START JWT CONFIGURATION ****
// 1. Bind JwtSettings from appsettings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
{
    // Add more robust validation in production
    throw new InvalidOperationException("JWT SecretKey is missing or too short in configuration. Ensure it is set securely and is at least 32 characters long.");
}


// 2. Add Authentication Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        // You might add ClockSkew = TimeSpan.Zero if precise expiry is needed
    };
});

// 3. Add Authorization Services (including Role-based)
builder.Services.AddAuthorization(options =>
{
    // Define policies if needed for more complex scenarios,
    // but basic role checks can use [Authorize(Roles = "...")] directly.
    options.AddPolicy("OrganizerOnly", policy => policy.RequireRole("Organizer"));
    options.AddPolicy("VolunteerOrOrganizer", policy => policy.RequireRole("Volunteer", "Organizer"));
    // Add more policies as needed
});
// **** END JWT CONFIGURATION ****

// Register dependencies from CleanUps.Configuration
builder.Services.AddAppDependencies(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **** IMPORTANT: Add Authentication and Authorization Middleware ****
// Must be between UseRouting() (implicitly added by MapControllers) and UseEndpoints() (implicitly added by MapControllers)
// And crucially, UseAuthentication MUST come before UseAuthorization
app.UseAuthentication(); // Authenticates the user based on the token (Metafor: The gaurd at the front-gate, to authenticate if you enter the caste)
app.UseAuthorization();  // Authorizes the user to access resources (Metafor: Which rooms/halls you have access to inside the castle, depending on your rank/role)

app.UseCors("AllowAny");

app.MapControllers();

app.Run();
