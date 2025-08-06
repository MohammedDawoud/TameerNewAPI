using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.Enums
{
    public enum NotificationCode
    {
        Project_New = 1,
        Project_AddParticipants = 2,
        Project_70PercentDuration = 3,
        Project_Delayed = 4,
        Project_MainStageCompleted = 5,
        Project_SubStageCompleted = 6,
        Project_UploadToExternal = 7,
        Project_ReplyFromExternal = 8,
        Project_Stopped = 9,
        Project_Reactivated = 10,
        Project_ServiceInvoiceIssued = 11,
        Project_QuotationReminder = 12,
        Task_New = 13,
        Task_Completed = 14,
        Task_70PercentTime = 15,
        Task_BecameRunning = 16,
        Task_ExtensionRequested = 17,
        Task_ExtensionAccepted = 18,
        Task_ExtensionRejected = 19,
        Task_TransferRequested = 20,
        Task_TransferAccepted = 21,
        Task_TransferRejected = 22,
        Task_FromDifferentBranch = 23,
        Meeting_AppointmentCreated = 24,
        Meeting_Reminder = 25,
        Supervision_Assigned = 26,
        Supervision_Received = 27,
        Project_Completed = 28,
        Task_SpecialRequirement = 29,

        HR_EmployeeStart = 31,
        HR_AdvanceRequest = 32,
        HR_AdvanceAccepted = 33,
        HR_AdvanceRejected = 34,
        HR_LeaveRequest = 35,
        HR_LeaveAccepted = 36,
        HR_LeaveRejected = 37,
        HR_AssignAsset = 38,
        HR_UnassignAsset = 39,
        HR_IdExpired = 40,
        HR_ContractExpired = 41,
        HR_TransferBranch = 42,
        HR_ChangeJobTitle = 43,

        Accounting_InvoiceDueReminder = 44,

        Dashboard_BackupReminder = 45
    }

}
