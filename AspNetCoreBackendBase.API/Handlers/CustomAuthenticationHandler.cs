using AspNetCoreBackendBase.Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AspNetCoreBackendBase.API.Handlers
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Token is missing");
            }

            // Validate the token using the TokenService
            var principal = ValidateToken(token);

            if (principal == null)
            {
                return AuthenticateResult.Fail("Invalid Custom Authentication Token");
            }
            // Create an authentication ticket if validation succeeds
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            Response.ContentType = "application/json";

            // Custom error message based on failure reason
            string errorMessage = "";

            // Pass custom message set by AuthenticateResult.Fail
            if (properties.Items.TryGetValue(".AuthResult", out var resultMessage))
                errorMessage = resultMessage ?? "You are not authorized to access this resource.";

            // Create a custom response object
            var result = new BaseResponse<object>
            {
                IsSuccessful = false,
                Message = errorMessage,
            };

            // Serialize the response into JSON
            var jsonResponse = JsonConvert.SerializeObject(result);

            // Write the JSON response to the body
            await Response.WriteAsync(jsonResponse);
        }

        // Method to validate a JWT token
        public ClaimsPrincipal ValidateToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                throw new Exception($"CreateTokenValidationParameters(): TokenValidationParameters");
                //return tokenHandler.ValidateToken(accessToken, CreateTokenValidationParameters(), out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                // Log or handle validation errors (for example, expired token, invalid signature, etc.)
                throw new Exception($"Token validation failed: {ex.Message}");
            }
        }
    }
}
