using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Services.Dtos
{
  public class TokenResponseDto
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public static implicit operator string(TokenResponseDto v)
        {
            return v?.Token ?? string.Empty;
        }
    }
}
