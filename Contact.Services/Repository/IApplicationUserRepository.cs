using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact.Common.Types;
using Contact.Services.Dtos;

namespace Contact.Services.Repository
{
   public interface IApplicationUserRepository
    {
        Task<Result<UserInfoResponseDto>> AuthenticateAsync(UserInfoRequestDto request);
        Task<Result<string>> RegisterAsync(RegisterUserDto request);
    }
}
