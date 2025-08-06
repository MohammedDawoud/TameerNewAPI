using TaamerProject.Models;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.Service.Services
{
    public class SMSProviderService : ISMSProviderService
    {
        List<SMSProviderVM> _providers = new List<SMSProviderVM>();

        public SMSProviderService() {
            SMSProviderVM Obj = new SMSProviderVM() { ProviderId = 1, Name= "تقنيات", BaseApiUrl= "https://api.taqnyat.sa/" };
        }

        public IEnumerable<SMSProviderVM> FillSelectList() {
            var PList = _providers.Select(x => new SMSProviderVM { ProviderId = x.ProviderId, Name = x.Name }).ToList();
            return PList;
        }
        public SMSProviderVM GetSelectedProvider(int Provider)
        {
            return new SMSProviderVM();
        }
    }

}
