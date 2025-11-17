using AdminDashboard.Services;
using AdminDashboard.Services.Dtos;
using ClosedXML.Excel;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web.Mvc;


namespace API.Controllers
{

    public class UserController : Controller
    {


        [HttpGet]
        public JsonResult GetUsers()
        {
            var users = UserService.GetAllUsers();

            return Json(users, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult AddUser(AddUserDto newUser)
        {
           
           int userId = UserService.AddUser(newUser);

            return Json(userId);
         


        }

        [HttpPost]
        public JsonResult Register(AddUserDto newUser)
        {

            int userId = UserService.Register(newUser);

            return Json(userId);



        }


        [HttpGet]
        public JsonResult GetUserById(int id)
        {
            var user = UserService.GetUserById(id);

            return Json(user, JsonRequestBehavior.AllowGet);


        }
        [Route("User/UpdateUser")]
        [HttpPost]
        public JsonResult UpdateUser(UserDetailDto user)
        {
            if (user == null || user.UserId == 0)
                return Json(new { success = false, message = "Invalid user data" });

            try
            {
                bool updated = UserService.UpdateUser(user);

                if (updated)
                    return Json(new { success = true, data = user });
                else
                    return Json(new { success = false, message = "User not found" });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult IsUserExists(string Username)
        {
            bool Exists = UserService.IsUserExists(Username);

            return Json(Exists, JsonRequestBehavior.AllowGet);
        }

        [Route("User/UpdateProfile")]
        [HttpPost]
        public JsonResult UpdateProfile(UpdateProfileDto user)
        {
            if (user == null || user.UserId == 0)
                return Json(new { success = false, message = "Invalid user data" });

            try
            {
                bool updated = UserService.UpdateProfile(user);

                if (updated)
                    return Json(new { success = true, data = user });
                else
                    return Json(new { success = false, message = "User not found" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }
        }


        public JsonResult DeleteById(int id)
        {
            UserService.DeleteById(id);

            return Json(new { success = true });
        }

        public JsonResult acceptUser(int id,int approvedBy)
        {
            UserService.acceptUser(id, approvedBy);

            return Json(new { success = true });
        }
        public JsonResult rejectUser(int id,int rejectedBy)
        {
            UserService.rejectUser(id, rejectedBy);

            return Json(new { success = true });
        }

        public JsonResult unlockUser(int id)
        {
            UserService.unlockUser(id);

            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult resetUserPassword(int id)
        {
            UserService.resetUserPassword(id);

            return Json(new { success = true });
        }

        public JsonResult SendEmail(EmailDto email)
        {
            EmailService.SendEmail(email.To, email.Subject, email.Body);

            return Json(new { success = true });
        }

        [HttpGet]

        public FileResult ExportToExcel()
        {
            var users = UserService.GetAllUsers();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");


                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Username";
                worksheet.Cell(1, 3).Value = "Full Name";
                worksheet.Cell(1, 4).Value = "Full Name Ar";
                worksheet.Cell(1, 5).Value = "Email";
                worksheet.Cell(1, 6).Value = "Type";
                worksheet.Cell(1, 7).Value = "Created By";
                worksheet.Cell(1, 8).Value = "Created Date";
                worksheet.Cell(1, 9).Value = "IsActive";
                worksheet.Cell(1, 10).Value = "IsLocked";
                worksheet.Cell(1, 11).Value = "Status";
                int row = 2;
                foreach (var user in users)
                {
                    worksheet.Cell(row, 1).Value = user.UserId;
                    worksheet.Cell(row, 2).Value = user.Username;
                    worksheet.Cell(row, 3).Value = user.FullName;
                    worksheet.Cell(row, 4).Value = user.FullNameAr;
                    worksheet.Cell(row, 5).Value = user.Email;
                    worksheet.Cell(row, 6).Value = user.Type;
                    worksheet.Cell(row, 7).Value = user.CreatedBy;
                    worksheet.Cell(row, 8).Value = user.CreatedDate.ToString();
                    worksheet.Cell(row, 9).Value = user.IsActive?"Yes":"No";
                    worksheet.Cell(row, 10).Value = user.IsLocked?"Locked":"Unlocked";
                    worksheet.Cell(row, 11).Value = user.Status;
                    row++;
                }
                worksheet.Column(2).Width = 10;
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 15;
                worksheet.Column(5).Width = 30;
                worksheet.Column(8).Width = 20;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "UsersReport.xlsx");
                }
            }
        }

    }
}
