using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface ISys_UserLoginRepository
    {
        Task<IEnumerable<Sys_UserLoginVM>> GetAllUserLogin(int Type);

    }
}
