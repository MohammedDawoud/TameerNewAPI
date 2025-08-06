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
    public class InstrumentSourcesRepository : IInstrumentSourcesRepository
    {
        private readonly TaamerProjectContext _TaamerProContext;

        public InstrumentSourcesRepository(TaamerProjectContext dataContext)
        {
            _TaamerProContext = dataContext;

        }

        public async Task< IEnumerable<InstrumentSourcesVM>> GetAllInstrumentSources(string SearchText)
        {
            if (SearchText == "")
            {
                var instrumentSources = _TaamerProContext.InstrumentSources.Where(s => s.IsDeleted == false).Select(x => new InstrumentSourcesVM
                {
                    SourceId = x.SourceId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return instrumentSources;
            }
            else
            {
                var instrumentSources = _TaamerProContext.InstrumentSources.Where(s => s.IsDeleted == false && s.NameAr.Contains(SearchText)).Select(x => new InstrumentSourcesVM
                {
                    SourceId = x.SourceId,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn,
                }).ToList();
                return instrumentSources;
            }
        }
        }
    }
