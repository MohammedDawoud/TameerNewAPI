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
    public class InstrumentsRepository : IInstrumentsRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public InstrumentsRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;
        } 
        
        public async Task< IEnumerable<InstrumentsVM>> GetAllInstruments(int ProjectId)
        {

            var instruments = _TaamerProContext.Instruments.Where(s => s.IsDeleted == false && s.ProjectId == ProjectId).Select(x => new InstrumentsVM
            {
                    InstrumentId = x.InstrumentId,
                    InstrumentNo = x.InstrumentNo,
                    Date = x.Date,
                    HijriDate = x.HijriDate,
                    ProjectId=x.ProjectId,
                    SourceId=x.SourceId,
                    ProjectName=x.project.ProjectName,
                    InstrumentName=x.instrumentSources.NameAr
                }).ToList();
                return instruments;
            }
        }
    }
