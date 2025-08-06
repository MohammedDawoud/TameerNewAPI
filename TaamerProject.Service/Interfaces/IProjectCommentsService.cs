using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface IProjectCommentsService  
    {
        Task<IEnumerable<ProjectCommentsVM>> GetAllProjectComments(int ProjectId);
        GeneralMessage SaveComment(ProjectComments comments,int UserId, int BranchId);
        GeneralMessage DeleteComment(int CommentId  ,int UserId, int BranchId);
    }
}
