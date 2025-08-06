using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_projectsReasonsRepository
    {
        Task<IEnumerable<Pro_projectsReasonsVM>> GetAllprojectsReasons();
    }
}
