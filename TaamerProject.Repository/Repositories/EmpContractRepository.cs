using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;

namespace TaamerProject.Repository.Repositories
{
    public class EmpContractRepository :  IEmpContractRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public EmpContractRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }


        public async Task<IEnumerable<EmpContractVM>> GetAllEmpContract(string lang, int BranchId)
        {
          
            var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate) ).Select(x => new EmpContractVM
            {
                ContractId = x.ContractId,
                ContractCode = x.ContractCode,
                 EmpId = x.EmpId,
                EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,

                NationalityId = x.NationalityId,
                BranchId = x.BranchId,
                OrgId = x.OrgId,
                CompanyRepresentativeId = x.CompanyRepresentativeId,
                PerSe = x.PerSe,
                ContTypeId = x.ContTypeId,
                ContDuration = x.ContDuration,
                StartDatetxt = x.StartDatetxt,
                StartWorkDate = x.StartWorkDate,
                EndWorkDate = x.EndWorkDate,
                EndDatetxt = x.EndDatetxt,


                ProbationTypeId = x.ProbationTypeId,
                ProbationDuration = x.ProbationDuration,
                Workingdaysperweek = x.Workingdaysperweek,
                Dailyworkinghours = x.Dailyworkinghours,
                Workinghoursperweek = x.Workinghoursperweek,

                Durationofannualleave = x.Durationofannualleave,
                FreelanceAmount = x.FreelanceAmount,
                Paycase = x.Paycase,
                Restrictedmode = x.Restrictedmode,
                NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                RestrictionDuration = x.RestrictionDuration,
                Identifyplaces = x.Identifyplaces,
                Withregardtowork = x.Withregardtowork,
                NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                SecretsWithregardtowork = x.SecretsWithregardtowork,
                ContractTerminationNotice = x.ContractTerminationNotice,
                Compensation = x.Compensation,
                CompensationBothParty = x.CompensationBothParty,
                Firstpartycompensation = x.Firstpartycompensation,
                Secondpartycompensation = x.Secondpartycompensation,
                AddDate = x.AddDate,
                UpdateDate = x.UpdateDate,
                AddUsers = x.AddUsers.FullName,
                UpdateUsers = x.UpdateUsers.FullName ?? ""


            }).ToList();
            return EmpContract;

        }

        public async Task<IEnumerable<EmpContractVM>> GetAllBranchEmpContract(string lang)
        {
            var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate)).Select(x => new EmpContractVM
            {

                ContractId = x.ContractId,
                ContractCode = x.ContractCode,
                EmpId = x.EmpId,
                EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,

                NationalityId = x.NationalityId,
                BranchId = x.BranchId,
                OrgId = x.OrgId,
                CompanyRepresentativeId = x.CompanyRepresentativeId,
                PerSe = x.PerSe,
                ContTypeId = x.ContTypeId,
                ContDuration = x.ContDuration,
                StartDatetxt = x.StartDatetxt,
                StartWorkDate = x.StartWorkDate,
                EndWorkDate = x.EndWorkDate,
                EndDatetxt = x.EndDatetxt,


                ProbationTypeId = x.ProbationTypeId,
                ProbationDuration = x.ProbationDuration,
                Workingdaysperweek = x.Workingdaysperweek,
                Dailyworkinghours = x.Dailyworkinghours,
                Workinghoursperweek = x.Workinghoursperweek,

                Durationofannualleave = x.Durationofannualleave,
                FreelanceAmount = x.FreelanceAmount,
                Paycase = x.Paycase,
                Restrictedmode = x.Restrictedmode,
                NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                RestrictionDuration = x.RestrictionDuration,
                Identifyplaces = x.Identifyplaces,
                Withregardtowork = x.Withregardtowork,
                NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                SecretsWithregardtowork = x.SecretsWithregardtowork,
                ContractTerminationNotice = x.ContractTerminationNotice,
                Compensation = x.Compensation,
                CompensationBothParty = x.CompensationBothParty,
                Firstpartycompensation = x.Firstpartycompensation,
                Secondpartycompensation = x.Secondpartycompensation,


            }).ToList();
            return EmpContract;

        }

        public async Task<IEnumerable<EmpContractVM>> GetAllEmpContract(string lang, int SearchAll, int Branch)
        {
            var EmpContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && string.IsNullOrEmpty(s.EndWorkDate)).Select(x => new EmpContractVM
            {
                ContractId = x.ContractId,
                ContractCode = x.ContractCode,
                EmpId = x.EmpId,
                EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,

                NationalityId = x.NationalityId,
                BranchId = x.BranchId,
                OrgId = x.OrgId,
                CompanyRepresentativeId = x.CompanyRepresentativeId,
                PerSe = x.PerSe,
                ContTypeId = x.ContTypeId,
                ContDuration = x.ContDuration,
                StartDatetxt = x.StartDatetxt,
                StartWorkDate = x.StartWorkDate,
                EndWorkDate = x.EndWorkDate,
                EndDatetxt = x.EndDatetxt,


                ProbationTypeId = x.ProbationTypeId,
                ProbationDuration = x.ProbationDuration,
                Workingdaysperweek = x.Workingdaysperweek,
                Dailyworkinghours = x.Dailyworkinghours,
                Workinghoursperweek = x.Workinghoursperweek,

                Durationofannualleave = x.Durationofannualleave,
                FreelanceAmount = x.FreelanceAmount,
                Paycase = x.Paycase,
                Restrictedmode = x.Restrictedmode,
                NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                RestrictionDuration = x.RestrictionDuration,
                Identifyplaces = x.Identifyplaces,
                Withregardtowork = x.Withregardtowork,
                NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                SecretsWithregardtowork = x.SecretsWithregardtowork,
                ContractTerminationNotice = x.ContractTerminationNotice,
                Compensation = x.Compensation,
                CompensationBothParty = x.CompensationBothParty,
                Firstpartycompensation = x.Firstpartycompensation,
                Secondpartycompensation = x.Secondpartycompensation,
                AddDate = x.AddDate,
                UpdateDate = x.UpdateDate,
                AddUsers = x.AddUsers.FullName,
                UpdateUsers = x.UpdateUsers.FullName ?? ""

            }).ToList();
            return EmpContract;

        }

        public async Task<IEnumerable<EmpContractVM>> GetAllEmpContractSearch(string lang, int BranchId)
        {

            var employeesContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.BranchId == BranchId && string.IsNullOrEmpty(s.EndWorkDate)).Select(x => new EmpContractVM
            {
                ContractId = x.ContractId,
                ContractCode = x.ContractCode,
                EmpId = x.EmpId,
                EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,
                NationalityName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,
                NationalityId = x.NationalityId,
                BranchId = x.BranchId,
                OrgId = x.OrgId,
                CompanyRepresentativeId = x.CompanyRepresentativeId,
                PerSe = x.PerSe,
                ContTypeId = x.ContTypeId,
                ContDuration = x.ContDuration,
                StartDatetxt = x.StartDatetxt,
                StartWorkDate = x.StartWorkDate,
                EndWorkDate = x.EndWorkDate,
                EndDatetxt = x.EndDatetxt,


                ProbationTypeId = x.ProbationTypeId,
                ProbationDuration = x.ProbationDuration,
                Workingdaysperweek = x.Workingdaysperweek,
                Dailyworkinghours = x.Dailyworkinghours,
                Workinghoursperweek = x.Workinghoursperweek,

                Durationofannualleave = x.Durationofannualleave,
                FreelanceAmount = x.FreelanceAmount,
                Paycase = x.Paycase,
                Restrictedmode = x.Restrictedmode,
                NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                RestrictionDuration = x.RestrictionDuration,
                Identifyplaces = x.Identifyplaces,
                Withregardtowork = x.Withregardtowork,
                NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                SecretsWithregardtowork = x.SecretsWithregardtowork,
                ContractTerminationNotice = x.ContractTerminationNotice,
                Compensation = x.Compensation,
                CompensationBothParty = x.CompensationBothParty,
                Firstpartycompensation = x.Firstpartycompensation,
                Secondpartycompensation = x.Secondpartycompensation,
                AddDate = x.AddDate,
                UpdateDate = x.UpdateDate,
                AddUsers = x.AddUsers.FullName,
                UpdateUsers = x.UpdateUsers.FullName ?? "",
                DailyEmpCost=x.DailyEmpCost,
                employeeno=x.Employees.EmployeeNo,
            }).ToList();
            return employeesContract;

        }

        public async Task<IEnumerable<EmpContractVM>> GetEmpcoById(int contractid, string lang)
        {

            var employeesContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractId== contractid).Select(x => new EmpContractVM
            {
                ContractId = x.ContractId,
                ContractCode = x.ContractCode,
                EmpId = x.EmpId,
                EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,

                NationalityId = x.NationalityId,
                BranchId = x.BranchId,
                OrgId = x.OrgId,
                CompanyRepresentativeId = x.CompanyRepresentativeId,
                PerSe = x.PerSe,
                ContTypeId = x.ContTypeId,
                ContDuration = x.ContDuration,
                StartDatetxt = x.StartDatetxt,
                StartWorkDate = x.StartWorkDate,
                EndWorkDate = x.EndWorkDate,
                EndDatetxt = x.EndDatetxt,


                ProbationTypeId = x.ProbationTypeId,
                ProbationDuration = x.ProbationDuration,
                Workingdaysperweek = x.Workingdaysperweek,
                Dailyworkinghours = x.Dailyworkinghours,
                Workinghoursperweek = x.Workinghoursperweek,

                Durationofannualleave = x.Durationofannualleave,
                FreelanceAmount = x.FreelanceAmount,
                Paycase = x.Paycase,
                Restrictedmode = x.Restrictedmode,
                NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                RestrictionDuration = x.RestrictionDuration,
                Identifyplaces = x.Identifyplaces,
                Withregardtowork = x.Withregardtowork,
                NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                SecretsWithregardtowork = x.SecretsWithregardtowork,
                ContractTerminationNotice = x.ContractTerminationNotice,
                Compensation = x.Compensation,
                CompensationBothParty = x.CompensationBothParty,
                Firstpartycompensation = x.Firstpartycompensation,
                Secondpartycompensation = x.Secondpartycompensation,
                AddDate = x.AddDate,
                UpdateDate = x.UpdateDate,
                AddUsers = x.AddUsers.FullName,
                UpdateUsers = x.UpdateUsers.FullName,
                employeeno=x.Employees.EmployeeNo,
            }).ToList();

          


            return employeesContract;

        }


        public async Task<IEnumerable<EmpContractVM>> GetAllEmpContractBySearchObject(EmpContractVM Search, string lang, int BranchId)        
        {
           
            var employeesContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false).Select(x => new EmpContractVM
                {
                    ContractId = x.ContractId,
                    ContractCode = x.ContractCode,
                    EmpId = x.EmpId,
                    EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                    BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,

                NationalityId = x.NationalityId,
                    BranchId = x.BranchId,
                    OrgId = x.OrgId,
                    CompanyRepresentativeId = x.CompanyRepresentativeId,
                    PerSe = x.PerSe,
                    ContTypeId = x.ContTypeId,
                    ContDuration = x.ContDuration,
                    StartDatetxt = x.StartDatetxt,
                    StartWorkDate = x.StartWorkDate,
                EndWorkDate = x.EndWorkDate,
                EndDatetxt = x.EndDatetxt,


                    ProbationTypeId = x.ProbationTypeId,
                    ProbationDuration = x.ProbationDuration,
                    Workingdaysperweek = x.Workingdaysperweek,
                    Dailyworkinghours = x.Dailyworkinghours,
                    Workinghoursperweek = x.Workinghoursperweek,

                    Durationofannualleave = x.Durationofannualleave,
                    FreelanceAmount = x.FreelanceAmount,
                    Paycase = x.Paycase,
                    Restrictedmode = x.Restrictedmode,
                    NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                    RestrictionDuration = x.RestrictionDuration,
                    Identifyplaces = x.Identifyplaces,
                    Withregardtowork = x.Withregardtowork,
                    NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                    SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                    SecretsWithregardtowork = x.SecretsWithregardtowork,
                    ContractTerminationNotice = x.ContractTerminationNotice,
                    Compensation = x.Compensation,
                    CompensationBothParty = x.CompensationBothParty,
                    Firstpartycompensation = x.Firstpartycompensation,
                    Secondpartycompensation = x.Secondpartycompensation,
                    AddDate = x.AddDate,
                    UpdateDate = x.UpdateDate,
                    AddUsers = x.AddUsers.FullName,
                    UpdateUsers = x.UpdateUsers.FullName,
                    DailyEmpCost=x.DailyEmpCost,
                    employeeno=x.Employees.EmployeeNo,
                   
                }).ToList();

            if (Search.EmpId > 0)
            {
                employeesContract= employeesContract.Where(w => w.EmpId == Search.EmpId).ToList();
            }
            if (Search.BranchId > 0)
            {
                employeesContract = employeesContract.Where(w => w.BranchId == Search.BranchId).ToList();
            }
            if (Search.EndWorkDate != null)
            {
                employeesContract = employeesContract.Where(w => DateTime.ParseExact(w.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime() 
                       <= DateTime.ParseExact(Search.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime()).ToList();
            }


                return employeesContract;

            }
               
        public async Task<EmpContractVM> GetEmpContractById(int ContractId, string lang)
        {
            var emp = _TaamerProContext.EmpContract.Where(s => s.ContractId == ContractId && !s.IsDeleted  && string.IsNullOrEmpty(s.EndWorkDate)).Select(x => new EmpContractVM
            {
                ContractId = x.ContractId,
                ContractCode = x.ContractCode,
                EmpId = x.EmpId,
                EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,

                NationalityId = x.NationalityId,
                BranchId = x.BranchId,
                OrgId = x.OrgId,
                CompanyRepresentativeId = x.CompanyRepresentativeId,
                PerSe = x.PerSe,
                ContTypeId = x.ContTypeId,
                ContDuration = x.ContDuration,
                StartDatetxt = x.StartDatetxt,
                StartWorkDate = x.StartWorkDate,
                EndWorkDate = x.EndWorkDate,
                EndDatetxt = x.EndDatetxt,


                ProbationTypeId = x.ProbationTypeId,
                ProbationDuration = x.ProbationDuration,
                Workingdaysperweek = x.Workingdaysperweek,
                Dailyworkinghours = x.Dailyworkinghours,
                Workinghoursperweek = x.Workinghoursperweek,

                Durationofannualleave = x.Durationofannualleave,
                FreelanceAmount = x.FreelanceAmount,
                Paycase = x.Paycase,
                Restrictedmode = x.Restrictedmode,
                NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                RestrictionDuration = x.RestrictionDuration,
                Identifyplaces = x.Identifyplaces,
                Withregardtowork = x.Withregardtowork,
                NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                SecretsWithregardtowork = x.SecretsWithregardtowork,
                ContractTerminationNotice = x.ContractTerminationNotice,
                Compensation = x.Compensation,
                CompensationBothParty = x.CompensationBothParty,
                Firstpartycompensation = x.Firstpartycompensation,
                Secondpartycompensation = x.Secondpartycompensation,
                employeeno=x.Employees.EmployeeNo,

            }).First();
            return emp;
        }
        public async Task<IEnumerable<EmpContractVM>> SearchEmpContract(EmpContractVM EmployeesSearch, string lang, int BranchId)
        {

            var employeesContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false )
                                                .Select(x => new EmpContractVM
                                                {
                                                    ContractId = x.ContractId,
                                                    ContractCode = x.ContractCode,
                                                    EmpId = x.EmpId,
                                                    EmployeeName = lang == "ltr" ? x.Employees.EmployeeNameEn : x.Employees.EmployeeNameAr,
                                                    BranchName = lang == "ltr" ? x.BranchName.NameEn : x.BranchName.NameAr,
                                                    NatName = lang == "ltr" ? x.Employees.Nationality.NameEn : x.Employees.Nationality.NameAr,

                                                    NationalityId = x.NationalityId,
                                                    BranchId = x.BranchId,
                                                    OrgId = x.OrgId,
                                                    CompanyRepresentativeId = x.CompanyRepresentativeId,
                                                    PerSe = x.PerSe,
                                                    ContTypeId = x.ContTypeId,
                                                    ContDuration = x.ContDuration,
                                                    StartDatetxt = x.StartDatetxt,
                                                    StartWorkDate = x.StartWorkDate,
                                                    EndWorkDate = x.EndWorkDate,
                                                    EndDatetxt = x.EndDatetxt,


                                                    ProbationTypeId = x.ProbationTypeId,
                                                    ProbationDuration = x.ProbationDuration,
                                                    Workingdaysperweek = x.Workingdaysperweek,
                                                    Dailyworkinghours = x.Dailyworkinghours,
                                                    Workinghoursperweek = x.Workinghoursperweek,

                                                    Durationofannualleave = x.Durationofannualleave,
                                                    FreelanceAmount = x.FreelanceAmount,
                                                    Paycase = x.Paycase,
                                                    Restrictedmode = x.Restrictedmode,
                                                    NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                                                    RestrictionDuration = x.RestrictionDuration,
                                                    Identifyplaces = x.Identifyplaces,
                                                    Withregardtowork = x.Withregardtowork,
                                                    NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                                                    SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                                                    SecretsWithregardtowork = x.SecretsWithregardtowork,
                                                    ContractTerminationNotice = x.ContractTerminationNotice,
                                                    Compensation = x.Compensation,
                                                    CompensationBothParty = x.CompensationBothParty,
                                                    Firstpartycompensation = x.Firstpartycompensation,
                                                    Secondpartycompensation = x.Secondpartycompensation,
                                                    employeeno=x.Employees.EmployeeNo,
                                                }).ToList();

            if (EmployeesSearch.EndWorkDate != null)
            {
                employeesContract = employeesContract.Where(w => DateTime.ParseExact(w.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime()
                       <= DateTime.ParseExact(EmployeesSearch.EndWorkDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToLocalTime()).ToList();
            }
            return employeesContract;
        }
        public async Task<int> GenerateNextEmpContractNumber(int BranchId)
        {
            if (_TaamerProContext.EmpContract != null)
            {
                var lastRow = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractSource !=1).OrderByDescending(u => u.ContractId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    return int.Parse(lastRow.ContractCode) + 1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        public async Task<int> GenerateNextQuaEmpContractNumber(int BranchId)
        {
            if (_TaamerProContext.EmpContract != null)
            {
                var lastRow = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false && s.ContractSource == 1).OrderByDescending(u => u.ContractId).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    return int.Parse(lastRow.ContractCode) + 1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }

        public async Task<EmpContractVM> GetEMployeeContractByEmp(int EmpId)
        {

            var employeesContract = _TaamerProContext.EmpContract.Where(s => s.IsDeleted == false &&s.EmpId== EmpId)
                                                .Select(x => new EmpContractVM
                                                {
                                                    ContractId = x.ContractId,
                                                    ContractCode = x.ContractCode,
                                                    EmpId = x.EmpId,
                                                    NationalityId = x.NationalityId,
                                                    BranchId = x.BranchId,
                                                    OrgId = x.OrgId,
                                                    CompanyRepresentativeId = x.CompanyRepresentativeId,
                                                    PerSe = x.PerSe,
                                                    ContTypeId = x.ContTypeId,
                                                    ContDuration = x.ContDuration,
                                                    StartDatetxt = x.StartDatetxt,
                                                    StartWorkDate = x.StartWorkDate,
                                                    EndWorkDate = x.EndWorkDate,
                                                    EndDatetxt = x.EndDatetxt,

                                                    ProbationTypeId = x.ProbationTypeId,
                                                    ProbationDuration = x.ProbationDuration,
                                                    Workingdaysperweek = x.Workingdaysperweek,
                                                    Dailyworkinghours = x.Dailyworkinghours,
                                                    Workinghoursperweek = x.Workinghoursperweek,

                                                    Durationofannualleave = x.Durationofannualleave,
                                                    FreelanceAmount = x.FreelanceAmount,
                                                    Paycase = x.Paycase,
                                                    Restrictedmode = x.Restrictedmode,
                                                    NotTodivulgeSecrets = x.NotTodivulgeSecrets,
                                                    RestrictionDuration = x.RestrictionDuration,
                                                    Identifyplaces = x.Identifyplaces,
                                                    Withregardtowork = x.Withregardtowork,
                                                    NotTodivulgeSecretsDuration = x.NotTodivulgeSecretsDuration,
                                                    SecretsIdentifyplaces = x.SecretsIdentifyplaces,
                                                    SecretsWithregardtowork = x.SecretsWithregardtowork,
                                                    ContractTerminationNotice = x.ContractTerminationNotice,
                                                    Compensation = x.Compensation,
                                                    CompensationBothParty = x.CompensationBothParty,
                                                    Firstpartycompensation = x.Firstpartycompensation,
                                                    Secondpartycompensation = x.Secondpartycompensation,
                                                    employeeno = x.Employees.EmployeeNo,
                                                }).ToList().OrderByDescending(x=>x.ContractId);

            return employeesContract.FirstOrDefault();
        }
    }
}