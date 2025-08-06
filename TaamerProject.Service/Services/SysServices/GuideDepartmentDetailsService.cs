using Bayanatech.TameerPro.Repository;
using System.Globalization;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaamerProject.Service.Services
{
    public class GuideDepartmentDetailsService :   IGuideDepartmentDetailsService
    {
        private readonly IGuideDepartmentDetailsRepository _GuideDepartmentDetailsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public GuideDepartmentDetailsService(IGuideDepartmentDetailsRepository GuideDepartmentDetailsRepository,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _GuideDepartmentDetailsRepository = GuideDepartmentDetailsRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<GuideDepartmentDetailsVM>> GetAllDepDetails(int DepId, string searchStr, int? DepDetailId = null)
        {
            var ItemTypes = _GuideDepartmentDetailsRepository.GetAllDepDetails(DepId, searchStr, DepDetailId);


            return ItemTypes;
        }
        public async Task<List<GuideDepartmentDetailsVM_Grouped>> GetAllDepDetails2( string searchStr)
        {
            var data =await _GuideDepartmentDetailsRepository.GetAllDepDetailsSearch(searchStr);
            List<GuideDepartmentDetailsVM_Grouped> datagrouped= new List<GuideDepartmentDetailsVM_Grouped>();
            if (data != null)
            {
                var groupedmodules = data.GroupBy(
                    module => new
                    {
                        module.DepId,
                        module.DepartmentName,

                    },
                    (key, detailsInGroup) => new GuideDepartmentDetailsVM_Grouped
                    {

                        DepId=key.DepId,
                        DepartmentName=key.DepartmentName,


                        GuideDepartments = detailsInGroup.Select(x => new GuideDepartmentDetailsVM
                        {
                            DepartmentName= x.DepartmentName,
                            DepId = x.DepId,
                            DepDetailsId=x.DepDetailsId,
                            Header=x.Header,
                            Link=x.Link,
                            Text=x.Text,
                            Type=x.Type,
                            NameAR=x.NameAR,
                            NameEn=x.NameEn,
                        }).ToList()
                    }).ToList();


                datagrouped = groupedmodules;
            }
            return datagrouped.ToList();
        }
    public GeneralMessage SaveDetails(GuideDepartmentDetails DepDetails, int UserId,int BranchId)
        {
            try
            {
                if (DepDetails.DepDetailsId == 0)
                {
                    DepDetails.AddUser = UserId;
                    DepDetails.AddDate = DateTime.Now;
                    _TaamerProContext.GuideDepartmentDetails.Add(DepDetails);
                     _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة تفاصيل رابط جديد";
                    _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully, ReturnedParm = DepDetails.DepDetailsId, ReturnedStr = DepDetails.DepDetailsId.ToString() };
                }
                else
                {
                    //var DepDetailsUpdated = _GuideDepartmentDetailsRepository.GetById(DepDetails.DepDetailsId);
                    GuideDepartmentDetails? DepDetailsUpdated = _TaamerProContext.GuideDepartmentDetails.Where(s => s.DepDetailsId == DepDetails.DepDetailsId).FirstOrDefault();
                    if (DepDetailsUpdated != null)
                    {
                        DepDetailsUpdated.Header = DepDetails.Header;
                        DepDetailsUpdated.Text = DepDetails.Text;
                        DepDetailsUpdated.Link = DepDetails.Link;
                        DepDetailsUpdated.Type = DepDetails.Type;
                        DepDetailsUpdated.NameAR = DepDetails.NameAR;
                        DepDetailsUpdated.NameEn = DepDetails.NameEn;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل  تفاصيل رابط رقم " + DepDetails.DepDetailsId;
                    _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =Resources.General_SavedSuccessfully, ReturnedParm = DepDetails.DepDetailsId, ReturnedStr = DepDetails.DepDetailsId.ToString() };

                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ  تفاصيل الرابط";
                _SystemAction.SaveAction("SaveClause", "Acc_ClausesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteDetails(int DepDetails, int UserId, int BranchId)
        {
            try
            {
                // var DepDetailsUpdated = _GuideDepartmentDetailsRepository.GetById(DepDetails);
                GuideDepartmentDetails? DepDetailsUpdated = _TaamerProContext.GuideDepartmentDetails.Where(s => s.DepDetailsId == DepDetails).FirstOrDefault();
                if (DepDetailsUpdated != null)
                {
                    DepDetailsUpdated.IsDeleted = true;
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف  تفاصيل رابط رقم " + DepDetails;
                    _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage() { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_DeletedSuccessfully };
                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف  تفاصيل رابط رقم " + DepDetails; ;
                _SystemAction.SaveAction("DeleteClause", "Acc_ClausesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage() {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_DeletedFailed };

                }
        }
      

    }
}
