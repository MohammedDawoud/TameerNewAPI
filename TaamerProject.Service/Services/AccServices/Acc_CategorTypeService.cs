using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.IGeneric;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
   public class Acc_CategorTypeService : IAcc_CategorTypeService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        private readonly IAcc_CategorTypeRepository _Acc_CategorTypeRepository;
        public Acc_CategorTypeService(IAcc_CategorTypeRepository acc_CategorTypeRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {

            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Acc_CategorTypeRepository = acc_CategorTypeRepository;

        }
        public Task<IEnumerable<Acc_CategorTypeVM>> GetAllCategorytype(string SearchText)
        {
            var Categories = _Acc_CategorTypeRepository.GetAllCategorytype(SearchText);
            return Categories;
        }


        public GeneralMessage SaveCategory(Acc_CategorType CategoryT, int UserId, int BranchId)
        {
            try
            {

                if (CategoryT.CategorTypeId == 0)
                {
                    CategoryT.AddUser = UserId;
                    CategoryT.AddDate = DateTime.Now;

                    _TaamerProContext.Acc_CategorType.Add(CategoryT); 
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة نوع صنف جديد";
                    _SystemAction.SaveAction("SaveCategory", "Acc_CategoriesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};

                }
                else
                {
                    //var categoryTrUpdated = _Acc_CategoriesRepository.GetById(CategoryT.CategorTypeId);
                    var categoryTrUpdated = _TaamerProContext.Acc_CategorType.Where(s => s.CategorTypeId == CategoryT.CategorTypeId).FirstOrDefault();

                    if (categoryTrUpdated != null)
                    {
                        categoryTrUpdated.NAmeAr = CategoryT.NAmeAr;
                        categoryTrUpdated.NAmeEn = CategoryT.NAmeEn;
                        categoryTrUpdated.UpdateUser = UserId;
                        categoryTrUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();
                    
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل نوع صنف رقم " + CategoryT.CategorTypeId;
                    _SystemAction.SaveAction("SaveCategory", "Acc_CategoriesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ نوع الصنف";
                _SystemAction.SaveAction("SaveCategory", "Acc_CategoriesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteCategory(int Categryid, int UserId, int BranchId)
        {
            try
            {
                //Acc_CategorType categoryT = _Acc_CategoriesRepository.GetById(Categryid);
                Acc_CategorType? categoryT = _TaamerProContext.Acc_CategorType.Where(s => s.CategorTypeId == Categryid).FirstOrDefault();
                if(categoryT != null)
                {
                    categoryT.IsDeleted = true;
                    categoryT.DeleteDate = DateTime.Now;
                    categoryT.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف نوع  صنف رقم " + Categryid;
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
