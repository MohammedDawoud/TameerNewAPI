using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Generic;

namespace TaamerProject.Service.Services
{
    public class PrivilegesService :  IPrivilegesService
    {
        private readonly IPrivilegesRepository _privilegesRepository;
        private readonly IUsersRepository _UsersRepository;
        public PrivilegesService(IPrivilegesRepository privilegesRepository, IUsersRepository UsersRepository)
        {
            _privilegesRepository = privilegesRepository;
            _UsersRepository = UsersRepository;
        }
        public Task<IEnumerable<UserPrivilegesVM>> GetUsersByPrivilegesIds(int? Priv)
        {
            var Privileges =  _privilegesRepository.GetUsersByPrivilegesIds(Priv);
            
            return Privileges;
        }
    }
}
