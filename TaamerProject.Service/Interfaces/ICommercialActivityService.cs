using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.ViewModels;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Service.Interfaces
{
    public interface ICommercialActivityService
    {
        Task<IEnumerable<CommercialActivityVM>> GetCommercialActivities(string SearchText,int Type);
        GeneralMessage SaveCommercialActivity(CommercialActivity commercialActivity, int UserId, int BranchId);
        GeneralMessage DeleteCommercialActivity(int Id, int UserId, int BranchId);
    }
}
