using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Repository.Interfaces
{
    public interface IPro_ProjectStepsRepository
    {
        Task<IEnumerable<Pro_ProjectStepsVM>> GetAllProjectSteps();
        Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectid(int projectid, int stepid);
        Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectidOnly(int projectid);


    }
}
