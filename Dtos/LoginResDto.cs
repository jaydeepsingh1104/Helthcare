using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPICore.Dtos
{
    public class LoginResDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
          public bool isactive { get; set; }         
        public string RefreshToken { get; set; }
    }
}