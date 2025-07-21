using Contact.Common.Types;
using Contact.Common;
using Contact.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Contact.Services.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Benzy.Contact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ApplicationUserController(IApplicationUserRepository applicationUsertRepository)
        {
            _applicationUserRepository = applicationUsertRepository;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(Result<RegisterUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<RegisterUserDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<RegisterUserDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var result = await _applicationUserRepository.RegisterAsync(request);
            return result.ToActionResult();
        }

        [HttpPost("Authenticate")]
        [ProducesResponseType(typeof(Result<UserInfoResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<UserInfoResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<UserInfoResponseDto>), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Authenticate([FromBody] UserInfoRequestDto request)
        {
            var result = await _applicationUserRepository.AuthenticateAsync(request);
            return result.ToActionResult();
        }
    }  
}
