using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.Helpers
{
    public class DatatableHelper
    {
        public static DataTable FilterRowID(DataTable Dt)
        {
            if (Dt.Columns.Contains("TotalFare1").Equals(true))
            {
                Dt.Columns.Remove("TotalFare1");
                Dt.AcceptChanges();
            }
            if (Dt.Columns.Contains("RowID1").Equals(true))
            {
                Dt.Columns.Remove("RowID1");
                Dt.AcceptChanges();
            }

            Dt.Columns.Add("TotalFare1", typeof(Int32), "Convert(TotalFare, 'System.Int32')");
            Dt.Columns.Add("RowID1", typeof(Int32), "Convert(RowID, 'System.Int32')");

            DataView dv = new DataView(Dt);
            dv.Sort = "TotalFare1 ASC,RowID1";
            Dt = dv.ToTable();
            Dt.Columns.Remove("TotalFare1");
            Dt.Columns.Remove("RowID1");
            Dt.AcceptChanges();
            return Dt;
        }
    }
}
