using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Reservmed.Common;
using Reservmed.DTOs;
using Reservmed.DTOs.Internal;
using Reservmed.Models.Identity;
using Reservmed.Services.Interfaces;
using System.Security.Claims;
using System.Text;

namespace Reservmed.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser?> GetUserIdentityAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        private string[] DecodeToken(string token)
        {
            string[] decoded = [];
            try
            {
                var bytes = WebEncoders.Base64UrlDecode(token);
                var decodedString = Encoding.UTF8.GetString(bytes);
                decoded = decodedString.Split("|");
            }
            catch (Exception)
            {
                return [];
            }

            return decoded;
        }

        public async Task<string> GenerateResetPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenCoded = $"{token}|{user.Email}";
            var tokenBytes = Encoding.UTF8.GetBytes(tokenCoded);
            return WebEncoders.Base64UrlEncode(tokenBytes);
        }


        public async Task<Result> ResetPasswordAsync(string token, string newPassword)
        {
            var tokenParts = DecodeToken(token);
            if (tokenParts.Length != 2)
            {
                return Result.Error("Bad token");
            }
            string emailPart = tokenParts[1];
            string tokenPart = tokenParts[0];

            var foundUser = await _userManager.FindByEmailAsync(emailPart);
            if (foundUser == null)
            {
                return Result.Error("Failed to reset password"); // Wrong User
            }

            var result = await _userManager.ResetPasswordAsync(foundUser, tokenPart, newPassword);
            if (!result.Succeeded)
            {
                return Result.Error("Failed to reset password"); // Wrong User
            }
            await _userManager.UpdateSecurityStampAsync(foundUser);
            return Result.Success("Succesfully reset password");
        }

        public async Task<string?> CreateRegistrationTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var payload = $"{user.Id}|{token}";
            var bytes = Encoding.UTF8.GetBytes(payload);
            var encodedToken = WebEncoders.Base64UrlEncode(bytes);
            return encodedToken;
        }


        public async Task<Result> AddUserClaimsAsync(ApplicationUser user, string role, string email)
        {
            var currentClaims = await _userManager.GetClaimsAsync(user);
            var hasNameClaim = currentClaims.FirstOrDefault((Claim claim) => claim.Type == ClaimTypes.Name);
            List<Claim> claims = new List<Claim>();
            if (hasNameClaim == null)
            {
                claims.Add(new Claim(ClaimTypes.Name, email));
            }

            var hasRoleClaim = currentClaims.FirstOrDefault((Claim claim) => claim.Type == ClaimTypes.Role && claim.Value == role);
            if (hasRoleClaim == null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            if (claims.Count > 0)
            {
                var roleAddResult = await _userManager.AddClaimsAsync(user, claims);
                if (!roleAddResult.Succeeded)
                {
                    return Result.Error("Failed to add claim");
                }

            }


            return Result.Success("Successfully added claim");
        }

        public async Task<Result<ApplicationUserCreationDto>> GetOrCreateIdentityAsync(string email, string password)
        {
            // Search for user with email
            var foundUser = await _userManager.FindByEmailAsync(email);
            if (foundUser == null)
            {

                // Create new identity
                var newIdentity = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                };
                var createdResult = await _userManager.CreateAsync(newIdentity, password);
                if (!createdResult.Succeeded)
                {
                    return Result<ApplicationUserCreationDto>.Error($"Failed to create: {createdResult.Errors.FirstOrDefault()?.Description ?? "Unknown error"}");
                }

                var createdIdentityDto = new ApplicationUserCreationDto
                {
                    UserIdentity = newIdentity,
                    IsNewUser = true
                };

                return Result<ApplicationUserCreationDto>.Success(createdIdentityDto, "Identity successfully created");
            }
            else
            {

                var validUser = await _userManager.CheckPasswordAsync(foundUser, password);
                if (validUser)
                {
                    var foundValidUserIdentity = new ApplicationUserCreationDto { UserIdentity = foundUser, IsNewUser = false };
                    return Result<ApplicationUserCreationDto>.Success(foundValidUserIdentity, "Succesfully retrieved correct user identity");
                }

            }
            return Result<ApplicationUserCreationDto>.Error("Invalid Credentials");
        }


        public async Task<string> MeAsync(string? email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            return email;
        }

        async Task<Result> IAuthService.ConfirmRegistrationAsync(string token)
        {
            var parts = DecodeToken(token);
            if (parts.Length != 2)
            {
                return Result.Error("Invalid token");
            }
            var foundUser = await _userManager.FindByIdAsync(parts[0]);
            if (foundUser == null)
            {
                return Result.Error("Invalid Token");
            }

            try
            {
                var emailConformation = await _userManager.ConfirmEmailAsync(foundUser, parts[1]);
                if (!emailConformation.Succeeded)
                {
                    return Result.Error("Invalid Token");
                }
                foundUser.isActive = true;
                await _userManager.UpdateAsync(foundUser);
            }
            catch (Exception)
            {
                return Result.Error("Failed to confirm User");
            }
            return Result.Success("Successfully confirmed " + foundUser.Email + " email");

        }



        async Task<Result> IAuthService.LoginAsync(LoginDto login)
        {
            var userFound = await _userManager.FindByEmailAsync(login.Email);
            if (userFound == null)
            {
                return Result.Error("Failed to login");
            }
            var result = await _signInManager.PasswordSignInAsync(userFound, login.Password, true, false);

            if (result.Succeeded)
            {
                if (!userFound.isActive)
                {
                    return Result.Error("User not activated");
                }

                return Result.Success("Succesfully logged in!");
            }
            return Result.Error("Failed to log in");
        }

        async Task<Result> IAuthService.LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Result.Success("Logged Out");
        }


    }
}
