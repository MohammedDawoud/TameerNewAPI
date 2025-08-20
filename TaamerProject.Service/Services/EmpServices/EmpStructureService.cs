
using System.Globalization;
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
    public class EmpStructureService :   IEmpStructureService
    {
        private readonly IEmpStructureRepository _EmpStructureRepository;
        private readonly INodeLocationsRepository _NodeLocationsRepository;
        private readonly IEmployeesRepository _EmployeeRepository;
        private readonly ISys_SystemActionsRepository _Sys_SystemActionsRepository;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        public EmpStructureService(TaamerProjectContext dataContext
            , ISystemAction systemAction, IEmpStructureRepository EmpStructureRepository
            , INodeLocationsRepository NodeLocationsRepository, IEmployeesRepository EmployeeRepository
            )
        {
            _EmpStructureRepository = EmpStructureRepository;
            _NodeLocationsRepository = NodeLocationsRepository;
            _EmployeeRepository = EmployeeRepository;
             _TaamerProContext = dataContext;
            _SystemAction = systemAction;

        }
        public GeneralMessage SaveEmpStructure(List<EmpStructure> empLink, List<NodeLocations> NodeLocList, int UserId, int BranchId)
        {
            try
            {
               // var existingempstruct= _EmpStructureRepository.GetMatching(s => s.IsDeleted == false && s.BranchId == BranchId);
                var existingempstruct = _TaamerProContext.EmpStructure.Where(s => s.IsDeleted == false && s.BranchId == BranchId);

                if (existingempstruct != null && existingempstruct.Count() > 0)
                {
                    _TaamerProContext.EmpStructure.RemoveRange(existingempstruct);
                }
                foreach (var item in empLink)
                {
                        var structure = new EmpStructure();
                        structure.EmpId = item.EmpId;
                        structure.ManagerId = item.ManagerId;
                        structure.BranchId = BranchId;
                        structure.IsDeleted = false;
                        structure.AddDate = DateTime.Now;
                        structure.AddUser = UserId;
                        _TaamerProContext.EmpStructure.Add(structure);
                }
                foreach (var node in NodeLocList)
                {
                    //var existingNodeLocation = _NodeLocationsRepository.GetMatching(s => s.EmpId == node.EmpId && s.IsDeleted == false).FirstOrDefault();
                    var existingNodeLocation = _TaamerProContext.NodeLocations.Where(s => s.EmpId == node.EmpId && s.IsDeleted == false).FirstOrDefault();


                    var relatedemp = _EmployeeRepository.GetById((int)node.EmpId);
                    if (existingNodeLocation != null)
                    {
                        existingNodeLocation.Location = node.Location;
                        existingNodeLocation.UpdateDate = DateTime.Now;
                        existingNodeLocation.UpdateUser = UserId;

                        relatedemp.LocationId = existingNodeLocation.LocationId;
                    }
                    else
                    {
                        var Loc = new NodeLocations();
                        Loc.EmpId = node.EmpId;
                        Loc.Location = node.Location;
                        Loc.AddDate = DateTime.Now;
                        Loc.AddUser = UserId;
                        _TaamerProContext.NodeLocations.Add(node);
                        _TaamerProContext.SaveChanges();

                        relatedemp.LocationId = Loc.LocationId;
                    }
                  
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.structureStaffSavedSuccessfully;
                _SystemAction.SaveAction("SaveEmpStructure", "EmpStructureService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage {  StatusCode = HttpStatusCode.OK,ReasonPhrase = Resources.structureStaffSavedSuccessfully };
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.structureStaffSavedSuccessfullyFaild;
                _SystemAction.SaveAction("SaveEmpStructure", "EmpStructureService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage {  StatusCode = HttpStatusCode.BadRequest,ReasonPhrase =  Resources.structureStaffSavedSuccessfullyFaild };
            }
        }
        public EmpNodeVM GetAllNodesEmps(string lang, int BranchId)
        {
            var NodeTasks = new EmpNodeVM();
            NodeTasks.nodeDataArray = _EmployeeRepository.GetAllEmployees(lang, BranchId).Result;
            NodeTasks.linkDataArray = _EmpStructureRepository.GetAllEmpStructure(BranchId).Result;
            return NodeTasks;
        }
        


    }
}
