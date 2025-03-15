using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    /// <summary>
    /// Represents the login model containing user credentials.
    /// </summary>
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}