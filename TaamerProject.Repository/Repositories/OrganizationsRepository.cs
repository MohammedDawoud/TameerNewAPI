using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace TaamerProject.Repository.Repositories
{
    public class OrganizationsRepository : IOrganizationsRepository
    {
        private readonly IBranchesRepository _BranchesRepository;
        private readonly TaamerProjectContext _TaamerProContext;

        public OrganizationsRepository(TaamerProjectContext dataContext, IBranchesRepository branchesRepository)
        {   
            _TaamerProContext = dataContext;
            _BranchesRepository = branchesRepository;

        }
        public async Task< OrganizationsVM> GetBranchOrganization(int? BranchId)
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false && s.BranchId == BranchId).Select(x => new OrganizationsVM
            {
                OrganizationId = x.OrganizationId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Mobile = x.Mobile,
                Email = x.Email,
                LogoUrl = x.LogoUrl,
                ServerName = x.ServerName,
                Address = x.Address,
                WebSite = x.WebSite,
                Fax = x.Fax,
                CityId = x.CityId,
                CityName = x.City != null ? x.City.NameAr ?? "" : "",
                ReportType = x.ReportType,
                TaxCode = x.TaxCode,
                VAT = x.VAT,
                VATSetting = x.VATSetting,
                NotificationsMail = x.NotificationsMail,
                PostalCode = x.PostalCode,
                Password = x.Password,
                SenderName = x.SenderName,
                Host = x.Host,
                Port = x.Port,
                SSL = x.SSL,
                IsFooter = x.IsFooter,
                EditUserName = x.UpdateUserT == null ? "" : x.UpdateUserT.FullName ?? "",
                EditUserDate = x.UpdateDate,
                RepresentorEmpId = x.RepresentorEmpId,
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BuildingNumber = x.BuildingNumber,
                ComDomainLink=x.ComDomainLink??"",
                ComDomainAddress = x.ComDomainAddress ?? "",
                BankId=x.BankId??0,
                BankId2=x.BankId2??0,
                AccountBank=x.AccountBank??null,
                AccountBank2=x.AccountBank2??null,
                BankIdImgURL=x.Bank!=null?x.Bank.BanckLogo:"",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                ApiBaseUri = x.ApiBaseUri ?? "",
                TameerAPIURL=x.TameerAPIURL ??"",

                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
                ModeType=x.ModeType??1,

            }).FirstOrDefault();
            return organizations;
        }
        public async Task<OrganizationsVM> GetBranchOrganization()
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false).Select(x => new OrganizationsVM
            {
                OrganizationId = x.OrganizationId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Mobile = x.Mobile,
                Email = x.Email,
                LogoUrl = x.LogoUrl,
                ServerName = x.ServerName,
                Address = x.Address,
                WebSite = x.WebSite,
                Fax = x.Fax,
                CityId = x.CityId,
                CityName = x.City != null ? x.City.NameAr ?? "" : "",
                ReportType = x.ReportType,
                TaxCode = x.TaxCode,
                VAT = x.VAT,
                VATSetting = x.VATSetting,
                NotificationsMail = x.NotificationsMail,
                PostalCode = x.PostalCode,
                Password = x.Password,
                SenderName = x.SenderName,
                Host = x.Host,
                Port = x.Port,
                SSL = x.SSL,
                IsFooter = x.IsFooter,
                EditUserName = x.UpdateUserT == null ? "" : x.UpdateUserT.FullName ?? "",
                EditUserDate = x.UpdateDate,
                RepresentorEmpId = x.RepresentorEmpId,
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                StreetName = x.StreetName,
                BuildingNumber = x.BuildingNumber,
                ComDomainLink = x.ComDomainLink ?? "",
                ComDomainAddress = x.ComDomainAddress ?? "",
                BankId = x.BankId ?? 0,
                BankId2 = x.BankId2 ?? 0,
                AccountBank = x.AccountBank ?? null,
                AccountBank2 = x.AccountBank2 ?? null,
                BankIdImgURL = x.Bank != null ? x.Bank.BanckLogo : "",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                ApiBaseUri = x.ApiBaseUri ?? "",
                TameerAPIURL = x.TameerAPIURL ?? "",

                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
                ModeType = x.ModeType ?? 1,

            }).FirstOrDefault();
            return organizations;
        }


        public async Task< OrganizationsVM> GetBranchOrganizationData(int OrgId)
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false && s.OrganizationId == OrgId).Select(x => new OrganizationsVM
            {
                OrganizationId = x.OrganizationId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Mobile = x.Mobile,
                Email = x.Email,
                LogoUrl = x.LogoUrl,
                ServerName = x.ServerName,
                Address = x.Address,
                WebSite = x.WebSite,
                Fax = x.Fax,
                CityId = x.CityId,
                CityName = x.City != null ? x.City.NameAr ?? "" : "",
                ReportType = x.ReportType,
                TaxCode = x.TaxCode,
                VAT = x.VAT,
                VATSetting = x.VATSetting,
                NotificationsMail = x.NotificationsMail,
                PostalCode = x.PostalCode,
                Password = x.Password,
                SenderName = x.SenderName,
                Host = x.Host,
                Port = x.Port,
                SSL = x.SSL,
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                BuildingNumber = x.BuildingNumber,
                StreetName = x.StreetName,
                IsFooter = x.IsFooter,
                EditUserName = x.UpdateUserT == null ? "" : x.UpdateUserT.FullName ?? "",
                EditUserDate = x.UpdateDate,
                RepresentorEmpId = x.RepresentorEmpId,
                Engineering_License=x.Engineering_License??"",
                Engineering_LicenseDate=x.Engineering_LicenseDate??"",
                ComDomainLink = x.ComDomainLink ?? "",
                BankId = x.BankId ?? 0,
                BankId2 = x.BankId2 ?? 0,
                AccountBank = x.AccountBank ?? null,
                AccountBank2 = x.AccountBank2 ?? null,
                BankIdImgURL = x.Bank != null ? x.Bank.BanckLogo : "",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                ApiBaseUri = x.ApiBaseUri ?? "",
                TameerAPIURL = x.TameerAPIURL ?? "",
                ComDomainAddress=x.ComDomainAddress??"",
                 SendCustomerMail=x.SendCustomerMail??false,

                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
                ModeType = x.ModeType ?? 1,
            })
            .FirstOrDefault();
            return organizations;
        }
        public async Task< OrganizationsVM> GetComDomainLink_Org(int OrgId)
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false && s.OrganizationId == OrgId).Select(x => new OrganizationsVM
            {
                OrganizationId = x.OrganizationId,
                ComDomainLink = x.ComDomainLink ?? "",
                ComDomainAddress = x.ComDomainAddress ?? "",
                ApiBaseUri=x.ApiBaseUri??"",
                TameerAPIURL = x.TameerAPIURL ?? "",
                

            })
            .FirstOrDefault();
            return organizations;
        }



        public async Task< OrganizationsVM> GetApplicationVersion_Org(int OrgId)
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false && s.OrganizationId == OrgId).Select(x => new OrganizationsVM
            {
                 OrganizationId = x.OrganizationId,
                 LastvesionAndroid=x.LastvesionAndroid??"",
                 LastversionIOS=x.LastversionIOS??"",
                 MessageUpdateAr=x.MessageUpdateAr??"",
                 MessageUpdateEn=x.MessageUpdateEn??"",
                 SupportMessageAr=x.SupportMessageAr??"",
                 SupportMessageEn=x.SupportMessageEn??"",

            })
            .FirstOrDefault();
            return organizations;
        }


        public async Task< OrganizationsVM> GetOrganizationDataLogin(string Lang)
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false).Select(x => new OrganizationsVM
            {
                OrganizationId = x.OrganizationId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Mobile = x.Mobile,
                Email = x.Email,
                LogoUrl = x.LogoUrl,
                ServerName = x.ServerName,
                Address = x.Address,
                WebSite = x.WebSite,
                Fax = x.Fax,
                CityId = x.CityId,
                CityName = x.City != null ? x.City.NameAr ?? "" : "",
                ReportType = x.ReportType,
                TaxCode = x.TaxCode,
                VAT = x.VAT,
                VATSetting = x.VATSetting,
                NotificationsMail = x.NotificationsMail,
                PostalCode = x.PostalCode,
                Password = x.Password,
                SenderName = x.SenderName,
                Host = x.Host,
                Port = x.Port,
                SSL = x.SSL,
                ExternalPhone = x.ExternalPhone,
                PostalCodeFinal = x.PostalCodeFinal,
                Country = x.Country,
                Neighborhood = x.Neighborhood,
                BuildingNumber = x.BuildingNumber,
                StreetName = x.StreetName,
                IsFooter = x.IsFooter,
                EditUserName = x.UpdateUserT == null ? "" : x.UpdateUserT.FullName ?? "",
                EditUserDate = x.UpdateDate,
                RepresentorEmpId = x.RepresentorEmpId,
                Engineering_License = x.Engineering_License ?? "",
                Engineering_LicenseDate = x.Engineering_LicenseDate ?? "",
                OrgName = Lang == "ltr" ? x.NameEn : x.NameAr,
                ComDomainLink = x.ComDomainLink ?? "",
                ComDomainAddress = x.ComDomainAddress ?? "",

                BankId = x.BankId ?? 0,
                BankId2 = x.BankId2 ?? 0,
                AccountBank = x.AccountBank ?? null,
                AccountBank2 = x.AccountBank2 ?? null,
                BankIdImgURL = x.Bank != null ? x.Bank.BanckLogo : "",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                ApiBaseUri = x.ApiBaseUri ?? "",
                TameerAPIURL = x.TameerAPIURL ?? "",
                 SendCustomerMail=x.SendCustomerMail??false,

                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
                ModeType = x.ModeType ?? 1,

            })
            .FirstOrDefault();
            try
            {
                var smsObj = _TaamerProContext.SMSSettings.FirstOrDefault(x => x.IsDeleted == false);
                if(smsObj != null) 
                { 
                    organizations.SendCustomerSMS = smsObj.SendCustomerSMS ?? false; 
                }
                //organizations.SendCustomerSMS=_TaamerProContext.SMSSettings.FirstOrDefault(x=>x.IsDeleted == false)!.SendCustomerSMS??false;

            }
            catch(Exception ex) { }
            return organizations;
        }

        public async Task< OrganizationsVM> GetBranchOrganizationDataInvoice(int OrgId)
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false && s.OrganizationId == OrgId).Select(x => new OrganizationsVM
            {
                OrganizationId = x.OrganizationId,
                NameAr = x.NameAr ?? "",
                NameEn = x.NameEn ?? "",
                Mobile = x.Mobile ?? "",
                Email = x.Email ?? "",
                LogoUrl = x.LogoUrl ?? "",
                ServerName = x.ServerName ?? "",
                Address = x.Address ?? "",
                WebSite = x.WebSite ?? "",
                Fax = x.Fax ?? "",
                CityId = x.CityId ?? 0,
                CityName = x.City!=null? x.City.NameAr ?? "":"",
                ReportType = x.ReportType ?? 0,
                TaxCode = x.TaxCode ?? "",
                VAT = x.VAT,
                VATSetting = x.VATSetting ,
                NotificationsMail = x.NotificationsMail ?? "",
                PostalCode = x.PostalCode ?? "",
                Password = x.Password ?? "",
                SenderName = x.SenderName ?? "",
                Host = x.Host ?? "",
                Port = x.Port ?? "",
                SSL = x.SSL,
                ExternalPhone = x.ExternalPhone ?? "",
                PostalCodeFinal = x.PostalCodeFinal ?? "",
                Country = x.Country ?? "",
                Neighborhood = x.Neighborhood ?? "",
                BuildingNumber = x.BuildingNumber ?? "",
                StreetName = x.StreetName??"",
                IsFooter = x.IsFooter??"",
                EditUserName = x.UpdateUserT == null ? "" : x.UpdateUserT.FullName ?? "",
                EditUserDate = x.UpdateDate,
                RepresentorEmpId = x.RepresentorEmpId??0,
                
                Engineering_License = x.Engineering_License ?? "",
                Engineering_LicenseDate = x.Engineering_LicenseDate ?? "",
                ComDomainLink = x.ComDomainLink ?? "",
                ComDomainAddress = x.ComDomainAddress ?? "",

                BankId = x.BankId ?? 0,
                BankId2 = x.BankId2 ?? 0,
                AccountBank = x.AccountBank ?? null,
                AccountBank2 = x.AccountBank2 ?? null,
                BankIdImgURL = x.Bank != null ? x.Bank.BanckLogo : "",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                ApiBaseUri = x.ApiBaseUri ?? "",
                TameerAPIURL = x.TameerAPIURL ?? "",

                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
                ModeType = x.ModeType ?? 1,
            })
            .FirstOrDefault();
            return organizations;
        }

        public async Task<string> GetDefaultLogoOfOrganization()
        {
            string LogoUrl = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false).FirstOrDefault().LogoUrl ;
            return LogoUrl;
        }
        public async Task< OrganizationsVM> CheckEmailOrganization(int? OrganizationId)
        {
            var organizations = _TaamerProContext.Organizations.Where(s => s.IsDeleted == false && s.OrganizationId == OrganizationId && s.NotificationsMail != null).Select(x => new OrganizationsVM
            {
                OrganizationId = x.OrganizationId,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
                Mobile = x.Mobile,
                Email = x.Email,
                LogoUrl = x.LogoUrl,
                ServerName = x.ServerName,
                Address = x.Address,
                WebSite = x.WebSite,
                Fax = x.Fax,
                CityId = x.CityId,
                CityName = x.City != null ? x.City.NameAr ?? "" : "",
                ReportType = x.ReportType,
                TaxCode = x.TaxCode,
                VAT = x.VAT,
                VATSetting = x.VATSetting,
                NotificationsMail = x.NotificationsMail,
                PostalCode = x.PostalCode,
                Password = x.Password,
                SenderName = x.SenderName,
                Host = x.Host,
                Port = x.Port,
                SSL = x.SSL,
                IsFooter = x.IsFooter,
                ComDomainLink = x.ComDomainLink ?? "",
                ComDomainAddress = x.ComDomainAddress ?? "",
                BankId = x.BankId ?? 0,
                BankId2 = x.BankId2 ?? 0,
                AccountBank = x.AccountBank ?? null,
                AccountBank2 = x.AccountBank2 ?? null,
                BankIdImgURL = x.Bank != null ? x.Bank.BanckLogo : "",
                BankId2ImgURL = x.Bank2 != null ? x.Bank2.BanckLogo : "",
                ApiBaseUri = x.ApiBaseUri ?? "",
                TameerAPIURL = x.TameerAPIURL ?? "",

                CSR = x.CSR ?? "",
                PrivateKey = x.PrivateKey ?? "",
                PublicKey = x.PublicKey ?? "",
                SecreteKey = x.SecreteKey ?? "",
                ModeType = x.ModeType ?? 1,

            }).FirstOrDefault();
            return organizations;
        }

        public async Task<List<FillSelectVM>> FillSelect_Proc(string Con, int? UserId, int Type, int? Param, int? Status, int? BranchId)
        {
            try
            {
                List<FillSelectVM> lmd = new List<FillSelectVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "FillSelect_Proc";
                        command.Connection = con;

                        //---------------------------------------------------------------------
                        if (UserId == 0 || UserId == null)
                            command.Parameters.Add(new SqlParameter("@UserId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@UserId", UserId));
                        //---------------------------------------------------------------------
                        if (Param == 0 || Param == null)
                            command.Parameters.Add(new SqlParameter("@Param", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Param", Param));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@Type", Type));
                        //---------------------------------------------------------------------
                        if (Status == null)
                            command.Parameters.Add(new SqlParameter("@Status", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Status", Status));
                        //---------------------------------------------------------------------
                        if (BranchId == null)
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        //---------------------------------------------------------------------
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new FillSelectVM
                            {
                                Id = Convert.ToInt32((dr["Id"]).ToString()),
                                Name = (dr["Name"]).ToString(),
                                ProjectNo = (dr["ProjectNo"]).ToString(),
                                CustomerName = (dr["CustomerName"]).ToString(),
                                CustomerNameW = (dr["CustomerNameW"]).ToString(),
                                CustomerId = Convert.ToInt32((dr["CustomerId"]).ToString()),
                                ContractId = Convert.ToInt32((dr["ContractId"]).ToString()),
                                TypeCode = Convert.ToInt32((dr["TypeCode"]).ToString()),
                                ProjectTypeId = Convert.ToInt32((dr["ProjectTypeId"]).ToString())
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<FillSelectVM> lmd = new List<FillSelectVM>();
                return lmd;
            }

        }
        public async Task<List<FillSelectVM>> FillSelect_Cust(string Con, int? UserId, int Type, int? Param, int? Status, int? BranchId)
        {
            try
            {
                List<FillSelectVM> lmd = new List<FillSelectVM>();
                using (SqlConnection con = new SqlConnection(Con))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "FillSelect_Cust";
                        command.Connection = con;

                        //---------------------------------------------------------------------
                        if (UserId == 0 || UserId == null)
                            command.Parameters.Add(new SqlParameter("@UserId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@UserId", UserId));
                        //---------------------------------------------------------------------
                        if (Param == 0 || Param == null)
                            command.Parameters.Add(new SqlParameter("@Param", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Param", Param));
                        //---------------------------------------------------------------------
                        command.Parameters.Add(new SqlParameter("@Type", Type));
                        //---------------------------------------------------------------------
                        if (Status == null)
                            command.Parameters.Add(new SqlParameter("@Status", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@Status", Status));
                        //---------------------------------------------------------------------
                        if (BranchId == null)
                            command.Parameters.Add(new SqlParameter("@BranchId", DBNull.Value));
                        else
                            command.Parameters.Add(new SqlParameter("@BranchId", BranchId));
                        //---------------------------------------------------------------------
                        con.Open();

                        SqlDataAdapter a = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        a.Fill(ds);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lmd.Add(new FillSelectVM
                            {
                                Id = Convert.ToInt32((dr["Id"]).ToString()),
                                Name = (dr["Name"]).ToString(),
                            });
                        }
                    }
                }
                return lmd;
            }
            catch (Exception ex)
            {
                List<FillSelectVM> lmd = new List<FillSelectVM>();
                return lmd;
            }

        }


        public IEnumerable<Organizations> GetAll()
        {
            throw new NotImplementedException();
        }

        public Organizations GetById(int Id)
        {
            return _TaamerProContext.Organizations.Where(x => x.OrganizationId == Id).FirstOrDefault();
        }

        public IEnumerable<Organizations> GetMatching(Func<Organizations, bool> where)
        {
            return _TaamerProContext.Organizations.Where(where).ToList<Organizations>();
        }



    }
}
