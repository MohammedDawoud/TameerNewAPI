using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.DomainObjects
{
    public class Guide_QuestionsAnswers :Auditable
    {
        public int Guide_QuestionsAnswersId { get; set; }
        public string? QuestionAr { get; set; }
        public string? QuestionEn { get; set; }
        public string? AnswersAr { get; set; }
        public string? AnswersEn { get; set; }
    }
}
