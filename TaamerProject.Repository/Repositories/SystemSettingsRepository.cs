using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class SystemSettingsRepository : ISystemSettingsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public SystemSettingsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public async Task<SystemSettingsVM> GetSystemSettingsByBranchId(int BranchId)
        {
            //var va = 1;
            var SystemSettings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/).Select(x => new SystemSettingsVM
            {
                SettingId = x.SettingId,
                FiscalYear = x.FiscalYear,
                BranchId= x.BranchId,
                AttendenceId = x.AttendenceId,
                VoucherGenerateCode = x.VoucherGenerateCode,
                NoReplyMail = x.NoReplyMail,
                ProjGenerateCode = x.ProjGenerateCode,
                OfferGenerateCode = x.OfferGenerateCode,
                EmpGenerateCode = x.EmpGenerateCode,
                ContractGenerateCode = x.ContractGenerateCode,
                CurrencyId = x.CurrencyId,
                CustGenerateCode = x.CustGenerateCode, 
                DecimalPoints = x.DecimalPoints,
                BranchGenerateCode =x.BranchGenerateCode,
                ActiveCodeInterval = x.ActiveCodeInterval,
                ActiveUserNumber = x.ActiveUserNumber,
                EnableSMS =x.EnableSMS ?? false,
                LogErrors =x.LogErrors ?? false,
                SMTPPort=x.SMTPPort,
                EnableNotification = x.EnableNotification ?? false,
                DefaultUserSession = x.DefaultUserSession,
                PhoneNoDigits = x.PhoneNoDigits,
                MobileNoDigits = x.MobileNoDigits,
                NationalIDDigits = x.NationalIDDigits,
                ContractGenerateCode2 = x.ContractGenerateCode2??"",
                CustomerNationalIdIsRequired = x.CustomerNationalIdIsRequired ?? false,
                CustomerMailIsRequired = x.CustomerMailIsRequired ?? false,
                OrgDataIsRequired =x.OrgDataIsRequired ?? false,
                Contract_Con_Code=x.Contract_Con_Code,
                Contract_Des_Code=x.Contract_Des_Code,
                Contract_Sup_Code=x.Contract_Sup_Code,
                CustomerphoneIsRequired=x.CustomerphoneIsRequired,
                UploadInvZatca = x.UploadInvZatca ?? false,
                ContractEndNote=x.ContractEndNote??0,
                ResedentEndNote = x.ResedentEndNote??0,
                ValueAddedSeparated=x.ValueAddedSeparated??false,
                
            }).FirstOrDefault();
            
            
            SystemSettings.DefaultUserSession = 5;
            return SystemSettings;
        }

        public async Task<bool> MaintenanceFunc (string Con, string Lang, int BranchId, int UserId,int Status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Sys_Maintenance";
                        command.Connection = con;
                        command.Parameters.Add(new SqlParameter("@Status", Status));
                        con.Open();
                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task< SystemSettingsVM> GetSystemSettingsByUserId(int BranchId,int UserID,string Con)
        {
        var SystemSettings = _TaamerProContext.SystemSettings.Where(s => s.IsDeleted == false /*&& s.BranchId == BranchId*/).Select(x => new SystemSettingsVM
            {
                SettingId = x.SettingId,
                FiscalYear = x.FiscalYear,
                BranchId = x.BranchId,
                AttendenceId = x.AttendenceId,
                VoucherGenerateCode = x.VoucherGenerateCode,
                NoReplyMail = x.NoReplyMail,
                ProjGenerateCode = x.ProjGenerateCode,
                OfferGenerateCode = x.OfferGenerateCode,
                EmpGenerateCode = x.EmpGenerateCode,
                ContractGenerateCode = x.ContractGenerateCode,
                CurrencyId = x.CurrencyId,
                CustGenerateCode = x.CustGenerateCode,
                DecimalPoints = x.DecimalPoints,
                BranchGenerateCode = x.BranchGenerateCode,
                ActiveCodeInterval = x.ActiveCodeInterval,
                ActiveUserNumber = x.ActiveUserNumber,
                EnableSMS = x.EnableSMS,
                LogErrors = x.LogErrors ?? false,
                SMTPPort = x.SMTPPort,
                EnableNotification = x.EnableNotification ?? false,
               // DefaultUserSession = x.DefaultUserSession,
                PhoneNoDigits = x.PhoneNoDigits,
                MobileNoDigits = x.MobileNoDigits,
                NationalIDDigits = x.NationalIDDigits,
                ContractGenerateCode2 = x.ContractGenerateCode2,
                CustomerMailIsRequired = x.CustomerMailIsRequired ?? false,
                CustomerNationalIdIsRequired = x.CustomerNationalIdIsRequired ?? false,
                EditUserName= x.UpdateUserT == null ? "":x.UpdateUserT.FullName??"",
                EditUserDate=x.UpdateDate,
                OrgDataIsRequired = x.OrgDataIsRequired ?? false,
                Contract_Con_Code = x.Contract_Con_Code ??"",
                Contract_Des_Code = x.Contract_Des_Code ??"",
                Contract_Sup_Code = x.Contract_Sup_Code ??"",
                CustomerphoneIsRequired=x.CustomerphoneIsRequired ?? false,
                UploadInvZatca = x.UploadInvZatca ?? false,
            ContractEndNote = x.ContractEndNote ?? 0,
            ResedentEndNote = x.ResedentEndNote ?? 0,
            ValueAddedSeparated = x.ValueAddedSeparated ?? false,


        }).FirstOrDefault();
            //updating user session var
            //SqlConnection con = new SqlConnection(Con);//SqlConnection("Data Source=144.91.68.47\\sqlexpress;Initial Catalog=TameerProDB;uID=sa;Password=admin_134711;");
            //SqlDataAdapter da = new SqlDataAdapter("SELECT COALESCE([Session],0) FROM Sys_Users where userid=" + UserID.ToString(), con);
            //da.SelectCommand.CommandType = CommandType.Text;
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //int UserSession;
            //DataRow Dr = ds.Tables[0].Rows[0];
            //UserSession = int.Parse(Dr[0].ToString());
            //con.Close();
            //if (UserSession != 0)
            //    SystemSettings.DefaultUserSession = UserSession;

            
            return SystemSettings;
        }


    }
}
