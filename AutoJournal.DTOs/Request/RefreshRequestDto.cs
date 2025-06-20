using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoJournal.DTOs.Request
{
    public class RefreshRequestDto
    {
        public required string AccessToken { get; set; }   
        public required string RefreshToken { get; set; }
    }
}
