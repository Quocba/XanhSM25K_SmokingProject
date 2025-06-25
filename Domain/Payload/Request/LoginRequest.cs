using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payload.Request
{
    public class LoginRequest
    {
        public string PhoneOrEmail {  get; set; }
        public string Password { get; set; }
    }
}
