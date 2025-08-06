using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Models;
using TaamerProject.Models.Common;

namespace TaamerProject.Repository.Repositories
{
    public class ProjectPiecesRepository : IProjectPiecesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public ProjectPiecesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task<IEnumerable<ProjectPiecesVM>> GetAllProjectPieces(int ProjectId,string SearchText)
        {
            if (SearchText == "")
            {
                var projectPieces = _TaamerProContext.ProjectPieces.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new ProjectPiecesVM
                {
                    PieceId = x.PieceId,
                    PieceNo=x.PieceNo,
                    Notes=x.Notes,
                    ProjectId=x.ProjectId,
                }).ToList();
                return projectPieces;
            }
            else
            {
                var projectPieces = _TaamerProContext.ProjectPieces.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId && (s.PieceNo.Contains(SearchText))).Select(x => new ProjectPiecesVM
                {
                    PieceNo = x.PieceNo,
                    PieceId = x.PieceId,
                    Notes=x.Notes,
                    ProjectId=x.ProjectId,

                }).ToList();
                return projectPieces;
            }
           }
        }
    }
