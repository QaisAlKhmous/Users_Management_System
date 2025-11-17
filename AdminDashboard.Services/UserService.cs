using AdminDashboard.Domain;
using AdminDashboard.Repositories;
using AdminDashboard.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Services
{
    public class UserService
    {
     static UserRepo userRepo = new UserRepo();
        public static IEnumerable<UserDto> GetAllUsers()
        {
            
      
                var users = userRepo.GetAllUsers().Select(u => new UserDto
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    FullNameAr = u.FullNameAr,
                    Username = u.Username,
                    Email = u.Email,
                    Type = u.Type.ToString(),
                    Status = u.Status.ToString(),
                    IsLocked = u.IsLocked,
                    IsActive = u.IsActive,
                    CreatedBy = u.CreatedByUser != null? u.CreatedByUser.Username:"",
                    CreatedDate = u.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    LastModifiedDate = u.LastModifiedDate.ToString("yyyy-MM-ddTHH:mm:ssZ")

                }).ToList();

                return users;
           
        }

       
        public static UserDetailDto GetUserById(int id)
        {

            var u = userRepo.GetUserById(id);
            User approvedByUser = null;
            if (u.ApprovedBy != 0 && u.ApprovedBy != null)
                approvedByUser = userRepo.GetUserById(u.ApprovedBy ?? 0);
            return
                    new UserDetailDto()
                    {
                        UserId = u.UserId,
                        Username = u.Username,
                        FullName = u.FirstName + " " + u.LastName,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        FirstNameAr = u.FirstNameAr,
                        LastNameAr = u.LastNameAr,
                        FullNameAr = u.FullNameAr,
                        Email = u.Email,
                        Phone = u.Phone,
                        Type = u.Type.ToString(),
                        Status = u.Status.ToString(),
                        CreatedBy = u.CreatedByUser != null
                                        ? u.CreatedByUser.FirstName + " " + u.CreatedByUser.LastName
                                        : null,
                        CreatedDate = u.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        LastModifiedDate = u.LastModifiedDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        IsActive = u.IsActive,
                        IsLocked = u.IsLocked,
                        approvedBy = approvedByUser != null ? approvedByUser.Username : ""
                    };
                 
            
        }

        public static int AddUser(AddUserDto user)
        {
           
            var newUser = new User();

          
       
                User addedBy = userRepo.GetUserById(user.CreatedById);

                 newUser = new User()
                {
                    Username = user.Username,
                    FullName = user.FirstName + " " + user.LastName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FirstNameAr = user.FirstNameAr,
                    LastNameAr = user.LastNameAr,
                    FullNameAr = user.FirstNameAr + " " + user.LastNameAr,
                    Email = user.Email,
                    Phone = user.Phone,
                    Password = SecurityService.HashPassword(user.Password),
                    Type = (enUserType)Enum.Parse(typeof(enUserType), user.Type),
                    Status = enRegStatus.approved,
                    CreatedBy = addedBy.UserId,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    IsActive = true,
                    IsLocked = false
                };
          
          



                return userRepo.AddUser(newUser);
        }


        public static int Register(AddUserDto user)
        {

            var newUser = new User();

            
                newUser = new User()
                {
                    Username = user.Username,
                    FullName = user.FirstName + " " + user.LastName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FirstNameAr = user.FirstNameAr,
                    LastNameAr = user.LastNameAr,
                    FullNameAr = user.FirstNameAr + " " + user.LastNameAr,
                    Email = user.Email,
                    Phone = user.Phone,
                    Password = SecurityService.HashPassword(user.Password),
                    Type = (enUserType)Enum.Parse(typeof(enUserType), user.Type),
                    Status = enRegStatus.pending,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    IsActive = true,
                    IsLocked = false
                };
          



            return userRepo.AddUser(newUser);
        }

        public static LoginResultDto Login(string Username,string Password)
        {
            var user = userRepo.GetUserByUsername(Username);

          

            if (user == null)
            {
                return null;
            }

            if(!SecurityService.VerifyPassword(Password, user.Password))
            {
                return new LoginResultDto
                {
                    UserId = user.UserId,
                    Username = Username,
                    Type = user.Type.ToString(),
                    Status = user.Status,
                    Password = false,
                    IsLocked = user.IsLocked,
                    Email = user.Email
                };
            }



            return new LoginResultDto
            {
                UserId = user.UserId,
                Username = Username,
                Type = user.Type.ToString(),
                Status = user.Status,
                Password = true,
                IsLocked = user.IsLocked,
                Email = user.Email
            };
        }

        public static bool UpdateUser(UserDetailDto user)
        {
            var existingUser = userRepo.GetUserById(user.UserId);

            if (existingUser == null)
                return false;


            existingUser.Username = user.Username;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.FullName = user.FirstName + " " + user.LastName;
            existingUser.FirstNameAr = user.FirstNameAr;
            existingUser.LastNameAr = user.LastNameAr;
            existingUser.FullNameAr = user.FirstNameAr + " " + user.LastNameAr;
            existingUser.Email = user.Email;
            existingUser.IsActive = user.IsActive;
            existingUser.IsLocked = user.IsLocked;
            existingUser.Status = (enRegStatus)Enum.Parse(typeof(enRegStatus), user.Status);
            existingUser.Type = (enUserType)Enum.Parse(typeof(enUserType), user.Type);
            existingUser.LastModifiedDate = DateTime.Now;
            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = SecurityService.HashPassword(user.Password);
            }

            return userRepo.Update(existingUser);

        }

        public static bool UpdateProfile(UpdateProfileDto user)
        {
            var existingUser = userRepo.GetUserById(user.UserId);

            if (existingUser == null)
                return false;


            
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.FullName = user.FirstName + " " + user.LastName;
            existingUser.FirstNameAr = user.FirstNameAr;
            existingUser.LastNameAr = user.LastNameAr;
            existingUser.FullNameAr = user.FirstNameAr + " " + user.LastNameAr;

            return userRepo.Update(existingUser);

        }

        public static void DeleteById(int id)
        {
            userRepo.DeleteById(id);
        }

        public static void InceremntFaildAttempts(string username)
        {
            User user = userRepo.GetUserByUsername(username);

            user.FailedAttempts++;

            if (user.FailedAttempts == 3)
            {
                user.IsLocked = true;
            }

            userRepo.Update(user);


        }

        public static void ResetFailedAttempts(string username)
        {
            User user = userRepo.GetUserByUsername(username);
            user.FailedAttempts = 0;
            userRepo.Update(user);

        }

        public static void acceptUser(int userId,int approvedBy)
        {
            User user = userRepo.GetUserById(userId);


            user.Status = enRegStatus.approved;
            user.ApprovedDate = DateTime.Now;
            user.ApprovedBy = approvedBy;

            userRepo.Update(user);

            EmailService.SendRegApprovedEmail(user.Email, "User Registration", user.FullName);
        }

        public static void rejectUser(int userId, int rejectedBy)
        {
            User user = userRepo.GetUserById(userId);


            user.Status = enRegStatus.rejected;
            user.IsActive = false;
            user.ApprovedDate = DateTime.Now;
            user.ApprovedBy = rejectedBy;

            userRepo.Update(user);

            EmailService.SendRegRejectedEmail(user.Email, "User Registration", user.FullName);
        }

        public static void unlockUser(int userId)
        {
            User user = userRepo.GetUserById(userId);


            user.IsLocked = false;
            user.FailedAttempts = 0;

            userRepo.Update(user);

            EmailService.SendUnlockedEmail(user.Email, "User Unlocked", user.FullName);
        }

        public static void resetUserPassword(int userId)
        {
            User user = userRepo.GetUserById(userId);

            string newPassword = SecurityService.GenerateRandomPassword();

            user.Password = SecurityService.HashPassword(newPassword);
            user.FailedAttempts = 0;   
            user.IsLocked = false;     

            userRepo.Update(user);

            EmailService.SendResetPasswordEmail(user.Email, "Reset Password", user.FullName,newPassword);
        }

        public static bool IsUserExists(string Username)
        {
            return userRepo.IsUserExists(Username);
        }
    }
}
