using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reservmed.DTOs;
using Reservmed.Models.Identity;
using Reservmed.Services.Interfaces;

namespace Reservmed.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;

        public AuthController(UserManager<ApplicationUser> appUser, IAuthService authService, IAccountService accountService)
        {
            _authService = authService;
            _userManager = appUser;
            _accountService = accountService;
        }

        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient(PatientRegistrationDto patientRegistrationData)
        {
            var result = await _accountService.RegisterPatientAsync(patientRegistrationData);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("register/doctor")]
        public async Task<IActionResult> RegisterDoctor(DoctorRegistrationDto doctorRegistrationData)
        {
            var result = await _accountService.RegisterDoctorAsync(doctorRegistrationData);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginData)
        {
            var result = await _authService.LoginAsync(loginData);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok("Succesfully logged out");

        }


        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            var result = await _accountService.AskForPasswordResetAsync(email);

            if (!result.IsSuccess)
            {
                return Conflict(result.Message);
            }

            return Ok("Reset password has been requested");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto passwordResetDto)
        {
            var result = await _accountService.ResetPasswordAsync(passwordResetDto);
            if (!result.IsSuccess)
            {
                return Conflict(result.Message);
            }
            return Ok("Succesfully reseted password");

            // TO IMPLEMENT

        }


        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmEmailDto dto)
        {
            var result = await _authService.ConfirmRegistrationAsync(dto.Token);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);

        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            // TO PROPER IMPLEMENT
            var userEmail = User.Identity?.Name;

            var result = await _authService.MeAsync(userEmail);
            return Ok(result);

        }
    }
}
