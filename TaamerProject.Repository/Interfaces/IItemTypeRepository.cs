using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayanatech.TameerPro.Repository
{
    public interface IItemTypeRepository
    {
        Task<IEnumerable<ItemTypeVM>> GetAllItemTypes(string SearchText);
    }
}
