using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Services.Dtos
{
  public class SignInRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
