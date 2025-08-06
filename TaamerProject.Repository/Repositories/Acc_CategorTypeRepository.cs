using TaamerProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
   public class Acc_CategorTypeRepository : IAcc_CategorTypeRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_CategorTypeRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }

        public async Task<IEnumerable<Acc_CategorTypeVM>> GetAllCategorytype(string SearchText)           
        {
                IEnumerable<Acc_CategorTypeVM> catg = new List<Acc_CategorTypeVM>();

                try
                {
                    if (SearchText == "")
                    {
                        var Categorytype = _TaamerProContext.Acc_CategorType.Where(s => s.IsDeleted == false).Select(x => new Acc_CategorTypeVM
                        {
                            CategorTypeId= x.CategorTypeId,
                            NAmeAr = x.NAmeAr ?? "",
                            NAmeEn = x.NAmeEn ?? "",
                          
                        }).ToList();
                        return Categorytype;
                    }
                    else

                    {
                        var Categorytype = _TaamerProContext.Acc_CategorType.Where(s => s.IsDeleted == false && (s.NAmeAr.Contains(SearchText) || s.NAmeEn.Contains(SearchText))).Select(x => new Acc_CategorTypeVM
                        {
                            CategorTypeId = x.CategorTypeId,
                            NAmeAr = x.NAmeAr ?? "",
                            NAmeEn = x.NAmeEn ?? "",
                        }).ToList();
                        return Categorytype;
                    }
                }
                catch (Exception ex)
                {
                    return catg;

                }         
        }
       
    }
    
}

