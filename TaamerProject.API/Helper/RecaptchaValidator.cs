using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaamerProject.API.Helper
{
    public class RecaptchaValidator
    {
        private const string RecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";

        public async Task<bool> ValidateRecaptchaAsync(string recaptchaResponse, string secretKey)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, RecaptchaVerifyUrl)
                {
                    Content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("secret", secretKey),
                    new KeyValuePair<string, string>("response", recaptchaResponse)
                })
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var recaptchaResponseData = JsonConvert.DeserializeObject<RecaptchaResponseData>(responseBody);

                return recaptchaResponseData.Success;
            }
        }
    }
    public class RecaptchaResponseData
    {
        public bool Success { get; set; }
        public string ChallengeTs { get; set; }
        public string Hostname { get; set; }
        public string ErrorCodes { get; set; }
    }
}