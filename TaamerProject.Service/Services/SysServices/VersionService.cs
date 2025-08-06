using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models;
using TaamerProject.Models.Common.FIlterModels;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;
using TaamerProject.Repository.Interfaces;
using TaamerProject.Models.DBContext;
using TaamerProject.Service.Generic;
using TaamerProject.Repository.Repositories;
using System.Net;
using TaamerProject.Service.IGeneric;

namespace TaamerProject.Service.Services
{
    public class VersionService :   IVersionService
    {
        
        private readonly IVersionRepository _VersionRepository;
        public VersionService(IVersionRepository versionRepository)
        {
            _VersionRepository = versionRepository;
        }
        public Task< VersionVM> GetVersion()
        {
            var Version = _VersionRepository.GetVersion();
            return Version;
        }
    }
}
