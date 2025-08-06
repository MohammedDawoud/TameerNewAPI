using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface IContactListRepository
    {
        Task<IEnumerable<ContactListVM>> GetContactLists(int Id,int Type, int UserId);
    }
}
