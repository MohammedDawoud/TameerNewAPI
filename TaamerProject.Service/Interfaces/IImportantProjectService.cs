using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IImportantProjectService
    {

        Task<IEnumerable<ImportantProjectVM>> GetImportantProjects(int projectid, int userid);

        GeneralMessage ChangeFlag(ImportantProject import, int UserId, int BranchId);
        GeneralMessage ChangeImportant(ImportantProject import, int UserId, int BranchId);
    }
}
