using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.AirCalculations
{
    class Calculation_b2c
    {
        public void CalculateDeal(string SearchCriteria, bool IsOnline, bool IsDeal, DataTable dtBound, DataTable dtCommission, DataTable dtST, DataTable dtCfee, string UserType, DataTable dtPriceTypeCommission)
        {
            ArrayList arRefId = DBCommon.CommonFunction.DataTable2ArrayList(dtBound, "RefID", true);
            for (int i = 0; i < arRefId.Count; i++)
            {
                CalculateDeal(SearchCriteria, IsOnline, IsDeal, arRefId[i].ToString(), dtBound, dtCommission, dtST, dtCfee, UserType, dtPriceTypeCommission);
            }
        }
        private void CalculateDeal(string SearchCriteria, bool IsOnline, bool IsDeal, string RefID, DataTable dtBound, DataTable dtCommission, DataTable dtST, DataTable dtCfee, string UserType, DataTable dtPriceTypeCommission)
        {
            DataRow[] rows = dtBound.Select("RefID ='" + RefID + "'");
            if (rows.Length > 0)
            {

                Decimal Adt_SF = 0;
                Decimal Adt_ST = 0;
                Decimal cfee = 0;
                Decimal Chd_SF = 0;
                Decimal Chd_ST = 0;
                int count = 0;

                foreach (DataRow dr in rows)
                {
                    int Adt = int.Parse(dr["Adt"].ToString());
                    int Chd = int.Parse(dr["Chd"].ToString());
                    int Inf = int.Parse(dr["Inf"].ToString());
                    string cc = dr["CarrierCode"].ToString();
                    if (cc.Equals("CI") || cc.Equals("CA"))
                    {

                    }

                    if (count.Equals(0))
                    {
                        Adt_SF = CommissionCalC.ServiceFee("A", dr, dtCommission, dtPriceTypeCommission); // service fee for company/sub/customers
                        cfee = CommissionCalC.GET_Cfee(UserType, dr, dtCfee);
                    }
                    dr["Adt_SF"] = Adt_SF;

                    if (dr.Table.Columns.Contains("TotalCfee"))
                    {
                        dr["TotalCfee"] = cfee;
                    }

                    if (count.Equals(0))
                    {
                        Adt_ST = CommissionCalC.ServiceTax("A", dr, dtST);
                    }

                    dr["Adt_ST"] = Adt_ST;
                    dr.AcceptChanges();

                    if (Chd > 0)
                    {
                        if (count.Equals(0))
                        {
                            Chd_SF = CommissionCalC.ServiceFee("C", dr, dtCommission, dtPriceTypeCommission);//service fee for company/sub/customer
                        }
                        dr["Chd_SF"] = Chd_SF;

                        if (count.Equals(0))
                        {
                            Chd_ST = CommissionCalC.ServiceTax("C", dr, dtST);
                        }

                        dr["Chd_ST"] = Chd_ST;
                        dr.AcceptChanges();
                    }

                    dtBound.AcceptChanges();
                    count++;
                }
            }
        }
    }
}
