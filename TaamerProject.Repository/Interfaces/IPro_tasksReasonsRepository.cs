using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_tasksReasonsRepository
    {
        Task<IEnumerable<Pro_tasksReasonsVM>> GetAlltasksReasons();
    }
}
