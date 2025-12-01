using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.Helpers
{
    public class NumberHelper
    {
        public static int ConvertToInt(object input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.ToString()))
                return 0;

            try
            {
                return Convert.ToInt32(Convert.ToDouble(input.ToString().Trim()));
            }
            catch (FormatException)
            {
                throw new FormatException($"Invalid numeric format: {input}");
            }
            catch (OverflowException)
            {
                throw new OverflowException($"Value too large or too small: {input}");
            }
        }

        public static int AvgAmount1(string Fees)
        {
            try
            {
                double Fees1 = Convert.ToDouble(Fees);

                int GetFees = 0;

                if (Fees.ToString().IndexOf(".") != -1)
                {
                    string k3 = Fees.ToString().Substring(0, Fees.ToString().IndexOf("."));

                    double k4 = Convert.ToDouble(k3) + .1;

                    if (Fees1 >= k4)
                    {
                        GetFees = int.Parse(k3) + 1;
                    }
                    else
                    {
                        GetFees = int.Parse(k3);
                    }
                }
                else
                {
                    GetFees = int.Parse(Fees1.ToString());
                }

                return GetFees;
            }
            catch
            {
                return int.Parse(Fees.ToString().Substring(0, Fees.ToString().IndexOf(".")));
            }
        }
        public static Int32 GetConvertAmount(int iValue, Double dCurrencyValue, string Currency)
        {
            if (!Currency.Equals("INR", StringComparison.OrdinalIgnoreCase)) 
            {
                double convertedAmount = iValue * dCurrencyValue;

                return Convert.ToInt32(Math.Round(convertedAmount));
            }

            return iValue;
        }



    }
}
