using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data;

namespace ZealTravel.Infrastructure.TBO
{
    class GetFlightModifierFunctions
    {
        public static DataTable ValidateRTFlights(DataTable dtBound)
        {
            ArrayList RemoveAr = new ArrayList();
            DataTable dtBoundUpdated = Schema.SchemaFlights;

            ArrayList RefAR = GetCommonFunctions.DataTable2ArrayList(dtBound, "RefID", true);
            if (RefAR.Count > 0)
            {
                for (int i = 0; i < RefAR.Count; i++)
                {
                    DataRow[] rows1 = dtBound.Select("RefID ='" + RefAR[i].ToString() + "' And FltType='" + "O" + "'");
                    DataRow[] rows2 = dtBound.Select("RefID ='" + RefAR[i].ToString() + "' And FltType='" + "I" + "'");
                    if (rows1.Length.Equals(0) || rows2.Length.Equals(0))
                    {
                        RemoveAr.Add(RefAR[i].ToString().Trim());
                    }
                }
            }

            if (RemoveAr.Count > 0)
            {
                foreach (DataRow dr in dtBound.Rows)
                {
                    if (RemoveAr.Contains(dr["RefID"].ToString().Trim()).Equals(false))
                    {
                        dtBoundUpdated.ImportRow(dr);
                    }
                }
            }

            if (RemoveAr.Count > 0)
            {
                return dtBoundUpdated;
            }
            else
            {
                return dtBound;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------
        public static DataTable Tempxml_Schema
        {
            get
            {
                DataTable Table = new DataTable();
                Table.Columns.Add("RefID", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("Fare", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FareBreakdown", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Segments", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("FareRules", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ResultIndex", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Source", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("IsLCC", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("IsRefundable", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("AirlineRemark", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("LastTicketDate", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("TicketAdvisory", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("AirlineCode", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("ValidatingAirline", typeof(string)).DefaultValue = string.Empty;
                return Table;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------
        public static string GetPriceType(string Source, string ClassOfService, string CarrierCode, string AirlineRemark, string FareBasisCode)
        {
            string PriceType = "PUB";
            try
            {
                if (Source.Equals("3"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("4"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("5"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("6"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("10"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("13"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("14"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("15"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("17"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("19"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("24"))
                {
                    PriceType = "COU";
                }
                else if (Source.Equals("25"))
                {
                    PriceType = "COU";
                }
                else if (Source.Equals("26"))
                {
                    PriceType = "COU";
                }
                else if (Source.Equals("27"))
                {
                    PriceType = "TBF";
                }
                else if (Source.Equals("28"))
                {
                    PriceType = "TBF";
                }
                else if (Source.Equals("29"))
                {
                    PriceType = "TBF";
                }
                else if (Source.Equals("30"))
                {
                    PriceType = "COU";
                }
                else if (Source.Equals("31"))
                {
                    PriceType = "COU";
                }
                else if (Source.Equals("32"))
                {
                    PriceType = "COU";
                }
                else if (Source.Equals("36"))
                {
                    PriceType = "COR";
                }
                else if (Source.Equals("37"))
                {
                    PriceType = "COR";
                }
                else if (Source.Equals("38"))
                {
                    PriceType = "COR";
                }
                else if (Source.Equals("42"))
                {
                    PriceType = "DST";
                }
                else if (Source.Equals("43"))
                {
                    PriceType = "DST";
                }
                else if (Source.Equals("44"))
                {
                    PriceType = "DST";
                }
                else if (Source.Equals("46"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("47"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("48"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("49"))
                {
                    PriceType = "PUB";
                }
                else if (Source.Equals("50"))
                {
                    PriceType = "PUB";
                }

                if (AirlineRemark.IndexOf("SME") != -1)
                {
                    PriceType = "SME";
                }

                if (CarrierCode.Equals("SG"))
                {
                    if (AirlineRemark.IndexOf("Family Fare") != -1)
                    {
                        PriceType = "Family";
                    }
                    else if (ClassOfService.Equals("HO"))
                    {
                        PriceType = "HB";
                    }
                }
                else if (CarrierCode.Equals("G8"))
                {
                    if (AirlineRemark.IndexOf("Family Fare") != -1)
                    {
                        PriceType = "Family";
                    }
                    else if (AirlineRemark.IndexOf("Hand Bag") != -1)
                    {
                        PriceType = "HB";
                    }
                }
                else if (CarrierCode.Equals("6E"))
                {
                    if (AirlineRemark.IndexOf("Family Fare") != -1)
                    {
                        PriceType = "Family";
                    }
                    else if (AirlineRemark.IndexOf("Hand Bag") != -1)
                    {
                        PriceType = "HB";
                    }
                }

                if (CarrierCode.Equals("IX"))
                {
                    if (AirlineRemark.IndexOf("FBA 20") != -1)
                    {
                        PriceType = "Exp 20Kg";
                    }
                    else if (AirlineRemark.IndexOf("FBA 25") != -1)
                    {
                        PriceType = "Exp 25Kg";
                    }
                    else if (AirlineRemark.IndexOf("FBA 30") != -1)
                    {
                        PriceType = "Exp 30Kg";
                    }
                    else if (AirlineRemark.IndexOf("FBA 40") != -1)
                    {
                        PriceType = "Exp 40Kg";
                    }
                }

                if (CarrierCode.Equals("SG") && FareBasisCode.Equals("UCRP"))
                {
                    PriceType = "COR";
                }

                PriceType = "*" + PriceType;
            }
            catch (Exception ex)
            {

            }
            return PriceType;
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------
        public static void SetJourneyTimeforConnectionFlights(DataTable dtBound)
        {
            ArrayList RefAR = GetCommonFunctions.DataTable2ArrayList(dtBound, "RefID", true);
            if (RefAR.Count > 0)
            {
                for (int i = 0; i < RefAR.Count; i++)
                {
                    DataRow[] dr = dtBound.Select("RefID='" + RefAR[i].ToString() + "'");
                    if (dr.Length > 1)
                    {
                        SetJourneyTimeforConnectionByRefid(dtBound, RefAR[i].ToString());
                    }
                }
            }
        }
        private static void SetJourneyTimeforConnectionByRefid(DataTable dtBound, string Refid)
        {
            int jtm = 0;
            DataRow[] dr = dtBound.Select("RefID='" + Refid + "'");
            if (dr.Length.Equals(2))
            {
                int journey1 = int.Parse(dr.CopyToDataTable().Rows[0]["JourneyTime"].ToString());
                int journey2 = int.Parse(dr.CopyToDataTable().Rows[1]["JourneyTime"].ToString());

                if (journey1 > journey2)
                {
                    jtm = journey1;
                }
                else
                {
                    jtm = journey2;
                }
            }
            else if (dr.Length.Equals(3))
            {
                int journey1 = int.Parse(dr.CopyToDataTable().Rows[0]["JourneyTime"].ToString());
                int journey2 = int.Parse(dr.CopyToDataTable().Rows[1]["JourneyTime"].ToString());
                int journey3 = int.Parse(dr.CopyToDataTable().Rows[2]["JourneyTime"].ToString());

                if (journey1 > journey2 && journey1 > journey3)
                {
                    jtm = journey1;
                }
                else if (journey2 > journey1 && journey2 > journey3)
                {
                    jtm = journey2;
                }
                else if (journey3 > journey1 && journey3 > journey2)
                {
                    jtm = journey3;
                }
                else
                {
                    jtm = journey3;
                }
            }

            if (jtm > 0)
            {
                DataRow[] rows = dtBound.Select("RefID='" + Refid + "'");
                if (rows.Length > 0)
                {
                    foreach (DataRow drSelect in rows)
                    {
                        drSelect["JourneyTime"] = jtm;
                    }
                }

                dtBound.AcceptChanges();
            }
        }

        public static void SetJourneyTimeforConnectionFlights_RT(DataTable dtBound)
        {
            try
            {
                ArrayList RefAR = GetCommonFunctions.DataTable2ArrayList(dtBound, "RefID", true);
                if (RefAR.Count > 0)
                {
                    for (int i = 0; i < RefAR.Count; i++)
                    {
                        string FltType = "O";
                        DataRow[] dr = dtBound.Select("RefID='" + RefAR[i].ToString() + "' And FltType='" + FltType + "'");
                        if (dr.Length > 1)
                        {
                            SetJourneyTimeforConnectionByRefid_RT(dtBound, RefAR[i].ToString(), FltType);
                        }

                        FltType = "I";
                        dr = dtBound.Select("RefID='" + RefAR[i].ToString() + "' And FltType='" + FltType + "'");
                        if (dr.Length > 1)
                        {
                            SetJourneyTimeforConnectionByRefid_RT(dtBound, RefAR[i].ToString(), FltType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private static void SetJourneyTimeforConnectionByRefid_RT(DataTable dtBound, string RefID, string FltType)
        {
            int jtm = 0;
            DataRow[] dr = dtBound.Select("RefID='" + RefID + "' And FltType='" + FltType + "'");
            if (dr.Length.Equals(2))
            {
                int journey1 = int.Parse(dr.CopyToDataTable().Rows[0]["JourneyTime"].ToString());
                int journey2 = int.Parse(dr.CopyToDataTable().Rows[1]["JourneyTime"].ToString());

                if (journey1 > journey2)
                {
                    jtm = journey1;
                }
                else
                {
                    jtm = journey2;
                }
            }
            else if (dr.Length.Equals(3))
            {
                int journey1 = int.Parse(dr.CopyToDataTable().Rows[0]["JourneyTime"].ToString());
                int journey2 = int.Parse(dr.CopyToDataTable().Rows[1]["JourneyTime"].ToString());
                int journey3 = int.Parse(dr.CopyToDataTable().Rows[2]["JourneyTime"].ToString());

                if (journey1 > journey2 && journey1 > journey3)
                {
                    jtm = journey1;
                }
                else if (journey2 > journey1 && journey2 > journey3)
                {
                    jtm = journey2;
                }
                else if (journey3 > journey1 && journey3 > journey2)
                {
                    jtm = journey3;
                }
                else
                {
                    jtm = journey3;
                }
            }

            if (jtm > 0)
            {
                DataRow[] rows = dtBound.Select("RefID='" + RefID + "' And FltType='" + FltType + "'");
                if (rows.Length > 0)
                {
                    foreach (DataRow drSelect in rows)
                    {
                        drSelect["JourneyTime"] = jtm;
                    }
                }

                dtBound.AcceptChanges();
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------
    }
}
