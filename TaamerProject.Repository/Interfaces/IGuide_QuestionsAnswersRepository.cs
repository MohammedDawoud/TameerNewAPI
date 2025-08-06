using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.ViewModels;

namespace TaamerProject.Repository.Interfaces
{
    public interface IGuide_QuestionsAnswersRepository
    {
        Task<IEnumerable<Guide_QuestionsAnswersVM>> GetAllquestionAnswers();
    }
}
