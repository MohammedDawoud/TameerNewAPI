using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models.DomainObjects;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Service.Interfaces
{
    public interface IGuide_QuestionsAnswersService
    {
        Task<IEnumerable<Guide_QuestionsAnswersVM>> GetAllquestionAnswers();
        GeneralMessage SaveQuestionAnswers(Guide_QuestionsAnswers Ques, int UserId, int BranchId);
        GeneralMessage DeleteQuestions(int Quest, int UserId, int BranchId);
    }
}
