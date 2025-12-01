using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.TBO
{
    class SetDealCalculator
    {
        public void DealCalculatorPasseneger(DataTable dtBound)
        {
            short Adt = short.Parse(dtBound.Rows[0]["Adt"].ToString().Trim());
            short Chd = short.Parse(dtBound.Rows[0]["Chd"].ToString().Trim());
            short Inf = short.Parse(dtBound.Rows[0]["Inf"].ToString().Trim());

            decimal taxes = 0;
            foreach (DataRow dr in dtBound.Rows)
            {
                taxes = 0;
                taxes += Convert.ToDecimal(dr["Adt_YQ"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_PSF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_UDF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_AUDF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_CUTE"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_GST"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_TF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_CESS"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_EX"].ToString());

                dr["AdtTotalTax"] = taxes;
                dr["AdtTotalFare"] = Convert.ToDecimal(dr["Adt_BASIC"].ToString()) + taxes;

                if (Chd > 0)
                {
                    taxes = 0;
                    taxes += Convert.ToDecimal(dr["Chd_YQ"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_PSF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_UDF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_AUDF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_CUTE"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_GST"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_TF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_CESS"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_EX"].ToString());

                    dr["ChdTotalTax"] = taxes;
                    dr["ChdTotalFare"] = Convert.ToDecimal(dr["Chd_BASIC"].ToString()) + taxes;
                }

                if (Inf > 0)
                {
                    dr["InfTotalTax"] = Convert.ToDecimal(dr["Inf_Tax"].ToString());
                    dr["InfTotalFare"] = Convert.ToDecimal(dr["Inf_BASIC"].ToString()) + Convert.ToDecimal(dr["Inf_Tax"].ToString());
                }

                dr["TotalFare"] = Adt * Convert.ToDecimal(dr["AdtTotalFare"].ToString()) + Chd * Convert.ToDecimal(dr["ChdTotalFare"].ToString()) + Inf * Convert.ToDecimal(dr["InfTotalFare"].ToString());
            }

            dtBound.AcceptChanges();
        }
    }
}
