using System.Collections;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public class PaymentGatewayHelper
    {
        public static Hashtable GetPaymentTypeAirline(string strPayment)
        {
            Hashtable htPayment = new Hashtable();
            try
            {

                bool IsFixed = false;
                bool IsPercent = false;
                Decimal dCharge = 0;
                string CardName = string.Empty;//DC,NET,CC,Prepaid
                string MerchantCode = string.Empty;
                string Card_Type_Name = string.Empty;//MASTER,VISA,AMEX...

                string[] split = strPayment.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    string value = split[i].ToString().Replace("--", "-");
                    string[] Valuesplit = value.Split('-');

                    if (Valuesplit[0].ToString().IndexOf("FIXED") != -1)
                    {
                        Boolean.TryParse(Valuesplit[1].ToString().Trim(), out IsFixed);
                    }
                    else if (Valuesplit[0].ToString().IndexOf("Charges") != -1)
                    {
                        dCharge = Convert.ToDecimal(Valuesplit[1].ToString().Trim());
                    }
                    else if (Valuesplit[0].ToString().IndexOf("PERCNT") != -1)
                    {
                        Boolean.TryParse(Valuesplit[1].ToString().Trim(), out IsPercent);
                    }
                    else if (Valuesplit[0].ToString().IndexOf("PG_Name") != -1)
                    {
                        CardName = Valuesplit[3].ToString().Trim();
                    }
                    else if (Valuesplit[0].ToString().IndexOf("CardName") != -1)
                    {
                        Card_Type_Name = Valuesplit[1].ToString().Trim();
                    }
                    else if (Valuesplit[0].ToString().IndexOf("MerchantCode") != -1)
                    {
                        MerchantCode = Valuesplit[1].ToString().Trim();
                    }
                }

                htPayment.Add("FXD", IsFixed);
                htPayment.Add("PER", IsPercent);
                htPayment.Add("CHG", dCharge);
                htPayment.Add("CN", CardName);
                htPayment.Add("CTN", Card_Type_Name);
                htPayment.Add("MER", MerchantCode);
            }
            catch (Exception ex)
            {
                //dbCommon.Logger.dbLogg(Initializer.get_StaffID(), 0, "getPaymentTypeAirline", "FlightDisplay.aspx", "", strPayment, ex.Message);
            }
            return htPayment;
        }
    }
}
