using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.DomainObjects;
using System.Diagnostics.Contracts;

namespace TaamerProject.Repository.Repositories
{
    public class DatabaseBackupRepository :IDatabaseBackupRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public DatabaseBackupRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<DatabaseBackupVM>> GetAllDBackup()
        {
            try
            {
                var AllDBackup = _TaamerProContext.DatabaseBackup.Where(w => w.IsDeleted == false).Select(x => new DatabaseBackupVM
                {
                    BackupId = x.BackupId,
                    SavedName = x.SavedName,
                    UserId = x.UserId,
                    UserName = x.Users.FullName,
                    LocalSavedPath = x.LocalSavedPath,
                    Date = x.Date,
                    FileSize = x.FileSize??"",

                }).ToList().OrderByDescending(o => o.BackupId);
                return AllDBackup;

            }
            catch(Exception ex) {
                return null;
               // return new List<DatabaseBackupVM>();
            }
           

        }

        public BackupStatistics GetBackupStatistics(string lang)
        {
            BackupStatistics statistics =new BackupStatistics();


            try {
                statistics.ProjectCount=_TaamerProContext.Project.Where(x=>x.IsDeleted==false && x.Status==0).Count();
                statistics.ProjectArchivedCount = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.Status == 1).Count();
                statistics.BranchesCount=_TaamerProContext.Branch.Where(x=>x.IsDeleted == false).Count();
                statistics.UsersCount=_TaamerProContext.Users.Where(x=>x.IsDeleted == false).Count();
                statistics.Customercount=_TaamerProContext.Customer.Where(x=>x.IsDeleted == false).Count();

                statistics.LastProject=_TaamerProContext.Project.Where(x=>x.IsDeleted==false).Select(x=> new ProjectVM
                {
                    ProjectId = x.ProjectId,
                    ProjectNo = x.ProjectNo,
                }).OrderByDescending(x=>x.ProjectId).FirstOrDefault();

                statistics.LastInvoice = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.Type == 2).Select(x => new InvoicesVM
                {

                    InvoiceId = x.InvoiceId,
                    InvoiceNumber = x.InvoiceNumber,
                    AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,


                }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();

                statistics.LastInvoiceRet=_TaamerProContext.Invoices.Where(x=>x.IsDeleted == false && x.Type == 2 && x.IsPost==true).Select(x =>new InvoicesVM
                {

                    InvoiceId = x.InvoiceId,
                    InvoiceNumber = x.InvoiceNumber,
                    AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                statistics.lastRevoucern = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.Type == 6).Select(x => new InvoicesVM
                {

                    InvoiceId = x.InvoiceId,
                    InvoiceNumber = x.InvoiceNumber,
                    AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                statistics.lastpayvoucern = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.Type == 5 ).Select(x => new InvoicesVM
                {

                    InvoiceId = x.InvoiceId,
                    InvoiceNumber = x.InvoiceNumber,
                    AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                statistics.lastEntyvoucher = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.Type == 8).Select(x => new InvoicesVM
                {

                    InvoiceId = x.InvoiceId,
                    InvoiceNumber = x.InvoiceNumber,
                    AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                statistics.LastCustomer = _TaamerProContext.Customer.Where(s => s.IsDeleted == false).Select(x => new CustomerVM
                {
                    CustomerId = x.CustomerId,
                    //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                    CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

             
                    BranchName = x.Branch != null ? x.Branch.NameAr : "",

                }).OrderByDescending(x=>x.CustomerId).FirstOrDefault();



                statistics.LastEmpContract= _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false ).Select(x => new EmpContractVM
                {
                    ContractId = x.ContractId,
                    ContractCode = x.ContractCode,
           
                    AddUsers = lang == "ltr" ? x.AddUsers.FullName :x.AddUsers.FullNameAr,
                    
                }).OrderByDescending(x=>x.ContractId).FirstOrDefault();


                return statistics;

            } 
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            
            }
        }

       



        public async Task<IEnumerable<DatabaseBackupVM>> GetDBackupByID( int Bakupid)
        {

            try
            {
                var AllDBackup = _TaamerProContext.DatabaseBackup.Where(w => w.IsDeleted == false && w.BackupId== Bakupid).Select(x => new DatabaseBackupVM
            {
                BackupId = x.BackupId,
                SavedName = x.SavedName,
                UserId = x.UserId,
                UserName = x.Users.FullName,
                LocalSavedPath = x.LocalSavedPath,
                Date = x.Date,
                FileSize = x.FileSize,
                TotalBranches=x.TotalBranches,
                TotalClient=x.TotalClient,
                TotalExp=x.TotalExp,
                TotalReve=x.TotalReve,
                TotalProject=x.TotalProject,
                TotalUsers=x.TotalUsers,
                Lastinvoice=x.Lastinvoice,
                LastPro=x.LastPro,
                LasCustomer=x.LasCustomer,
                LasEmpContract=x.LasEmpContract,
                LastEntyvoucher=x.LastEntyvoucher,
                LastpayVoucher=x.LastpayVoucher,
                LastreVoucher=x.LastreVoucher,
                LastVoucherRet=x.LastVoucherRet,
                TotalarchiveProject = x.TotalarchiveProject ?? 0,


            }).ToList();




            return AllDBackup;
            }
            catch (Exception ex)
            {
                return null;
                // return new List<DatabaseBackupVM>();
            }
        }

        public async Task<BackupStatistics> GetDBackupByIDWithDetails(int Bakupid,string lang)
        {

            try
            {
                var AllDBackup = _TaamerProContext.DatabaseBackup.Where(w => w.IsDeleted == false && w.BackupId == Bakupid).Select(x => new DatabaseBackupVM
                {
                    BackupId = x.BackupId,
                    SavedName = x.SavedName,
                    UserId = x.UserId,
                    UserName = x.Users.FullName,
                    LocalSavedPath = x.LocalSavedPath,
                    Date = x.Date,
                    FileSize = x.FileSize,
                    TotalBranches = x.TotalBranches,
                    TotalClient = x.TotalClient,
                    TotalExp = x.TotalExp,
                    TotalReve = x.TotalReve,
                    TotalProject = x.TotalProject,
                    TotalUsers = x.TotalUsers,
                    Lastinvoice = x.Lastinvoice,
                    LastPro = x.LastPro,
                    LasCustomer = x.LasCustomer,
                    LasEmpContract = x.LasEmpContract,
                    LastEntyvoucher = x.LastEntyvoucher,
                    LastpayVoucher = x.LastpayVoucher,
                    LastreVoucher = x.LastreVoucher,
                    LastVoucherRet = x.LastVoucherRet,
                    TotalarchiveProject = x.TotalarchiveProject ?? 0,


                }).FirstOrDefault();

                BackupStatistics statistics = new BackupStatistics();


              
                    statistics.ProjectCount = AllDBackup.TotalProject;
                statistics.ProjectArchivedCount = AllDBackup.TotalarchiveProject;
                    statistics.BranchesCount = AllDBackup.TotalBranches;
                statistics.UsersCount = AllDBackup.TotalUsers;
                statistics.Customercount = AllDBackup.TotalClient;
                statistics.TotalDetailedExpensed = AllDBackup.TotalExp;
                statistics.TotalDetailedRevenu = AllDBackup.TotalReve;

                    statistics.LastProject = _TaamerProContext.Project.Where(x => x.IsDeleted == false && x.ProjectId== AllDBackup.LastPro).Select(x => new ProjectVM
                    {
                        ProjectId = x.ProjectId,
                        ProjectNo = x.ProjectNo,
                    }).OrderByDescending(x => x.ProjectId).FirstOrDefault();

                    statistics.LastInvoice = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false &&x.InvoiceId== AllDBackup.Lastinvoice).Select(x => new InvoicesVM
                    {

                        InvoiceId = x.InvoiceId,
                        InvoiceNumber = x.InvoiceNumber,
                        AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,


                    }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();

                    statistics.LastInvoiceRet = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.InvoiceId== AllDBackup.LastVoucherRet).Select(x => new InvoicesVM
                    {

                        InvoiceId = x.InvoiceId,
                        InvoiceNumber = x.InvoiceNumber,
                        AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                    }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                    statistics.lastRevoucern = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.InvoiceId== AllDBackup.LastreVoucher).Select(x => new InvoicesVM
                    {

                        InvoiceId = x.InvoiceId,
                        InvoiceNumber = x.InvoiceNumber,
                        AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                    }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                    statistics.lastpayvoucern = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.Type == 5 &&x.InvoiceId== AllDBackup.LastpayVoucher).Select(x => new InvoicesVM
                    {

                        InvoiceId = x.InvoiceId,
                        InvoiceNumber = x.InvoiceNumber,
                        AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                    }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                    statistics.lastEntyvoucher = _TaamerProContext.Invoices.Where(x => x.IsDeleted == false && x.Type == 8 &&x.InvoiceId== AllDBackup.LastEntyvoucher).Select(x => new InvoicesVM
                    {

                        InvoiceId = x.InvoiceId,
                        InvoiceNumber = x.InvoiceNumber,
                        AddUser = x.AddUsers!.FullNameAr == null ? x.AddUsers!.FullName : x.AddUsers!.FullNameAr,
                    }).OrderByDescending(x => x.InvoiceId).FirstOrDefault();


                    statistics.LastCustomer = _TaamerProContext.Customer.Where(s => s.IsDeleted == false&&s.CustomerId== AllDBackup.LasCustomer).Select(x => new CustomerVM
                    {
                        CustomerId = x.CustomerId,
                        //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                        CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                    : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,


                        BranchName = x.Branch != null ? x.Branch.NameAr : "",

                    }).OrderByDescending(x => x.CustomerId).FirstOrDefault();



                    statistics.LastEmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false &&s.ContractId== AllDBackup.LasEmpContract).Select(x => new EmpContractVM
                    {
                        ContractId = x.ContractId,
                        ContractCode = x.ContractCode,

                        AddUsers = lang == "ltr" ? x.AddUsers.FullName : x.AddUsers.FullNameAr,

                    }).OrderByDescending(x => x.ContractId).FirstOrDefault();


                    return statistics;
            }
            catch (Exception ex)
            {
                return null;
                // return new List<DatabaseBackupVM>();
            }
        }


        public async Task<DatabaseBackup> GetBackupByID(int Bakupid)
        {
            var backup = _TaamerProContext.DatabaseBackup.Where(w => w.IsDeleted == false && w.BackupId == Bakupid).FirstOrDefault();


            return backup;
        }
    }
}
