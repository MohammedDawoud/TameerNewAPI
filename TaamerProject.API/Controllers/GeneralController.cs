using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TaamerProject.API.Helper;
using TaamerProject.Models.Common;
using TaamerProject.Service.Interfaces;

namespace TaamerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class GeneralController : ControllerBase
    {
        public GeneralController()
        {
        }
        [HttpGet("GetHijriDate")]
        public IActionResult GetHijriDate(DateTime Date)
        {
            var date = ConvertDateCalendar(Date, "Hijri", "en-US");
            return Ok(date);
        }
        [HttpPost("ConvertDateCalendar")]
        public string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {

            }
            DateLangCulture = "ar-sa";
            /// Set the date time format to the given culture
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;

            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;
                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;
                default:
                    return "";
            }
            DTFormat.ShortDatePattern = @"yyyy/MM/dd";
            DTFormat.DateSeparator = "-";
            var ss = DateConv.Date.ToString("d", DTFormat);
            return (DateConv.Date.ToString("d", DTFormat));
        }
        [HttpGet("HijriToGreg")]
        public IActionResult HijriToGreg(string hijri)
        {
            try
            {
                CultureInfo arCul = new CultureInfo("ar-SA");
                CultureInfo enCul = new CultureInfo("en");
                string DAtee = HijriToGreg2(hijri);
                return Ok(DAtee);
            }
            catch (Exception)
            {
                return Ok("");
            }
        }
        [HttpGet("HijriToGregMethod")]
        public DateTime HijriToGregMethod(string hijri)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            HijriCalendar h = new HijriCalendar();
            arCul.DateTimeFormat.Calendar = h;
            DateTime date = DateTime.ParseExact(hijri, "yyyy-MM-dd", arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return date;
        }

        private string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
           "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"
        };

        [HttpGet("HijriToGreg2")]
        public string HijriToGreg2(string hijri)
        {
            CultureInfo arCul = new CultureInfo("ar-SA");
            CultureInfo enCul = new CultureInfo("en");
            DateTime tempDate = DateTime.ParseExact(hijri, allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate.ToString("yyyy-MM-dd", enCul.DateTimeFormat);
        }
        [HttpGet("GetProjectDurationStr")]
        public IActionResult GetProjectDurationStr(string start, string end)
        {
            try
            {
                DateTime startdate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                DateTime enddate = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en"));
                var days = enddate.Subtract(startdate).Days+1;
                string TimeStr = (days < 30) ? days + " يوم " : (days == 30) ? days / 30 + " شهر " : (days > 30) ? ((days / 30) + " شهر " + (days % 30) + " يوم  ") : "";
                return Ok(new GeneralMessage { ReasonPhrase = TimeStr, ReturnedParm= days});
            }
            catch (Exception)
            {
                return Ok(new GeneralMessage { ReasonPhrase = "", ReturnedParm=0 });
            }
        }
        [HttpGet("ConvertNumToString")]
        public IActionResult ConvertNumToString(string Num)
        {
            if (Num != "NaN")
            {
                CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
                ToWord toWord = new ToWord(Convert.ToDecimal(Num), _currencyInfo);
                return Ok(new GeneralMessage {ReasonPhrase=toWord.ConvertToArabic()});
            }
            return Ok(new GeneralMessage { ReasonPhrase = "" });
        }
        [HttpGet("ConvertNumToStringEnglish")]
        public IActionResult ConvertNumToStringEnglish(string Num)
        {
            if (Num != "NaN")
            {
                CurrencyInfo _currencyInfo = new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia);
                ToWord toWord = new ToWord(Convert.ToDecimal(Num), _currencyInfo);
                return Ok(new GeneralMessage { ReasonPhrase = toWord.ConvertToEnglish() });
            }
            return Ok(new GeneralMessage { ReasonPhrase = "" });
        }
    }
}
