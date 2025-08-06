using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class TameerProRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public TameerProRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
    }
}
