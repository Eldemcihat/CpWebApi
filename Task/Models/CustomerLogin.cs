using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Models
{
    public class CustomerLogin
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}