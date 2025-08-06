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
    public class Acc_CategoriesRepository :  IAcc_CategoriesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public Acc_CategoriesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<Acc_CategoriesVM>> GetAllCategories(string SearchText)
        {
            IEnumerable<Acc_CategoriesVM> catg = new List<Acc_CategoriesVM>();

            try
            {
                if (SearchText == "")
                { //test
                    var Categories = _TaamerProContext.Acc_Categories.Where(s => s.IsDeleted == false).Select(x => new Acc_CategoriesVM
                    {
                        CategoryId = x.CategoryId,
                        NAmeAr = x.NAmeAr ?? "",
                        NAmeEn = x.NAmeEn ?? "",
                        Code = x.Code == null ? "" : x.Code,
                        Note = x.Note == null ? "" : x.Note,
                        Price = x.Price ?? 0,
                        CategorTypeId = x.CategorTypeId ?? 0,
                        CategorTypeName = x.CategorType != null ? x.CategorType.NAmeAr : "",
                        AccountId=x.AccountId??0,
                    }).ToList();
                    return Categories;
                }
                else

                {
                    var Categories = _TaamerProContext.Acc_Categories.Where(s => s.IsDeleted == false && (s.NAmeAr.Contains(SearchText) || s.NAmeEn.Contains(SearchText) || s.Code.Contains(SearchText))).Select(x => new Acc_CategoriesVM
                    {
                        CategoryId = x.CategoryId,
                        NAmeAr = x.NAmeAr ?? "",
                        NAmeEn = x.NAmeEn ?? "",
                        Code = x.Code == null ? "" : x.Code,
                        Note = x.Note == null ? "" : x.Note,
                        Price = x.Price ?? 0,
                        CategorTypeId = x.CategorTypeId ?? 0,
                        CategorTypeName = x.CategorType != null ? x.CategorType.NAmeAr : "",
                        AccountId = x.AccountId ?? 0,

                    }).ToList();
                    return Categories;
                }
            }
            catch(Exception ex)
            {
                return catg;

            }
        }


    }
}

