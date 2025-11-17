using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Domain
{
    public enum enRegStatus
    {
        pending = 0,
        approved = 1,
        rejected = 2
    }

    public enum enUserType
    {
        Admin = 0,
        User = 1
    }
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string FirstNameAr { get; set; }
        public string LastNameAr { get; set; }
        public string FullNameAr { get; set; }

        [Index(IsUnique = true)]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public enUserType Type { get; set; }

        
        public int? CreatedBy { get; set; }
        public User CreatedByUser { get; set; }  
        public ICollection<User> CreatedUsers { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public enRegStatus Status { get; set; }

        public int? ApprovedBy { get; set; }
        public User ApprovedByUser { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public ICollection<User> ApprovedUsers { get; set; }
        public int FailedAttempts { get; set; }

    }
}
