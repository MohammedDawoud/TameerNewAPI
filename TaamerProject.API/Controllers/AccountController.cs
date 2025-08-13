using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using TaamerProject.API.Helper;
using TaamerProject.Models;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Require2FA")]

    public class AccountController : ControllerBase
    {



        private IAccountsService _accountsService;
        private IBranchesService _branchesService;
        private IOrganizationsService _organizationsservice;
        private IFiscalyearsService _fiscalyearsservice;
        private ISystemSettingsService _systemSettingsService;
        private ICostCenterService _CostCenterservice;
        private ICustomerService _customerservice;
        private IProjectService _projectService;
        private readonly IFiscalyearsService _FiscalyearsService;
        private IConfiguration Configuration;
        public GlobalShared _globalshared;
        private readonly IBranchesRepository _BranchesRepository;


        private byte[] ReportPDF;
        private string Con;
        public AccountController(IConfiguration _configuration, IAccountsService accountsService, IBranchesService branchesService, IOrganizationsService organizationsService,
            IFiscalyearsService fiscalyearsService, ISystemSettingsService systemSettingsService, ICostCenterService costCenterService, ICustomerService customerService,
            IProjectService projectService, IBranchesRepository branchesRepository, IFiscalyearsService fiscalyears)
        {
            this._accountsService = accountsService;
            Configuration = _configuration;
            Con = Con = this.Configuration.GetConnectionString("DBConnection");
            this._branchesService = branchesService;
            this._organizationsservice = organizationsService;
            this._fiscalyearsservice = fiscalyearsService;
            this._systemSettingsService = systemSettingsService;
            this._CostCenterservice = costCenterService;
            this._customerservice = customerService;
            this._projectService = projectService;
            this._FiscalyearsService = fiscalyears;
            _BranchesRepository = branchesRepository;

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
        }


        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts(string? SearchText)
        {
            List<AccountVM> resu =await _accountsService.GetAllAccounts(SearchText??"", _globalshared.Lang_G, _globalshared.BranchId_G);
            var GroupByMS = resu.OrderBy(s => s.Code);
            return Ok(GroupByMS);
        }

        [HttpGet("GetCustMainAccByBranchId")]
        public IActionResult GetCustMainAccByBranchId(int BranchId)
        {
            return Ok(_accountsService.GetCustMainAccByBranchId(BranchId));
        }
        [HttpGet("FillAccountSelect")]
        public IActionResult FillAccountSelect(int? param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string SelectStetment = "";
            if (param == 0)
            {
                SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where AccountCode =0  and ismain=0 and IsDeleted=0";
            }
            if (param == 1)
            {
                SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where AccountCode like '124%' and ismain=0 and IsDeleted=0";
            }
            else if (param == 2) // الصندوق
            {
                SelectStetment = "select accountid,accountcode+' '+ namear from Acc_Accounts where AccountCode like '121%' and ismain=0 and IsDeleted=0";
            }
            else if (param == 3)//حسابات بنكية
            {

                var Branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
                int? AccIDParent = 0;
                if (Branch == null || Branch.CheckInvoicesAccId == null)
                {
                    AccIDParent = 0;

                }
                else
                {
                    AccIDParent = Branch.CheckInvoicesAccId;
                }


                //SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where AccountCode like '122%' and ismain=0 and IsDeleted=0";
                SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where ParentId = " + AccIDParent + "and IsDeleted=0";

            }
            else if (param == 4)//عهدة
            {
                SelectStetment = "select accountid,accountcode+' '+ namear from Acc_Accounts where AccountCode like '126%' and ismain=0 and IsDeleted=0";
            }
            else if (param == 5)//خصم مكتسب
            {
                SelectStetment = "select accountid,accountcode+' '+ namear from Acc_Accounts where AccountCode like '4102%' and ismain=0 and IsDeleted=0";
            }
            else if (param == 6)//خسم مسموح
            {
                SelectStetment = "select accountid,accountcode+' '+ namear from Acc_Accounts where AccountCode like '5205%' and ismain=0 and IsDeleted=0";
            }
            else if (param == 7)
            {
                SelectStetment = "select CostCenterId,code+' '+ namear from Acc_CostCenters where code like '11%'";
            }
            var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment);
            return Ok(Accounts);
        }


        [HttpGet("FillAccountPayVoucherSelect")]

        public IActionResult FillAccountPayVoucherSelect(int? param)
        {
            //dawoud
            string SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where ParentId = " + param + "and IsDeleted=0";
            var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment);
            return Ok(Accounts);
        }
        [HttpGet("GetAllYearsDrop")]

        public IActionResult GetAllYearsDrop()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string SelectStetment = "";
            if (_globalshared.UserId_G == 1)
            {
                SelectStetment = "select distinct FiscalId as Id, CAST(fis.YearId AS nvarchar) + '-01' + '-01' + ' / ' + CAST(fis.YearId AS nvarchar) + '-12' + '-31' as Name,fis.YearId as YearId from Acc_FiscalYears fis where fis.IsDeleted=0";
            }
            else
            {
                SelectStetment = "select distinct FiscalId as Id, CAST(fis.YearId AS nvarchar) + '-01' + '-01' + ' / ' + CAST(fis.YearId AS nvarchar) + '-12' + '-31' as Name,fis.YearId as YearId from Acc_FiscalYears fis left join Acc_EmpFinYears empfin on fis.FiscalId = empfin.YearID where (IsNULL(fis.IsDeleted,1)=0) and ((fis.IsActive=1) or (fis.IsActive=0 and (IsNULL(empfin.IsDeleted,1)=0))) and ((empfin.EmpID)=" + _globalshared.UserId_G + " or (fis.IsActive=1))and ((empfin.BranchID)= " + _globalshared.BranchId_G + " or (fis.IsActive=1))";
            }

            var FiscalyearsPriv_M = _accountsService.FillYearsSelect(Con, SelectStetment);
            return Ok(FiscalyearsPriv_M);
        }

        [HttpGet("GetAllHirearchialAccounts")]
        public IActionResult GetAllHirearchialAccounts()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_accountsService.GetAllHirearchialAccounts(_globalshared.BranchId_G,_globalshared.Lang_G));
        }
        [HttpPost("SaveAccount")]

        public IActionResult SaveAccount(Accounts Account)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.SaveAccount(Account, _globalshared.UserId_G, _globalshared.BranchId_G, _globalshared.YearId_G);
            var result2 = _systemSettingsService.MaintenanceFunc(Con, _globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.UserId_G, 0);
            return Ok(result);
        }
        [HttpPost("DeleteAccount")]

        public IActionResult DeleteAccount(int AccountId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _accountsService.DeleteAccount(AccountId, _globalshared.UserId_G, _globalshared.BranchId_G);
         
            return Ok(result);
        }

        [HttpGet("GetAccountTreeIncome")]

        public IActionResult GetAccountTreeIncome()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            int? AccIDParent = 0;
            if (Branch == null || Branch.CustomersAccId == null){AccIDParent = 0;}
            else{AccIDParent = Branch.CustomersAccId; }
            var treeAccounts = _accountsService.GetAccountTreeIncome(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(treeAccounts);
        }

        [HttpGet("GetAccountTree")]

        public IActionResult GetAccountTree()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var treeAccounts = _accountsService.GetAccountTree(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(treeAccounts);
        }
        [HttpPost("SaveAccountTree")]

        public IActionResult SaveAccountTree(List<int> PrivIds)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.SaveAccountTree(PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G, Con);
            return Ok( result);
        }
        [HttpPost("SaveAccountTreeEA")]

        public IActionResult SaveAccountTreeEA(List<int> PrivIds)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.SaveAccountTreeEA(PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G, Con);
            return Ok(result);
        }

        [HttpPost("SaveAccountTreePublicRev")]
        public IActionResult SaveAccountTreePublicRev(List<int> PrivIds)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.SaveAccountTreePublicRev(PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }
        [HttpPost("SaveAccountTreeotherrev")]
        public IActionResult SaveAccountTreeotherrev(List<int> PrivIds)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.SaveAccountTreeotherrev(PrivIds, _globalshared.UserId_G, _globalshared.BranchId_G);
            return Ok(result);
        }


        [HttpGet("GetAccountTreeKD")]
        public IActionResult GetAccountTreeKD()
        {
            return Ok(_accountsService.GetAccountTreeKD());
        }
        [HttpGet("GetAccountTreeEA")]
        public IActionResult GetAccountTreeEA()
        {
            return Ok(_accountsService.GetAccountTreeEA());
        }

        [HttpGet("GetAccountTreeotherrev")]

        public IActionResult GetAccountTreeotherrev()
        {
            return Ok(_accountsService.GetAccountTreeotherrev() );
        }
        [HttpGet("GetAccountTreepublicrev")]
        public IActionResult GetAccountTreepublicrev()
        {
            return Ok(_accountsService.GetAccountTreepublicrev() );
        }
        [HttpGet("GetAccountById")]
        public IActionResult GetAccountById(int AccountId)
        {
            var Account = _accountsService.GetAccountById(AccountId);
            return Ok(Account );
        }
        [HttpGet("GetAccountByCode")]
        public IActionResult GetAccountByCode(string Code)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Account = _accountsService.GetAccountByCode(Code, _globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Account );
        }
        [HttpGet("CheckAccTreeExist")]
        public IActionResult CheckAccTreeExist(int ToBranchId)
        {
            return Ok(_accountsService.CheckAccTreeExist(ToBranchId) );
        }
        [HttpGet("TransferAccounts")]
        public IActionResult TransferAccounts(int ToBranchId)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.TransFerAccounts(_globalshared.BranchId_G, ToBranchId, _globalshared.UserId_G);
            return Ok(result );
        }
        [HttpGet("FillAccountsSelect")]
        public IActionResult FillAccountsSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            return Ok(Accounts );
        }
        [HttpGet("FillAccountsSelectCustomer")]
        public IActionResult FillAccountsSelectCustomer()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAllAccountsCustomerBranch("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            return Ok(Accounts );
        }
        [HttpGet("FillAccountIdAhlakSelect")]
        public IActionResult FillAccountIdAhlakSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(s => s.ParentId == 27).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr,
                IsMain = s.IsMain,
                ParentId = s.ParentId,
            });
            foreach (var acc in Accounts)
            {
                if (acc.IsMain == true)
                {
                    var subAccounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(s => s.ParentId == acc.Id).Select(s => new
                    {
                        Id = s.AccountId,
                        Name = s.Code + " - " + s.NameAr,
                        IsMain = s.IsMain,
                        ParentId = s.ParentId,
                    });
                    var SubAccounts = Accounts.Union(subAccounts);
                    Accounts = SubAccounts.Where(s => s.IsMain == false).ToList();
                }


            }
            return Ok(Accounts );


        }
        [HttpGet("FillAccountsSelectOpening")]
        public IActionResult FillAccountsSelectOpening()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAllAccountsOpening("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            return Ok(Accounts );
        }
        [HttpGet("FillAccountsSelect2")]
        public IActionResult FillAccountsSelect2()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAllAccounts2("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            return Ok(Accounts );
        }
        [HttpGet("FillAllAccountsSelect")]
        public IActionResult FillAllAccountsSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var Accounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, 0).Result.Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            foreach (var userBranch in userBranchs)
            {

                var subAccounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, userBranch.BranchId).Result.Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.NameAr
                });
                var SubAccounts = Accounts.Union(subAccounts);
                Accounts = SubAccounts.ToList();
            }
            return Ok(Accounts );
        }
        [HttpGet("FillAllAccountsSelect_Revenue")]
        public IActionResult FillAllAccountsSelect_Revenue()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var Accounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, 0).Result.Where(s => s.Classification == 10).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            foreach (var userBranch in userBranchs)
            {
                var subAccounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, userBranch.BranchId).Result.Where(s => s.Classification == 10).Select(s => new
                {
                    Id = s.AccountId,
                    Name = s.Code + " - " + s.NameAr
                });
                var SubAccounts = Accounts.Union(subAccounts);
                Accounts = SubAccounts.ToList();
            }
            return Ok(Accounts );
        }
        [HttpGet("FillAllAccountCodesSelect")]
        public IActionResult FillAllAccountCodesSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Select(s => new
            {
                Id = s.Code,
                Name = s.Code + " - " + s.NameAr
            });
            return Ok(Accounts );
        }
        [HttpGet("FillAccountsSelectByBranchId")]
        public IActionResult FillAccountsSelectByBranchId(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, param).Result.Where(t => t.IsMain == false).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            return Ok(Accounts );
        }
        [HttpGet("FillGuranteesAccountsSelect")]
        public IActionResult FillGuranteesAccountsSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var Accounts = _accountsService.GetAllAccounts("",_globalshared.Lang_G, _globalshared.BranchId_G).Result.Where(t => t.IsMain == false && (t.Classification == 6 || t.Classification == 1)).Select(s => new
            {
                Id = s.AccountId,
                Name = s.Code + " - " + s.NameAr
            });
            return Ok(Accounts );
        }
        [HttpGet("FillCustAccountsSelect")]
        public IActionResult FillCustAccountsSelect(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string SelectStetment = "";
            if (param == 9)
            {
                var Branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
                int? AccIDParent = 0;
                if (Branch == null || Branch.CheckInvoicesAccId == null)
                {
                    AccIDParent = 0;

                }
                else
                {
                    AccIDParent = Branch.CheckInvoicesAccId;
                }


                //SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where AccountCode like '122%' and ismain=0 and IsDeleted=0";
                SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where ParentId = " + AccIDParent + "and IsDeleted=0";
                var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment);
                return Ok(Accounts );
            }
            else
            {
                var Accounts = _accountsService.FillCustAccountsSelect(_globalshared.Lang_G, _globalshared.BranchId_G, param);
                return Ok(Accounts );
            }

        }
        [HttpGet("FillServiceAccount")]
        public IActionResult FillServiceAccount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string SelectStetment = "";
            var Branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            int? AccIDParent = 0;
            if (Branch == null || Branch.ContractsAccId == null)
            {
                AccIDParent = 0;

            }
            else
            {
                AccIDParent = Branch.ContractsAccId;
            }
            SelectStetment = "select accountid,accountcode+' '+ namear  from  Acc_Accounts where IsMain=0 and IsDeleted=0 and (ParentId = " + AccIDParent + " or AccountId = " + AccIDParent + ") ";
            var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment).Result;
            var AccountNew = _accountsService.FillAccountSelect2(AccIDParent ?? 0);
            foreach (var accLoop in AccountNew)
            {
                if(accLoop.ChildAccounts!=null)
                {
                    foreach (var accLoop2 in accLoop.ChildAccounts)
                    {
                        var SelectStetment2 = "select accountid,accountcode+' '+ namear  from  Acc_Accounts where IsMain=0 and IsDeleted=0 and (ParentId = " + accLoop2.AccountId + " or AccountId = " + accLoop2.AccountId + ") ";
                        var Accounts2 = _accountsService.FillAccountSelect(Con, SelectStetment2).Result;
                        Accounts = Accounts.Union(Accounts2);
                    }
                }

            }

            return Ok(Accounts );

        }
        [HttpGet("FillServiceAccountPurchase")]
        public IActionResult FillServiceAccountPurchase()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string SelectStetment = "";
            //int? AsoloParent = _accountsService.GetAccountByClassificationParent(15).AccountId;
            int AsoloParent = 3; //أصول ثابتة

            SelectStetment = "select accountid,accountcode+' '+ namear,IsMain from  Acc_Accounts where IsDeleted=0 and (ParentId = " + AsoloParent + " or AccountId = " + AsoloParent + ") ";
            var AccountsAsolo = _accountsService.FillAccountSelectPurchase(Con, SelectStetment).Result;
            var AccountNewAsolo = _accountsService.FillAccountSelect2(AsoloParent);
            foreach (var accLoop in AccountNewAsolo)
            {
                foreach (var accLoop2 in accLoop.ChildAccounts)
                {
                    var SelectStetment2 = "select accountid,accountcode+' '+ namear,IsMain from  Acc_Accounts where  IsDeleted=0 and (ParentId = " + accLoop2.AccountId + " or AccountId = " + accLoop2.AccountId + ") ";
                    var Accounts2Asolo = _accountsService.FillAccountSelectPurchase(Con, SelectStetment2).Result;
                    AccountsAsolo = AccountsAsolo.Union(Accounts2Asolo);
                }
            }



            int? MsrofParent = _accountsService.GetAccountByClassificationParent(19).Result.AccountId;

            SelectStetment = "select accountid,accountcode+' '+ namear,IsMain from  Acc_Accounts where IsDeleted=0 and (ParentId = " + MsrofParent + " or AccountId = " + MsrofParent + ") ";
            var AccountsMsrof = _accountsService.FillAccountSelectPurchase(Con, SelectStetment).Result;
            var AccountNewMsrof = _accountsService.FillAccountSelect2(MsrofParent ?? 0);
            foreach (var accLoop in AccountNewMsrof)
            {
                foreach (var accLoop2 in accLoop.ChildAccounts)
                {
                    var SelectStetment2 = "select accountid,accountcode+' '+ namear,IsMain from  Acc_Accounts where  IsDeleted=0 and (ParentId = " + accLoop2.AccountId + " or AccountId = " + accLoop2.AccountId + ") ";
                    var Accounts2Msrof = _accountsService.FillAccountSelectPurchase(Con, SelectStetment2).Result;
                    AccountsMsrof = AccountsMsrof.Union(Accounts2Msrof);
                }
            }
            AccountsAsolo = AccountsAsolo.Union(AccountsMsrof);
            return Ok(AccountsAsolo );

        }

        [HttpGet("FillCustAccountsSelect2")]
        public IActionResult FillCustAccountsSelect2(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            string SelectStetment = "";

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where AccountId =0 and IsDeleted=0";
            var SomeAccounts = _accountsService.FillAccountSelect(Con, SelectStetment).Result;
            foreach (var userBranch in userBranchs)
            {

                if (param == 9)
                {
                    var Branch = _branchesService.GetBranchById(userBranch.BranchId).Result;
                    int? AccIDParent = 0;
                    if (Branch == null || Branch.CheckInvoicesAccId == null)
                    {
                        AccIDParent = 0;

                    }
                    else
                    {
                        AccIDParent = Branch.CheckInvoicesAccId;
                    }

                    SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where(AccountId = " + AccIDParent + " or ParentId = " + AccIDParent + ") and IsMain=0 and IsDeleted=0";

                    var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment).Result;
                    var Accountst = SomeAccounts.Union(Accounts);
                    SomeAccounts = Accountst.ToList();
                    //return Ok(Accounts );
                }
                else if (param == 6)
                {
                    var Branch = _branchesService.GetBranchById(userBranch.BranchId).Result;
                    int? AccIDParent = 0;
                    if (Branch == null || Branch.CheckInvoicesAccId == null)
                    {
                        AccIDParent = 0;

                    }
                    else
                    {
                        AccIDParent = Branch.CheckInvoicesAccId;
                    }

                    SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where (AccountId = " + AccIDParent + " or ParentId = " + AccIDParent + ") and IsMain=0 and IsDeleted=0";

                    var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment).Result;
                    var Accountst = SomeAccounts.Union(Accounts);
                    SomeAccounts = Accountst.ToList();
                    //return Ok(Accounts );
                }
                else if (param == 17)
                {
                    var Branch = _branchesService.GetBranchById(userBranch.BranchId).Result;
                    int? AccIDParent = 0;
                    if (Branch == null || Branch.BoxAccId2 == null)
                    {
                        AccIDParent = 0;

                    }
                    else
                    {
                        AccIDParent = Branch.BoxAccId2;
                    }

                    SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where (AccountId = " + AccIDParent + " or ParentId = " + AccIDParent + ") and IsMain=0 and IsDeleted=0";

                    var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment).Result;
                    var Accountst = SomeAccounts.Union(Accounts);
                    SomeAccounts = Accountst.ToList();

                }
                else if (param == 7)
                {
                    SelectStetment = "select CostCenterId,code+' '+ namear from Acc_CostCenters where code like '11%'";
                    var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment).Result;
                    var Accountst = SomeAccounts.Union(Accounts);
                    SomeAccounts = Accountst.ToList();
                    //return Ok(Accounts );
                }
                else
                {
                    var Accounts = _accountsService.FillCustAccountsSelect2(_globalshared.Lang_G, userBranch.BranchId, param).Result;
                    var Accountst = SomeAccounts.Union(Accounts);
                    SomeAccounts = Accountst.ToList();
                    //return Ok(Accounts );
                }
            }

            return Ok(SomeAccounts );

        }
        [HttpGet("FillSuppAccountsSelect")]
        public IActionResult FillSuppAccountsSelect(int param)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            //dawoud
            string SelectStetment = "";
            if (param == 8)
            {
                var Branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
                int? AccIDParent = 0;
                if (Branch == null || Branch.SuppliersAccId == null)
                {
                    AccIDParent = 0;

                }
                else
                {
                    AccIDParent = Branch.SuppliersAccId;
                }

                SelectStetment = "select accountid,accountcode+' '+ namear from  Acc_Accounts where ParentId = " + AccIDParent + " and IsDeleted=0";
                var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment);
                return Ok(Accounts );
            }
            else
            {
                var Accounts = _accountsService.FillCustAccountsSelect(_globalshared.Lang_G, _globalshared.BranchId_G, param);
                return Ok(Accounts );
            } 

        }

        [HttpGet("FillSubAccountLoad")]
        public IActionResult FillSubAccountLoad()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.FillSubAccountLoad(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Accounts );
        }
        [HttpGet("FillSubAccountLoad_Branch")]
        public IActionResult FillSubAccountLoad_Branch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SelectStetment = "";
            SelectStetment = "select  AccountId,AccountCode+'-'+NameAr from Acc_Accounts where IsDeleted=0 and (BranchId in(select branchid from Sys_UserBranches where userid=" + _globalshared.UserId_G + ") or BranchId in(select branchid from Sys_Users where userid=" + _globalshared.UserId_G + ")) union  select  AccountId,AccountCode+'-'+NameAr from Acc_Accounts where IsDeleted=0 and  ParentId in (select AccountId from V_ParentAccountBranches where (branchid in(select branchid from Sys_UserBranches where userid=" + _globalshared.UserId_G + ") or BranchId in(select branchid from Sys_Users where userid=" + _globalshared.UserId_G + ")))  union   select  AccountId,AccountCode+'-'+NameAr from Acc_Accounts where ParentId not in (select AccountId from V_ParentAccountBranches) and IsDeleted=0";

            var Accounts = _accountsService.FillAccountSelect(Con, SelectStetment);
            
            return Ok(Accounts);
        }
        [HttpGet("FillSubAccountLoadNotMain_Branch")]
        public IActionResult FillSubAccountLoadNotMain_Branch()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var SelectStetment = "";
            SelectStetment = "select  AccountId,AccountCode+'-'+NameAr,Classification from Acc_Accounts where IsDeleted=0 and IsMain=0  and (BranchId in(select branchid from Sys_UserBranches where userid=" + _globalshared.UserId_G + ") or BranchId in(select branchid from Sys_Users where userid=" + _globalshared.UserId_G + ")) union  select  AccountId,AccountCode+'-'+NameAr,Classification from Acc_Accounts where IsDeleted=0 and ParentId in (select AccountId from V_ParentAccountBranches where (branchid in(select branchid from Sys_UserBranches where userid=" + _globalshared.UserId_G + ") or BranchId in(select branchid from Sys_Users where userid=" + _globalshared.UserId_G + ")))  union   select  AccountId,AccountCode+'-'+NameAr,Classification from Acc_Accounts where ParentId not in (select AccountId from V_ParentAccountBranches) and IsDeleted=0 and IsMain=0";
            var Accounts = _accountsService.FillAccountNewSelect(Con, SelectStetment);
            return Ok(Accounts);
        }
        [HttpGet("GetAccCodeFormID")]
        public IActionResult GetAccCodeFormID(int AccID)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.GetAccCodeFormID(AccID,_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Accounts );
        }

        [HttpGet("FillEmpAccountsSelect")]
        public IActionResult FillEmpAccountsSelect()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Accounts = _accountsService.FillEmpAccountsSelect(_globalshared.Lang_G, _globalshared.BranchId_G);
            return Ok(Accounts );
        }
        [HttpGet("GetAllAccountsTransactions")]
        public IActionResult GetAllAccountsTransactions(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_accountsService.GetAllAccountsTransactions(FromDate, ToDate,_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.YearId_G) );
        }
        [HttpGet("GetAccountsByType")]
        public IActionResult GetAccountsByType(string accountName)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_accountsService.GetAccountsByType(accountName,_globalshared.Lang_G) );
        }

        [HttpGet("GetAllAccountsTransactionsByAccType")]
        public IActionResult GetAllAccountsTransactionsByAccType(int AccType, string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_accountsService.GetAllAccountsTransactionsByAccType(AccType, FromDate, ToDate,_globalshared.Lang_G, _globalshared.BranchId_G, _globalshared.YearId_G) );
        }
        [HttpGet("GetAllSubAccounts")]
        public IActionResult GetAllSubAccounts(string? SearchText, int Type)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var userBranchs = _branchesService.GetAllBranchesByUserId(_globalshared.Lang_G, _globalshared.UserId_G).Result;
            var someAccount = _accountsService.GetAllSubAccounts(SearchText??"",_globalshared.Lang_G, 0).Result;
            foreach (var userBranch in userBranchs)
            {

                var AllSubAccounts = _accountsService.GetAllSubAccounts(SearchText??"",_globalshared.Lang_G, userBranch.BranchId).Result;
                var Account = someAccount.Union(AllSubAccounts);
                someAccount = Account;
            }

            return Ok(someAccount );

        }

        #region Reports
        [HttpGet("GetAccountStatment")]
        public IActionResult GetAccountStatment(int AccountId, int? CostCenterId, string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.GetAccountSatement(_globalshared.BranchId_G,_globalshared.Lang_G, AccountId, CostCenterId ?? 0, FromDate, ToDate, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpGet("GetAssistantAccByAccCode")]
        public IActionResult GetAssistantAccByAccCode(string AccountCode, string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Acc = _accountsService.GetAccountByCode(AccountCode,_globalshared.Lang_G, _globalshared.BranchId_G).Result;
            if (Acc != null)
            {
                var result = _accountsService.GetAccountSatement(_globalshared.BranchId_G,_globalshared.Lang_G, Acc.AccountId, 0, FromDate, ToDate, _globalshared.YearId_G);
                return Ok(result );
            }
            else
            {
                return Ok(null );
            }
        }
        [HttpGet("GetGeneralBudget")]
        public IActionResult GetGeneralBudget(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var budget = _accountsService.GetGeneralBudget(_globalshared.BranchId_G,_globalshared.Lang_G, FromDate, ToDate, _globalshared.YearId_G);
            return Ok(budget );
        }
        [HttpGet("GetGeneralBudgetDGV")]

        public IActionResult GetGeneralBudgetDGV(string FromDate, string ToDate)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var budget = _accountsService.GetGeneralBudgetDGV(_globalshared.BranchId_G, _globalshared.Lang_G, FromDate, ToDate, Con, _globalshared.YearId_G);
            return Ok(budget );
        }
        [HttpGet("GetGeneralLedger")]

        public IActionResult GetGeneralLedger(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            var result = _accountsService.GetGeneralLedger(_globalshared.BranchId_G, _globalshared.Lang_G, FromDate, ToDate, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpGet("GetGeneralLedgerDGV")]

        public IActionResult GetGeneralLedgerDGV(string FromDate, string ToDate)
        {
            var result = _accountsService.GetGeneralLedgerDGV(_globalshared.BranchId_G, _globalshared.Lang_G, FromDate, ToDate, Con, _globalshared.BranchId_G);
            return Ok(result );
        }
        [HttpGet("GetTrailBalanceDGV_old")]

        public IActionResult GetTrailBalanceDGV_old(string FromDate, string ToDate, string CostCenter, int ZeroCheck, string AccountCode, string LVL)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int costID = Convert.ToInt32(CostCenter);
            var result = _accountsService.GetTrailBalanceDGVNew_old(FromDate, ToDate, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck, AccountCode, LVL);
            return Ok(result );
        }
        [HttpGet("GetTrailBalanceDGV2_old")]

        public IActionResult GetTrailBalanceDGV2_old(string FromDate, string ToDate, string CostCenter, int ZeroCheck, string AccountCode, string LVL)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int costID = Convert.ToInt32(CostCenter);
            var result = _accountsService.GetTrailBalanceDGVNew2_old(FromDate, ToDate, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck, AccountCode, LVL);
            return Ok(result );
        }
        [HttpPost("GetTrailBalanceDGV")]
        public IActionResult GetTrailBalanceDGV([FromForm]string? FromDate, [FromForm] string? ToDate, [FromForm] string? CostCenter, [FromForm] bool? isCheckedYear, [FromForm] int? ZeroCheck, [FromForm] string? AccountCode, [FromForm] string LVL, [FromForm] int FilteringType, [FromForm] string? FilteringTypeStr, [FromForm] string AccountIds)
        {                     
            int costID = Convert.ToInt32(CostCenter??"0");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int YearIDCheck = _globalshared.YearId_G;
            if (isCheckedYear == true) YearIDCheck = 0;

            var result = _accountsService.GetTrailBalanceDGVNew(FromDate??"", ToDate??"", costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, YearIDCheck, ZeroCheck??0, AccountCode ?? "", LVL, FilteringType, FilteringTypeStr??"", AccountIds);
            return Ok(result );
        }
        [HttpPost("GetGeneralBudgetAMRDGVNew")]
        public IActionResult GetGeneralBudgetAMRDGVNew([FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string? CostCenter, [FromForm] bool? isCheckedYear, [FromForm] bool? isCheckedBranch, [FromForm] int? ZeroCheck, [FromForm] string? AccountCode, [FromForm] string LVL, [FromForm] int FilteringType, [FromForm] string? FilteringTypeStr, [FromForm] string AccountIds)
        {
            int costID = Convert.ToInt32(CostCenter ?? "0");
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int YearIDCheck = _globalshared.YearId_G;
            if (isCheckedYear == true) YearIDCheck = 0;
            int BranchIDCheck = _globalshared.BranchId_G;
            if (isCheckedBranch == false) BranchIDCheck = 0;

            var result = _accountsService.GetGeneralBudgetAMRDGVNew(FromDate ?? "", ToDate ?? "", costID, BranchIDCheck, _globalshared.Lang_G, Con, YearIDCheck, ZeroCheck ?? 0, AccountCode ?? "", LVL, FilteringType, FilteringTypeStr ?? "", AccountIds);
            return Ok(result);
        }
        [HttpGet("GetTrailBalanceDGV2")]

        public IActionResult GetTrailBalanceDGV2(string FromDate, string ToDate, string CostCenter, int ZeroCheck, string AccountCode, string LVL, int FilteringType, string FilteringTypeStr, string AccountIds)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            int costID = Convert.ToInt32(CostCenter);
            var result = _accountsService.GetTrailBalanceDGVNew2(FromDate, ToDate, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck, AccountCode, LVL, FilteringType, FilteringTypeStr, AccountIds);
            return Ok(result );
        }
        [HttpGet("GetDetailsMonitor")]

        public IActionResult GetDetailsMonitor(string? FromDate, string? ToDate, string? CostCenter, bool? isCheckedYear, int FilteringType, string FilteringTypeStr, int AccountId, int Type, int Type2)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int YearIDCheck = _globalshared.YearId_G;
            if (isCheckedYear == true) YearIDCheck = 0;
            int? costID = string.IsNullOrEmpty(CostCenter) ? (int?)null : Convert.ToInt32(CostCenter);

            var result = _accountsService.GetDetailsMonitor(FromDate??"", ToDate??"", costID??0, _globalshared.BranchId_G,_globalshared.Lang_G, Con, YearIDCheck, FilteringType, FilteringTypeStr, AccountId, Type, Type2);
            return Ok(result );
        }
        [HttpGet("GetIncomeStatmentDGV")]

        public IActionResult GetIncomeStatmentDGV(string FromDate, string ToDate, string CostCenter, int ZeroCheck, string LVL)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int costID = Convert.ToInt32(CostCenter);
            var result = _accountsService.GetIncomeStatmentDGVNew(FromDate, ToDate, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck, LVL);
            return Ok(result );

        }
        [HttpPost("GetIncomeStatmentDGVLevels")]

        public IActionResult GetIncomeStatmentDGVLevels([FromForm]string? FromDate,[FromForm] string? ToDate, [FromForm] string? CostCenter, [FromForm] int? ZeroCheck, [FromForm] string LVL, [FromForm] int FilteringType, [FromForm] int FilteringTypeAll, [FromForm] string? FilteringTypeStr, [FromForm] string? FilteringTypeAllStr, [FromForm] string? AccountIds, [FromForm] int PeriodFillterType, [FromForm] int PeriodCounter, [FromForm] int TypeF)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            int costID = Convert.ToInt32(CostCenter??"0");
            var result = _accountsService.GetIncomeStatmentDGVLevels(FromDate??"", ToDate??"", costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck??0, LVL, FilteringType, FilteringTypeAll, FilteringTypeStr??"", FilteringTypeAllStr??"", AccountIds??"", PeriodFillterType, PeriodCounter, TypeF);
            return Ok(result );

        }

        [HttpGet("GetIncomeStatmentDGVLevelsdetails")]

        public IActionResult GetIncomeStatmentDGVLevelsdetails(int AccointId, int Type, int Type2, string FromDate, string ToDate, string CostCenter, int ZeroCheck, string LVL, int FilteringType, int FilteringTypeAll, string FilteringTypeStr, string FilteringTypeAllStr, string AccountIds, int PeriodFillterType, int PeriodCounter, int TypeF)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int costID = Convert.ToInt32(CostCenter);
            var result = _accountsService.GetIncomeStatmentDGVLevelsdetails(AccointId, Type, Type2, FromDate, ToDate, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck, LVL, FilteringType, FilteringTypeAll, FilteringTypeStr, FilteringTypeAllStr, AccountIds, PeriodFillterType, PeriodCounter, TypeF);
            return Ok(result );

        }
        [HttpGet("GetAllIncomeStatmentDGVNew")]

        public IActionResult GetAllIncomeStatmentDGVNew(string FromDate, string ToDate, string CostCenter, int ZeroCheck, string LVL)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            int costID = Convert.ToInt32(CostCenter);

            var result = _accountsService.GetAllIncomeStatmentDGVNew(FromDate, ToDate, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck, LVL);
            return Ok(result );

        }
        [HttpPost("GetGeneralBudgetAMRDGV")]

        public IActionResult GetGeneralBudgetAMRDGV([FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string lvl, [FromForm] string? CCID, [FromForm] int ZeroCheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            int costID = Convert.ToInt32(CCID??"0");
            var result = _accountsService.GetGeneralBudgetAMRDGV(FromDate??"", ToDate??"", lvl, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck);
            return Ok(result );
        }
        [HttpGet("GetGeneralBudgetFRENCHDGV")]

        public async Task<DataTable> GetGeneralBudgetFRENCHDGV(string FromDate, string ToDate, string lvl, string CCID, int ZeroCheck)
        {
            
             
            int costID = Convert.ToInt32(CCID);
            return await _accountsService.GetGeneralBudgetFRENCHDGV(FromDate, ToDate, lvl, costID, _globalshared.BranchId_G,_globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck);
        }
        [HttpPost("GetGeneralManagerRevenueAMRDGV")]

        public IActionResult GetGeneralManagerRevenueAMRDGV([FromForm]string? ManagerId, [FromForm] string? FromDate, [FromForm] string? ToDate)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int managerId = Convert.ToInt32(ManagerId);//(int? ManagerId,string FromDate, string ToDate,  int BranchId,  string Con, int? yearid);
            var result = _accountsService.GetGeneralManagerRevenueAMRDGV(managerId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpGet("GetClosingVouchers")]

        public IActionResult GetClosingVouchers()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var result = _accountsService.GetClosingVouchers(_globalshared.BranchId_G, Con, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpPost("GetCostCenterEX_RE")]

        public IActionResult GetCostCenterEX_RE([FromForm]int? CostCenterId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string? FlagTotal)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.GetCostCenterEX_RE(CostCenterId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G, FlagTotal);
            return Ok(result );
        }
        [HttpPost("GetDetailedRevenu")]

        public IActionResult GetDetailedRevenu([FromForm]string? CustomerId, [FromForm] string? FromDate, [FromForm] string? ToDate)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int? customerId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);
            var result = _accountsService.GetDetailedRevenu(customerId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpPost("GetDetailedRevenuExtra")]

        public IActionResult GetDetailedRevenuExtra([FromForm] string? CustomerId, [FromForm] string? ProjectId, [FromForm] string? FromDate, [FromForm] string? ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            int? customerId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);
            int? projectId = string.IsNullOrEmpty(ProjectId) ? (int?)null : Convert.ToInt32(ProjectId);

            var result = _accountsService.GetDetailedRevenuExtra(customerId, projectId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpPost("GetInvoicedue")]

        public IActionResult GetInvoicedue([FromForm] int? CustomerId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] int? BranchId)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            int? customerId = CustomerId==null?0 : Convert.ToInt32(CustomerId);
            int? branchId = BranchId == null ? 0 : Convert.ToInt32(BranchId);

            var result = _accountsService.GetInvoicedue(customerId??0, FromDate ?? "", ToDate ?? "", branchId??0, Con, _globalshared.YearId_G);
            return Ok(result);
        }

        [HttpGet("GetDetailedRevenuCount")]

        public double GetDetailedRevenuCount()
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.GetDetailedRevenu(null, "", "", _globalshared.BranchId_G, Con, _globalshared.YearId_G);
            return result.Result.Sum(item => Convert.ToDouble(item.TotalValue));
        }
        [HttpGet("GetBoxNetCount")]

        public double GetBoxNetCount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            var Branch = _BranchesRepository.GetById(_globalshared.BranchId_G);
            if (Branch == null || Branch.BoxAccId == null)
            {
                return Convert.ToDouble(0);
            }
            string SelectStetment = "";
            SelectStetment = "select Sum((Depit)-(Credit)) as NetValue from Acc_Transactions where Type<>12 and IsDeleted=0 and BranchId='" + _globalshared.BranchId_G + "' and AccountId in (select a.AccountId from Acc_Accounts a left join Acc_Accounts b on a.ParentId=b.AccountId where a.IsDeleted=0 and (b.AccountId='" + Branch.BoxAccId + "' or b.ParentId='" + Branch.BoxAccId + "' or a.AccountId='" + Branch.BoxAccId + "' or a.ParentId='" + Branch.BoxAccId + "' )) and YearId='" + _globalshared.YearId_G + "' and IsPost=1";
            var result = _accountsService.GetNetValue(Con, SelectStetment).Result.ToList();
            System.Reflection.PropertyInfo pi = result[0].GetType().GetProperty("NetValue");
            string value = (string)(pi.GetValue(result[0], null));
            if (value == "") value = "0";
            return Convert.ToDouble(value ?? "0");
        }
        [HttpGet("GetBankNetCount")]

        public double GetBankNetCount()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var Branch = _BranchesRepository.GetById(_globalshared.BranchId_G);
            if (Branch == null || Branch.CheckInvoicesAccId == null)
            {
                return Convert.ToDouble(0);
            }
            string SelectStetment = "";
            SelectStetment = "select Sum((Depit)-(Credit)) as NetValue from Acc_Transactions where Type<>12 and IsDeleted=0 and BranchId='" + _globalshared.BranchId_G + "' and AccountId in (select a.AccountId from Acc_Accounts a left join Acc_Accounts b on a.ParentId=b.AccountId where a.IsDeleted=0 and (b.AccountId='" + Branch.CheckInvoicesAccId + "' or b.ParentId='" + Branch.CheckInvoicesAccId + "' or a.AccountId='" + Branch.CheckInvoicesAccId + "' or a.ParentId='" + Branch.CheckInvoicesAccId + "' )) and YearId='" + _globalshared.YearId_G + "' and IsPost=1";

            var result = _accountsService.GetNetValue(Con, SelectStetment).Result.ToList();
            System.Reflection.PropertyInfo pi = result[0].GetType().GetProperty("NetValue");
            string value = (string)(pi.GetValue(result[0], null));
            if(value=="") value= "0";

            return Convert.ToDouble(value??"0");
        }


        [HttpPost("GetDetailedExpensesd")]

        public IActionResult GetDetailedExpensesd([FromForm]int? AccountId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string? ExpenseType)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            if (ExpenseType == "2")
            {
                var result = _accountsService.GetDetailedExpensesd(AccountId, FromDate??"", ToDate??"", "1", _globalshared.BranchId_G, Con, _globalshared.YearId_G).Result.Where(x => x.Taxes != "" && x.Taxes != "0");
                return Ok(result );

            }
            else if (ExpenseType == "3")
            {
                var result = _accountsService.GetDetailedExpensesd(AccountId, FromDate??"", ToDate ?? "", "1", _globalshared.BranchId_G, Con, _globalshared.YearId_G).Result.Where(x => x.Taxes == "" || x.Taxes == "0");
                return Ok(result );

            }
            else
            {
                var result = _accountsService.GetDetailedExpensesd(AccountId, FromDate ?? "", ToDate ?? "", "1", _globalshared.BranchId_G, Con, _globalshared.YearId_G).Result;
                return Ok(result );

            }
        }

        [HttpGet("GetDetailedExpensesdCount")]

        public double GetDetailedExpensesdCount()
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.GetDetailedExpensesd(null, "", "", "", _globalshared.BranchId_G, Con, _globalshared.YearId_G);
            return result.Result.Sum(item => Convert.ToDouble(item.Price));
        }
        [HttpPost("GetCustomerFinancialDetailsByFilter")]
        
        public ActionResult GetCustomerFinancialDetailsByFilter([FromForm] string? CustomerId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] int ZeroCheck)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int? CustId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);
            var result = _accountsService.GetCustomerFinancialDetailsByFilter(CustId, FromDate??"", ToDate??"", _globalshared.BranchId_G,_globalshared.Lang_G, _globalshared.YearId_G, ZeroCheck);
            return Ok(result );
        }
        [HttpPost("GetCustomerFinancialDetailsNew")]

        public ActionResult GetCustomerFinancialDetailsNew([FromForm] string? CustomerId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] int ZeroCheck, [FromForm] bool? isCheckedYear)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int YearIDCheck = _globalshared.YearId_G;
            if (isCheckedYear == true) YearIDCheck = 0;
            var LVL = "10"; var costID = 0; var FilteringType = 4; var AccountIds = ""; var AccountCode = "0";
            var FilteringTypeStr = _globalshared.BranchId_G.ToString(); 
            int? CustId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);
            if(CustId==null)
            {
                var MainCustomerAccount = _BranchesRepository.GetById(_globalshared.BranchId_G).CustomersAccId;
                if (MainCustomerAccount != null){AccountIds = MainCustomerAccount.ToString();}
                else{return Ok(new List<TrainBalanceVM>());}
            }   
            else
            {
                AccountIds = _customerservice.GetCustomersByCustomerId(CustId ?? 0, _globalshared.Lang_G).Result.AccountId.ToString();
            }
            var result = _accountsService.GetTrailBalanceDGVNew(FromDate ?? "", ToDate ?? "", costID, _globalshared.BranchId_G, _globalshared.Lang_G, Con, YearIDCheck, ZeroCheck, AccountCode ?? "", LVL, FilteringType, FilteringTypeStr ?? "", AccountIds);
            return Ok(result);
        }
        [HttpPost("PrintCustomerFinancialDetailsNew")]
        public IActionResult PrintVoucher([FromForm] string? CustomerId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] int ZeroCheck, [FromForm] bool? isCheckedYear)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int YearIDCheck = _globalshared.YearId_G;
            if (isCheckedYear == true) YearIDCheck = 0;
            CustomerFinancialPrintVM _customerFinancialPrintVM = new CustomerFinancialPrintVM();

            var LVL = "10"; var costID = 0; var FilteringType = 4; var AccountIds = ""; var AccountCode = "0";
            var FilteringTypeStr = _globalshared.BranchId_G.ToString();
            int? CustId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);
            if (CustId == null)
            {
                var MainCustomerAccount = _BranchesRepository.GetById(_globalshared.BranchId_G).CustomersAccId;
                if (MainCustomerAccount != null) { AccountIds = MainCustomerAccount.ToString(); }
                else { return Ok(_customerFinancialPrintVM); }
            }
            else
            {
                AccountIds = _customerservice.GetCustomersByCustomerId(CustId ?? 0, _globalshared.Lang_G).Result.AccountId.ToString();
            }
            var result = _accountsService.GetTrailBalanceDGVNew(FromDate ?? "", ToDate ?? "", costID, _globalshared.BranchId_G, _globalshared.Lang_G, Con, YearIDCheck, ZeroCheck, AccountCode ?? "", LVL, FilteringType, FilteringTypeStr ?? "", AccountIds).Result.ToList();

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };


            _customerFinancialPrintVM.result = result;
            _customerFinancialPrintVM.StartDate = FromDate;
            _customerFinancialPrintVM.EndDate = ToDate;
            _customerFinancialPrintVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _customerFinancialPrintVM.Org_VD = objOrganization2;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _customerFinancialPrintVM.BranchName = branch.NameAr;
            return Ok(_customerFinancialPrintVM);
        }

        //ahmed atia
        [HttpGet("GetAccountStatmentDGV")]

        public IActionResult GetAccountStatmentDGV(string FromDate, string ToDate, string AccountCode, string CCID)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.GetFullAccountStatmentDGV(FromDate, ToDate, AccountCode, CCID, Con, _globalshared.BranchId_G, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpGet("GetNewCodeByParentId")]

        public IActionResult GetNewCodeByParentId(int ParentId)
        {
            var code = _accountsService.GetNewCodeByParentId(ParentId,0);

            return Ok(code);
        }

        [HttpGet("GetCustomerFinancialDetails")]

        public IActionResult GetCustomerFinancialDetails()
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            var result = _accountsService.GetCustomerFinancialDetails(_globalshared.BranchId_G,_globalshared.Lang_G, _globalshared.YearId_G);
            return Ok(result );
        }
        [HttpGet("GetAccTransByAccType")]

        public IActionResult GetAccTransByAccType(int Type, string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            return Ok(_accountsService.GetAccsTransByType(Type, _globalshared.BranchId_G,_globalshared.Lang_G, FromDate, ToDate, _globalshared.YearId_G) );
        }
        [HttpGet("GetAccProfitAndLosses")]

        public IActionResult GetAccProfitAndLosses(string FromDate, string ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);


            return Ok(_accountsService.GetAccsProfitAndLosses(_globalshared.BranchId_G,_globalshared.Lang_G, FromDate, ToDate, _globalshared.YearId_G) );
        }

        [HttpGet("GetAccountSearchValue")]

        public IActionResult GetAccountSearchValue(string searchName)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_accountsService.GetAccountSearchValue(searchName, _globalshared.BranchId_G) );
        }
        [HttpGet("GetAllAccount")]

        public IActionResult GetAllAccount()
        {
            return Ok(_accountsService.GetAllAccount(_globalshared.BranchId_G) );
        }
        [HttpGet("GetAllAccountBySearch")]

        public IActionResult GetAllAccountBySearch(AccountVM Account)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);

            return Ok(_accountsService.GetAllAccountBySearch(Account, _globalshared.BranchId_G) );
        }
        #endregion


        [HttpPost("DetailedRevenuReportNew")]
        public IActionResult DetailedRevenuReportNew([FromForm]int? customerId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string[] Sortedlist)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            RevenuReportVM _revenuReportVM = new RevenuReportVM();
            //int? customerId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);
            var result = _accountsService.GetDetailedRevenu(customerId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G).Result;

            string s = Sortedlist[0];
            string[] values = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> vale = new List<string>();
            List<DetailedRevenuVM> _resultVM = new List<DetailedRevenuVM>();
            foreach (var item in values)
            {
                string Intitem = string.Empty;
                for (int i = 0; i < item.Length; i++)
                {
                    if (Char.IsDigit(item[i]))
                        Intitem += item[i];
                }
                vale.Add(Intitem);
            }
            int GridLength = 0;
            GridLength = result.Count();
            for (int i = 0; i < GridLength; i++)
            {
                _resultVM.Add(result.Where(d => Convert.ToInt32(d.TransactionId) == Convert.ToInt32(vale[i])).FirstOrDefault());
            }
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = {_globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
                objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            _revenuReportVM.Result = result.ToList();
            _revenuReportVM.StartDate = FromDate;
            _revenuReportVM.EndDate = ToDate;

            _revenuReportVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _revenuReportVM.Org_VD = objOrganization2;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _revenuReportVM.BranchName = branch.NameAr;
            return Ok(_revenuReportVM);

        }
        [HttpPost("CostCenterEX_REReportNew")]

        public IActionResult CostCenterEX_REReportNew([FromForm] string? CostCenterId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string FlagTotal, [FromForm] string[] Sortedlist)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            CostCenterEX_REVMReportVM _costCenterEX_REVMReportVM = new CostCenterEX_REVMReportVM();
            int? costCenterId = string.IsNullOrEmpty(CostCenterId) ? (int?)null : Convert.ToInt32(CostCenterId);

            var result = _accountsService.GetCostCenterEX_RE(costCenterId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G, FlagTotal).Result;

            string s = Sortedlist[0];
            string[] values = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> vale = new List<string>();
            List<CostCenterEX_REVM> _resultVM = new List<CostCenterEX_REVM>();
            foreach (var item in values)
            {
                string Intitem = string.Empty;
                for (int i = 0; i < item.Length; i++)
                {
                    if (Char.IsDigit(item[i]))
                        Intitem += item[i];
                }
                vale.Add(Intitem);
            }
            int GridLength = 0;
            GridLength = result.Count();
            for (int i = 0; i < GridLength - 1; i++)
            {
                _resultVM.Add(result.Where(d => Convert.ToInt32(d.CostCenterId) == Convert.ToInt32(vale[i])).FirstOrDefault());
            }
            _resultVM.Add(result.Where(a => a.Flag == "1").FirstOrDefault());

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = {_globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
                objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            _costCenterEX_REVMReportVM.Result = _resultVM.ToList();
            _costCenterEX_REVMReportVM.StartDate = FromDate;
            _costCenterEX_REVMReportVM.EndDate = ToDate;

            _costCenterEX_REVMReportVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _costCenterEX_REVMReportVM.Org_VD = objOrganization2;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _costCenterEX_REVMReportVM.BranchName = branch.NameAr;
            return Ok(_costCenterEX_REVMReportVM);

        }

        [HttpPost("DetailedRevenuReportExtra")]

        public IActionResult DetailedRevenuReportExtra([FromForm] string? CustomerId, [FromForm] string? ProjectId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string? ProjectNo)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            RevenuReportVM _revenuReportVM = new RevenuReportVM();


            int? customerId = string.IsNullOrEmpty(CustomerId) ? (int?)null : Convert.ToInt32(CustomerId);
            int? projectId = string.IsNullOrEmpty(ProjectId) ? (int?)null : Convert.ToInt32(ProjectId);

            if (customerId == 0) customerId = null;
            if (projectId == 0) projectId = null;

            var result = _accountsService.GetDetailedRevenuExtra(customerId, projectId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G).Result;
            //List<DetailedRevenuVM> res = result.ToList();
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = {_globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
                objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            string custName = "";
            string ProjectN = "";
            var cust = _customerservice.GetCustomersByCustId(customerId).Result.FirstOrDefault();
            var pro = new ProjectVM();
            if (projectId!=null && projectId != 0)
            {
                pro = _projectService.GetProjectById(_globalshared.Lang_G, projectId ?? 0).Result;
            }
            if (cust != null)
            {
                custName = cust.CustomerName;

            }
            if (pro != null)
            {
                ProjectN = pro.ProjectNo;

            }

            _revenuReportVM.Result = result.ToList();
            _revenuReportVM.custName = custName;
            _revenuReportVM.ProjectNo = ProjectN;
            _revenuReportVM.StartDate = FromDate;
            _revenuReportVM.EndDate = ToDate;

            _revenuReportVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _revenuReportVM.Org_VD = objOrganization2;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _revenuReportVM.BranchName = branch.NameAr;
            return Ok(_revenuReportVM);
        }
        [HttpPost("DetailedExpensesdReportGrid")]

        public IActionResult DetailedExpensesdReportGrid([FromForm] string? AccountId, [FromForm] string? FromDate, [FromForm] string? ToDate, [FromForm] string ExpenseType, [FromForm] string[] Sortedlist)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            DetailedExpensesdtVMReportVM _detailedExpensesdtVMReportVM = new DetailedExpensesdtVMReportVM();
            int? accountId = string.IsNullOrEmpty(AccountId) ? (int?)null : Convert.ToInt32(AccountId);
            var result = _accountsService.GetDetailedExpensesd(accountId, FromDate??"", ToDate??"", ExpenseType, _globalshared.BranchId_G, Con, _globalshared.YearId_G).Result;
            string s = Sortedlist[0];
            string[] values = s.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> vale = new List<string>();
            List<DetailedExpenseVM> _DetailedExpenseVM = new List<DetailedExpenseVM>();
            foreach (var item in values)
            {
                string Intitem = string.Empty;
                for (int i = 0; i < item.Length; i++)
                {
                    if (Char.IsDigit(item[i]))
                        Intitem += item[i];
                }
                vale.Add(Intitem);
            }
            for (int i = 0; i < result.Count(); i++)
            {
                _DetailedExpenseVM.Add(result.Where(d => d.TransactionId == vale[i]).FirstOrDefault());
            }
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = {_globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email,
                objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };


            _detailedExpensesdtVMReportVM.Result = _DetailedExpenseVM.ToList();
            _detailedExpensesdtVMReportVM.StartDate = FromDate;
            _detailedExpensesdtVMReportVM.EndDate = ToDate;

            _detailedExpensesdtVMReportVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _detailedExpensesdtVMReportVM.Org_VD = objOrganization2;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _detailedExpensesdtVMReportVM.BranchName = branch.NameAr;
            return Ok(_detailedExpensesdtVMReportVM);

        }

        [HttpPost("GetReportOfGeneralBudget")]

        public IActionResult GetReportOfGeneralBudget([FromForm]string? FromDate, [FromForm] string? ToDate, [FromForm] string? CCID, [FromForm] int? ZeroCheck, [FromForm] string? LVL, [FromForm] string? AccountCode, [FromForm] string? typeOfReport)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ManagerRevenueReport _managerRevenueReport = new ManagerRevenueReport();

            int costID = Convert.ToInt32(CCID);

            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            var result2 = _accountsService.GetGeneralBudgetAMRDGV(FromDate??"", ToDate??"", LVL, costID, _globalshared.BranchId_G, _globalshared.Lang_G, Con, _globalshared.YearId_G, ZeroCheck??0).Result.ToList();

            _managerRevenueReport.result2 = result2;
            _managerRevenueReport.StartDate = FromDate;
            _managerRevenueReport.EndDate = ToDate;
            _managerRevenueReport.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _managerRevenueReport.BranchName = branch.NameAr;
            _managerRevenueReport.Org_VD = objOrganization2;
            return Ok(_managerRevenueReport);

        }

        [HttpPost("PrintProjectManagerRevenueReport")]

        public IActionResult PrintProjectManagerRevenueReport([FromForm]int? ManagerId, [FromForm] string? ManagerName, [FromForm] string? FromDate, [FromForm] string? ToDate)
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            ManagerRevenueReport _managerRevenueReport = new ManagerRevenueReport();
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;


            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            List<GeneralmanagerRevVM> GeneralmanagerRevVM = _accountsService.GetGeneralManagerRevenueAMRDGV(ManagerId, FromDate??"", ToDate??"", _globalshared.BranchId_G, Con, _globalshared.YearId_G).Result.ToList();

            _managerRevenueReport.result = GeneralmanagerRevVM;
            _managerRevenueReport.StartDate = FromDate;
            _managerRevenueReport.EndDate = ToDate;
            _managerRevenueReport.ManagerName = ManagerName;
            _managerRevenueReport.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _managerRevenueReport.Org_VD = objOrganization2;
            var branch = _branchesService.GetBranchById(_globalshared.BranchId_G).Result;
            _managerRevenueReport.BranchName = branch.NameAr;
            return Ok(_managerRevenueReport);
        }


        [HttpGet("PrintTrewView")]

        public IActionResult PrintTrewView()
        {
            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            TreeReportVM _treeReportVM = new TreeReportVM();
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;

            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };

            DataTable TreeView = _accountsService.TreeView(Con).Result;


            List<TreeVMDatatable> VMList = new List<TreeVMDatatable>();
            for (int i = 0; i < TreeView.Rows.Count; i++)
            {
                TreeVMDatatable TreeVM = new TreeVMDatatable();
                TreeVM.AccountCode = TreeView.Rows[i]["AccountCode"].ToString();
                TreeVM.AccountLevel = TreeView.Rows[i]["AccountLevel"].ToString();
                TreeVM.Level1 = TreeView.Rows[i]["Level1"].ToString();
                TreeVM.Level2 = TreeView.Rows[i]["Level2"].ToString();
                TreeVM.Level3 = TreeView.Rows[i]["Level3"].ToString();
                TreeVM.Level4 = TreeView.Rows[i]["Level4"].ToString();
                TreeVM.Level5 = TreeView.Rows[i]["Level5"].ToString();
                TreeVM.Level6 = TreeView.Rows[i]["Level6"].ToString();
                TreeVM.Level7 = TreeView.Rows[i]["Level7"].ToString();
                VMList.Add(TreeVM);
            }
            _treeReportVM.TreeViewVM = VMList;
            string Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
            _treeReportVM.DateTimeNow = Date;

            var objBranch = _branchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
            _treeReportVM.DateTimeNow = Date;
            _treeReportVM.Branch = objBranch;

            var objOrganization2 = _organizationsservice.GetBranchOrganizationDataInvoice(orgId).Result;
            _treeReportVM.Org_VD = objOrganization2;
            return Ok(_treeReportVM);
        }
        [HttpPost("ChangeTrialBalance_PDF")]

        public IActionResult ChangeTrialBalance_PDF([FromForm]string? FromDate, [FromForm] string? ToDate, [FromForm] string? CCID, [FromForm] bool? isCheckedYear, [FromForm] string? Zerocheck, [FromForm] string? AccountCode, [FromForm] string? LVL, [FromForm] string? typeOfReport, [FromForm] int? FilteringType, [FromForm] string? FilteringTypeStr, [FromForm] string? AccountIds)
        {

            HttpContext httpContext = HttpContext; _globalshared = new GlobalShared(httpContext);
            int YearIDCheck = _globalshared.YearId_G;
            if (isCheckedYear == true) YearIDCheck = 0;
            TrialBalance_PDFVM _trialBalance_PDFVM = new TrialBalance_PDFVM();
            int costID = Convert.ToInt32(CCID == "" ? "0" : CCID);
            int ZerocheckValue = Convert.ToInt32(Zerocheck == "" ? "0" : Zerocheck);
            var result = _accountsService.GetTrailBalanceDGVNew(FromDate??"", ToDate??"", costID,_globalshared.BranchId_G, _globalshared.Lang_G, Con, YearIDCheck, ZerocheckValue, AccountCode ?? "", LVL??"", FilteringType??0, FilteringTypeStr??"", AccountIds??"").Result.ToList();
            int orgId = _branchesService.GetOrganizationId(_globalshared.BranchId_G).Result;
            var objOrganization = _organizationsservice.GetBranchOrganizationData(orgId).Result;
            string[] infoDoneTasksReport = { _globalshared.Lang_G == "en" ? objOrganization.NameEn : objOrganization.NameAr, objOrganization.LogoUrl, objOrganization.Address, objOrganization.Email, objOrganization.Fax, objOrganization.Mobile, objOrganization.IsFooter, objOrganization.WebSite, objOrganization.TaxCode };
            _trialBalance_PDFVM.Org_VD = objOrganization;

            var costCenterNam = "";
            if (costID != 0)
            {

                costCenterNam = _CostCenterservice.GetCostCenterById(costID).Result.NameAr;
            }
            else
            {
                costCenterNam = "";
            }
            var filtrtyp = Get_filtertype(FilteringType??0);
            _trialBalance_PDFVM.filtertypename = filtrtyp;


            var objBranch = _branchesService.GetBranchByBranchId(_globalshared.Lang_G, _globalshared.BranchId_G).Result.FirstOrDefault();
            var OrgIsRequired = _systemSettingsService.GetSystemSettingsByBranchId(_globalshared.BranchId_G).Result.OrgDataIsRequired;



            _trialBalance_PDFVM.costCenterNam = costCenterNam;
            _trialBalance_PDFVM.Branch_VD = objBranch;
            _trialBalance_PDFVM.TrainBalanceVM_VD = result;

            _trialBalance_PDFVM.OrgIsRequired_VD = OrgIsRequired;
            _trialBalance_PDFVM.TempCheck = typeOfReport;
            _trialBalance_PDFVM.InfoDoneTasksReport = infoDoneTasksReport;
            _trialBalance_PDFVM.DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en")); ;
            _trialBalance_PDFVM.StartDate = FromDate;
            _trialBalance_PDFVM.EndDate = ToDate;

            return Ok(_trialBalance_PDFVM);
        }
        [HttpGet("Get_filtertype")]

        public string Get_filtertype(int filtertype)
        {
            if (filtertype == 1)
            {
                return "المشاريع";
            }
            else if (filtertype == 2)
            {
                return "العملاء";
            }
            else if (filtertype == 3)
            {
                return "الموردين";
            }
            else if (filtertype == 4)
            {
                return "الفروع";
            }
            else if (filtertype == 5)
            {
                return "الموظفين";
            }
            else if (filtertype == 6)
            {
                return "الخدمات";
            }
            else
            {
                return "";
            }
        }
    }
}
