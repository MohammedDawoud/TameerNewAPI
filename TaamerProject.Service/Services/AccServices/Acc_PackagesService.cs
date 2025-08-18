using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Acc_PackagesService : IAcc_PackagesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAcc_PackagesRepository _Acc_PackagesRepository;
        public Acc_PackagesService(IAcc_PackagesRepository acc_Packages
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Acc_PackagesRepository = acc_Packages;
        }
        public async Task<IEnumerable<Acc_PackagesVM>> GetAllPackages(string SearchText)
        {
            var Packages = await _Acc_PackagesRepository.GetAllPackages(SearchText);
            return Packages;
        }
        public GeneralMessage SavePackage(Acc_Packages Package, int UserId, int BranchId)
        {
            try
            {

                if (Package.PackageId == 0)
                {


                    Package.AddUser = UserId;
                    Package.AddDate = DateTime.Now;
                    _TaamerProContext.Acc_Packages.Add(Package);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة باقة جديد";
                    _SystemAction.SaveAction("SavePackage", "Acc_PackagesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var PackageUpdated = _TaamerProContext.Acc_Packages.Where(x=>x.PackageId==Package.PackageId).FirstOrDefault();
                    if (PackageUpdated != null)
                    {



                        PackageUpdated.PackageName = Package.PackageName;
                        PackageUpdated.MeterPrice1 = Package.MeterPrice1;
                        PackageUpdated.MeterPrice2 = Package.MeterPrice2;
                        PackageUpdated.MeterPrice3 = Package.MeterPrice3;
                        PackageUpdated.PackageRatio1 = Package.PackageRatio1;
                        PackageUpdated.PackageRatio2 = Package.PackageRatio2;
                        PackageUpdated.PackageRatio3 = Package.PackageRatio3;
                        PackageUpdated.UpdateUser = UserId;
                        PackageUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل باقة رقم " + Package.PackageId;
                    _SystemAction.SaveAction("SavePackage", "Acc_PackagesService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};

                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الباقة";
                _SystemAction.SaveAction("SavePackage", "Acc_PackagesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage DeletePackage(int PackageId, int UserId, int BranchId)
        {
            try
            {


                Acc_Packages Package = _TaamerProContext.Acc_Packages.Where(s=>s.PackageId==PackageId)!.FirstOrDefault()!;
                if(Package!=null)
                {
                    Package.IsDeleted = true;
                    Package.DeleteDate = DateTime.Now;
                    Package.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف باقة رقم " + PackageId;
                    _SystemAction.SaveAction("DeletePackage", "Acc_PackagesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                }
                else
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
                }

            }
            catch (Exception ex)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف باقة رقم " + PackageId; ;
                _SystemAction.SaveAction("DeletePackage", "Acc_PackagesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
    }
}
