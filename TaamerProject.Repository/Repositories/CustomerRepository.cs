using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using Microsoft.EntityFrameworkCore;

namespace TaamerProject.Repository.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public CustomerRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        }
        public Customer GetById(int customerid)
        {
            return _TaamerProContext.Customer.Where(x => x.CustomerId == customerid).FirstOrDefault();
        }

        public async Task<IQueryable<CustomerVM>> GetAllCustomersexist(string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false ).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                PostalCodeFinal = x.PostalCodeFinal,
                ExternalPhone = x.ExternalPhone,
                Country = x.Country,
                BuildingNumber = x.BuildingNumber,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AccountCodee = x.Accounts!.Code,
                TotalRevenue = 0,
                TotalExpenses = 0,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                BranchId = x.BranchId,
                CompAddress = x.CompAddress,
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                AddUser = x.AddUsers.FullName,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                AddDate = x.AddDate
            });
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId, bool isPrivate)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) &&
            (s.IsPrivate == false || s.IsPrivate == isPrivate)).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName = 
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),


                CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                PostalCodeFinal = x.PostalCodeFinal,
                ExternalPhone = x.ExternalPhone,
                Country = x.Country,
                BuildingNumber = x.BuildingNumber,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AccountCodee = x.Accounts!.Code,
                TotalRevenue = 0,
                TotalExpenses = 0,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                BranchId = x.BranchId,
                CompAddress = x.CompAddress,
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                AddUser = x.AddUsers.FullName,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                AddDate = x.AddDate,
                AddedcustomerImg=x.AddUsers.ImgUrl??"",
                Customer_Branches=x.Customer_Branches,
                OtherBranches = x.Customer_Branches.Where(cb => cb.BranchId.HasValue).Select(cb => cb.BranchId.Value).ToList(),
            });
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomersSearch(string searchtxt,string lang, int BranchId, bool isPrivate)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && (s.IsPrivate == false || s.IsPrivate == isPrivate) &&
            (s.CustomerNameEn == searchtxt || s.CustomerNameAr.Contains(searchtxt) || s.CustomerMobile== searchtxt || s.CustomerMobile.Contains(searchtxt) || searchtxt == null)).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName  =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),

                CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                PostalCodeFinal = x.PostalCodeFinal,
                ExternalPhone = x.ExternalPhone,
                Country = x.Country,
                BuildingNumber = x.BuildingNumber,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AccountCodee = x.Accounts!.Code,
                TotalRevenue = 0,
                TotalExpenses = 0,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                BranchId = x.BranchId,
                CompAddress = x.CompAddress,
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                AddUser = x.AddUsers.FullName,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                AddDate = x.AddDate,
                AddedcustomerImg = x.AddUsers.ImgUrl ?? ""
            });
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomersW(string lang, int BranchId, bool isPrivate)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerTypeId!=3 && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,
                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AccountCodee = x.Accounts!.Code,
                TotalRevenue = 0,
                TotalExpenses = 0,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                BranchId = x.BranchId,
                CompAddress = x.CompAddress,
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                AddUser = x.AddUsers.FullName,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                AddDate = x.AddDate
            });
            return customers;
        }

        public async Task<IEnumerable<CustomerVM>> GetAllCustomer()
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = x.CustomerNameAr??"",
                CustomerName =  x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)":x.CustomerNameAr,
                CustomerNameAr = x.CustomerNameAr??"",
                CustomerNameEn = x.CustomerNameEn??"",
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                CompAddress = x.CompAddress,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                CustomerTypeName = x.CustomerTypeId == 1 ? ("مواطن") : x.CustomerTypeId == 2 ? ("مستثمرون وشركات") : x.CustomerTypeId == 3 ? ("جهة حكومية") : "",

            }).ToList();
            return customers;
        }

        public async Task<IQueryable<CustomerVM>> GetCustomersArchiveProjects(string lang, int BranchId, bool isPrivate)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && (s.IsPrivate == false || s.IsPrivate == isPrivate)
             & s.Projects!.Where(x => x.Status == 1 && !x.IsDeleted && s.BranchId == BranchId).Count() > 0).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                 //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                 CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                 CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                 CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,
                 CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AccountCodee = x.Accounts!.Code,
                TotalRevenue = 0,
                TotalExpenses = 0,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                BranchId = x.BranchId,
                 CompAddress = x.CompAddress,
                 CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                AddUser = x.AddUsers.FullName,
                 CityId = x.CityId,
                 CityName = x.city != null ? x.city.NameAr : "",
                 AddDate = x.AddDate
            });
            return customers;
        }

        public async Task<IQueryable<CustomerVM>> GetCustomersOwnProjects(string lang, int BranchId, bool isPrivate)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && (s.IsPrivate == false || s.IsPrivate == isPrivate)
             & s.Projects!.Where(x => !x.IsDeleted && s.BranchId == BranchId).Count() > 0).Select(x => new CustomerVM
             {
                 CustomerId = x.CustomerId,
                 CustomerCode = x.CustomerCode,
                 //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                 CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                 CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                 CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                 CustomerNationalId = x.CustomerNationalId,
                 NationalIdSource = x.NationalIdSource,
                 CustomerAddress = x.CustomerAddress,
                 CustomerEmail = x.CustomerEmail,
                 CustomerPhone = x.CustomerPhone,
                 CustomerMobile = x.CustomerMobile,
                 Notes = x.Notes,
                 CustomerTypeId = x.CustomerTypeId,
                 LogoUrl = x.LogoUrl,
                 AttachmentUrl = x.AttachmentUrl,
                 CommercialActivity = x.CommercialActivity,
                 CommercialRegister = x.CommercialRegister,
                 CommercialRegDate = x.CommercialRegDate,
                 CommercialRegHijriDate = x.CommercialRegHijriDate,
                 AccountId = x.AccountId,
                 GeneralManager = x.GeneralManager,
                 AgentName = x.AgentName,
                 AgentType = x.AgentType,
                 AccountCodee = x.Accounts!.Code,
                 TotalRevenue = 0,
                 TotalExpenses = 0,
                 AgentNumber = x.AgentNumber,
                 AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                 ResponsiblePerson = x.ResponsiblePerson,
                 AgentAttachmentUrl = x.AgentAttachmentUrl,
                 BranchId = x.BranchId,
                 CompAddress = x.CompAddress,
                 CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                 AddUser = x.AddUsers.FullName,
                 CityId = x.CityId,
                 CityName = x.city != null ? x.city.NameAr : "",
                 AddDate = x.AddDate
             });
            return customers;
        }


        public async Task<IQueryable<CustomerVM>> GetCustomersOwnNotArcheivedProjects(string lang, int BranchId, bool isPrivate)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && (s.IsPrivate == false || s.IsPrivate == isPrivate)
             & s.Projects!.Where(x => !x.IsDeleted && s.BranchId == BranchId && x.Status != 1 &&
                x.ProjectPhasesTasks.Where(y => y.IsDeleted == false && y.IsMerig == -1 &&
                y.Type == 3 && (y.Status == 1 || y.Status == 2 || y.Status == 3)).Count() > 0
             ).Count() > 0).Select(x => new CustomerVM
             {
                 CustomerId = x.CustomerId,
                 CustomerCode = x.CustomerCode,
                 //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                 CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                 CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                 CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                 CustomerNationalId = x.CustomerNationalId,
                 NationalIdSource = x.NationalIdSource,
                 CustomerAddress = x.CustomerAddress,
                 CustomerEmail = x.CustomerEmail,
                 CustomerPhone = x.CustomerPhone,
                 CustomerMobile = x.CustomerMobile,
                 Notes = x.Notes,
                 CustomerTypeId = x.CustomerTypeId,
                 LogoUrl = x.LogoUrl,
                 AttachmentUrl = x.AttachmentUrl,
                 CommercialActivity = x.CommercialActivity,
                 CommercialRegister = x.CommercialRegister,
                 CommercialRegDate = x.CommercialRegDate,
                 CommercialRegHijriDate = x.CommercialRegHijriDate,
                 AccountId = x.AccountId,
                 GeneralManager = x.GeneralManager,
                 AgentName = x.AgentName,
                 AgentType = x.AgentType,
                 AccountCodee = x.Accounts!.Code,
                 TotalRevenue = 0,
                 TotalExpenses = 0,
                 AgentNumber = x.AgentNumber,
                 AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                 ResponsiblePerson = x.ResponsiblePerson,
                 AgentAttachmentUrl = x.AgentAttachmentUrl,
                 BranchId = x.BranchId,
                 CompAddress = x.CompAddress,
                 CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                 AddUser = x.AddUsers.FullName,
                 CityId = x.CityId,
                 CityName = x.city != null ? x.city.NameAr : "",
                 AddDate = x.AddDate
             });
            return customers;
        }


        public async Task<List<CustomerVM>> GetCustomersNewTask(string lang,int UserId, int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted != true 
             && s.Projects!.Where(x => x.IsDeleted==false && x.ProjectPhasesTasks.Where(y => y.IsDeleted == false && y.IsMerig == -1 &&
y.Type == 3 && (y.Status == 1 || y.Status == 2 || y.Status == 3) && y.UserId == UserId && y.BranchId == BranchId && (y.Remaining > 0 || y.Remaining == null)).Count() > 0).Count() > 0).Select(x => new CustomerVM
             {
                 CustomerId = x.CustomerId,
                 CustomerCode = x.CustomerCode,
                 //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                 CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                 CustomerNameAr = x.CustomerNameAr,
                 CustomerNameEn = x.CustomerNameEn,
                 CustomerNationalId = x.CustomerNationalId,
                 NationalIdSource = x.NationalIdSource,
                 CustomerAddress = x.CustomerAddress,
                 CustomerEmail = x.CustomerEmail,
                 CustomerPhone = x.CustomerPhone,
                 CustomerMobile = x.CustomerMobile,
                 Notes = x.Notes,
                 CustomerTypeId = x.CustomerTypeId,
                 LogoUrl = x.LogoUrl,
                 AttachmentUrl = x.AttachmentUrl,
                 CommercialActivity = x.CommercialActivity,
                 CommercialRegister = x.CommercialRegister,
                 CommercialRegDate = x.CommercialRegDate,
                 CommercialRegHijriDate = x.CommercialRegHijriDate,
                 AccountId = x.AccountId,
                 GeneralManager = x.GeneralManager,
                 AgentName = x.AgentName,
                 AgentType = x.AgentType,
                 AccountCodee = x.Accounts!.Code,
                 TotalRevenue = 0,
                 TotalExpenses = 0,
                 AgentNumber = x.AgentNumber,
                 AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                 ResponsiblePerson = x.ResponsiblePerson,
                 AgentAttachmentUrl = x.AgentAttachmentUrl,
                 BranchId = x.BranchId,
                 CompAddress = x.CompAddress,
                 CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                 AddUser = x.AddUsers.FullName,
                 CityId = x.CityId,
                 CityName = x.city != null ? x.city.NameAr : "",
                 AddDate = x.AddDate
             }).ToList();
            return customers;
        }

        public async Task<CustomerVM> GetCustomerInfo(string SearchText)
        {
            var Customer = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.CustomerNationalId == SearchText || s.CommercialRegister== SearchText)).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = x.CustomerNameAr,
                CustomerName = x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,
                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CompAddress = x.CompAddress,
                CommercialRegister = x.CommercialRegister,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
            }).FirstOrDefault();

            return Customer;

        }

        public async Task<IQueryable<CustomerVM>> GetAllCustomers(string lang, int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AccountCodee = x.Accounts!.Code,
                TotalRevenue = 0,
                TotalExpenses = 0,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                BranchId = x.BranchId,
                CompAddress = x.CompAddress,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",

            });
            return customers;
        }

        public async Task<IQueryable<CustomerVM>> GetAllCustomersCount(int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = x.CustomerNameAr,
                CustomerName = x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AccountCodee = x.Accounts!.Code,
                TotalRevenue = 0,
                TotalExpenses = 0,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                CompAddress = x.CompAddress,
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",

                // CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",

            });
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllCustomersByCustomerTypeId(int? CustomerTypeId, string lang, int BranchId, bool isPrivate)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerTypeId == CustomerTypeId && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                AddUser = x.AddUsers.FullName,
                AddDate = x.AddDate,
                AccountCodee = x.Accounts!.Code??"",
                CompAddress = x.CompAddress,
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                BuildingNumber = x.BuildingNumber,
                         TotalRevenue = x.Invoicess
                            .Where(i => i.IsDeleted == false && i.Type == 2 && i.IsPost == true && i.Rad !=true)
                            .Sum(i => i.InvoiceValue) - x.Invoicess
                            .Where(i => i.IsDeleted == false && (i.Type == 29 || i.Type==4)&& i.IsPost == true && i.Rad !=true)
                            .Sum(i => i.InvoiceValue),
                         CommercialActivityName = x.commercialActivities.NameAr,
                         BranchActivityName = x.BranchActivity.NameAr,


                     });
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> GetAllPrivateCustomers(string lang, int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.IsPrivate == true && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AccountCodee = x.Accounts!.Code ?? "",
                CompAddress = x.CompAddress,
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BuildingNumber = x.BuildingNumber,
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",

            });
            return customers;
        }
        public async Task<IQueryable<CustomerVM>> SearchCustomers(CustomerVM CustomersSearch, string lang, int BranchId)
        {
            IQueryable<CustomerVM> Customers = null;
            if (lang == "ltr")
            {
                Customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && ((s.CustomerId == CustomersSearch.CustomerId) || CustomersSearch.CustomerId == null || CustomersSearch.CustomerId == 0) &&
                                              (s.CustomerNationalId == CustomersSearch.CustomerNationalId || s.CustomerNationalId.Contains(CustomersSearch.CustomerNationalId) || CustomersSearch.CustomerNationalId == null) &&
                                              (s.CustomerEmail == CustomersSearch.CustomerEmail || s.CustomerEmail.Contains(CustomersSearch.CustomerEmail) || CustomersSearch.CustomerEmail == null) &&


                                              (s.CustomerMobile == CustomersSearch.CustomerMobile || s.CustomerMobile.Contains(CustomersSearch.CustomerMobile) || CustomersSearch.CustomerMobile == null))
                                              .Select(x => new CustomerVM
                                              {
                                                  CustomerId = x.CustomerId,
                                                  CustomerCode = x.CustomerCode,
                                                  CustomerNameAr = x.CustomerNameAr,
                                                  CustomerNameEn = x.CustomerNameEn,
                                                  //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                                                  CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                                                        : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                                                  CustomerNationalId = x.CustomerNationalId,
                                                  NationalIdSource = x.NationalIdSource,
                                                  CustomerAddress = x.CustomerAddress,
                                                  CustomerEmail = x.CustomerEmail,
                                                  CustomerPhone = x.CustomerPhone,
                                                  CustomerMobile = x.CustomerMobile,
                                                  Notes = x.Notes,
                                                  CustomerTypeId = x.CustomerTypeId,
                                                  LogoUrl = x.LogoUrl,
                                                  AttachmentUrl = x.AttachmentUrl,
                                                  CommercialActivity = x.CommercialActivity,
                                                  CommercialRegister = x.CommercialRegister,
                                                  CommercialRegDate = x.CommercialRegDate,
                                                  AccountId = x.AccountId,
                                                  GeneralManager = x.GeneralManager,
                                                  AgentName = x.AgentName,
                                                  AgentType = x.AgentType,
                                                  AgentNumber = x.AgentNumber,
                                                  AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                                                  ResponsiblePerson = x.ResponsiblePerson,
                                                  AgentAttachmentUrl = x.AgentAttachmentUrl,
                                                  AccountCodee = x.Accounts!.Code ?? "",
                                                  CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                                                  BranchId = x.BranchId,
                                                  CompAddress = x.CompAddress,
                                                  ExternalPhone = x.ExternalPhone,
                                                  PostalCodeFinal = x.PostalCodeFinal,
                                                  Country = x.Country,
                                                  Neighborhood = x.Neighborhood,
                                                  StreetName = x.StreetName,
                                                  CityId = x.CityId,
                                                  CityName = x.city != null ? x.city.NameAr : "",
                                                  BuildingNumber = x.BuildingNumber
                                              });
            }
            else
            {
                Customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && ((s.CustomerId == CustomersSearch.CustomerId) || CustomersSearch.CustomerId == null || CustomersSearch.CustomerId == 0) &&
                                                   (s.CustomerNationalId == CustomersSearch.CustomerNationalId || s.CustomerNationalId.Contains(CustomersSearch.CustomerNationalId) || CustomersSearch.CustomerNationalId == null) &&
                                                   (s.CustomerMobile == CustomersSearch.CustomerMobile || s.CustomerMobile.Contains(CustomersSearch.CustomerMobile) || CustomersSearch.CustomerMobile == null))
                                                   .Select(x => new CustomerVM
                                                   {
                                                       CustomerId = x.CustomerId,
                                                       CustomerCode = x.CustomerCode,
                                                       CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                                                       CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                                                       //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                                                       CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                                                       CustomerNationalId = x.CustomerNationalId,
                                                       NationalIdSource = x.NationalIdSource,
                                                       CustomerAddress = x.CustomerAddress,
                                                       CustomerEmail = x.CustomerEmail,
                                                       CustomerPhone = x.CustomerPhone,
                                                       CustomerMobile = x.CustomerMobile,
                                                       Notes = x.Notes,
                                                       CustomerTypeId = x.CustomerTypeId,
                                                       LogoUrl = x.LogoUrl,
                                                       AttachmentUrl = x.AttachmentUrl,
                                                       CommercialActivity = x.CommercialActivity,
                                                       CommercialRegister = x.CommercialRegister,
                                                       CommercialRegDate = x.CommercialRegDate,
                                                       AccountId = x.AccountId,
                                                       GeneralManager = x.GeneralManager,
                                                       AgentName = x.AgentName,
                                                       AgentType = x.AgentType,
                                                       AgentNumber = x.AgentNumber,
                                                       AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                                                       ResponsiblePerson = x.ResponsiblePerson,
                                                       AgentAttachmentUrl = x.AgentAttachmentUrl,
                                                       AccountCodee = x.Accounts!.Code ?? "",
                                                       CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                                                       BranchId = x.BranchId,
                                                       CompAddress = x.CompAddress,
                                                       ExternalPhone = x.ExternalPhone,
                                                       PostalCodeFinal = x.PostalCodeFinal,
                                                       Country = x.Country,
                                                       Neighborhood = x.Neighborhood,
                                                       StreetName = x.StreetName,
                                                       CityId = x.CityId,
                                                       CityName = x.city != null ? x.city.NameAr : "",
                                                       BuildingNumber = x.BuildingNumber
                                                   });
            }
            return Customers; ;
        }
        public async Task<IEnumerable<CustomerVM>> CustomerInterval(string FromDate, string ToDate, int BranchId, string lang)
        {
            var Customer = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AddDate = x.AddDate,
                AccountCodee = x.Accounts!.Code ?? "",
                CompAddress = x.CompAddress,
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                BuildingNumber = x.BuildingNumber

            }).ToList().Where(s => s.AddDate >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && s.AddDate <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            ;
            return Customer;
        }

        public async Task<IEnumerable<CustomerVM>> CustomerIntervalByCustomerType(string FromDate, string ToDate, int customerType, int BranchId, string lang)
        {
            var Customer = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && (s.CustomerTypeId == customerType || customerType == 0)).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AddDate = x.AddDate,
                AccountCodee = x.Accounts!.Code ?? "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BranchId = x.BranchId,
                BuildingNumber = x.BuildingNumber
            }).ToList();
            if(FromDate!="" && ToDate!="")
            {
                Customer = Customer.Where(s => s.AddDate >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && s.AddDate <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
            }

            return Customer;
        }
        public async Task<int?> GenerateNextCustomerCodeNumber()
        {
            if (_TaamerProContext.Customer != null)
            {
                var lastRow = _TaamerProContext.Customer.OrderByDescending(u => u.AddDate).Take(1).FirstOrDefault();
                if (lastRow != null)
                {
                    return int.Parse(lastRow.CustomerCode) + 1;
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
        public async Task<int> GetCitizensCount(int BranchId)
        {
            return _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerTypeId == 1 && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Count();
        }
        public async Task<int> GetInvestorCompanyCount(int BranchId)
        {
            return _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerTypeId == 2 && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Count();
        }
        public async Task<int> GetCustomerByEmail(string CustomerSearchEmail, int BranchId)
        {
            var CustomerSearch = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && s.CustomerEmail == CustomerSearchEmail).Count();
            return CustomerSearch;
        }
        public async Task<int> GetGovernmentSideCount(int BranchId)
        {
            return _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerTypeId == 3 && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Count();
        }
        public async Task<int> GetCustomersCount(int BranchId)
        {
            return _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Count();

        }

        public async Task<IEnumerable<CustomerVM>> GetAllCustomersProj(string lang, int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AccountCodee = x.Accounts!.Code ?? "",
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                Projects = x.Projects!.Select(p => new ProjectVM
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    ProjectNo = p.ProjectNo,
                    CustomerId = p.CustomerId,
                    ProjectTypeName = p.ProjectTypeName,
                    CityId = p.CityId,
                    ContractNo = p.Contracts!.ContractNo,
                    ContractorName = p.ContractorName,
                    ProjectDescription = p.ProjectDescription,
                    ProjectDate = p.ProjectDate,
                    ProjectHijriDate = p.ProjectHijriDate,
                    ProjectTypeId = p.ProjectTypeId,
                    MangerId = p.MangerId,
                    ParentProjectId = p.ParentProjectId,
                    BuildingType = p.BuildingType,
                    SubProjectTypeId = p.SubProjectTypeId,
                    TransactionTypeId = p.TransactionTypeId,
                    CustomerName = p.customer.CustomerNameAr,
                    ProjectTypesName = p.projecttype!.NameAr,
                    ProjectSubTypeName = p.projectsubtype!.NameAr,
                    ProjectMangerName = p.Users!.FullName,
                    CityName = p.city!.NameAr,
                    TransactionTypeName = p.transactionTypes!.NameAr,
                    RegionTypeId = p.RegionTypeId,
                    RegionTypeName = p.regionTypes!.NameAr,
                    FileCount = p.ProjectFiles!.Count(),
                    ExpectedTime = p.ProjectPhasesTasks!.Sum(s => s.TimeMinutes),
                    CurrentMainPhase = p.ActiveMainPhase!.DescriptionAr,
                    CurrentSubPhase = p.ActiveSubPhase!.DescriptionAr,
                    ProjectPhasesTasks = p.ProjectPhasesTasks.Where(s => s.Type == 3).Select(t => new ProjectPhasesTasksVM
                    {
                        PhaseTaskId = t.PhaseTaskId,
                        DescriptionAr = t.DescriptionAr,
                        DescriptionEn = t.DescriptionEn,
                        Type = t.Type,
                        ProjectId = t.ProjectId,
                        TimeMinutes = t.TimeMinutes,
                        TimeType = t.TimeType,
                        Remaining = t.Remaining,
                        IsUrgent = t.IsUrgent,
                        Cost = t.Cost,
                        StopCount = t.StopCount,
                        UserName = t.Users!.FullName,
                        //StartDate = t.StartDate,
                        //EndDate = t.EndDate,
                        TaskTypeName = t.TaskTypeModel!.NameAr, //t.TaskType == 1 ? "رفع ملفات" : t.TaskType == 2 ? "تحصيل دفعه" : t.TaskType == 3 ? "طلعة اشراف" : "مهمة خارجية",
                        ToUserId = t.ToUserId,
                        Notes = x.Notes ?? "",
                        BranchId = x.BranchId,
                        //MainPhaseName = t.MainPhase!.DescriptionAr,
                        SubPhaseName = t.SubPhase!.DescriptionAr,
                        ProjectSubTypeName = t.ProjectSubTypes!.NameAr,
                        ProjectTypeName = t.ProjectSubTypes!.ProjectType!.NameAr,
                        ClientName = t.Project!.customer!.CustomerNameAr,
                        ProjectMangerName = t.Project!.Users!.FullName,
                        ProjectNumber = t.Project!.ProjectNo,
                        StatusName = t.Status == 0 ? "غير معلومة" :
                            t.Status == 1 ? " لم تبدأ " :
                            t.Status == 2 ? " قيد التشغيل " :
                            t.Status == 3 ? " متوقفة " :
                            t.Status == 4 ? " منتهية " :
                            t.Status == 5 ? " ملغاة " :
                            t.Status == 6 ? " محذوفة " : " تم تحويلها",
                        PlayingTime = t.TimeMinutes - t.Remaining,

                    }).ToList()
                }).ToList(),
            });
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenue(string FromDate, string ToDate, int BranchId, string lang)
        {
            try
            {
                var Customer = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && s.Accounts!.Transactions.Where(t => t.IsDeleted == false).Sum(t => t.Credit) > 0).Select(x => new CustomerVM
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                    //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                    CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                    NationalIdSource = x.NationalIdSource,
                    CustomerAddress = x.CustomerAddress,
                    CustomerEmail = x.CustomerEmail,
                    CustomerPhone = x.CustomerPhone,
                    CustomerMobile = x.CustomerMobile,
                    Notes = x.Notes,
                    CommercialActivity = x.CommercialActivity,
                    CommercialRegister = x.CommercialRegister,
                    CommercialRegDate = x.CommercialRegDate,
                    CommercialRegHijriDate = x.CommercialRegHijriDate,
                    AccountId = x.AccountId,
                    GeneralManager = x.GeneralManager,
                    AgentName = x.AgentName,
                    AccountCodee = x.Accounts!.Code,
                    TotalRevenue = x.Accounts!.Transactions.Where(t => t.IsDeleted == false).Sum(t => t.Credit) ?? 0, //x.Transactions.Where(t => t.IsDeleted == false && (DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)) &&
                    //(DateTime.ParseExact(t.TransactionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).Sum(t => t.Credit) ?? 0,
                    TotalExpenses = x.Accounts!.Transactions.Where(t => t.IsDeleted == false).Sum(t => t.Depit) ?? 0,
                    AgentType = x.AgentType,
                    AgentNumber = x.AgentNumber,
                    AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                    ResponsiblePerson = x.ResponsiblePerson,
                    AgentAttachmentUrl = x.AgentAttachmentUrl,
                    AddDate = x.AddDate,
                    BranchId = x.BranchId,
                    CityId = x.CityId,
                    CityName = x.city != null ? x.city.NameAr : "",
                    CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                    Invoices = x.Invoicess.Select(i => new InvoicesVM
                    {
                        InvoiceNumber = i.InvoiceNumber,
                        InvoiceId = i.InvoiceId,
                        Type = i.Type,
                        IsPost = i.IsPost,
                        PostDate = i.PostDate,
                        PostHijriDate = i.PostHijriDate,
                        StatusName = i.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = i.Date,
                        HijriDate = i.HijriDate,
                        InvoiceReference = i.InvoiceReference,
                        Notes = i.Notes,
                        TotalValue = i.TotalValue,
                        InvoiceValue = i.InvoiceValue,
                        TaxAmount = i.TaxAmount,
                        ToAccountId = i.ToAccountId,
                        InvoiceValueText = i.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerName = i.Customer!.CustomerNameAr,
                    }).ToList(),
                }).ToList().Where(s => s.Invoices.Any(d => DateTime.ParseExact(d.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && DateTime.ParseExact(d.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList();
                return Customer;
            }
            catch 
            {
                var Customer = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && s.Accounts!.Transactions.Where(t => t.IsDeleted == false).Sum(t => t.Credit) > 0).Select(x => new CustomerVM
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                    //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                    CustomerName =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        lang == "ltr"
        ? (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
            : x.CustomerNameEn
        )
        : (
            x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
            : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
            : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
            : x.CustomerNameAr
        )
    ),
                         CustomerNameAr = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameAr,

                         CustomerNameEn = (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") + x.CustomerNameEn,

                         CustomerNationalId = x.CustomerNationalId,
                    NationalIdSource = x.NationalIdSource,
                    CustomerAddress = x.CustomerAddress,
                    CustomerEmail = x.CustomerEmail,
                    CustomerPhone = x.CustomerPhone,
                    CustomerMobile = x.CustomerMobile,
                    Notes = x.Notes,
                    CommercialActivity = x.CommercialActivity,
                    CommercialRegister = x.CommercialRegister,
                    CommercialRegDate = x.CommercialRegDate,
                    CommercialRegHijriDate = x.CommercialRegHijriDate,
                    AccountId = x.AccountId,
                    GeneralManager = x.GeneralManager,
                    AgentName = x.AgentName,
                    AccountCodee = x.Accounts!.Code,
                    TotalRevenue = x.Accounts!.Transactions!.Where(t => t.IsDeleted == false).Sum(t => t.Credit) ?? 0,
                    TotalExpenses = x.Accounts!.Transactions!.Where(t => t.IsDeleted == false).Sum(t => t.Depit) ?? 0,
                    AgentType = x.AgentType,
                    AgentNumber = x.AgentNumber,
                    AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                    ResponsiblePerson = x.ResponsiblePerson,
                    AgentAttachmentUrl = x.AgentAttachmentUrl,
                    AddDate = x.AddDate,
                    CityId = x.CityId,
                    CityName = x.city != null ? x.city.NameAr : "",
                    CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                    Invoices = x.Invoicess!.Select(i => new InvoicesVM
                    {
                        InvoiceNumber = i.InvoiceNumber,
                        InvoiceId = i.InvoiceId,
                        Type = i.Type,
                        IsPost = i.IsPost,
                        PostDate = i.PostDate,
                        PostHijriDate = i.PostHijriDate,
                        StatusName = i.IsPost == true ? "تم الترحيل" : "غير مرحل",
                        Date = i.Date,
                        HijriDate = i.HijriDate,
                        InvoiceReference = i.InvoiceReference,
                        Notes = i.Notes,
                        TotalValue = i.TotalValue,
                        InvoiceValue = i.InvoiceValue,
                        TaxAmount = i.TaxAmount,
                        ToAccountId = i.ToAccountId,
                        InvoiceValueText = i.InvoiceValueText,
                        BranchId = x.BranchId,
                        CustomerName = i.Customer!.CustomerNameAr,
                    }).ToList(),
                }).ToList();
                return Customer;
            }
        }
        public async Task<IEnumerable<CustomerVM>> GetCustomerExpensesRevenueDGV(string FromDate, string ToDate, int BranchId, string lang, string Con)
        {
            try
            {
                List<CustomerVM> lmd = new List<CustomerVM>();
                SqlConnection con = new SqlConnection(Con);//SqlConnection("Data Source=144.91.68.47\\sqlexpress;Initial Catalog=TameerProDB;uID=sa;Password=admin_134711;");
                SqlDataAdapter da = new SqlDataAdapter("CustomerRevenue", con);
                //SqlCommand cmd = new SqlCommand();
                //SqlParameter id = new SqlParameter("@id", SqlDbType.Int.ToString());
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                da.Fill(ds);
                foreach (DataRow dr in ds.Tables[0].Rows)

                // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new CustomerVM
                    {
                        // adding data from dataset row in to list<modeldata>  

                        CustomerName = dr["CustomerName"].ToString(),
                        AccountCodee = dr["AccountCodee"].ToString(),
                        TotalRevenue = Convert.ToInt64(dr["TotalRevenue"])
                    });
                }
                con.Close();
                return lmd;
            }
            catch (Exception ex)
            {
                List<CustomerVM> lmd = new List<CustomerVM>();
                return lmd;
            }
        }
        public async Task<CustomerVM> GetCustomersByCustomerId(int? CustomerId, string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false &&(CustomerId == null || s.CustomerId == CustomerId)).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                BranchId=x.BranchId,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AccountCodee = x.Accounts!.Code ?? "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                PostalCodeFinal=x.PostalCodeFinal??"",
                ExternalPhone = x.ExternalPhone ?? "",
                Country = x.Country ?? "",
                Neighborhood = x.Neighborhood ?? "",
                StreetName = x.StreetName ?? "",
                BuildingNumber = x.BuildingNumber ?? "",
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                BranchName= x.Branch !=null ? x.Branch.NameAr : "",

            }).FirstOrDefault();
            return customers;
        }
        public async Task<CustomerVM> GetCustomersByCustomerIdInvoice(int? CustomerId, string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerId == CustomerId).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerTypeId == 2?x.CompAddress??"":x.CustomerAddress??"",
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AccountCodee = x.Accounts!.Code ?? "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                PostalCodeFinal = x.PostalCodeFinal ?? "",
                ExternalPhone = x.ExternalPhone ?? "",
                Country = x.Country ?? "",
                Neighborhood = x.Neighborhood ?? "",
                StreetName = x.StreetName ?? "",
                BuildingNumber = x.BuildingNumber ?? "",
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                CommercialRegInvoice = x.CustomerTypeId==2?x.CustomerAddress:x.CommercialRegister??"",

            }).FirstOrDefault();
            return customers;
        }

        public async Task<CustomerVM> GetCustomersByCustomerIdOnly(int? CustomerId, string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerId == CustomerId).Select(x => new CustomerVM
            {
                AccountName = (x.Accounts != null) ? (x.Accounts!.NameAr + " - " + x.Accounts!.Code) : "",
            }).FirstOrDefault();
            return customers;
        }
        public async Task<CustomerVM> GetCustomersByAccountId(int? AccountId, string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.AccountId == AccountId).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AccountCodee = x.Accounts!.Code ?? "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BranchId = x.BranchId,
                BuildingNumber = x.BuildingNumber
            }).FirstOrDefault();
            return customers;
        }


        public async Task<IEnumerable<CustomerVM>> GetCustomersByCustId(int? CustomerId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerId == CustomerId).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = x.CustomerNameAr,
                CustomerName = x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AccountCodee = x.Accounts!.Code ?? "",
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                CustomerTypeName = "مواطن",
                  ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BranchId = x.BranchId,
                BuildingNumber = x.BuildingNumber
            }).ToList();
            return customers;
        }
         
        public async Task<CustomerVM> GetCustomersByNationalId(string NationalId, string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CustomerNationalId == NationalId && s.CustomerTypeId == 1).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AccountCodee = x.Accounts!.Code ?? "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                BuildingNumber = x.BuildingNumber
            }).FirstOrDefault();
            return customers;
        }



        public async Task<CustomerVM> GetCustomersByCommercialRegister(string CommercialRegister, string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && s.CommercialRegister == CommercialRegister && s.CustomerTypeId == 2).Select(x => new CustomerVM
            {
                CustomerId = x.CustomerId,
                CustomerCode = x.CustomerCode,
                //CustomerName = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                CustomerName = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                CustomerNameAr = x.CustomerNameAr,
                CustomerNameEn = x.CustomerNameEn,
                CustomerNationalId = x.CustomerNationalId,
                NationalIdSource = x.NationalIdSource,
                CustomerAddress = x.CustomerAddress,
                CustomerEmail = x.CustomerEmail,
                CustomerPhone = x.CustomerPhone,
                CustomerMobile = x.CustomerMobile,
                Notes = x.Notes,
                CustomerTypeId = x.CustomerTypeId,
                LogoUrl = x.LogoUrl,
                AttachmentUrl = x.AttachmentUrl,
                CommercialActivity = x.CommercialActivity,
                CommercialRegister = x.CommercialRegister,
                CommercialRegDate = x.CommercialRegDate,
                CommercialRegHijriDate = x.CommercialRegHijriDate,
                AccountId = x.AccountId,
                GeneralManager = x.GeneralManager,
                AccountName = (x.Accounts != null) ? (x.Accounts!.Code + " - " + x.Accounts!.NameAr) : "",
                AgentName = x.AgentName,
                AgentType = x.AgentType,
                AgentNumber = x.AgentNumber,
                ResponsiblePerson = x.ResponsiblePerson,
                AgentAttachmentUrl = x.AgentAttachmentUrl,
                AccountCodee = x.Accounts!.Code ?? "",
                CustomerTypeName = x.CustomerTypeId == 1 ? (lang == "ltr" ? "citizen" : "مواطن") : x.CustomerTypeId == 2 ? (lang == "ltr" ? "Investors and companies" : "مستثمرون وشركات") : x.CustomerTypeId == 3 ? (lang == "ltr" ? "Governmental entity" : "جهة حكومية") : "",
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BranchId = x.BranchId,
                CityId = x.CityId,
                CityName = x.city != null ? x.city.NameAr : "",
                BuildingNumber = x.BuildingNumber
            }).FirstOrDefault();
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetAllCustomers(string SearchText)
        {
            if (SearchText == "")
            {
                var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false).Select(x => new CustomerVM
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                    //CustomerNameAr = x.CustomerNameAr,
                    CustomerNameAr = x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                    CustomerNameEn = x.CustomerNameEn,
                    Notes = x.Notes
                }).ToList();
                return customers;
            }
            else
            {
                var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.CustomerNameAr.Contains(SearchText) || s.CustomerNameEn.Contains(SearchText))).Select(x => new CustomerVM
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                    //CustomerNameAr = x.CustomerNameAr,
                    CustomerNameAr = x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,

                    CustomerNameEn = x.CustomerNameEn,
                    Notes = x.Notes
                }).ToList();
                return customers;
            }
        }
        public async Task<IEnumerable<CustomerVM>> FillAllCustomerSelectWithBranch(string SearchText,int BranchId)
        {
            if (SearchText == "")
            {
                var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                         CustomerNameAr =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)"
        : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)"
        : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)"
        : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)"
        : x.CustomerNameAr
    ),
                         CustomerNameEn = x.CustomerNameEn,
                    Notes = x.Notes
                }).ToList();
                return customers;
            }
            else
            {
                var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) && (s.CustomerNameAr.Contains(SearchText) || s.CustomerNameEn.Contains(SearchText))).Select(x => new CustomerVM
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                         CustomerNameAr =
    (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
    (
        x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)"
        : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)"
        : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)"
        : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)"
        : x.CustomerNameAr
    ),
                         CustomerNameEn = x.CustomerNameEn,
                    Notes = x.Notes
                }).ToList();
                return customers;
            }
        }


        public async Task<IEnumerable<CustomerVM>> GetAllCustomersNotHaveProj(string lang, int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false &&
            s.Projects!.Where(x => x.IsDeleted == false && x.Status ==0
             ).Count() == 0).Select(x => new CustomerVM
             {
                 CustomerId = x.CustomerId,
                 CustomerNameAr = x.CustomerNameAr,
             });
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> FillAllCustomerSelectNotHaveProjWithBranch(string lang, int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false && (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId)) &&
            s.Projects!.Where(x => x.IsDeleted == false && x.Status == 0
             ).Count() == 0).Select(x => new CustomerVM
             {
                 CustomerId = x.CustomerId,
                 CustomerNameAr =
                (x.BranchId != BranchId &&
                 x.Customer_Branches.Any(cb => cb.BranchId == BranchId)
                ? "→ " : "") + x.CustomerNameAr,
             });
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetAllCustomersNotHaveProjWithout(string lang, int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false &&
            s.Projects!.Where(x => x.IsDeleted == false 
             ).Count() == 0).Select(x => new CustomerVM
             {
                 CustomerId = x.CustomerId,
                 CustomerNameAr = x.CustomerNameAr,
             });
            return customers;
        }


        public async Task<IEnumerable<CustomerVM>> GetAllCustomerForDrop(string lang)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false).Select(x => new CustomerVM
            {
                Id = x.CustomerId,
                //Name = lang == "ltr" ? x.CustomerNameEn : x.CustomerNameAr,
                //Name = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn+"*" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn+"**" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn+"***" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn+"****" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn+"VIP" : x.CustomerNameAr
                //: x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr+"*" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr+"**" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr+"***" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr+"****" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr+"VIP" : x.CustomerNameAr,
                Name = lang == "ltr" ? x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameEn : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameEn  : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameEn + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameEn + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameEn + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameEn + "(VIP)" : x.CustomerNameAr
                : x.Projects!.Where(p => p.IsDeleted == false).Count() == 0 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? x.CustomerNameAr : x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? x.CustomerNameAr + "(*)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? x.CustomerNameAr + "(**)" : x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? x.CustomerNameAr + "(***)" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? x.CustomerNameAr + "(VIP)" : x.CustomerNameAr,
                
                NoOfCustProj = x.Projects!.Where(p=>p.IsDeleted==false).Count(),
                NoOfCustProjMark= x.Projects!.Where(p => p.IsDeleted == false).Count()==0?"": x.Projects!.Where(p => p.IsDeleted == false).Count() == 1 ? "*": x.Projects!.Where(p => p.IsDeleted == false).Count() == 2 ? "**": x.Projects!.Where(p => p.IsDeleted == false).Count() == 3 ? "***": x.Projects!.Where(p => p.IsDeleted == false).Count() == 4 ? "****" : x.Projects!.Where(p => p.IsDeleted == false).Count() >= 5 ? "VIP":"",
            }).ToList();
            return customers;
        }
        public async Task<IEnumerable<CustomerVM>> GetAllCustomerForDropWithBranch(string lang,int BranchId)
        {
            var customers = _TaamerProContext.Customer.Where(s => s.IsDeleted == false &&
                    (s.BranchId == BranchId ||
                     s.Customer_Branches.Any(cb => cb.BranchId == BranchId))).Select(x => new CustomerVM
            {
                Id = x.CustomerId,
                         Name =
                            (x.BranchId != BranchId && x.Customer_Branches.Any(cb => cb.BranchId == BranchId) ? "→ " : "") +
                            (
                                lang == "ltr"
                                ? (
                                    x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameEn + "(*)"
                                    : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameEn + "(**)"
                                    : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameEn + "(***)"
                                    : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameEn + "(VIP)"
                                    : x.CustomerNameEn
                                )
                                : (
                                    x.Projects!.Count(p => p.IsDeleted == false) == 2 ? x.CustomerNameAr + "(*)"
                                    : x.Projects!.Count(p => p.IsDeleted == false) == 3 ? x.CustomerNameAr + "(**)"
                                    : x.Projects!.Count(p => p.IsDeleted == false) == 4 ? x.CustomerNameAr + "(***)"
                                    : x.Projects!.Count(p => p.IsDeleted == false) >= 5 ? x.CustomerNameAr + "(VIP)"
                                    : x.CustomerNameAr
                                )
                            ),
                         CustomerNameAr =
                (x.BranchId != BranchId &&
                 x.Customer_Branches.Any(cb => cb.BranchId == BranchId)
                ? "→ " : "") + x.CustomerNameAr,
                         CustomerNameEn =
                (x.BranchId != BranchId &&
                 x.Customer_Branches.Any(cb => cb.BranchId == BranchId)
                ? "→ " : "") + x.CustomerNameEn,
                     }).ToList();
            return customers;
        }

    }
}
