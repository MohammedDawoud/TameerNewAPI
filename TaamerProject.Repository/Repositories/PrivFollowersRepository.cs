using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class PrivFollowersRepository : IPrivFollowersRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public PrivFollowersRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
    }
}
