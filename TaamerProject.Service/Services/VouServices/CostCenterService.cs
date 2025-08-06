using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using System.Net;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class CostCenterService : ICostCenterService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ICostCenterRepository _costCenterRepository;



        public CostCenterService(TaamerProjectContext dataContext, ISystemAction systemAction, ICostCenterRepository costCenterRepository)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _costCenterRepository = costCenterRepository;
        }
        public async Task<IEnumerable<CostCentersVM>> GetAllCostCenters(string SearchText, string lang, int BranchId)
        {
            var Priorities = await _costCenterRepository.GetAllCostCenters(SearchText.Trim(), lang, BranchId);
            return Priorities;
        }
        public async Task<IEnumerable<CostCentersVM>> GetAllCostCenters_B(string SearchText, string lang, int BranchId)
        {
            var Priorities = await _costCenterRepository.GetAllCostCenters_B(SearchText.Trim(), lang, BranchId);
            return Priorities;
        }
        public async Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCostBranch(string SearchText, string lang, int BranchId, int CostBranchId)
        {
            var Priorities = await _costCenterRepository.GetAllCostCentersByCostBranch(SearchText.Trim(), lang, BranchId, CostBranchId);
            return Priorities;
        }

        public async Task<CostCentersVM> GetBranch_Costcenter(string lang, int BranchId)
        {
            var Priorities =await _costCenterRepository.GetBranch_Costcenter(lang, BranchId);
            return Priorities;
        }
        public GeneralMessage SaveCostCenter(CostCenters costCenter, int UserId, int BranchId)
        {
            try
            {

                if (costCenter.CostCenterId == 0)
                {
                    var codeExist = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.Code == costCenter.Code && s.BranchId == BranchId).FirstOrDefault();
                    if (codeExist != null)
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ مركز التكلفة";
                        _SystemAction.SaveAction("SaveCostCenter", "CostCenterService", 1, Resources.TheCodeAlreadyExists, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------

                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.TheCodeAlreadyExists };
                    }
                    costCenter.AddUser = UserId;
                    costCenter.AddDate = DateTime.Now;
                    costCenter.CustomerId = costCenter.CustomerId;
                    costCenter.Code = costCenter.Code;
                    costCenter.Level = costCenter.Level;
                    costCenter.NameAr = costCenter.NameAr;
                    costCenter.NameEn = costCenter.NameEn;
                    if (costCenter.ParentId != null)
                    {
                        var CostCenterByid = _costCenterRepository.GetById((int)costCenter.ParentId);

                        costCenter.BranchId = CostCenterByid.BranchId;
                    }
                    else
                    {
                        costCenter.BranchId = BranchId;

                    }
                    costCenter.ParentId = costCenter.ParentId;
                    costCenter.ProjId = costCenter.ProjId;

                    _TaamerProContext.CostCenters.Add(costCenter);

                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مركز تكلفة جديد";
                    _SystemAction.SaveAction("SaveCostCenter", "CostCenterService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedSuccessfully };

                }
                else
                {
                    var costCenterUpdated = _costCenterRepository.GetById(costCenter.CostCenterId);
                    if (costCenterUpdated != null)
                    {
                        costCenterUpdated.Code = costCenter.Code;
                        costCenterUpdated.CustomerId = costCenter.CustomerId;
                        costCenterUpdated.NameAr = costCenter.NameAr;
                        costCenterUpdated.NameEn = costCenter.NameEn;
                        costCenterUpdated.Level = costCenter.Level;
                        if (costCenter.ParentId != null)
                        {

                            if (costCenterUpdated.ProjId == 0)
                            {
                                //-----------------------------------------------------------------------------------------------------------------
                                string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                                string ActionNote3 = "فشل في الحفظ يجب ان يكون الفرع هو المركز الرئيسي";
                                _SystemAction.SaveAction("SaveCostCenter", "CostCenterService", 1, Resources.General_SavedFailed, "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                                //-----------------------------------------------------------------------------------------------------------------

                                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "فشل في الحفظ يجب ان يكون الفرع هو مركز التكلفة الرئيسي" };
                            }
                            else
                            {
                                var CostCenterByid = _costCenterRepository.GetById((int)costCenter.ParentId);

                                costCenterUpdated.BranchId = CostCenterByid.BranchId;
                            }


                        }
                        costCenterUpdated.ParentId = costCenter.ParentId;
                        //costCenterUpdated.ProjId = costCenter.ProjId;
                        costCenterUpdated.UpdateUser = UserId;
                        costCenterUpdated.UpdateDate = DateTime.Now;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مركز تكلفة رقم " + costCenter.CostCenterId;
                    _SystemAction.SaveAction("SaveCostCenter", "CostCenterService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مركز التكلفة";
                _SystemAction.SaveAction("SaveCostCenter", "CostCenterService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteCostCenter(int CostCenterId, int UserId, int BranchId)
        {
            try
            {


                int Count = _TaamerProContext.CostCenters.Where(s => s.IsDeleted == false && s.ParentId == CostCenterId).Count();
                if (Count > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حذف مركز التكلفة";
                   _SystemAction.SaveAction("DeleteCostCenter", "CostCenterService", 1, "لا يمكن حذف مركز التكلفة , مركز التكلفة مرتبط بمراكز فرعية", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
                }
                var CostCenterUpdated = _TaamerProContext.CostCenters.Where(s => s.CostCenterId == CostCenterId && s.IsDeleted == false && s.ProjId == 0).FirstOrDefault();
                if (CostCenterUpdated != null)
                {


                    Branch bran = _TaamerProContext.Branch.Where(x=>x.BranchId==CostCenterUpdated.BranchId).FirstOrDefault();
                    if (bran != null)
                    {
                        if (bran.IsDeleted == true)
                        {
                            CostCenters costcenter2 = _costCenterRepository.GetById(CostCenterId);
                            costcenter2.IsDeleted = true;
                            costcenter2.DeleteDate = DateTime.Now;
                            costcenter2.DeleteUser = UserId;
                            _TaamerProContext.SaveChanges();
                            //-----------------------------------------------------------------------------------------------------------------
                            string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                            string ActionNote2 = " حذف مركز تكلفة رقم " + CostCenterId;
                            _SystemAction.SaveAction("DeleteCostCenter", "CostCenterService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate2, UserId, BranchId, ActionNote2, 1);
                            //-----------------------------------------------------------------------------------------------------------------

                            return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
                        }
                    }


                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حذف مركز التكلفة";
                    _SystemAction.SaveAction("DeleteCostCenter", "CostCenterService", 1, "لا يمكن حذف مركز التكلفة , مركز التكلفة مرتبط بفرع", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
                }
                var projectUpdated = _TaamerProContext.Project.Where(s => s.IsDeleted == false && s.CostCenterId == CostCenterId);
                if (projectUpdated.Count() > 0)
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate3 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote3 = "فشل في حذف مركز التكلفة";
                    _SystemAction.SaveAction("DeleteCostCenter", "CostCenterService", 1, "لا يمكن حذف مركز التكلفة , مركز التكلفة مرتبط بمشاريع ", "", "", ActionDate3, UserId, BranchId, ActionNote3, 0);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
                }

                CostCenters costcenter = _costCenterRepository.GetById(CostCenterId);
                costcenter.IsDeleted = true;
                costcenter.DeleteDate = DateTime.Now;
                costcenter.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف مركز تكلفة رقم " + CostCenterId;
                _SystemAction.SaveAction("DeleteCostCenter", "CostCenterService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مركز تكلفة رقم " + CostCenterId; ;
                _SystemAction.SaveAction("DeleteCostCenter", "CostCenterService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }

        public List<CostCenterTreeVM> GetCostCenterTree(string Lang, int BranchId)
        {
            var costCenters = _costCenterRepository.GetAllCostCenters("", Lang, BranchId).Result.OrderBy(s => s.CostCenterId);
            if (costCenters != null && costCenters.Count() > 0)
            {
                List<CostCenterTreeVM> treeItems = new List<CostCenterTreeVM>();
                foreach (var item in costCenters)
                {
                    treeItems.Add(new CostCenterTreeVM(item.CostCenterId.ToString(), ((item.ParentId == 0 || item.ParentId == null) ? "#" : item.ParentId.ToString()), item.NameAr = item.NameAr + " - " + item.Code));
                }
                return treeItems;
            }
            else
            {
                return new List<CostCenterTreeVM>();
            }
        }
        public async Task<CostCentersVM> GetCostCenterById(int costCenterId)
        {
            return await _costCenterRepository.GetCostCenterById(costCenterId);
        }
        public async Task<CostCentersVM> GetCostCenterByProId(int ProjectId)
        {
            return await _costCenterRepository.GetCostCenterByProId(ProjectId);
        }
        public async Task<CostCentersVM> GetCostCenterByCode(string Code, string lang, int BranchId)
        {
            return await _costCenterRepository.GetCostCenterByCode(Code, lang, BranchId);
        }
        public async Task<IEnumerable<CostCentersVM>> GetAllCostCentersTransactions(string FromDate, string ToDate, string lang, int BranchId, int? yearid)
        {
            return await _costCenterRepository.GetAllCostCentersTransactions(FromDate, ToDate, yearid ?? default(int), lang, BranchId);
        }
        public async Task<IEnumerable<CostCentersVM>> GetCostCenterReport(int BranchId, string lang, string FromDate, string ToDate, int? yearid)
        {
            return await _costCenterRepository.GetCostCenterReport(BranchId, lang, yearid ?? default(int), FromDate, ToDate);
        }
        public async Task<IEnumerable<CostCentersVM>> GetCostCenterTransaction(int BranchId, string lang, int CostCenterId, string FromDate, string ToDate, int? yearid)
        {
            return await _costCenterRepository.GetCostCenterTransaction(BranchId, lang, yearid ?? default(int), CostCenterId, FromDate, ToDate);
        }
        public async Task<IEnumerable<CostCentersVM>> GetCostCenterAccountTransaction(int BranchId, string lang, int CostCenterId, string FromDate, string ToDate, int? yearid)
        {
            return await _costCenterRepository.GetCostCenterAccountTransaction(BranchId, lang, yearid ?? default(int), CostCenterId, FromDate, ToDate);
        }
        public IEnumerable<object> FillAllCostCenterSelect(int BranchId, int custID)
        {
            return _costCenterRepository.GetAllCostCenterByCustomers(BranchId, custID).Result.Select(s => new
            {
                Id = s.CostCenterId,
                Name = s.NameAr
            });
        }
        //heba
        public async Task<DataTable> TreeViewOfCostCenter(string Con)
        {
            return await _costCenterRepository.TreeViewOfCostCenter(Con);
        }

        public async Task<IEnumerable<CostCentersVM>> GetAllCostCentersByCustId(string lang, int BranchId, int? CustId)
        {
            var Priorities = await _costCenterRepository.GetAllCostCentersByCustId(lang, BranchId, CustId);
            return Priorities;
        }
    }
}
