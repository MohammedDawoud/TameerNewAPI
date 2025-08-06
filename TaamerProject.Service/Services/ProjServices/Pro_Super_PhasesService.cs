using System;
using System.Collections.Generic;
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
using TaamerProject.Service.Generic;
using System.Diagnostics.Contracts;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Pro_Super_PhasesService :   IPro_Super_PhasesService
    {
        private readonly IPro_Super_PhasesRepository _Pro_Super_PhasesRepository;
        private readonly IPro_Super_PhaseDetRepository _Pro_Super_PhaseDetRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public Pro_Super_PhasesService(IPro_Super_PhasesRepository Pro_Super_PhasesRepository,
            IPro_Super_PhaseDetRepository Pro_Super_PhaseDetRepository,TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _Pro_Super_PhasesRepository = Pro_Super_PhasesRepository;
            _Pro_Super_PhaseDetRepository = Pro_Super_PhaseDetRepository;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }

        public Task<IEnumerable<Pro_Super_PhasesVM>> GetAllSuper_Phases(string SearchText)
        {
            var Super_Phases = _Pro_Super_PhasesRepository.GetAllSuperPhases(SearchText);
            return Super_Phases;
        }
        public Task<Pro_Super_PhasesVM> GetSuper_PhasesById(int SuperId)
        {
            var Super_Phases = _Pro_Super_PhasesRepository.GetSuper_PhasesById(SuperId);
            return Super_Phases;
        }
        public Task<IEnumerable<Pro_Super_PhaseDetVM>> GetAllSuper_PhaseDet(int? PhaseId)
        {
            var Super_PhaseDet = _Pro_Super_PhaseDetRepository.GetAllSuper_PhaseDet(PhaseId);
            return Super_PhaseDet;
        }
        public GeneralMessage SaveSuper_Phases(Pro_Super_Phases Phases, int UserId, int BranchId)
        {
            try
            {

                if (Phases.PhaseId == 0)
                {


                    Phases.AddUser = UserId;
                    Phases.IsRead = false;
                    Phases.BranchId = BranchId;
                    Phases.UserId = UserId;
                    Phases.AddDate = DateTime.Now;
                    _TaamerProContext.Pro_Super_Phases.Add(Phases);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة مرحلة اشراف جديد";
                    _SystemAction.SaveAction("SaveSuper_Phases", "Pro_Super_PhasesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_SavedSuccessfully };
                }
                else
                {
                    //var PhasesUpdated = _Pro_Super_PhasesRepository.GetById(Phases.PhaseId);
                    Pro_Super_Phases? PhasesUpdated = _TaamerProContext.Pro_Super_Phases.Where(s => s.PhaseId == Phases.PhaseId).FirstOrDefault();
                    if (PhasesUpdated != null)
                    {

                        PhasesUpdated.NameAr = Phases.NameAr;
                        PhasesUpdated.NameEn = Phases.NameEn;
                        PhasesUpdated.UpdateUser = UserId;
                        PhasesUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل مرحلة اشراف رقم " + Phases.PhaseId;
                    _SystemAction.SaveAction("SaveSuper_Phases", "Pro_Super_PhasesService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_EditedSuccessfully };
                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مرحلة اشراف";
                _SystemAction.SaveAction("SaveSuper_Phases", "Pro_Super_PhasesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_SavedFailed };
            }
        }
        public GeneralMessage DeleteSuper_Phases(int PhaseId, int UserId, int BranchId)
        {
            try
            {
                if(PhaseId<=14)
                {
                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "لا يمكنك حذف هذه المرحلة" };
                }


                //  Pro_Super_Phases Phases = _Pro_Super_PhasesRepository.GetById(PhaseId);
                Pro_Super_Phases? Phases = _TaamerProContext.Pro_Super_Phases.Where(s => s.PhaseId == PhaseId).FirstOrDefault();
                if (Phases != null)
                {
                    Phases.IsDeleted = true;
                    Phases.DeleteDate = DateTime.Now;
                    Phases.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مرحلة اشراف رقم " + PhaseId;
                    _SystemAction.SaveAction("DeleteSuper_Phases", "Pro_Super_PhasesService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }

                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase =  Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مرحلة اشراف رقم " + PhaseId; ;
                _SystemAction.SaveAction("DeleteSuper_Phases", "Pro_Super_PhasesService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.General_DeletedFailed };
            }
        }


        public GeneralMessage UpdateStatus_SuperPhaseDet(int PhaseDetailesId,bool? ISRead, int UserId, int BranchId)
        {
            try
            {

                //var PhaseDetUpdated = _Pro_Super_PhaseDetRepository.GetById(PhaseDetailesId);
                Pro_Super_PhaseDet? PhaseDetUpdated = _TaamerProContext.Pro_Super_PhaseDet.Where(s => s.PhaseDetailesId == PhaseDetailesId).FirstOrDefault();

                if (PhaseDetUpdated != null)
                {
                    PhaseDetUpdated.IsRead = ISRead;

                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل حالة تفاصيل مرحلة اشراف  ";
                _SystemAction.SaveAction("SaveSuperPhaseDet", "Pro_Super_PhasesService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في تعديل حالة تفاصيل مرحلة اشراف";
                _SystemAction.SaveAction("SaveSuperPhaseDet", "SettingsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
                }

        }

        public GeneralMessage SaveSuperPhaseDet(List<Pro_Super_PhaseDet> PhaseDets, int UserId, int BranchId)
        {
            try
            {
                // var PhaseDetBefore = _Pro_Super_PhaseDetRepository.GetMatching(s=>s.PhaseId == PhaseDets[0].PhaseId).ToList();
                var PhaseDetBefore = _TaamerProContext.Pro_Super_PhaseDet.Where(s => s.PhaseId == PhaseDets[0].PhaseId).ToList();

                _TaamerProContext.Pro_Super_PhaseDet.RemoveRange(PhaseDetBefore.ToList());

                foreach (Pro_Super_PhaseDet PhaseDet in PhaseDets)
                {
                    if (PhaseDet.PhaseDetailesId == 0)
                    {
                        PhaseDet.BranchId = BranchId;
                        PhaseDet.IsRead = false;
                        PhaseDet.PhaseId = PhaseDet.PhaseId;
                        PhaseDet.NameAr = PhaseDet.NameAr;
                        PhaseDet.Note = PhaseDet.Note;
                        PhaseDet.AddUser = UserId;
                        PhaseDet.AddDate = DateTime.Now;
                        _TaamerProContext.Pro_Super_PhaseDet.Add(PhaseDet);
                    }
                    else
                    {
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate2 = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote2 = "فشل في حفظ مجموعة تفاصيل مرحلة اشراف";
                        _SystemAction.SaveAction("SaveSuperPhaseDet", "Pro_Super_PhasesService", 1, Resources.General_SavedFailed, "", "", ActionDate2, UserId, BranchId, ActionNote2, 0);
                        //-----------------------------------------------------------------------------------------------------------------
                    }

                    _TaamerProContext.SaveChanges();
                }
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "اضافة تفاصيل مرحلة اشراف";
                _SystemAction.SaveAction("SaveSuperPhaseDet", "Pro_Super_PhasesService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully };
                }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مجموعة تفاصيل مرحلة اشراف";
                _SystemAction.SaveAction("SaveSuperPhaseDet", "Pro_Super_PhasesService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest,ReasonPhrase = Resources.General_SavedFailed };
                }

        }

    
    }
}
