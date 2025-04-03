// CleanUps.BusinessLogic.Services/AuthService.cs
using CleanUps.BusinessLogic.Interfaces.PublicAccess;
using CleanUps.BusinessLogic.Interfaces.PrivateAccess; // For IRepository
using CleanUps.BusinessLogic.Models;
using CleanUps.Shared.DTOs;
using CleanUps.Shared.ErrorHandling;
using Microsoft.Extensions.Configuration; // To read JWT settings
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CleanUps.BusinessLogic.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _context;

        private const string OrganizerRoleName = "Organizer";
        private const int OrganizerRoleId = 1;

        private const string VolunteerRoleName = "Volunteer";
        private const int VolunteerRoleId = 2;

        public AuthService( IConfiguration configuration, IAuthRepository context)
        {
            _configuration = configuration;
            _context = context; // Inject context
        }

        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginRequestDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Result<LoginResponseDTO>.BadRequest("Email and password are required.");
            }

            try
            {
                // 1. Find the user by email - Include Role!
                // IRepository might not support Include easily, so use DbContext directly here
                var user = await _context.Users
                                      .Include(u => u.RoleId) // Eager load the Role
                                      .SingleOrDefaultAsync(u => u.Email == request.Email);

                // 2. Check if user exists and if password is correct
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    // Generic message for security - don't reveal if email exists or password is wrong
                    return Result<LoginResponseDTO>.Unauthorized("Invalid email or password.");
                }

                // 3. Check if Role was loaded
                if (user.RoleId != OrganizerRoleId || user.RoleId != VolunteerRoleId)
                {
                    // This indicates a data integrity issue or configuration problem
                    // Log this error server-side
                    Console.WriteLine($"Error: Role not loaded for user {user.Email} (UserId: {user.UserId}, RoleId: {user.RoleId})");
                    return Result<LoginResponseDTO>.InternalServerError("Login failed due to server configuration issue.");
                }


                // 4. Generate JWT
                var token = GenerateJwtToken(user);

                // 5. Create Response DTO
                var response = new LoginResponseDTO(
                    Token: token,
                    Email: user.Email,
                    Name: user.Name,
                    Role: user.RoleId == OrganizerRoleId ? OrganizerRoleName : VolunteerRoleName // Get role name from the loaded Role entity
                );

                return Result<LoginResponseDTO>.Ok(response);
            }
            catch (Exception ex)
            {
                // Log the exception ex
                Console.WriteLine($"Login Error: {ex}"); // Replace with proper logging
                return Result<LoginResponseDTO>.InternalServerError("An error occurred during login.");
            }
        }


        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var duration = double.Parse(jwtSettings["DurationInMinutes"] ?? "60"); // Default 60 mins

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT SecretKey is not configured.");
            }


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create Claims
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // Subject = UserId
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name), // Or use given_name etc.
            new Claim(ClaimTypes.Role, user.RoleId == OrganizerRoleId ? OrganizerRoleName : VolunteerRoleName), // Add Role claim
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token identifier
        };

            // Create Token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(duration),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Optional: Implement RegisterAsync if needed
        // public async Task<Result<UserDTO>> RegisterAsync(UserCreateDTO request) { ... }
    }
}
