using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //try
            //{
            //    this.Cursor = Cursors.WaitCursor;
            //    ShowStatusBar(string.Empty, true);

            //    if (IsDeviceConnected)
            //    {
            //        IsDeviceConnected = false;
            //        this.Cursor = Cursors.Default;

            //        return;
            //    }

            //    string ipAddress = X.tbxDeviceIP.Text.Trim();
            //    string port = tbxPort.Text.Trim();
            //    if (ipAddress == string.Empty || port == string.Empty)
            //        throw new Exception("The Device IP Address and Port is mandotory !!");

            //    int portNumber = 4370;
            //    if (!int.TryParse(port, out portNumber))
            //        throw new Exception("Not a valid port number");

            //    bool isValidIpA = UniversalStatic.ValidateIP(ipAddress);
            //    if (!isValidIpA)
            //        throw new Exception("The Device IP is invalid !!");

            //    isValidIpA = UniversalStatic.PingTheDevice(ipAddress);
            //    if (!isValidIpA)
            //        throw new Exception("The device at " + ipAddress + ":" + port + " did not respond!!");

            //    objZkeeper = new ZkemClient(RaiseDeviceEvent);
            //    IsDeviceConnected = objZkeeper.Connect_Net(ipAddress, portNumber);

            //    if (IsDeviceConnected)
            //    {
            //        string deviceInfo = manipulator.FetchDeviceInfo(objZkeeper, int.Parse(tbxMachineNumber.Text.Trim()));
            //        lblDeviceInfo.Text = deviceInfo;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ShowStatusBar(ex.Message, false);
            //}
            //this.Cursor = Cursors.Default;
        }
    }
}
