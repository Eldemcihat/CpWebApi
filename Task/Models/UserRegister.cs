using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Models
{
    public class UserRegister
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Mail { get; set; }
        public required string Msisdn { get; set; }
        public required string Roles { get; set; }
    }
}