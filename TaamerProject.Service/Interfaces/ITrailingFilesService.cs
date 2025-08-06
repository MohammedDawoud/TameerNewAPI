using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ITrailingFilesService 
    {
        //IEnumerable<TrailingFilesVM> GetAllTrailingFiles();
        GeneralMessage SaveTrailingFiles(TrailingFiles trailingFiles, int UserId,int BranchId);
        GeneralMessage DeleteTrailingFiles(int FileId, int UserId, int BranchId);

    }
}
