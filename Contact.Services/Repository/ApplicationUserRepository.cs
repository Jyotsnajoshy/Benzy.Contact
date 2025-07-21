using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact.Common.Types;
using Contact.Common;
using Contact.Services.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Contact.Entity.Models;

namespace Contact.Services.Repository
{
   public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly IValidator<RegisterUserDto> _registerUserValidator;
        private readonly IValidator<UserInfoRequestDto> _validator;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public ApplicationUserRepository(
            IValidator<RegisterUserDto> registerUserValidator,
            UserManager<ApplicationUser> userManager,
            IValidator<UserInfoRequestDto> validator,
            SignInManager<ApplicationUser> signInManager,
            ITokenRepository tokenRepository)
        {
            _registerUserValidator = registerUserValidator;
            _validator = validator;
            _signInManager = signInManager;
            _tokenRepository = tokenRepository;
            _userManager = userManager;
        }
        public async Task<Result<string>> RegisterAsync(RegisterUserDto request)
        {
            // 1. Validate input using FluentValidation
            var validationResult = await _registerUserValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new Result<string>(ResultType.InvalidData)
                {
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            // 2. Check if user already exists by email
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new Result<string>(ResultType.InvalidData)
                    .AddError("Email already exists", ResultType.InvalidData);
            }

            // 3. Create the user
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString().Replace("-", ""),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                var allErrors = identityResult.Errors.Select(e => e.Description);
                return new Result<string>(ResultType.InvalidData)
                    .AddError(string.Join("; ", allErrors), ResultType.InvalidData);
            }

            // 7. Return success message
            return new Result<string>("User registered successfully");
        }

        public async Task<Result<UserInfoResponseDto>> AuthenticateAsync(UserInfoRequestDto request)
        {
            //1) Run FluentValidation on request
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var allMessages = validationResult.Errors.Select(e => e.ErrorMessage);
                return new Result<UserInfoResponseDto>(ResultType.InvalidData)
                {
                    Errors = allMessages.ToList()
                };
            }

            // 2) Attempt to find the user by email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new Result<UserInfoResponseDto>(ResultType.CompletedWithErrors)
                {

                    Message = "Login Failed",
                    Errors = ["Invalid Email"]
                };
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            // 3) Check the password -> InvalidData (400) if incorrect
            if (!signInResult.Succeeded)

            {
                return new Result<UserInfoResponseDto>(ResultType.CompletedWithErrors)
                {
                    Message = "Login Failed",
                    Errors = ["Invalid Password"]
                };

            }

            //4) Generate Token (e.g., JWT)
            var tokenResponse = await _tokenRepository.GenerateToken(user);
            if (string.IsNullOrEmpty(tokenResponse))
            {
                return new Result<UserInfoResponseDto>(ResultType.CompletedWithErrors)
                    .AddError("Token generation failed", ResultType.CompletedWithErrors);
            }

            var userInfo = new UserInfoResponseDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TokenResponse = tokenResponse
            };

            return new Result<UserInfoResponseDto>(userInfo); ;
        }
    }
}
