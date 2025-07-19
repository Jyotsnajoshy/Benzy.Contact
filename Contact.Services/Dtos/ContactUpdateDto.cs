using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Services.Dtos
{
    public class ContactUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }

    }
}
