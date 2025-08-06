using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface ICommercialActivityRepository
    {
        Task<IEnumerable<CommercialActivityVM>> GetCommercialActivities(string SearchText,int Type);

    }
}
