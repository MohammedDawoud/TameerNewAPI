using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Acc_FloorsService : IAcc_FloorsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAcc_FloorsRepository _acc_FloorsRepository;
        public Acc_FloorsService(IAcc_FloorsRepository acc_FloorsRepository
            , TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _acc_FloorsRepository = acc_FloorsRepository;
        }
        public  Task<IEnumerable<Acc_FloorsVM>> GetAllFloors(string SearchText)
        {
            var Floors = _acc_FloorsRepository.GetAllFloors(SearchText);
            return Floors;
        }
        public GeneralMessage SaveFloor(Acc_Floors Floor, int UserId, int BranchId)
        {
            try
            {

                if (Floor.FloorId == 0)
                {


                    Floor.AddUser = UserId;
                    Floor.AddDate = DateTime.Now;
                    _TaamerProContext.Acc_Floors.Add(Floor);
                    _TaamerProContext.SaveChanges();
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة دور جديد";
                    _SystemAction.SaveAction("SaveFloor", "Acc_FloorsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase =Resources.General_SavedSuccessfully};

                }
                else
                {
                    var FloorUpdated = _TaamerProContext.Acc_Floors.Where(x=>x.FloorId==Floor.FloorId).FirstOrDefault();
                    if (FloorUpdated != null)
                    {
                        FloorUpdated.FloorName = Floor.FloorName;
                        FloorUpdated.FloorRatio = Floor.FloorRatio;
                        FloorUpdated.UpdateUser = UserId;
                        FloorUpdated.UpdateDate = DateTime.Now;

                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل دور رقم " + Floor.FloorId;
                    _SystemAction.SaveAction("SaveFloor", "Acc_FloorsService", 2,Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.General_SavedSuccessfully};

                }

            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ الدور";
                _SystemAction.SaveAction("SaveFloor", "Acc_FloorsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
        public GeneralMessage DeleteFloor(int FloorId, int UserId, int BranchId)
        {
            try
            {

                Acc_Floors Floor = _TaamerProContext.Acc_Floors.Where(x => x.FloorId == FloorId).FirstOrDefault();
                Floor.IsDeleted = true;
                Floor.DeleteDate = DateTime.Now;
                Floor.DeleteUser = UserId;
                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " حذف دور رقم " + FloorId;
                _SystemAction.SaveAction("DeleteFloor", "Acc_FloorsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

            }
            catch (Exception)
            {

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف دور رقم " + FloorId; ;
                _SystemAction.SaveAction("DeleteFloor", "Acc_FloorsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };

            }
        }
    }
}
