using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Service.IGeneric;
using Twilio.Base;
using TaamerP.Service.LocalResources;

namespace TaamerProject.Service.Services
{
    public class Pro_ProjectStepsService: IPro_ProjectStepsService
    {
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;

        private readonly IPro_ProjectStepsRepository _Pro_ProjectStepsRepository;

        public Pro_ProjectStepsService(IPro_ProjectStepsRepository Pro_ProjectStepsRepository, TaamerProjectContext dataContext
            , ISystemAction systemAction)
        {
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
            _Pro_ProjectStepsRepository = Pro_ProjectStepsRepository;
        }
        public Task<IEnumerable<Pro_ProjectStepsVM>> GetAllProjectSteps()
        {
            var ProjectSteps = _Pro_ProjectStepsRepository.GetAllProjectSteps();
            return ProjectSteps;
        }
        public Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectid(int projectid, int stepid)
        {
            var ProjectSteps = _Pro_ProjectStepsRepository.GetProjectStepsbyprojectid(projectid, stepid);
            return ProjectSteps;
        }
        public Task<IEnumerable<Pro_ProjectStepsVM>> GetProjectStepsbyprojectidOnly(int projectid)
        {
            var ProjectSteps = _Pro_ProjectStepsRepository.GetProjectStepsbyprojectidOnly(projectid);
            return ProjectSteps;
        }
        public GeneralMessage UpdateProjectStepStatus(Pro_ProjectSteps ProjectStep, int UserId, int BranchId)
        {
            try
            {
                var ProjectSteprUpdated = _TaamerProContext.Pro_ProjectSteps.Where(s => s.ProjectStepId == ProjectStep.ProjectStepId).FirstOrDefault();
                if (ProjectSteprUpdated != null)
                {
                    ProjectSteprUpdated.Status = ProjectStep.Status;
                    ProjectSteprUpdated.UserId = UserId;
                    ProjectSteprUpdated.BranchId = BranchId;
                    ProjectSteprUpdated.Date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    ProjectSteprUpdated.UpdateUser = UserId;
                    ProjectSteprUpdated.UpdateDate = DateTime.Now;
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تعديل حالة مرحلة المشروع رقم " + ProjectStep.ProjectStepId;
                _SystemAction.SaveAction("SaveProjectStep", "Pro_ProjectStepsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ حالة مرحلة المشروع";
                _SystemAction.SaveAction("SaveProjectStep", "Pro_ProjectStepsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }
        public GeneralMessage ReturnProjectStepStatus(Pro_ProjectSteps ProjectStep, int UserId, int BranchId)
        {
            try
            {
                var ProjectSteprUpdated = _TaamerProContext.Pro_ProjectSteps.Where(s => s.ProjectStepId == ProjectStep.ProjectStepId).FirstOrDefault();
                if (ProjectSteprUpdated != null)
                {
                    ProjectSteprUpdated.Status = false;
                    ProjectSteprUpdated.UserId = null;
                    ProjectSteprUpdated.BranchId = null;
                    ProjectSteprUpdated.Date = null;
                    ProjectSteprUpdated.UpdateUser = UserId;
                    ProjectSteprUpdated.UpdateDate = DateTime.Now;
                }

                _TaamerProContext.SaveChanges();

                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " تراجع عن حالة مرحلة المشروع رقم " + ProjectStep.ProjectStepId;
                _SystemAction.SaveAction("SaveProjectStep", "Pro_ProjectStepsService", 2, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ التراجع عن مرحلة المشروع";
                _SystemAction.SaveAction("SaveProjectStep", "Pro_ProjectStepsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage SaveProjectStep(int projectId, int UserId, int BranchId)
        {
            try
            {
                var StepDetailsList = _TaamerProContext.Pro_StepDetails.ToList();
                var ProjectStepsList = _TaamerProContext.Pro_ProjectSteps.Where(s=>s.ProjectId== projectId).ToList();
                if(ProjectStepsList.Count()>0)
                {
                    _TaamerProContext.Pro_ProjectSteps.RemoveRange(ProjectStepsList);
                }
                var ProjectAchievementsList = _TaamerProContext.Pro_ProjectAchievements.Where(s => s.ProjectId == projectId).ToList();
                if (ProjectAchievementsList.Count() > 0)
                {
                    _TaamerProContext.Pro_ProjectAchievements.RemoveRange(ProjectAchievementsList);
                }

                foreach (var StepDetails in StepDetailsList) {
                    Pro_ProjectSteps ProjectStep = new Pro_ProjectSteps();
                    ProjectStep.ProjectStepId = 0;
                    ProjectStep.ProjectId = projectId;
                    ProjectStep.StepId = StepDetails.StepId;
                    ProjectStep.StepDetailId = StepDetails.StepDetailId;
                    ProjectStep.Status = false;
                    ProjectStep.UserId = null;
                    ProjectStep.BranchId = BranchId;
                    ProjectStep.Date = null;
                    ProjectStep.Notes = null;
                    ProjectStep.AddDate = DateTime.Now;
                    ProjectStep.AddUser = UserId;
                    _TaamerProContext.Pro_ProjectSteps.Add(ProjectStep);
                }

                for (int i = 0; i < 25; i++)
                {
                    var NameCh = "";var Step = 1;
                    if(i < 6)
                    {
                        Step = 1;
                        if (i == 0) NameCh = "أسست فريق العمل الخاص بالمشروع";
                        else if (i == 1) NameCh = "قمت بالتواصل مع العميل وفهم متطلباته والنقاط المهمة بالنسبة له ";
                        else if (i == 2) NameCh = "وضعت خطة المشروع ، وحددت الأهداف ونطاق المشروع";
                        else if (i == 3) NameCh = "راجعت وتحققت من جميع الإجراءات الإدارية وطريقة التنفيذ";
                        else if (i == 4) NameCh = "تأسيس بيئة إدارة المشروع وكتيب المشروع (استمارة المشروع)\r\nأسست بيئة إدارة المشروع ، وجمعت الأدوات اللازمة ، ونظمت المستخدمين العاملين على المشروع\r\n";
                        else if (i == 5) NameCh = "راجعت العقود وبنودها ، ونسقت مع الجهة المالية ، وتحققت من جميع الشروط ، والنماذج ذات الصلة";
                    }
                    else if(i < 16)
                    {
                        Step = 2;
                        if (i == 6) NameCh = "وضعت الأطر اللازمة للمشروع ، وراجعت البدائل ، وناقش دراسة الجدوى بشكل دقيق وكامل";
                        else if (i == 7) NameCh = "قسمت المشروع إلى مراحل ووضعت لكل مرحلة مهامها ، وأنشات سير المشروع طبقا لكل مرحلة";
                        else if (i == 8) NameCh = "قدرت الموارد الموارد اللازمة للمشروع ، سواء الموارد الداخلية / أو الموارد الخارجية وحددت الاحتياجات";
                        else if (i == 9) NameCh = "وضعت جدولا زمنيا دقيقا بناء على تجارب سابقة ، مع الأخذ في الاعتبار جميع الأحداث الطارئة وغير المتوقعة";
                        else if (i == 10) NameCh = "أسست خطة التواصل مع الفريق ، ومع الإدارة ، ومع العمل ، ووضعت كل الخططة اللازمة لبقاء الجميع على اطلاع وتواصل";
                        else if (i == 11) NameCh = "حددت المعايير ، وراجعت الإجراءات والنماذج اللازمة للمشروع";
                        else if (i == 12) NameCh = "حددت الإجراءات الوقائية وقيمت وضع المخاطر المتوقعة ، سواء المباشر منها وغير المباشر";
                        else if (i == 13) NameCh = "وضعت ميزانية أولية للمشروع بالتنسيق مع الإدارة ، ومع الحسابات ، وكنت في تواصل دائما مع المعنيين";
                        else if (i == 14) NameCh = "طوال فترة المشروع كنت أتابع وأراقب وأحدث الانحرافات في المشروع من خلال متابعة الإطار العام المرسوم";
                        else if (i == 15) NameCh = "وضعت الخطة الأساسية للمشروع والتزمت بتنفيذها ، وتأكدت من أن الجميع يسيرون على نهج الخطة المرسومة";
                    }
                    else if (i < 21)
                    {
                        Step = 3;
                        if (i == 16) NameCh = "أشرفت على تنفيذ خطة المشروع الأساسية ، وتحققت من جودة ودقة مخرجات العمل ونتائجه وراجعت كل ذلك";
                        else if (i == 17) NameCh = "أراقب لحظة بلحظة تقدم المشروع ، ومدى مطابقته للخطة الموضوعة ، والتأكد من أنه يتماشى مع الجدول الزمني المخطط له";
                        else if (i == 18) NameCh = "أدرت كافة التغييرات التي حصلت على خطة المشروع ، وعالجة ما يظهر من عقبات واحتياجات";
                        else if (i == 19) NameCh = "تأكد أن جميع البيانات والملفات الخاصة بالمشروع موجودة في كتيب المشروع (الاستمارة)";
                        else if (i == 20) NameCh = "قمت بمراجعة المهام والمراحل في المشروع ، وتأكدت من نسب الإنجاز ومدى توافقها مع خطة التقدم والتنفيذ ، كما قمت بمراجعة جميع المهام لضمان دقة النتائج وانعكاسها فعليا على المشروع";
                    }
                    else if (i < 25)
                    {
                        Step = 4;
                        if (i == 21) NameCh = "قيمت أعضاء الفريق ، ورفعت تقريرا بذلك للإدارة ، اجتمعت مع فريق العمل وراجعنا الإخفاقات";
                        else if (i == 22) NameCh = "راجعت مع فريق العمل كافة الصعوبات التي تعرض لها المشروع ، واستسقينا العبر وتبادلنا الآراء التحسينية في المستقبل للمشاريع المماثلة";
                        else if (i == 23) NameCh = "تأكد من الأمور المالية مع قسم الحسابات ، تمهيد لإنهاء المشروع";
                        else if (i == 24) NameCh = "تم تسليم المشروع للعميل ، وقمت بإغلاق المشروع وتحويله إلى الأرشيف";
                    }
                    else NameCh = "";
                    Pro_ProjectAchievements ProjectAchievement = new Pro_ProjectAchievements();
                    ProjectAchievement.ProjectAchievementId = 0;
                    ProjectAchievement.NameAr = NameCh;
                    ProjectAchievement.NameEn = null;

                    ProjectAchievement.ProjectId = projectId;
                    ProjectAchievement.StepId = Step;
                    ProjectAchievement.LineNumber = i+1;
                    ProjectAchievement.UserId = null;
                    ProjectAchievement.BranchId = BranchId;
                    ProjectAchievement.AddDate = DateTime.Now;
                    ProjectAchievement.AddUser = UserId;
                    _TaamerProContext.Pro_ProjectAchievements.Add(ProjectAchievement);
                }
                _TaamerProContext.SaveChanges();
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = Resources.addnewitem;
                _SystemAction.SaveAction("SaveProjectStep", "Pro_ProjectStepsService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully };

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ مرحلة المشروع";
                _SystemAction.SaveAction("SaveProjectStep", "Pro_ProjectStepsService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }

        public GeneralMessage DeleteProjectStep(int ProjectStepid, int UserId, int BranchId)
        {
            try
            {
                Pro_ProjectSteps? ProjectStep = _TaamerProContext.Pro_ProjectSteps.Where(s => s.ProjectStepId == ProjectStepid).FirstOrDefault();
                if (ProjectStep != null)
                {
                    ProjectStep.IsDeleted = true;
                    ProjectStep.DeleteDate = DateTime.Now;
                    ProjectStep.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " حذف مرحلة المشروع رقم " + ProjectStepid;
                    _SystemAction.SaveAction("DeleteProjectStep", "Pro_ProjectStepsService", 3, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------

                }
                return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };
            }
            catch (Exception)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = " فشل في حذف مرحلة المشروع رقم " + ProjectStepid; ;
                _SystemAction.SaveAction("DeleteProjectStep", "Pro_ProjectStepsService", 3, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------
                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }
    }
}
