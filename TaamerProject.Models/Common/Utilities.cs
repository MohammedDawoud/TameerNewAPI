using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaamerProject.Models.Common
{
    public class Utilities
    {
        string NumberAr;
        public Utilities(string Number)
        {
            NumberAr = ConvertNumberToAlpha(Number);
        }
        private string ConvertNumberToAlpha(string Number)
        {

            if (Number.Contains('.'))
                {
                    if (Number.Split('.')[0].ToCharArray().Length > 6)
                    {
                        return " ";
                    }
                    else
                    {
                    var ss = "";
                    try
                    {
                        ss = convertTwoDigits(Number.Split('.')[1]);

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            ss = convertOneDigits(Number.Split('.')[1]);
                        }
                        catch (Exception ex2)
                        {
                            ss=convertThreeDigits(Number.Split('.')[1]);
                        }

                    }

                    if (ss != "")
                    {
                        var val = "";
                        var FirstValue = Number.Split('.')[0];

                        switch (Number.Split('.')[0].ToCharArray().Length)
                            {
                                case 1:
                                try
                                {
                                    if(FirstValue=="0")
                                    {
                                        val =  convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertOneDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    return val;
                                }
                                catch (Exception ex3)
                                {

                                    if (FirstValue == "0")
                                    {
                                        val =  convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertOneDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }


                                    return val;
                                }
                                case 2:
                                try
                                {

                                    if (FirstValue == "0")
                                    {
                                        val =  convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertTwoDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }

                                    return val;
                                }
                                catch (Exception ex3)
                                {
                                    if (FirstValue == "0")
                                    {
                                        val =convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertTwoDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    return val;
                                }
                                case 3:
                                try
                                {
                                    if (FirstValue == "0")
                                    {
                                        val =convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertThreeDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    return val;
                                }
                                catch (Exception ex3)
                                {

                                    if (FirstValue == "0")
                                    {
                                        val =  convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertThreeDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }

                                    return val;
                                }
                                case 4:
                                try
                                {

                                    if (FirstValue == "0")
                                    {
                                        val =  convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertFourDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }

                                    return val;
                                }
                                catch (Exception ex3)
                                {
                                    if (FirstValue == "0")
                                    {
                                        val = convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertFourDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    return val;
                                }
                                case 5:
                                try
                                {

                                    if (FirstValue == "0")
                                    {
                                        val = convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertFiveDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }

                                    return val;
                                }
                                catch (Exception ex3)
                                {
                                    if (FirstValue == "0")
                                    {
                                        val = convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertFiveDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }

                                    return val;
                                }
                                case 6:
                                try
                                {
                                    if (FirstValue == "0")
                                    {
                                        val = convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertSixDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertTwoDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    return val;
                                }
                                catch (Exception ex3)
                                {
                                    if (FirstValue == "0")
                                    {
                                        val =  convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    else
                                    {
                                        val = convertSixDigits(Number.Split('.')[0].ToString()) + " ريال " + " و " + convertOneDigits(Number.Split('.')[1]) + " هلله " + " فقط ";

                                    }
                                    return val;
                                }
                            default:
                                return "";

                            }
                        }
                        else
                        {


                        if (Number == "0")
                        {
                            return " صفر " + " ريال ";
                        }
                        else
                        {

                            if (Number.ToCharArray().Length > 7)
                            {
                                return " ";
                            }
                            else
                            {

                                switch (Number.Split('.')[0].ToCharArray().Length)
                                {
                                    case 1: return convertOneDigits(Number.Split('.')[0].ToString()) + " ريال " + " فقط ";
                                    case 2: return convertTwoDigits(Number.Split('.')[0].ToString()) + " ريال " + " فقط ";
                                    case 3: return convertThreeDigits(Number.Split('.')[0].ToString()) + " ريال " + " فقط ";
                                    case 4: return convertFourDigits(Number.Split('.')[0].ToString()) + " ريال " + " فقط ";
                                    case 5: return convertFiveDigits(Number.Split('.')[0].ToString()) + " ريال " + " فقط ";
                                    case 6: return convertSixDigits(Number.Split('.')[0].ToString()) + " ريال " + " فقط ";
                                    case 7: return convertSevenDigits(Number.Split('.')[0].ToString()) + " ريال " + " فقط ";

                                    default: return "";
                                }

                            }
                        }

                        }
                    }
                }
            else
            {

                if(Number=="0")
                {
                    return  " صفر " + " ريال ";
                }
                else
                {

                    if (Number.ToCharArray().Length > 6)
                    {
                        return " ";
                    }
                    else
                    {
                        switch (Number.ToCharArray().Length)
                        {
                            case 1: return convertOneDigits(Number.ToString()) + " ريال " + " فقط ";
                            case 2: return convertTwoDigits(Number.ToString()) + " ريال " + " فقط ";
                            case 3: return convertThreeDigits(Number.ToString()) + " ريال " + " فقط ";
                            case 4: return convertFourDigits(Number.ToString()) + " ريال " + " فقط ";
                            case 5: return convertFiveDigits(Number.ToString()) + " ريال " + " فقط ";
                            case 6: return convertSixDigits(Number.ToString()) + " ريال " + " فقط ";
                            default: return "";
                        }

                    }
                }

            }
        }
        private string convertOneDigits(string OneDigits)
        {
            switch (int.Parse(OneDigits))
            {
                case 1: return "واحد";
                case 2: return "إثنان";
                case 3: return "ثلاثه";
                case 4: return "أربعه";
                case 5: return "خمسه";
                case 6: return "سته";
                case 7: return "سبعه";
                case 8: return "ثمانيه";
                case 9: return "تسعه";
                default: return "";
            }
        }
        private string convertTwoDigits(string TwoDigits)
            {
                string returnAlpha = "00";
                if (TwoDigits.ToCharArray()[0] == '0' && TwoDigits.ToCharArray()[1] != '0')
                {
                    return convertOneDigits(TwoDigits.ToCharArray()[1].ToString());
                }
                else
                {
                    switch (int.Parse(TwoDigits.ToCharArray()[0].ToString()))
                    {
                        case 1:
                            {
                                if (int.Parse(TwoDigits.ToCharArray()[1].ToString()) == 1)
                                {
                                    return "إحدى عشر";
                                }
                                else if (int.Parse(TwoDigits.ToCharArray()[1].ToString()) == 2)
                                {
                                    return "إثنى عشر";
                                }
                                else
                                {
                                    returnAlpha = "عشر";
                                    return convertOneDigits(TwoDigits.ToCharArray()[1].ToString()) + " " + returnAlpha;
                                }
                            }
                        case 2: returnAlpha = "عشرون"; break;
                        case 3: returnAlpha = "ثلاثون"; break;
                        case 4: returnAlpha = "أريعون"; break;
                        case 5: returnAlpha = "خمسون"; break;
                        case 6: returnAlpha = "ستون"; break;
                        case 7: returnAlpha = "سبعون"; break;
                        case 8: returnAlpha = "ثمانون"; break;
                        case 9: returnAlpha = "تسعون"; break;
                        default: returnAlpha = ""; break;
                    }
                }
                if (convertOneDigits(TwoDigits.ToCharArray()[1].ToString()).Length == 0)
                { return returnAlpha; }
                else
                {
                    return convertOneDigits(TwoDigits.ToCharArray()[1].ToString()) + " و " + returnAlpha;
                }
            }
        private string convertThreeDigits(string ThreeDigits)
            {
                switch (int.Parse(ThreeDigits.ToCharArray()[0].ToString()))
                {
                    case 1:
                        {
                            if (int.Parse(ThreeDigits.ToCharArray()[1].ToString()) == 0)
                            {
                                if (int.Parse(ThreeDigits.ToCharArray()[2].ToString()) == 0)
                                {
                                    return "مائه";
                                }
                                return "مائه" + " و " + convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                            }
                            else
                            {
                                return "مائه" + " و " + convertTwoDigits(ThreeDigits.Substring(1, 2));
                            }
                        }
                    case 2:
                        {
                            if (int.Parse(ThreeDigits.ToCharArray()[1].ToString()) == 0)
                            {
                                if (int.Parse(ThreeDigits.ToCharArray()[2].ToString()) == 0)
                                {
                                    return "مائتين";
                                }
                                return "مائتين" + " و " + convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                            }
                            else
                            {
                                return "مائتين" + " و " + convertTwoDigits(ThreeDigits.Substring(1, 2));
                            }
                        }
                    case 3:
                        {
                            if (int.Parse(ThreeDigits.ToCharArray()[1].ToString()) == 0)
                            {
                                if (int.Parse(ThreeDigits.ToCharArray()[2].ToString()) == 0)
                                {
                                    return convertOneDigits(ThreeDigits.ToCharArray()[0].ToString()).Split('ه')[0] + "مائه";
                                }
                                return convertOneDigits(ThreeDigits.ToCharArray()[0].ToString()).Split('ه')[0] + "مائه" + " و " + convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                            }
                            else
                            {
                                return convertOneDigits(ThreeDigits.ToCharArray()[0].ToString()).Split('ه')[0] + "مائه" + " و " + convertTwoDigits(ThreeDigits.Substring(1, 2));
                            }
                        }
                    case 4:
                        {
                            goto case 3;
                        }
                    case 5:
                        {
                            goto case 3;
                        }
                    case 6:
                        {
                            goto case 3;
                        }
                    case 7:
                        {
                            goto case 3;
                        }
                    case 8:
                        {
                            goto case 3;
                        }
                    case 9:
                        {
                            goto case 3;
                        }
                    case 0:
                        {
                            if (ThreeDigits.ToCharArray()[1] == '0')
                            {
                                if (ThreeDigits.ToCharArray()[2] == '0')
                                {
                                    return "";
                                }
                                else
                                {
                                    return convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                                }
                            }
                            else
                            {
                                return convertTwoDigits(ThreeDigits.Substring(1, 2));
                            }
                        }
                    default: return "";
                }
            }
        private string convertFourDigits(string FourDigits)
            {
                switch (int.Parse(FourDigits.ToCharArray()[0].ToString()))
                {
                    case 1:
                        {
                            if (int.Parse(FourDigits.ToCharArray()[1].ToString()) == 0)
                            {
                                if (int.Parse(FourDigits.ToCharArray()[2].ToString()) == 0)
                                {
                                    if (int.Parse(FourDigits.ToCharArray()[3].ToString()) == 0)
                                        return "ألف";
                                    else
                                    {
                                        return "ألف" + " و " + convertOneDigits(FourDigits.ToCharArray()[3].ToString());
                                    }
                                }
                                return "ألف" + " و " + convertTwoDigits(FourDigits.Substring(2, 2));
                            }
                            else
                            {
                                return "ألف" + " و " + convertThreeDigits(FourDigits.Substring(1, 3));
                            }
                        }
                    case 2:
                        {
                            if (int.Parse(FourDigits.ToCharArray()[1].ToString()) == 0)
                            {
                                if (int.Parse(FourDigits.ToCharArray()[2].ToString()) == 0)
                                {
                                    if (int.Parse(FourDigits.ToCharArray()[3].ToString()) == 0)
                                        return "ألفين";
                                    else
                                    {
                                        return "ألفين" + " و " + convertOneDigits(FourDigits.ToCharArray()[3].ToString());
                                    }
                                }
                                return "ألفين" + " و " + convertTwoDigits(FourDigits.Substring(2, 2));
                            }
                            else
                            {
                                return "ألفين" + " و " + convertThreeDigits(FourDigits.Substring(1, 3));
                            }
                        }
                    case 3:
                        {
                            if (int.Parse(FourDigits.ToCharArray()[1].ToString()) == 0)
                            {
                                if (int.Parse(FourDigits.ToCharArray()[2].ToString()) == 0)
                                {
                                    if (int.Parse(FourDigits.ToCharArray()[3].ToString()) == 0)
                                        return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف";
                                    else
                                    {
                                        return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف" + " و " + convertOneDigits(FourDigits.ToCharArray()[3].ToString());
                                    }
                                }
                                return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف" + " و " + convertTwoDigits(FourDigits.Substring(2, 2));
                            }
                            else
                            {
                                return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف" + " و " + convertThreeDigits(FourDigits.Substring(1, 3));
                            }
                        }
                    case 4:
                        {
                            goto case 3;
                        }
                    case 5:
                        {
                            goto case 3;
                        }
                    case 6:
                        {
                            goto case 3;
                        }
                    case 7:
                        {
                            goto case 3;
                        }
                    case 8:
                        {
                            goto case 3;
                        }
                    case 9:
                        {
                            goto case 3;
                        }
                    default: return "";
                }
            }
        private string convertFiveDigits(string FiveDigits)
            {
                if (convertThreeDigits(FiveDigits.Substring(2, 3)).Length == 0)
                {
                    return convertTwoDigits(FiveDigits.Substring(0, 2)) + " ألف ";
                }
                else
                {
                    return convertTwoDigits(FiveDigits.Substring(0, 2)) + " ألفا " + " و " + convertThreeDigits(FiveDigits.Substring(2, 3));
                }
            }
        private string convertSixDigits(string SixDigits)
            {
                if (convertThreeDigits(SixDigits.Substring(2, 3)).Length == 0)
                {
                    return convertThreeDigits(SixDigits.Substring(0, 3)) + " ألف ";
                }
                else
                {
                    return convertThreeDigits(SixDigits.Substring(0, 3)) + " ألفا " + " و " + convertThreeDigits(SixDigits.Substring(3, 3));
                }
            }
        private string convertSevenDigits(string SevenDigits)
            {
                if (convertThreeDigits(SevenDigits.Substring(2, 3)).Length == 0)
                {
                    return convertThreeDigits(SevenDigits.Substring(0, 4)) + " مليون ";
                }
                else
                {
                    return convertThreeDigits(SevenDigits.Substring(0, 3)) + " مليونا " + " و " + convertThreeDigits(SevenDigits.Substring(4, 3));
                }
            }
        public string GetNumberAr()
            {
                return NumberAr;
            }
        private string convertNotoStr(int number)
        {
            string[] num = { "", "واحد", "اثنان", "ثلاثة", "أربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "احدي عشر", "اثنا عشر", "تلاثة عشر", "اربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
            string[] af = { "", "", "عشرون", "ثلاثون", "أربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };
            if (number <= 19)
            {
                return num[number];
            }
            else if (number > 19 && number < 100)
            {
                int n1 = number / 10;
                int n2 = number - n1 * 10;
                return af[n1] + " " + num[n2];
            }
            else
            {
                int n1 = number / 100;
                int n2 = number - n1 * 100;
                if (n2 <= 19)
                {
                    return num[n1] + "مائة" + num[n2];
                }
                else
                {
                    int n3 = n2 / 10;
                    int n4 = n2 - n3 * 10;
                    return num[n1] + "مائة" + af[n3] + " " + num[n4];
                }
            }
        }

        /// <summary>
        /// Its call Like this ConvertDateCalendar((DateTime), "Gregorian", "en-US")
        /// </summary>
        /// <param name="DateConv"></param>
        /// <param name="Calendar"></param>
        /// <param name="DateLangCulture"></param>
        /// <returns></returns>
        public static string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {

            }
            DateLangCulture = "ar-sa";
            /// Set the date time format to the given culture
            DTFormat = new CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new HijriCalendar();
                    break;
                case "Gregorian":
                    DTFormat.Calendar = new GregorianCalendar();
                    break;
                default:
                    return "";
            }
            /// We format the date structure to whatever we want
            DTFormat.ShortDatePattern = @"yyyy/MM/dd";
            DTFormat.DateSeparator = "-";
            var ss = DateConv.Date.ToString("d", DTFormat);
            return DateConv.Date.ToString("d", DTFormat);
        }
    }
}
