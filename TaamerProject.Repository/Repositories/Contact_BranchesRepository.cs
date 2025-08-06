using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class Contact_BranchesRepository : IContact_BranchesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Contact_BranchesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }
        public async Task<IEnumerable<Contact_BranchesVM>> GetAllContactBranches()
            {
                var CB = _TaamerProContext.Contact_Branches.Where(s => s.IsDeleted == false).Select(x => new Contact_BranchesVM
                {
                 ContactId=x.ContactId,
                 BranchAddress=x.BranchAddress,
                 BranchCS=x.BranchCS,
                 BranchEmail=x.BranchEmail,
                 BranchName=x.BranchName,
                 BranchPhone=x.BranchPhone,
                }).ToList();
                return CB;
            }
        
     
    }
}
