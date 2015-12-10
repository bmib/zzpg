using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickBootstrap1.Entities;
using QuickBootstrap1.Services.Util;

namespace QuickBootstrap1.Services
{
    public interface IUserManageService
    {
        PagedResult<User> GetAll();

        User Get(string username = "");

        bool Edit(User model);

        bool Create(User model);

        bool Delete(string username);

        bool ExistsUser(string username);
    }
}