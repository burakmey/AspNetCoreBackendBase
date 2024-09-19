using Application.Helpers;
using AspNetCoreBackendBase.Application;
using AspNetCoreBackendBase.Application.CQRS.Commands.AuthCommands;
using AspNetCoreBackendBase.Application.DTOs;
using AspNetCoreBackendBase.Application.DTOs.Auth;
using AspNetCoreBackendBase.Application.Exceptions;
using AspNetCoreBackendBase.Application.Services;
using AspNetCoreBackendBase.Domain.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreBackendBase.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly ITokenService _tokenService;
        readonly IMailService _mailService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IMailService mailService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        public async Task<BaseResponse<LoginCommandResponse>> LoginAsync(LoginCommandRequest model)
        {
            // Attempt to find the user by userName.
            User? user = await _userManager.FindByNameAsync(model.UsernameOrEmail);

            // If the user is not found by username, attempt to find by email.
            user ??= await _userManager.FindByEmailAsync(model.UsernameOrEmail);

            // If the user is still not found, throw an exception.
            if (user == null)
                throw new UserNotFoundException();

            // Check the provided password against the user account.
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            // If the password check is successful.
            if (result.Succeeded)
            {
                // Generate a new access token for the user.
                Token token = _tokenService.CreateAccessToken(user);

                // Update the user's refresh token in the database.
                await UpdateRefreshTokenAsync(token.RefreshToken, user, token.AccessTokenExpiration);

                // Return a successful response with the generated token.
                return new()
                {
                    IsSuccessful = true,
                    Data = new() { Token = token },
                    Message = "User login successfully."
                };
            }
            else
                // If the password check fails, throw an exception
                throw new InvalidLoginException();
        }

        public async Task<BaseResponse<LoginGoogleCommandResponse>> GoogleLoginAsync(LoginGoogleCommandRequest model)
        {
            // Define settings for Google token validation, specifying the expected audience.
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { Configuration.GetExternalLoginGoogleClientId }
            };

            // Validate the provided Google ID token and get the payload containing user information.
            var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);

            // Create a UserLoginInfo object to represent the Google login.
            var info = new UserLoginInfo(model.Provider, payload.Subject, model.Provider);

            // Attempt to find the user by their Google login info.
            User? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            // Check if the user was found.
            bool result = user != null;

            // If the user was not found by Google login info, try to find the user by email.
            if (user == null)
            {
                // If the user is still not found, create a new user.
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        Name = model.Name,
                        Surname = model.Surname
                    };
                    // Create the new user in the system.
                    var identityResult = await _userManager.CreateAsync(user);

                    // Check if user creation succeeded.
                    result = identityResult.Succeeded;
                }
            }

            // If the user exists or was created successfully, add the Google login information to the user.
            if (result)
                await _userManager.AddLoginAsync(user, info);
            else
                // If user creation or login association failed, throw an exception.
                throw new GoogleLoginInvalidAuthenticationException();

            // Generate a new access token for the user.
            Token token = _tokenService.CreateAccessToken(user);

            // Update the user's refresh token in the database.
            await UpdateRefreshTokenAsync(token.RefreshToken, user, token.AccessTokenExpiration);

            // Return a successful response with the generated token.
            return new()
            {
                IsSuccessful = true,
                Data = new() { Token = token },
                Message = "User login successfully."
            };
        }

        public async Task<BaseResponse<RefreshCommandResponse>> RefreshAsync(RefreshCommandRequest model)
        {
            // Attempt to find the user based on the provided refresh token.
            User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == model.RefreshToken);

            // Check if the user was found and if the refresh token is still valid.
            if (user != null && user.RefreshTokenExpiration > DateTime.UtcNow)
            {
                // Create a new access token for the user.
                Token token = _tokenService.CreateAccessToken(user);

                // Update the user's refresh token in the database.
                await UpdateRefreshTokenAsync(token.RefreshToken, user, token.AccessTokenExpiration);

                // Return a successful response with the new generated token.
                return new()
                {
                    IsSuccessful = true,
                    Data = new() { Token = token },
                    Message = "User refresh successfully."
                };
            }
            else
            {
                // If no user was found with the provided refresh token, throw exception.
                if (user == null)
                    throw new UserNotFoundException();

                // If user not null, then the refresh token has expired, throw exception.
                else
                    throw new RefreshTokenExpiredException();
            }
        }

        public async Task<BaseResponse<object>> ResetPasswordAsync(ResetPasswordCommandRequest model)
        {
            // Find the user by email address from the request model.
            User? user = await _userManager.FindByEmailAsync(model.Email);

            // If no user is found with the provided email address, throw exception.
            if (user == null)
                throw new UserNotFoundException();

            // Generate a password reset token for the user.
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode the token to make it URL-safe (e.g., for use in a URL).
            resetToken = resetToken.UrlEncode();

            // Send a password reset email to the user with the reset token.
            await _mailService.SendPasswordResetMailAsync(model.Email, user.Id, resetToken);

            // Return a response indicating that the operation was successful.
            return new()
            {
                IsSuccessful = true,
                Message = "Email sent if the address exists."
            };
        }

        public async Task<BaseResponse<object>> VerifyResetTokenAsync(VerifyResetTokenCommandRequest model)
        {
            // Retrieve the user by their ID from the request model.
            User? user = await _userManager.FindByIdAsync(model.UserId);

            // If no user is found with the given ID, throw exception.
            if (user == null)
                throw new UserNotFoundException("There is no user with the given Id.");

            // Decode the reset token from the request model.
            string resetToken = model.ResetToken.UrlDecode();

            // Verify the reset token for the user using the token provider and purpose 'ResetPassword'.
            bool isVerified = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);

            // Return a response indicating whether the token was successfully verified or not.
            return new()
            {
                IsSuccessful = isVerified,
                Message = isVerified ? "Reset token verified successfully." : "Invalid reset token."
            };
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, User user, DateTime accessTokenExpiration)
        {
            // Retrieve the additional minutes to extend the refresh token expiration from configuration.
            int additionalMinute = int.Parse(Configuration.GetTokenRefreshTokenMinute);

            // Update the user's refresh token and its expiration time.
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = accessTokenExpiration.AddMinutes(additionalMinute);

            // Save the updated user information in the database.
            await _userManager.UpdateAsync(user);
        }
    }
}
