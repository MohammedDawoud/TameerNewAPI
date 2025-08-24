
using System.Globalization;
using System.Net;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;
using TaamerProject.Models.DomainObjects;

namespace TaamerProject.Service.Services.EmpServices
{
    public class Law_regulationsService : ILaw_regulationsService
    {

        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly ILaw_regulationsRepository _law_Regulations;



        public Law_regulationsService(TaamerProjectContext dataContext, ISystemAction systemAction, ILaw_regulationsRepository law_Regulations)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _law_Regulations = law_Regulations;
        }
        public Task<List<Emp_AbsenceListVM>> GetAbsenceLate()
        {
           return _law_Regulations.GetAbsenceLate();
        }

        public Task<List<Emp_LateListVM>> GetLateLaw()
        {
             return _law_Regulations.GetLateLaw();
        }

        public Law_Regulations GetLaw_Regulations()
        {
            return _law_Regulations.Getlaw_Regulations();
        }




        public GeneralMessage saveLateLaw(Emp_LateList late, int UserId, int BranchId)
        {
            try
            {
                
                    if (late.ID == 0)
                    {
                        //    late.AddUser = UserId;
                        //    late.AddDate = DateTime.Now;
                        //    _TaamerProContext.LateLists.Add(late);
                        //    _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة بند";
                        _SystemAction.SaveAction("saveLateLaw", "Law_eregulations", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                    }
                    else
                    {
                        var latelawUpdated = _TaamerProContext.LateLists.Where(x => x.ID == late.ID).FirstOrDefault();

                        if (latelawUpdated != null)
                        {
                            latelawUpdated.First = late.First;
                            latelawUpdated.Second = late.Second;
                            latelawUpdated.Third = late.Third;
                            latelawUpdated.Fourth = late.Fourth;


                            latelawUpdated.UpdateUser = UserId;
                            latelawUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "تعديل بند رقم " + late.ID;
                        _SystemAction.SaveAction("saveLateLaw", "Law_eregulations", 1, "تم التعديل", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                    }
                

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العطلة الرسمية";
                _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage saveAbsenceLaw(Emp_AbsenceList absence, int UserId, int BranchId)
        {
            try
            {
               
                    if (absence.ID == 0)
                    {
                        //    late.AddUser = UserId;
                        //    late.AddDate = DateTime.Now;
                        //    _TaamerProContext.LateLists.Add(late);
                        //    _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة بند";
                        _SystemAction.SaveAction("saveAbsenceLaw", "Law_eregulations", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                    }
                    else
                    {
                        var absencelawUpdated = _TaamerProContext.AbsenceLists.Where(x => x.ID == absence.ID).FirstOrDefault();

                        if (absencelawUpdated != null)
                        {
                            absencelawUpdated.First = absence.First;
                            absencelawUpdated.Second = absence.Second;
                            absencelawUpdated.Third = absence.Third;
                            absencelawUpdated.Fourth = absence.Fourth;


                            absencelawUpdated.UpdateUser = UserId;
                            absencelawUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "تعديل بند رقم " + absence.ID;
                        _SystemAction.SaveAction("saveAbsenceLaw", "Law_eregulations", 1, "تم التعديل", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                    }
                

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العطلة الرسمية";
                _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage saveLateLaw(List<Emp_LateList> latelist, int UserId, int BranchId)
        {
            try
            {
                foreach (var late in latelist)
                {
                    if (late.ID == 0)
                    {
                        //    late.AddUser = UserId;
                        //    late.AddDate = DateTime.Now;
                        //    _TaamerProContext.LateLists.Add(late);
                        //    _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة بند";
                        _SystemAction.SaveAction("saveLateLaw", "Law_eregulations", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                    }
                    else
                    {
                        var latelawUpdated = _TaamerProContext.LateLists.Where(x => x.ID == late.ID).FirstOrDefault();

                        if (latelawUpdated != null)
                        {
                            latelawUpdated.First = late.First;
                            latelawUpdated.Second = late.Second;
                            latelawUpdated.Third = late.Third;
                            latelawUpdated.Fourth = late.Fourth;


                            latelawUpdated.UpdateUser = UserId;
                            latelawUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "تعديل بند رقم " + late.ID;
                        _SystemAction.SaveAction("saveLateLaw", "Law_eregulations", 1, "تم التعديل", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                    }
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العطلة الرسمية";
                _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage saveAbsenceLaw(List<Emp_AbsenceList> Absencelist, int UserId, int BranchId)
        {
            try
            {
                foreach (var absence in Absencelist)
                {
                    if (absence.ID == 0)
                    {
                        //    late.AddUser = UserId;
                        //    late.AddDate = DateTime.Now;
                        //    _TaamerProContext.LateLists.Add(late);
                        //    _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "اضافة بند";
                        _SystemAction.SaveAction("saveAbsenceLaw", "Law_eregulations", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

                    }
                    else
                    {
                        var absencelawUpdated = _TaamerProContext.AbsenceLists.Where(x => x.ID == absence.ID).FirstOrDefault();

                        if (absencelawUpdated != null)
                        {
                            absencelawUpdated.First = absence.First;
                            absencelawUpdated.Second = absence.Second;
                            absencelawUpdated.Third = absence.Third;
                            absencelawUpdated.Fourth = absence.Fourth;


                            absencelawUpdated.UpdateUser = UserId;
                            absencelawUpdated.UpdateDate = DateTime.Now;
                        }
                        _TaamerProContext.SaveChanges();
                        //-----------------------------------------------------------------------------------------------------------------
                        string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                        string ActionNote = "تعديل بند رقم " + absence.ID;
                        _SystemAction.SaveAction("saveAbsenceLaw", "Law_eregulations", 1, "تم التعديل", "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                        //-----------------------------------------------------------------------------------------------------------------
                        return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

                    }
                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_EditedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ العطلة الرسمية";
                _SystemAction.SaveAction("SaveOfficalHoliday", "OfficalHolidayService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

    }
}
