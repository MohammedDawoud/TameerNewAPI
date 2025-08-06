using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;

namespace TaamerProject.Service.Interfaces.UsersF
{
    public interface IUsersService
    {
        Task<UsersVM> GetUser(string username);
        Task<IEnumerable<Acc_CategoriesVM>> GetAllCategories();
        Task<PagedLists<UsersVM>> GetAllAsync(RequestParam<UsersFilterDTO> Param);
        GeneralMessage Login(string username, string password, string activationCode, string Lang);
        Task<UsersVM> GetUserWithPrivilliges(string username);

    }
}
