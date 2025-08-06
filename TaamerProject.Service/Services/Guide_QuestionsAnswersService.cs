using Bayanatech.TameerPro.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Repository.Repositories;
using TaamerProject.Service.IGeneric;
using TaamerP.Service.LocalResources;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class Guide_QuestionsAnswersService : IGuide_QuestionsAnswersService
    {
        private readonly IGuide_QuestionsAnswersRepository _guide_QuestionsAnswers;
        private readonly TaamerProjectContext _TaamerProContext;
        private readonly ISystemAction _SystemAction;


        public Guide_QuestionsAnswersService(IGuide_QuestionsAnswersRepository guide_QuestionsAnswers,
            TaamerProjectContext dataContext, ISystemAction systemAction)
        {
            _guide_QuestionsAnswers = guide_QuestionsAnswers;
            _TaamerProContext = dataContext;
            _SystemAction = systemAction;
        }


        public Task<IEnumerable<Guide_QuestionsAnswersVM>> GetAllquestionAnswers()
        {
            var items = _guide_QuestionsAnswers.GetAllquestionAnswers();
            return items;
        }
        public GeneralMessage SaveQuestionAnswers(Guide_QuestionsAnswers Ques, int UserId, int BranchId)
        {
            try
            {
                if (Ques.Guide_QuestionsAnswersId == 0)
                {
                    Ques.AddUser = UserId;
                    Ques.AddDate = DateTime.Now;
                    _TaamerProContext.Guide_QuestionsAnswers.Add(Ques);
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "اضافة سوال جديد";
                    _SystemAction.SaveAction("SaveQuestionAnswers", "Guide_QuestionsAnswersService", 1, Resources.General_SavedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};

                }
                else
                {
                    //var ItemUpdated = _GuideDepartmentsRepository.GetById(Dep.DepId);
                    Guide_QuestionsAnswers? ItemUpdated = _TaamerProContext.Guide_QuestionsAnswers.Where(s => s.Guide_QuestionsAnswersId == Ques.Guide_QuestionsAnswersId).FirstOrDefault();
                   

                    if (ItemUpdated != null)
                    {
                        ItemUpdated.QuestionAr = Ques.QuestionAr;
                        ItemUpdated.QuestionEn = Ques.QuestionEn;
                        ItemUpdated.AnswersAr = Ques.AnswersAr;
                        ItemUpdated.AnswersEn = Ques.AnswersEn;
                    }
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تعديل سوال  " ;
                    _SystemAction.SaveAction("SaveQuestionAnswers", "Guide_QuestionsAnswersService", 2, Resources.General_EditedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_SavedSuccessfully};


                }
            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حفظ سوال";
                _SystemAction.SaveAction("SaveQuestionAnswers", "Guide_QuestionsAnswersService", 1, Resources.General_SavedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_SavedFailed };
            }
        }


        public GeneralMessage DeleteQuestions(int Quest, int UserId, int BranchId)
        {
            try
            {
                if (Quest != null)
                {
                   
                    var dept = _TaamerProContext.Guide_QuestionsAnswers.Where(x => x.Guide_QuestionsAnswersId == Quest).FirstOrDefault();
                    dept.IsDeleted = true;
                    dept.DeleteDate = DateTime.Now;
                    dept.DeleteUser = UserId;
                    _TaamerProContext.SaveChanges();

                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = " تم الحذف ";
                    _SystemAction.SaveAction("DeleteQuestions", "Guide_QuestionsAnswersService", 1, Resources.General_DeletedSuccessfully, "", "", ActionDate, UserId, BranchId, ActionNote, 1);
                    //-----------------------------------------------------------------------------------------------------------------
                    return new GeneralMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = Resources.General_DeletedSuccessfully };

                }
                else
                {
                    //-----------------------------------------------------------------------------------------------------------------
                    string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                    string ActionNote = "فشل في حذف ";
                    _SystemAction.SaveAction("DeleteQuestions", "Guide_QuestionsAnswersService", 1, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                    //-----------------------------------------------------------------------------------------------------------------

                    return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
                }

            }
            catch (Exception ex)
            {
                //-----------------------------------------------------------------------------------------------------------------
                string ActionDate = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                string ActionNote = "فشل في حذف ";
                _SystemAction.SaveAction("DeleteQuestions", "Guide_QuestionsAnswersService", 1, Resources.General_DeletedFailed, "", "", ActionDate, UserId, BranchId, ActionNote, 0);
                //-----------------------------------------------------------------------------------------------------------------

                return new GeneralMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = Resources.General_DeletedFailed };
            }
        }


    }
}
