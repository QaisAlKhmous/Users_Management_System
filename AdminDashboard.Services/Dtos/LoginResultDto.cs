using AdminDashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Services.Dtos
{
    public class LoginResultDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Type;
        public enRegStatus Status { get; set; }
        public bool Password { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
     
    }
}
