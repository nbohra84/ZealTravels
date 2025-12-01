using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
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
                taxes += Convert.ToDecimal(dr["Adt_TRF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_ASF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_PSF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_RCS"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_TF"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_CGST"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_IGST"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_SGST"].ToString());
                taxes += Convert.ToDecimal(dr["Adt_EXT"].ToString());

                dr["AdtTotalTax"] = taxes;
                dr["AdtTotalFare"] = Convert.ToDecimal(dr["Adt_BASIC"].ToString()) + taxes;

                if (Chd > 0)
                {
                    taxes = 0;
                    taxes += Convert.ToDecimal(dr["Chd_YQ"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_TRF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_ASF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_PSF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_RCS"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_TF"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_CGST"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_IGST"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_SGST"].ToString());
                    taxes += Convert.ToDecimal(dr["Chd_EXT"].ToString());

                    dr["ChdTotalTax"] = taxes;
                    dr["ChdTotalFare"] = Convert.ToDecimal(dr["Chd_BASIC"].ToString()) + taxes;
                }

                if (Inf > 0)
                {
                    taxes = 0;
                    taxes += Convert.ToDecimal(dr["Inf_YQ"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_TRF"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_ASF"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_PSF"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_RCS"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_TF"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_CGST"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_IGST"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_SGST"].ToString());
                    taxes += Convert.ToDecimal(dr["Inf_EXT"].ToString());

                    dr["InfTotalTax"] = taxes;
                    dr["InfTotalFare"] = Convert.ToDecimal(dr["Inf_BASIC"].ToString()) + taxes;
                }

                dr["TotalFare"] = Adt * Convert.ToDecimal(dr["AdtTotalFare"].ToString()) + Chd * Convert.ToDecimal(dr["ChdTotalFare"].ToString()) + Inf * Convert.ToDecimal(dr["InfTotalFare"].ToString());
            }

            dtBound.AcceptChanges();
        }
    }
}
