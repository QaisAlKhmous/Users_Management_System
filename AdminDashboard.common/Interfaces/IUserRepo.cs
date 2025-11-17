using AdminDashboard.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Domain.Interfaces
{
    public interface IUserRepo
    {
        int AddUser(User user);
        User GetUserByUsername(string username);
        User GetUserById(int id);
        IEnumerable<User> GetAllUsers();

    }
}