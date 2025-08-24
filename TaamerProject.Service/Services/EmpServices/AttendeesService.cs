using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.IGeneric;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class AttendeesService : IAttendeesService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;
        private readonly IAttendeesRepository _AttendeesRepository;

        public AttendeesService(TaamerProjectContext dataContext, ISystemAction systemAction, IAttendeesRepository attendees)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _AttendeesRepository = attendees;

        }
        public async Task<IEnumerable<AttendeesVM>> GetAllAttendees(int BranchId)
        {
            var Attendees =await _AttendeesRepository.GetAllAttendees(BranchId);
            return Attendees.ToList();
        }
        public async Task<IEnumerable<AttendeesVM>> GetAttendeesbyStatus(int Status, string Date, int BranchId)
        {
            var Attendees = await _AttendeesRepository.GetAttendeesbyStatus(Status, Date, BranchId);
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAttendeeslate(bool IsLate, string Date, int BranchId)
        {
            var Attendees = await _AttendeesRepository.GetAttendeeslate(IsLate, Date, BranchId);
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAttendeesEarlyCheckOut(bool IsEarlyCheckOut, string Date, int BranchId)
        {
            var Attendees = await _AttendeesRepository.GetAttendeesEarlyCheckOut(IsEarlyCheckOut, Date, BranchId);
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAttendeesOut(bool IsOut, string Date, int BranchId)
        {
            var Attendees = await _AttendeesRepository.GetAttendeesOut(IsOut, Date, BranchId);
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAllEmpAbsence(int Status, string Date, int BranchId)
        {
            var Attendees = await _AttendeesRepository.GetAllEmpAbsence(Status, Date, BranchId);
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAllEmplate(bool IsLateCheckIn, string Date, int BranchId)
        {
            var Attendees = await _AttendeesRepository.GetAllEmplate(IsLateCheckIn, Date, BranchId);
            return Attendees;
        }
        public async Task<IEnumerable<AttendeesVM>> GetAllEmpAbsencelate(string Date, int BranchId)
        {
            var Attendees = await _AttendeesRepository.GetAllEmpAbsencelate(Date, BranchId);
            return Attendees;
        }
        public void GetEmpAttendenceDevice(List<Thing> things)
        {
            
        }
    }
}
