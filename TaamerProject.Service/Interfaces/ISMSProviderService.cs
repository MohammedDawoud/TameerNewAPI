using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaamerProject.Models.Common;
using TaamerProject.Models;

namespace TaamerProject.Service.Interfaces
{
    public interface ISMSProviderService
    {
        IEnumerable<SMSProviderVM> FillSelectList();
        SMSProviderVM GetSelectedProvider(int Provider);
    }
}
