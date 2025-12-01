using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Infrastructure.DBCommon;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetCombineAvailability
    {
        private Int32 iRefId;
        public DataTable CombineDT;
        public void CombineTripDT(DataTable OutBoundDT, DataTable InBoundDT)
        {
            iRefId = Convert.ToInt32(OutBoundDT.Rows[0]["RefID"].ToString().Trim());
            CombineDT = Schema.SchemaFlights;

            ArrayList OutRefId_Ar = new ArrayList();
            OutRefId_Ar = GetCommonFunctions.DataTable2ArrayList(OutBoundDT, "RefID", true);
            ArrayList InRefId_Ar = new ArrayList();
            InRefId_Ar = GetCommonFunctions.DataTable2ArrayList(InBoundDT, "RefID", true);

            for (int i = 0; i < OutRefId_Ar.Count; i++)
            {
                DataRow[] OutSelectedDT = OutBoundDT.Select("RefID='" + OutRefId_Ar[i].ToString() + "'");
                for (int j = 0; j < InRefId_Ar.Count; j++)
                {
                    DataRow[] InSelectedDT = InBoundDT.Select("RefID='" + InRefId_Ar[j].ToString() + "'");
                    BindDT(OutSelectedDT.CopyToDataTable(), InSelectedDT.CopyToDataTable());
                }
            }

            for (int i = 0; i < CombineDT.Rows.Count; i++)
            {
                CombineDT.Rows[i]["RowId"] = i + 1;
            }

            CombineDT.AcceptChanges();
        }
        private void BindDT(DataTable OutSelctedDT, DataTable InSelectedDT)
        {

            foreach (DataRow dr in OutSelctedDT.Rows)
            {
                dr["FltType"] = "O";
                dr["RefId"] = iRefId;
                //dr["Flightid"] = "OI" + dr["AirlineID"].ToString() + iRefId.ToString();
            }
            foreach (DataRow dr in InSelectedDT.Rows)
            {
                dr["FltType"] = "I";
                dr["RefId"] = iRefId;
                //dr["Flightid"] = "OI" + dr["AirlineID"].ToString() + iRefId;
            }

            foreach (DataRow dr in OutSelctedDT.Rows)
            {
                CombineDT.ImportRow(dr);
            }
            foreach (DataRow dr in InSelectedDT.Rows)
            {
                CombineDT.ImportRow(dr);
            }
            iRefId++;
        }
    }
}
