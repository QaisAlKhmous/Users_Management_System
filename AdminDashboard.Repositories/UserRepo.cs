using AdminDashboard.Domain;
using AdminDashboard.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using AdminDashboard.Repositories.Data;

namespace AdminDashboard.Repositories
{
    public class UserRepo : IUserRepo
    {
     
        public  IEnumerable<User> GetAllUsers()
        {
            using (var context = new AppDbContext())
            {
                var users = context.Users.Include(u => u.CreatedByUser).ToList();

                return users;
            }
        }

        
        public  User GetUserById(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Users
                    .FirstOrDefault(u => u.UserId == id);
            }
        }
        public User GetUserByUsername(string username)
        {
            using (var context = new AppDbContext())
            {
                return context.Users
                    .FirstOrDefault(u => u.Username == username);
            }
        }

        public int AddUser(User user)
        {
            using (var context = new AppDbContext())
            {
                user.UserId = -1;
                context.Users.Add(user);
                context.SaveChanges();

                return user.UserId;
            }
        }

        public bool Update(User user)
        {
            using (var context = new AppDbContext())
            {
                context.Users.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();

                return true;
            }
        }

        public void DeleteById(int id)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.Find(id);

                context.Users.Remove(user);

                context.SaveChanges();
            }
        }

        public bool IsUserExists(string Username)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == Username);

                if(user == null)
                return false;

                return true;
            }
        }

    }
}
