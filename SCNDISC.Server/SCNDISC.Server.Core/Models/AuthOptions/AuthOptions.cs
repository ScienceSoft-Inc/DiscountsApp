using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace SCNDISC.Server.Core.Models.AuthOptions
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer";              
        public const string AUDIENCE = "Client";  
        const string KEY = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";                                
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
