using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Services.Dtos
{
    public class AddUserDto
    {
        public int CreatedById { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string FirstNameAr { get; set; }
        public string LastNameAr { get; set; }
        public string Username { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Type { get; set; }
     
       
    }
}
