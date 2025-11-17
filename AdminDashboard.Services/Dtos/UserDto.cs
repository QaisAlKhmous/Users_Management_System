using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Services.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string FullNameAr { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public bool IsLocked { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string LastModifiedDate { get; set; }
    }
}
