using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Service.IGeneric;
using Twilio.Base;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Acc_CategoriesService :  IAcc_CategoriesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        private readonly IAcc_CategoriesRepository _Acc_CategoriesRepository;

        public Acc_CategoriesService(IAcc_CategoriesRepository acc_CategoriesRepository
            ,TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Acc_CategoriesRepository = acc_CategoriesRepository;
        }
        public Task<IEnumerable<Acc_CategoriesVM>> GetAllCategories(string SearchText)
        {
            var Categories = _Acc_CategoriesRepository.GetAllCategories(SearchText);
            return Categories;
        }


        public GeneralMessage SaveCategory(Acc_Categories Category, int UserId, int BranchId)
        {
            try
            {

                if (Category.CategoryId == 0)
                {
                    Category.AddUser = UserId;
                    Category.AddDate = DateTime.Now;
                    Category.Code = "";
                    _TaamerProContext.Acc_Categories.Add(Category);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = Resources.addnewitem;
                    _SystemAction.SaveAction("SaveCategory", "Acc_CategoriesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    //var categoryrUpdated = _Acc_CategoriesRepository.GetById(Category.CategoryId);
                    var categoryrUpdated = _TaamerProContext.Acc_Categories.Where(s=>s.CategoryId == Category.CategoryId).FirstOrDefault();

                    if (categoryrUpdated != null)
                    {
                        categoryrUpdated.NAmeAr = Category.NAmeAr;
                        categoryrUpdated.NAmeEn = Category.NAmeEn;
                        categoryrUpdated.Code = Category.Code;
                        categoryrUpdated.Price = Category.Price;
                        categoryrUpdated.Note = Category.Note;
                        categoryrUpdated.UpdateUser = UserId;
                        categoryrUpdated.UpdateDate = DateTime.Now;
                        categoryrUpdated.CategorTypeId = Category.CategorTypeId;
                        categoryrUpdated.AccountId = Category.AccountId;

                    }

                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل صنف رقم " + Category.CategoryId;
                    _SystemAction.SaveAction("SaveCategory", "Acc_CategoriesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};
                }

            }
            catch (Exception ex )
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الصنف";
                _SystemAction.SaveAction("SaveCategory", "Acc_CategoriesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteCategory(int Categryid, int UserId, int BranchId)
        {
            try
            {
                //Acc_Categories category = _Acc_CategoriesRepository.GetById(Categryid);
                Acc_Categories? category = _TaamerProContext.Acc_Categories.Where(s => s.CategoryId == Categryid).FirstOrDefault();
                if(category!=null)
                {
                    category.IsDeleted = true;
                    category.DeleteDate = DateTime.Now; 
                    category.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف صنف رقم " + Categryid;
                    _SystemAction.SaveAction("DeleteCategory", "Acc_CategoriesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف صنف رقم " + Categryid; ;
                _SystemAction.SaveAction("DeleteCategory", "Acc_CategoriesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
