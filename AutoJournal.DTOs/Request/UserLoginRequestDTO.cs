using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoJournal.DTOs.Request
{
    public class UserLoginRequestDTO
    {
        public required string Username {  get; set; }
        public required string Password { get; set; }
    }
}
