
using Microsoft.EntityFrameworkCore;
using TaamerProject.Models.DBContext;
using Microsoft.Extensions.DependencyInjection; 
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.Interfaces;
using TaamerProject.Service.Generic;
using TaamerProject.Service.IGeneric;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaamerProject.Service.Services;
using IUsersService = TaamerProject.Service.Interfaces.IUsersService;
using UsersService = TaamerProject.Service.Services.UsersService;
using Bayanatech.TameerPro.Repository;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using TaamerProject.Service.Services.EmpServices;
using TaamerProject.Models;
using TaamerProject.Service.Services.ProjServices;
using TaamerProject.Service.Services.SysServices;
using Microsoft.AspNetCore.Authorization;
using TaamerProject.Service.Services.AccServices;
using Microsoft.AspNetCore.Identity;

namespace TaamerProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy(
            //    name: "AllowOrigin",
            //    builder => {
            //        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //    });
            //});

            //------------------------------------------------------------------------

            //var allowSpecificOrigins = "two_factor_auth_cors";
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy(allowSpecificOrigins,
            //        builder =>
            //          builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});

            var allowSpecificOrigins = "two_factor_auth_cors";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(allowSpecificOrigins,

                    builder => builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Require2FA", policy =>
                    policy.RequireClaim("mfa", "true"));
            });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
            })
            .AddEntityFrameworkStores<TaamerProjectContext>()
            .AddDefaultTokenProviders();


            //------------------------------------------------------------------------

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // builder.Services.InjectServiceandRepository();
            builder.Services.AddDbContext<TaamerProjectContext>();

            //builder.Services.AddScoped<IUsersService, UsersService>();

            //builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            //builder.Services.AddScoped<IAcc_CategoriesRepository, Acc_CategoriesRepository>();

            #region Services
            builder.Services.AddTransient<ISystemAction, SystemAction>();

            builder.Services.AddTransient<IUsersService, UsersService>();
            builder.Services.AddTransient<ISys_UserLoginService, Sys_UserLoginService>();
            builder.Services.AddTransient<IAcc_CategoriesService, Acc_CategoriesService>();
            builder.Services.AddTransient<IAcc_CategorTypeService, Acc_CategorTypeService>();
            builder.Services.AddTransient<IAcc_ClausesService, Acc_ClausesService>();
            builder.Services.AddTransient<IAcc_InvoicesRequestsService, Acc_InvoicesRequestsService>();

            builder.Services.AddTransient<IFiscalyearsService, FiscalyearsService>();
            builder.Services.AddTransient<IUserPrivilegesRepository, UserPrivilegesRepository>();
            builder.Services.AddTransient<IAcc_EmpFinYearsService, Acc_EmpFinYearsService>();
            builder.Services.AddTransient<IAcc_FloorsService, Acc_FloorsService>();
            builder.Services.AddTransient<IAcc_PackagesService, Acc_PackagesService>();
            builder.Services.AddTransient<IAcc_SuppliersService, Acc_SuppliersService>();
            builder.Services.AddTransient<IAcc_TotalSpacesRangeService, Acc_TotalSpacesRangeService>();
            builder.Services.AddTransient<IAccountsService, AccountsService>();
            builder.Services.AddTransient<IAllowanceService, AllowanceService>();
            builder.Services.AddTransient<IAllowanceTypeService, AllowanceTypeService>();
            builder.Services.AddTransient<IAppraisalService, AppraisalService>();
            builder.Services.AddTransient<IArchiveFilesService, ArchiveFilesService>();
            builder.Services.AddTransient<ICustodyService, CustodyService>();
            builder.Services.AddTransient<ICustomerFilesService, CustomerFilesService>();
            builder.Services.AddTransient<ICustomerMailService, CustomerMailService>();
            builder.Services.AddTransient<ICustomerPaymentsService,  CustomerPaymentsService>();
            builder.Services.AddTransient<ICustomerSMSService, CustomerSMSService>();
            builder.Services.AddTransient<IDiscountRewardService, DiscountRewardService>();
            builder.Services.AddTransient<IDraftService, DraftService>();
            builder.Services.AddTransient<IAttAbsentDayService, AttAbsentDayService>();
            builder.Services.AddTransient<IAttDeviceService, AttDeviceSettingService>();
            builder.Services.AddTransient<IAttendaceTimeService, AttendaceTimeService>();
            builder.Services.AddTransient<IAttendeesService, AttendeesService>();
            builder.Services.AddTransient<IAttendenceDeviceService, AttendenceDeviceService>();
            builder.Services.AddTransient<IAttendenceService, AttendenceService>();
            builder.Services.AddTransient<IAttTimeDetailsService, AttTimeDetailsService>();
            builder.Services.AddTransient<ICustodyService, CustodyService>();
            builder.Services.AddTransient<IDeviceAttService, DeviceAttService>();
            builder.Services.AddTransient<IDraftDetailsService, DraftDetailsService>();
            builder.Services.AddTransient<IDrafts_TemplatesService, Drafts_TemplatesService>();
            builder.Services.AddTransient<IDropBoxService, DropBoxService>();

            builder.Services.AddTransient<IEmpContractDetailsService, EmpContractDetailsService>();
            builder.Services.AddTransient<IEmpContractService, EmpContractService>();
            builder.Services.AddTransient<IEmployeesService, EmployeeService>();
            builder.Services.AddTransient<IEmpStructureService, EmpStructureService>();
            builder.Services.AddTransient<IExpensesGovernmentService, ExpensesGovernmentService>();
            builder.Services.AddTransient<IExpensesGovernmentTypeService, ExpensesGovernmentTypeService>();
            builder.Services.AddTransient<IExpRevenuExpensesService, ExpRevenuExpensesService>();
            builder.Services.AddTransient<IExternalEmployeesService, ExternalEmployeesService>();
            builder.Services.AddTransient<IOfficalHolidayService, OfficalHolidayService>();
            builder.Services.AddTransient<IOfficialDocumentsService, OfficialDocumentsService>();
            builder.Services.AddTransient<IOneDriveService, OneDriveService>();
            builder.Services.AddTransient<IOutInBoxSerialService, OutInBoxSerialService>();
            builder.Services.AddTransient<IOutInBoxService, OutInBoxService>();
            builder.Services.AddTransient<IOutInBoxTypeService, OutInBoxTypeService>();
            builder.Services.AddTransient<IOutMovementsService, OutMovementsService>();
            builder.Services.AddTransient<IPayrollMarchesService, PayrollMarchesService>();
            builder.Services.AddTransient<IVacationService, VacationService>();
            builder.Services.AddTransient<IVacationTypeService, VacationTypeService>();
            ///////////////////////////////////////////////////////////////////////////////////
            builder.Services.AddTransient<IAttachmentService, AttachmentService>();
            builder.Services.AddTransient<IDependencySettingsService, DependencySettingsService>();
            builder.Services.AddTransient<IFilesAuthService, FilesAuthService>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IFileTypeService, FileTypeService>();
            builder.Services.AddTransient<IFollowProjService, FollowProjService>();
            builder.Services.AddTransient<IFullProjectsReportService, FullProjectsReportService>();
            builder.Services.AddTransient<IGoogleDriveService, GoogleDriveService>();

            builder.Services.AddTransient<IImportantProjectService, ImportantProjectService>();
            builder.Services.AddTransient<IInstrumentsService, InstrumentsService>();
            builder.Services.AddTransient<IInstrumentSourcesService, InstrumentSourcesService>();
            builder.Services.AddTransient<IItemService, ItemService>();
            builder.Services.AddTransient<IItemTypeService, ItemTypeService>();
            builder.Services.AddTransient<IModelRequirementsService, ModelRequirementsService>();
            builder.Services.AddTransient<IModelService, ModelService>();
            builder.Services.AddTransient<IModelTypeService, ModelTypeService>();
            builder.Services.AddTransient<IPro_MunicipalService, Pro_MunicipalService>();
            builder.Services.AddTransient<IPro_SubMunicipalityService, Pro_SubMunicipalityService>();
            builder.Services.AddTransient<IPro_Super_PhasesService, Pro_Super_PhasesService>();
            builder.Services.AddTransient<IPro_SuperContractorService, Pro_SuperContractorService>();
            builder.Services.AddTransient<IPro_SupervisionDetailsService, Pro_SupervisionDetailsService>();
            builder.Services.AddTransient<IProjectArchivesSeeService, ProjectArchivesSeeService>();
            builder.Services.AddTransient<IProjectArchivesReService, ProjectArchivesReService>();
            builder.Services.AddTransient<IProjectCommentsService, ProjectCommentsService>();
            builder.Services.AddTransient<IProjectExtractsService, ProjectExtractsService>();
            builder.Services.AddTransient<IProjectPhasesTasksService, ProjectPhasesTasksService>();
            builder.Services.AddTransient<IProjectPiecesService, ProjectPiecesService>();
            builder.Services.AddTransient<IProjectRequirementsGoalsService, ProjectRequirementsGoalsService>();
            builder.Services.AddTransient<IProjectRequirementsService, ProjectRequirementsService>();
            builder.Services.AddTransient<IProjectService, ProjectService>();
            builder.Services.AddTransient<IProjectSubTypesService, ProjectSubTypeService>();
            builder.Services.AddTransient<IPro_projectsReasonsService, Pro_projectsReasonsService>();
            builder.Services.AddTransient<IPro_tasksReasonsService, Pro_tasksReasonsService>();

            builder.Services.AddTransient<IPro_DestinationsService, Pro_DestinationsService>();
            builder.Services.AddTransient<IPro_filesReasonsService, Pro_filesReasonsService>();
            builder.Services.AddTransient<IPro_DestinationDepartmentsService, Pro_DestinationDepartmentsService>();
            builder.Services.AddTransient<IPro_DestinationTypesService, Pro_DestinationTypesService>();

            builder.Services.AddTransient<IProjectTrailingService, ProjectTrailingService>();
            builder.Services.AddTransient<IProjectTypeService, ProjectTypeService>();
            builder.Services.AddTransient<IProjectWorkersService, ProjectWorkersService>();
            builder.Services.AddTransient<IProSettingDetailsService, ProSettingDetailsService>();
            builder.Services.AddTransient<IProUserPrivilegesService, ProUserPrivilegesService>();
            builder.Services.AddTransient<IPro_ProjectStepsService, Pro_ProjectStepsService>();
            builder.Services.AddTransient<IPro_StepDetailsService, Pro_StepDetailsService>();
            builder.Services.AddTransient<IPro_ProjectAchievementsService, Pro_ProjectAchievementsService>();
            builder.Services.AddTransient<IPro_ProjectChallengesService, Pro_ProjectChallengesService>();
            builder.Services.AddTransient<IReasonLeaveService, ReasonLeaveService>();
            builder.Services.AddTransient<IRequirementsandGoalsService, RequirementsandGoalsService>();
            builder.Services.AddTransient<IRequirementsService, RequirementsService>();
            builder.Services.AddTransient<ISupervisionsService, SupervisionsService>();
            builder.Services.AddTransient<ITasksDependencyService, TasksDependencyService>();
            builder.Services.AddTransient<ITaskTypeService, TaskTypeService>();
            builder.Services.AddTransient<ITemplatesService, TemplatesService>();
            builder.Services.AddTransient<ITemplatesTasksService, TemplatesTasksService>();
            builder.Services.AddTransient<ITimeOutRequestsService, TimeOutRequestsService>();
            builder.Services.AddTransient<ITrailingFilesService, TrailingFilesService>();
            builder.Services.AddTransient<IWorkOrdersService, WorkOrdersService>();
            builder.Services.AddTransient<IBackupAlertService, BackupAlertService>();
            builder.Services.AddTransient<IBanksService, BanksService>();
            builder.Services.AddTransient<IBranchesService, BranchesService>();
            builder.Services.AddTransient<IBuildTypesService, BuildTypeService>();
            builder.Services.AddTransient<IChattingLogService, ChattingLogService>();
            builder.Services.AddTransient<ICityPassService, CityPassService>();
            builder.Services.AddTransient<ICityService, CityService>();
            builder.Services.AddTransient<IDatabaseBackupService, DatabaseBackupService>();
            builder.Services.AddTransient<IDepartmentService, DepartmentService>();
            builder.Services.AddTransient<IEmailSettingService, EmailSettingService>();
            builder.Services.AddTransient<IGroupPrivilegeService, GroupPrivilegeService>();
            builder.Services.AddTransient<IGroupsService, GroupsService>();
            builder.Services.AddTransient<IGuideDepartmentDetailsService, GuideDepartmentDetailsService>();
            builder.Services.AddTransient<IGuideDepartmentsService, GuideDepartmentsService>();
            builder.Services.AddTransient<IHomeService, HomeService>();
            builder.Services.AddTransient<IJobService, JobService>();
            builder.Services.AddTransient<ILicencesService, LicencesService>();
            builder.Services.AddTransient<INationalityService, NationalityService>();
            builder.Services.AddTransient<INewsService, NewsService>();
            builder.Services.AddTransient<INotificationService, NotificationService>();
            builder.Services.AddTransient<INotificationSettingsService, NotificationSettingsService>();
            builder.Services.AddTransient<IOrganizationsService, OrganizationsService>();
            builder.Services.AddTransient<IPrivFollowersServices, PrivFollowersServices>();
            builder.Services.AddTransient<IPrivilegesService, PrivilegesService>();
            builder.Services.AddTransient<IRegionTypesService, RegionTypesService>();
            builder.Services.AddTransient<ISettingsService, SettingsService>();
            builder.Services.AddTransient<ISMSProviderService, SMSProviderService>();
            builder.Services.AddTransient<ISMSSettingsService, SMSSettingsService>();
            builder.Services.AddTransient<IWhatsAppSettingsService, WhatsAppSettingsService>();

            builder.Services.AddTransient<ISocialMediaLinksService, SocialMediaLinksService>();
            builder.Services.AddTransient<ISupportRequestsService, SupportRequestsService>();
            builder.Services.AddTransient<ISys_SystemActionsService, Sys_SystemActionsService>();
            builder.Services.AddTransient<ISystemSettingsService, SystemSettingsService>();
            builder.Services.AddTransient<IUserMailsService, UserMailsService>();
            builder.Services.AddTransient<IUserNotificationPrivilegesService, UserNotificationPrivilegesService>();
            builder.Services.AddTransient<IUsersLocationsService, UsersLocationsService>();
            builder.Services.AddTransient<IUsersService, UsersService>();
            builder.Services.AddTransient<IVersionService, VersionService>();
            builder.Services.AddTransient<ICarMovementsService, CarMovementsService>();
            builder.Services.AddTransient<ICarMovementsTypeService, CarMovementsTypeService>();
            builder.Services.AddTransient<IChecksService, ChecksService>();
            builder.Services.AddTransient<IContacFilesService, ContacFilesService>();
            builder.Services.AddTransient<IContractServicesService, ContractServicesService>();
            builder.Services.AddTransient<IContractService, ContractService>();
            builder.Services.AddTransient<IContractStageService, ContractStageService>();
            builder.Services.AddTransient<ICostCenterService, CostCenterService>();
            builder.Services.AddTransient<ICurrencyService, CurrencyService>();
            builder.Services.AddTransient<IFiscalyearsService, FiscalyearsService>();
            builder.Services.AddTransient<IGuranteesService, GuranteesService>();
            builder.Services.AddTransient<IInvoicesService, InvoicesService>();
            builder.Services.AddTransient<IJournalsService, JournalsService>();
            builder.Services.AddTransient<ILoanService, LoanService>();
            builder.Services.AddTransient<IOfferpriceconditionService, OfferConditionsService>();
            builder.Services.AddTransient<IOfferserviceService, OfferserviceService>();
            builder.Services.AddTransient<IOffersPricesService,OffersPricesService>();
            builder.Services.AddTransient<IServiceService, ServiceService>();
            builder.Services.AddTransient<IServicesPriceService, ServicesPriceService>();
            builder.Services.AddTransient<IServicesPriceOfferService, ServicesPriceOfferService>();

            builder.Services.AddTransient<IServicesPricingFormService, ServicesPricingFormService>();
            builder.Services.AddTransient<ITransactionsService, TransactionsService>();
            builder.Services.AddTransient<ITransactionTypesService, TransactionTypesService>();
            builder.Services.AddTransient<IVoucherService, VoucherService>();
            builder.Services.AddTransient<IVoucherSettingsService, VoucherSettingsService>();
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IContact_BranchesService, Contact_BranchesService>();
            builder.Services.AddTransient<IContractDetailsService, ContractDetailsService>();
            builder.Services.AddTransient<IGuide_QuestionsAnswersService,Guide_QuestionsAnswersService>();
            builder.Services.AddTransient<ILaw_regulationsService, Law_regulationsService>();
            builder.Services.AddTransient<IContactListsService, ContactListsService>();
            builder.Services.AddTransient<IAttendenceLocationSettingsService, AttendenceLocationSettingsService>();
            builder.Services.AddTransient<IPermissionService, PermissionService>();
            builder.Services.AddTransient<IPermissionTypeService, PermissionTypeService>();
            builder.Services.AddTransient<ICommercialActivityService, CommercialActivityService>();
            builder.Services.AddTransient<INotificationConfigurationService, NotificationConfigurationService>();
            

            #endregion
            #region Repository
            builder.Services.AddTransient<IUsersRepository, UsersRepository>();
            builder.Services.AddTransient<ISys_UserLoginRepository, Sys_UserLoginRepository>();
            builder.Services.AddTransient<IAcc_CategoriesRepository, Acc_CategoriesRepository>();
            builder.Services.AddTransient<IAcc_CategorTypeRepository, Acc_CategorTypeRepository>();
            builder.Services.AddTransient<IAcc_ClausesRepository, Acc_ClausesRepository>();
            builder.Services.AddTransient<IAcc_InvoicesRequestsRepository, Acc_InvoicesRequestsRepository>();

            builder.Services.AddTransient<IFiscalyearsRepository, FiscalyearsRepository>();
            builder.Services.AddTransient<IUserPrivilegesRepository, UserPrivilegesRepository>();
            builder.Services.AddTransient<IAcc_EmpFinYearsRepository, Acc_EmpFinYearsRepository>();
            builder.Services.AddTransient<IAcc_FloorsRepository, Acc_FloorsRepository>();
            builder.Services.AddTransient<IAcc_PackagesRepository, Acc_PackagesRepository>();
            builder.Services.AddTransient<IAcc_SuppliersRepository, Acc_SuppliersRepository>();
            builder.Services.AddTransient<IAcc_TotalSpacesRangeRepository, Acc_TotalSpacesRangeRepository>();
            builder.Services.AddTransient<IAccountsRepository, AccountsRepository>();
            builder.Services.AddTransient<IAllowanceRepository, AllowanceRepository>();
            builder.Services.AddTransient<IAllowanceTypeRepository, AllowanceTypeRepository>();
            builder.Services.AddTransient<IAppraisalRepository, AppraisalRepository>();
            builder.Services.AddTransient<IArchiveFilesRepository, ArchiveFilesRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<ICustomerFilesRepository, CustomerFilesRepository>();
            builder.Services.AddTransient<ICustomerMailRepository, CustomerMailRepository>();
            builder.Services.AddTransient<ICustomerPaymentsRepository, CustomerPaymentsRepository>();
            builder.Services.AddTransient<ICustomerSMSRepository, CustomerSMSRepository>();
            builder.Services.AddTransient<IDiscountRewardRepository, DiscountRewardRepository>();
            builder.Services.AddTransient<IDraftRepository, DraftRepository>();
            builder.Services.AddTransient<IAttAbsentDayRepository, AttAbsentDayRepository>();
            builder.Services.AddTransient<IAttDevicesettingRepository, AttDeviceSettingRepository>();
            builder.Services.AddTransient<IAttendaceTimeRepository, AttendaceTimeRepository>();
            builder.Services.AddTransient<IAttendeesRepository, AttendeesRepository>();
            builder.Services.AddTransient<IAttendenceDeviceRepository, AttendenceDeviceRepository>();
            builder.Services.AddTransient<IAttendenceRepository, AttendenceRepository>();
            builder.Services.AddTransient<IAttTimeDetailsRepository, AttTimeDetailsRepository>();
            builder.Services.AddTransient<ICustodyRepository, CustodyRepository>();
            builder.Services.AddTransient<IDeviceAttRepository, DeviceAttRepository>();
            builder.Services.AddTransient<IDraftDetailsRepository, DraftDetailsRepository>();
            builder.Services.AddTransient<IDrafts_TemplatesRepository, Drafts_TemplatesRepository>();
            builder.Services.AddTransient<IEmpContractDetailRepository, EmpContractDetailsRepository>();
            builder.Services.AddTransient<IEmpContractRepository, EmpContractRepository>();
            builder.Services.AddTransient<ILoanDetailsRepository, LoanDetailsRepository>();
            builder.Services.AddTransient<IEmpLocationsRepository, EmpLocationsRepository>();
            builder.Services.AddTransient<IEmployeesRepository, EmployeesRepository>();
            builder.Services.AddTransient<IEmpStructureRepository, EmpStructureRepository>();
            builder.Services.AddTransient<IExpensesGovernmentRepository, ExpensesGovernmentRepository>();
            builder.Services.AddTransient<IExpensesGovernmentTypeRepository, ExpensesGovernmentTypeRepository>();
            builder.Services.AddTransient<IExpRevenuExpensesRepository, ExpRevenuExpensesRepository>();
            builder.Services.AddTransient<IExternalEmployeesRepository, ExternalEmployeesRepository>();
            builder.Services.AddTransient<IOfficalHolidayRepository, OfficalHolidayRepository>();
            builder.Services.AddTransient<IOfficialDocumentsRepository, OfficialDocumentsRepository>();
            builder.Services.AddTransient<IOutInBoxSerialRepository, OutInBoxSerialRepository>();
            builder.Services.AddTransient<IOutInBoxRepository, OutInBoxRepository>();
            builder.Services.AddTransient<IOutInBoxTypeRepository, OutInBoxTypeRepository>();
            builder.Services.AddTransient<IOutMovementsRepository, OutMovementsRepository>();
            builder.Services.AddTransient<IPayrollMarchesRepository, PayrollMarchesRepository>();
            builder.Services.AddTransient<IVacationRepository, VacationRepository>();
            builder.Services.AddTransient<IVacationTypeRepository, VacationTypeRepository>();
            ////////////////////////////////////////////////////////////////////////////////////////////////
            
            builder.Services.AddTransient<IAttachmentRepository, AttachmentRepository>();
            builder.Services.AddTransient<IDependencySettingsRepository, DependencySettingsRepository>();
            builder.Services.AddTransient<IFilesAuthRepository, FilesAuthRepository>();
            builder.Services.AddTransient<IFileRepository, FileRepository>();
            builder.Services.AddTransient<IFileTypeRepository, FileTypeRepository>();
            builder.Services.AddTransient<IFollowProjRepository, FollowProjRepository>();
            builder.Services.AddTransient<IFullProjectsReportRepository, FullProjectsReportRepository>();
            builder.Services.AddTransient<IImportantProjectRepository, ImportantProjectRepository>();
            builder.Services.AddTransient<IInstrumentsRepository, InstrumentsRepository>();
            builder.Services.AddTransient<IInstrumentSourcesRepository, InstrumentSourcesRepository>();
            builder.Services.AddTransient<IItemRepository, ItemRepository>();
            builder.Services.AddTransient<IItemTypeRepository, ItemTypeRepository>();
            builder.Services.AddTransient<IModelRequirementsRepository, ModelRequirementsRepository>();
            builder.Services.AddTransient<IModelRepository, ModelRepository>();
            builder.Services.AddTransient<IModelTypeRepository, ModelTypeRepository>();
            builder.Services.AddTransient<IPro_MunicipalRepository, Pro_MunicipalRepository>();
            builder.Services.AddTransient<IPro_SubMunicipalityRepository, Pro_SubMunicipalityRepository>();
            builder.Services.AddTransient<IPro_Super_PhasesRepository, Pro_Super_PhasesRepository>();
            builder.Services.AddTransient<IPro_SuperContractorRepository, Pro_SuperContractorRepository>();
            builder.Services.AddTransient<IPro_SupervisionDetailsRepository, Pro_SupervisionDetailsRepository>();
            builder.Services.AddTransient<IProjectArchivesSeeRepository, ProjectArchivesSeeRepository>();
            builder.Services.AddTransient<IProjectArchivesReRepository, ProjectArchivesReRepository>();
            builder.Services.AddTransient<IProjectCommentsRepository, ProjectCommentsRepository>();
            builder.Services.AddTransient<IProjectExtractsRepository, ProjectExtractsRepository>();
            builder.Services.AddTransient<IProjectPhasesTasksRepository, ProjectPhasesTasksRepository>();
            builder.Services.AddTransient<IProjectPiecesRepository, ProjectPiecesRepository>();
            builder.Services.AddTransient<IProjectRequirementsGoalsRepository, ProjectRequirementsGoalsRepository>();
            builder.Services.AddTransient<IProjectRequirementsRepository, ProjectRequirementsRepository>();
            builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
            builder.Services.AddTransient<IProjectSubTypeRepository, ProjectSubTypeRepository>();
            builder.Services.AddTransient<IPro_projectsReasonsRepository, Pro_projectsReasonsRepository>();
            builder.Services.AddTransient<IPro_tasksReasonsRepository, Pro_tasksReasonsRepository>();

            builder.Services.AddTransient<IPro_DestinationsRepository, Pro_DestinationsRepository>();
            builder.Services.AddTransient<IPro_filesReasonsRepository, Pro_filesReasonsRepository>();
            builder.Services.AddTransient<IPro_DestinationDepartmentsRepository, Pro_DestinationDepartmentsRepository>();
            builder.Services.AddTransient<IPro_DestinationTypesRepository, Pro_DestinationTypesRepository>();

            builder.Services.AddTransient<IProjectTrailingRepository, ProjectTrailingRepository>();
            builder.Services.AddTransient<IProjectTypeRepository, ProjectTypeRepository>();
            builder.Services.AddTransient<IProjectWorkersRepository, ProjectWorkersRepository>();
            builder.Services.AddTransient<IProSettingDetailsRepository, ProSettingDetailsRepository>();
            builder.Services.AddTransient<IProSettingDetailsNewRepository, ProSettingDetailsNewRepository>();
            builder.Services.AddTransient<IProUserPrivilegesRepository, ProUserPrivilegesRepository>();
            builder.Services.AddTransient<IPro_ProjectStepsRepository, Pro_ProjectStepsRepository>();
            builder.Services.AddTransient<IPro_StepDetailsRepository, Pro_StepDetailsRepository>();
            builder.Services.AddTransient<IPro_ProjectAchievementsRepository, Pro_ProjectAchievementsRepository>();
            builder.Services.AddTransient<IPro_ProjectChallengesRepository, Pro_ProjectChallengesRepository>();


            builder.Services.AddTransient<IReasonLeaveRepository, ReasonLeaveRepository>();
            builder.Services.AddTransient<IRequirementsandGoalsRepository, RequirementsandGoalsRepository>();
            builder.Services.AddTransient<IRequirementsRepository, RequirementsRepository>();
            builder.Services.AddTransient<ISupervisionsRepository, SupervisionsRepository>();
            builder.Services.AddTransient<ITasksDependencyRepository, TasksDependencyRepository>();
            builder.Services.AddTransient<ITaskTypeRepository, TaskTypeRepository>();
            builder.Services.AddTransient<ITemplatesRepository, TemplatesRepository>();
            builder.Services.AddTransient<ITemplatesTasksRepository, TemplatesTasksRepository>();
            builder.Services.AddTransient<ITimeOutRequestsRepository, TimeOutRequestsRepository>();
            builder.Services.AddTransient<ITrailingFilesRepository, TrailingFilesRepository>();
            builder.Services.AddTransient<IWorkOrdersRepository, WorkOrdersRepository>();
            builder.Services.AddTransient<IBackupAlertRepository, BackupAlertRepository>();
            builder.Services.AddTransient<IBanksRepository, BanksRepository>();
            builder.Services.AddTransient<IBranchesRepository, BranchesRepository>();
            builder.Services.AddTransient<IBuildTypesRepository, BuildTypesRepository>();
            builder.Services.AddTransient<IChattingLogRepository, ChattingLogRepository>();
            builder.Services.AddTransient<ICityPassRepository, CityPassRepository>();
            builder.Services.AddTransient<ICityRepository, CityRepository>();
            builder.Services.AddTransient<IDatabaseBackupRepository, DatabaseBackupRepository>();
            builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddTransient<IEmailSettingRepository, EmailSettingRepository>();
            builder.Services.AddTransient<IGroupPrivilegesRepository, GroupPrivilegesRepository>();
            builder.Services.AddTransient<IGroupsRepository, GroupsRepository>();
            builder.Services.AddTransient<IGuideDepartmentDetailsRepository, GuideDepartmentDetailsRepository>();
            builder.Services.AddTransient<IGuideDepartmentsRepository, GuideDepartmentsRepository>();
            builder.Services.AddTransient<IJobRepository, JobRepository>();
            builder.Services.AddTransient<ILicencesRepository, LicencesRepository>();
            builder.Services.AddTransient<INationalityRepository, NationalityRepository>();
            builder.Services.AddTransient<INewsRepository, NewsRepository>();
            builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
            builder.Services.AddTransient<INotificationSettingsRepository, NotificationSettingsRepository>();
            builder.Services.AddTransient<IOrganizationsRepository, OrganizationsRepository>();
            builder.Services.AddTransient<IPrivFollowersServices, PrivFollowersServices>();
            builder.Services.AddTransient<IPrivilegesRepository, PrivilegesRepository>();
            builder.Services.AddTransient<IRegionTypesRepository, RegionTypesRepository>();
            builder.Services.AddTransient<ISettingsRepository, SettingsRepository>();
            builder.Services.AddTransient<ISMSSettingsRepository, SMSSettingRepository>();
            builder.Services.AddTransient<IWhatsAppSettingsRepository, WhatsAppSettingsRepository>();
            builder.Services.AddTransient<ISocialMediaLinksRepository, SocialMediaLinksRepository>();
            builder.Services.AddTransient<ISupportRequestsReplayRepository, SupportRequestsReplayRepository>();
            builder.Services.AddTransient<ISupportResquestsRepository, SupportResquestsRepository>();
            builder.Services.AddTransient<ISys_SystemActionsRepository, Sys_SystemActionsRepository>();
            builder.Services.AddTransient<ISystemSettingsRepository, SystemSettingsRepository>();
            builder.Services.AddTransient<IUserMailsRepository, UserMailsRepository>();
            builder.Services.AddTransient<IUserNotificationPrivilegesRepository, UserNotificationPrivilegesRepository>();
            builder.Services.AddTransient<IUsersLocationsRepository, UsersLocationsRepository>();
            builder.Services.AddTransient<IUsersRepository, UsersRepository>();
            builder.Services.AddTransient<IVersionRepository, VersionRepository>();
            builder.Services.AddTransient<ICarMovementsRepository, CarMovementsRepository>();
            builder.Services.AddTransient<ICarMovementsTypeRepository, CarMovementsTypeRepository>();
            builder.Services.AddTransient<IChecksRepository, ChecksRepository>();
            builder.Services.AddTransient<IContacFilesRepository, ContacFilesRepository>();
            builder.Services.AddTransient<IContractServicesRepository, ContractServicesRepository>();
            builder.Services.AddTransient<IContractRepository, ContractsRepository>();
            builder.Services.AddTransient<IContractStageRepository, ContractStageRepository>();
            builder.Services.AddTransient<ICostCenterRepository, CostCentersRepository>();
            builder.Services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddTransient<IFiscalyearsRepository, FiscalyearsRepository>();
            builder.Services.AddTransient<IGuranteesRepository, GuranteesRepository>();
            builder.Services.AddTransient<IInvoicesRepository, InvoicesRepository>();
            builder.Services.AddTransient<IJournalsRepository, JournalsRepository>();
            builder.Services.AddTransient<ILoanRepository, LoanRepository>();
            builder.Services.AddTransient<IOfferpriceconditionRepository, OfferpricesconditionRepository>();
            builder.Services.AddTransient<IOfferServiceRepository, OfferServiceRepository>();
            builder.Services.AddTransient<IOffersPricesRepository, OfferPricesRepository>();
            builder.Services.AddTransient<IServicesRepository, ServicesRepository>();
            builder.Services.AddTransient<IServicesPriceServiceRepository, ServicesPriceServiceRepository>();
            builder.Services.AddTransient<IServicesPriceServiceOfferRepository, ServicesPriceServiceOfferRepository>();

            builder.Services.AddTransient<IServicesPricingFormRepository, ServicesPricingFormRepository>();
            builder.Services.AddTransient<ITransactionsRepository, TransactionsRepository>();
            builder.Services.AddTransient<ITransactionTypesRepository, TransactionTypesRepository>();
            builder.Services.AddTransient<IVoucherSettingsRepository, VoucherSettingsRepository>();
            builder.Services.AddTransient<IUserBranchesRepository, UserBranchesRepository>();
            builder.Services.AddTransient<INodeLocationsRepository, NodeLocationsRepository>();
            builder.Services.AddTransient<IOutInImagesToRepository, OutInImagesToRepository>();
            builder.Services.AddTransient<IPro_Super_PhaseDetRepository, Pro_Super_PhaseDetRepository>();
            builder.Services.AddTransient<IProjectStatusTasksRepository, ProjectStatusTasksRepository>();
            builder.Services.AddTransient<IVoucherDetailsRepository, VoucherDetailsRepository>();
            builder.Services.AddTransient<IPrivFollowersRepository, PrivFollowersRepository>();
            builder.Services.AddTransient<IContractDetailsRepository, ContractDetailsRepository>();
            builder.Services.AddTransient<IContact_BranchesRepository, Contact_BranchesRepository>();
            builder.Services.AddTransient<IGuide_QuestionsAnswersRepository, Guide_QuestionsAnswersRepository>();
            builder.Services.AddTransient<ILaw_regulationsRepository, Law_regulationsRepository>();
            builder.Services.AddTransient<IContactListRepository, ContactListRepository>();
            builder.Services.AddTransient<IAttendenceLocationSettingsRepository, AttendenceLocationSettingsRepository>();
            builder.Services.AddTransient<IPermissionsRepository, PermissionsRepository>();
            builder.Services.AddTransient<IPermissionTypeRepository, PermissionTypeRepository>();
            builder.Services.AddTransient<ICommercialActivityRepository, CommercialActivityRepository>();
            builder.Services.AddTransient<INotificationConfigurationRepository, NotificationConfigurationRepository>();
            #endregion

         builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });



            //builder.Services.AddDbContext<AccountsContext>(options => { options.UseSqlServer("name=ConnectionStrings:DBConnection"); }, (ServiceLifetime.Transient));
            //builder.Services.AddDbContext<CustomersContext>(options => { options.UseSqlServer("name=ConnectionStrings:DBConnection"); }, (ServiceLifetime.Transient));
            //builder.Services.AddDbContext<EmployeeContext>(options => { options.UseSqlServer("name=ConnectionStrings:DBConnection"); }, (ServiceLifetime.Transient));
            //builder.Services.AddDbContext<OrganizationandSettingContext>(options => { options.UseSqlServer("name=ConnectionStrings:DBConnection"); }, (ServiceLifetime.Transient));
            //builder.Services.AddDbContext<ProjectsContext>(options => { options.UseSqlServer("name=ConnectionStrings:DBConnection"); }, (ServiceLifetime.Transient));
            //builder.Services.AddDbContext<UsersContext>(options => { options.UseSqlServer("name=ConnectionStrings:DBConnection"); }, (ServiceLifetime.Transient));



            //builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            //builder.Services.AddLocalization(options =>
            //{
            //    options.ResourcesPath = "Resources";
            //});

            builder.Services.AddLocalization();

            //builder.Services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //    new CultureInfo("en-US"),
            //    new CultureInfo("ar-SA")
            //};
            //    options.DefaultRequestCulture = new RequestCulture("en-US");
            //    options.SupportedUICultures = supportedCultures;
            //    options.SupportedCultures = supportedCultures;
            //});

            //builder.Services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new[]
            //    {
            //        new CultureInfo("en-US"),
            //        new CultureInfo("ar-SA")
            //    };
            //    options.DefaultRequestCulture = new RequestCulture("en-US");
            //    options.SupportedUICultures = supportedCultures;
            //    options.SupportedCultures = supportedCultures;

            //    options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
            //    {
            //       //...
            //        var userLangs = context.Request.Headers["Accept-Language"].ToString();
            //        var firstLang = userLangs.Split(',').FirstOrDefault();
            //        var defaultLang = string.IsNullOrEmpty(firstLang) ? "ar-SA" : firstLang;
            //        return Task.FromResult(new ProviderCultureResult(defaultLang, defaultLang));
            //    }));
            //});



            var app = builder.Build();
            //builder.Services.AddCors(Options => { Options.AddPolicy("CorsPolicy", Policy => { Policy.AllowAnyOrigin(); Policy.AllowAnyHeader(); Policy.AllowAnyMethod(); }); });
            // Configure the HTTP request pipeline.

            app.UseCors("AllowOrigin");

            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCors("CorsPolicy");


            //var supportedCultures = new[]
            //{
            // new CultureInfo("en-US"),
            // new CultureInfo("ar-SA"),
            //};
            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("en-US"),
            //    // Formatting numbers, dates, etc.
            //    SupportedCultures = supportedCultures,
            //    // UI strings that we have localized.
            //    SupportedUICultures = supportedCultures
            //});



            //app.UseRequestLocalization(options =>
            //{
            //    var questStringCultureProvider = options.RequestCultureProviders[0];
            //    options.RequestCultureProviders.RemoveAt(0);
            //    options.RequestCultureProviders.Insert(1, questStringCultureProvider);
            //});

            //app.UseRequestLocalization();

            //---------------------------------------------
            //Using localization
            var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            //app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            //---------------------------------------------
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".dwg"] = "application/acad";
            //provider.Mappings[".dwg"] = "application/dwg";
            provider.Mappings[".bak"] = "application/octet-stream";
            provider.Mappings[".rvt"] = "application/octet-stream";
            provider.Mappings[".woff2"] = "application/font-woff";
            provider.Mappings[".kml"] = "application/vnd.google-earth.kml+xml";
            provider.Mappings[".kmz"] = "application/vnd.google-earth.kmz";
            //---------------------------------------------

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });


            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Attachment")),
                RequestPath = "/Uploads/Attachment",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/BackupDb")),
                RequestPath = "/Uploads/BackupDb",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Banks")),
                RequestPath = "/Uploads/Banks",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/ChatGroup")),
                RequestPath = "/Uploads/ChatGroup",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Contract")),
                RequestPath = "/Uploads/Contract",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/ContractFileExtra")),
                RequestPath = "/Uploads/ContractFileExtra",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Customers")),
                RequestPath = "/Uploads/Customers",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/CustomersMails")),
                RequestPath = "/Uploads/CustomersMails",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Drafts")),
                RequestPath = "/Uploads/Drafts",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Employees")),
                RequestPath = "/Uploads/Employees",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Financefile")),
                RequestPath = "/Uploads/Financefile",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Invoices")),
                RequestPath = "/Uploads/Invoices",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Items")),
                RequestPath = "/Uploads/Items",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/News")),
                RequestPath = "/Uploads/News",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Notifications")),
                RequestPath = "/Uploads/Notifications",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Organizations")),
                RequestPath = "/Uploads/Organizations",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/ProjectRequirements")),
                RequestPath = "/Uploads/ProjectRequirements",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Projects")),
                RequestPath = "/Uploads/Projects",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/ProjectTasks")),
                RequestPath = "/Uploads/ProjectTasks",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Service")),
                RequestPath = "/Uploads/Service",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Supervision")),
                RequestPath = "/Uploads/Supervision",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Users")),
                RequestPath = "/Uploads/Users",
                ContentTypeProvider = provider
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
       Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Ticket")),
                RequestPath = "/Uploads/Ticket",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "Files/SuperDetailesFile")),
                RequestPath = "/Files/SuperDetailesFile",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Files/somefiles")),
                RequestPath = "/Files/somefiles",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Files/ProjectFiles")),
                RequestPath = "/Files/ProjectFiles",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "TempFiles")),
                RequestPath = "/TempFiles",
                ContentTypeProvider = provider
            });
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "distnew/images")),
            //    RequestPath = "/distnew/images"
            //});
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "distnew")),
                RequestPath = "/distnew",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "ServicePriceFormPDF")),
                RequestPath = "/ServicePriceFormPDF",
                ContentTypeProvider = provider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Reports")),
                RequestPath = "/Reports",
                ContentTypeProvider = provider
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
         Path.Combine(Directory.GetCurrentDirectory(), "Uploads/Organizations/DomainLink")),
                RequestPath = "/Uploads/Organizations/DomainLink",
                ContentTypeProvider = provider
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
         Path.Combine(Directory.GetCurrentDirectory(), "Backup")),
                RequestPath = "/Backup",
                ContentTypeProvider = provider
            });

            app.MapControllers();
            //app.MapControllerRoute("LocalizedDefault", "{culture:cultrue}/{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}