using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact.Entity.Models;
using Contact.Services.Dtos;

namespace Contact.Services.Repository
{
    public interface ITokenRepository
    {
        public Task<TokenResponseDto> GenerateToken(ApplicationUser userInfo);
    }
}
