using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class UIutility
    {
        public static Int32 GetDoubleToInt(string Value)
        {
            return Convert.ToInt32(Convert.ToDouble(Value));
        }
        public static Int32 GetConvertAmount(int iValue, Double dCurrencyValue, string Currency)
        {
            if (!Currency.Equals("INR"))
            {
                return CommonFunction.AvgAmount1((iValue * dCurrencyValue).ToString());
            }
            return iValue;
        }
        public static string GetLower(string text)
        {
            return text.ToLower();
        }
        public static string GetUpper(string text)
        {
            return text.ToUpper();
        }
        public static string Get_FirstTenCharacter(string CText)
        {
            string Result = string.Empty;
            try
            {
                if (CText.Length > 10)
                {
                    Result = CText.Substring(0, 10).Trim() + "...";

                }
                else
                {
                    Result = CText.Trim() + "...";
                }
            }
            catch
            {
                Result = CText;
            }

            return Result.Trim();
        }
        public static string Get_FirstCharacterCapital(string CText)
        {
            string Result = string.Empty;
            try
            {
                string Str = CText.ToLower();
                Result = Str.Substring(0, 1).ToUpper().Trim() + Str.Substring(1, Str.Length - 1).ToLower().Trim();
            }
            catch
            {
                Result = CText.Trim();
            }

            return Result.Trim();
        }
        public static string Get_ALL_FirstCharacterCapital(string CText)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CText.ToLower());
        }
        public static string Get_ConvertedDateTime(string CText)
        {
            string Result = string.Empty;
            try
            {
                DateTime Dt = Convert.ToDateTime(CText.Trim());
                Result = Dt.ToString("dd-MMM-yyyy HH:mm");
            }
            catch
            {
                Result = CText.Trim();
            }

            return Result.Trim();
        }
        public static string Get_ConvertedDate(string CText)
        {
            string Result = string.Empty;
            try
            {
                DateTime Dt = Convert.ToDateTime(CText.Trim());
                Result = Dt.ToString("dd-MMM-yyyy");
            }
            catch
            {
                Result = CText.Trim();
            }

            return Result.Trim();
        }
        public static string NumberToText(int number, bool isUK)
        {
            if (number == 0) return "Zero";
            string and = isUK ? "and " : ""; // deals with UK or US numbering
            if (number == -2147483648)
                return "Minus Two Billion One Hundred " + and +
"Forty Seven Million Four Hundred " + and + "Eighty Three Thousand " +
"Six Hundred " + and + "Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Million ", "Billion " };
            num[0] = number % 1000;           // units
            num[1] = number / 1000;
            num[2] = number / 1000000;
            num[1] = num[1] - 1000 * num[2];  // thousands
            num[3] = number / 1000000000;     // billions
            num[2] = num[2] - 1000 * num[3];  // millions
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10;              // ones
                t = num[i] / 10;
                h = num[i] / 100;             // hundreds
                t = t - 10 * h;               // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
