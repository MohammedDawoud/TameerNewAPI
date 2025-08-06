using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.DBContext;
using TaamerProject.Models.ViewModels;
using TaamerProject.Repository.Interfaces;

namespace TaamerProject.Repository.Repositories
{
    public class Guide_QuestionsAnswersRepository : IGuide_QuestionsAnswersRepository
    {

        private readonly TaamerProjectContext _TaamerProContext;

        public Guide_QuestionsAnswersRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }


        public async Task<IEnumerable<Guide_QuestionsAnswersVM>> GetAllquestionAnswers()
        {
            var drafts = _TaamerProContext.Guide_QuestionsAnswers.Where(s => s.IsDeleted == false).Select(x => new Guide_QuestionsAnswersVM
            {
                QuestionAr=x.QuestionAr,
                Guide_QuestionsAnswersId=x.Guide_QuestionsAnswersId,
                QuestionEn =x.QuestionEn,
                AnswersEn=x.AnswersEn,
                AnswersAr=x.AnswersAr,
            }).ToList();
            return drafts;
        }


    }
}
