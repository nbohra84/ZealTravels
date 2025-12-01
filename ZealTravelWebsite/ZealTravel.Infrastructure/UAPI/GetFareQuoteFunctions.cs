using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetFareQuoteFunctions
    {
        public DataTable SetSector_SSR(string SearchID, string CompanyID, int BookingRef, DataTable dtSSRItinerary, DataTable dtBound)
        {
            try
            {
                foreach (DataRow dr in dtSSRItinerary.Rows)
                {
                    DataRow[] drSegement = dtBound.Select("JourneySellKey='" + dr["AirSegment"].ToString() + "'");
                    if (drSegement.Length > 0)
                    {
                        if (dr["CodeType"].ToString().Equals("B"))
                        {
                            dr["DepartureStation"] = drSegement.CopyToDataTable().Rows[0]["Origin"].ToString();
                            dr["ArrivalStation"] = drSegement.CopyToDataTable().Rows[0]["Destination"].ToString();
                        }
                        else
                        {
                            dr["DepartureStation"] = drSegement.CopyToDataTable().Rows[0]["DepartureStation"].ToString();
                            dr["ArrivalStation"] = drSegement.CopyToDataTable().Rows[0]["ArrivalStation"].ToString();
                        }
                    }
                }
                dtSSRItinerary.AcceptChanges();

                DataTable dtMeal = GetMeal(SearchID, CompanyID, BookingRef, dtSSRItinerary);
                DataTable dtBaggage = GetBaggages(SearchID, CompanyID, BookingRef, dtSSRItinerary, dtBound);
                if (dtMeal != null && dtBaggage != null && dtMeal.Rows.Count > 0 && dtBaggage.Rows.Count > 0)
                {
                    dtMeal.Merge(dtBaggage);
                    dtSSRItinerary = dtMeal;
                }
                else if (dtMeal != null && dtMeal.Rows.Count > 0)
                {
                    dtSSRItinerary = dtMeal;
                }
                else if (dtBaggage != null && dtBaggage.Rows.Count > 0)
                {
                    dtSSRItinerary = dtBaggage;
                }
                if (dtSSRItinerary != null && dtSSRItinerary.Rows.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow dr in dtSSRItinerary.Rows)
                    {
                        dr["RowID"] = i;
                        i++;
                    }
                    dtSSRItinerary.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SetSector_SSR", "GetFareQuoteFunctions", "", SearchID, ex.Message);
            }
            return dtSSRItinerary;
        }
        private DataTable GetBaggages(string SearchID, string CompanyID, int BookingRef, DataTable dtSSRItinerary, DataTable dtBound)
        {
            DataTable dtSsr1 = new DataTable();
            try
            {
                DataRow[] drSelect = dtSSRItinerary.Select("FltType='" + "O" + "' And CodeType='" + "B" + "'");
                if (drSelect.Length > 0)
                {
                    string JourneySellKey = dtBound.Select("FltType='" + "O" + "'").CopyToDataTable().Rows[0]["JourneySellKey"].ToString();
                    dtSsr1 = RemoveConnectingForMeal(SearchID, CompanyID, BookingRef, drSelect.CopyToDataTable(), JourneySellKey);
                }

                drSelect = dtSSRItinerary.Select("FltType='" + "I" + "' And CodeType='" + "B" + "'");
                if (drSelect.Length > 0)
                {
                    string JourneySellKey = dtBound.Select("FltType='" + "I" + "'").CopyToDataTable().Rows[0]["JourneySellKey"].ToString();
                    dtSsr1.Merge(RemoveConnectingForMeal(SearchID, CompanyID, BookingRef, drSelect.CopyToDataTable(), JourneySellKey));
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBaggages", "GetFareQuoteFunctions", "", SearchID, ex.Message);
            }
            return dtSsr1;
        }
        private DataTable GetMeal(string SearchID, string CompanyID, int BookingRef, DataTable dtSSRItinerary)
        {
            DataTable dtSSR = new DataTable();
            try
            {
                ArrayList Ar_JourneySellKey = new ArrayList();

                DataRow[] drSelect = dtSSRItinerary.Select("FltType='" + "O" + "' And CodeType='" + "M" + "'");
                if (drSelect.Length > 0)
                {
                    foreach (DataRow dr in drSelect.CopyToDataTable().Rows)
                    {
                        if (!Ar_JourneySellKey.Contains(dr["AirSegment"].ToString()))
                        {
                            Ar_JourneySellKey.Add(dr["AirSegment"].ToString());
                        }
                    }
                    dtSSR = Remove_Duplicate_SSR(SearchID, CompanyID, BookingRef, drSelect.CopyToDataTable(), Ar_JourneySellKey, "O");
                }

                Ar_JourneySellKey.Clear();
                drSelect = dtSSRItinerary.Select("FltType='" + "I" + "' And CodeType='" + "M" + "'");
                if (drSelect.Length > 0)
                {
                    foreach (DataRow dr in drSelect.CopyToDataTable().Rows)
                    {
                        if (!Ar_JourneySellKey.Contains(dr["AirSegment"].ToString()))
                        {
                            Ar_JourneySellKey.Add(dr["AirSegment"].ToString());
                        }
                    }
                    dtSSR.Merge(Remove_Duplicate_SSR(SearchID, CompanyID, BookingRef, drSelect.CopyToDataTable(), Ar_JourneySellKey, "I"));
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetMeal", "GetFareQuoteFunctions", "", SearchID, ex.Message);
            }
            return dtSSR;
        }
        private DataTable Remove_Duplicate_SSR(string SearchID, string CompanyID, int BookingRef, DataTable dtSSRItinerary, ArrayList Ar_JourneySellKey, string FltType)
        {
            DataTable dtSsr = new DataTable();
            try
            {
                DataRow[] drSelect = dtSSRItinerary.Select("FltType='" + FltType + "'");
                if (drSelect.Length > 0)
                {
                    for (int i = 0; i < Ar_JourneySellKey.Count; i++)
                    {
                        dtSsr.Merge(RemoveConnectingForMeal(SearchID, CompanyID, BookingRef, drSelect.CopyToDataTable(), Ar_JourneySellKey[i].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "Remove_Duplicate_SSR", "GetFareQuoteFunctions", "", SearchID, ex.Message);
            }
            return dtSsr;
        }
        private DataTable RemoveConnectingForMeal(string SearchID, string CompanyID, int BookingRef, DataTable dtSSRItinerary, string AirSegment)
        {
            DataTable dtSSRItineraryUpdated = dtSSRItinerary.Clone();
            try
            {
                string Detail = "";
                int RowID = 0;
                string CarrierCode = "";
                string Code = "";
                string FltType = "";
                string CodeType = "";
                string AirSegment2 = "";
                string Amount = "";
                string Description = "";

                string DepartureStation = "";
                string ArrivalStation = "";

                ArrayList ArSSRcodeList = DBCommon.CommonFunction.DataTable2ArrayList(dtSSRItinerary, "Code", true);
                for (int i = 0; i < ArSSRcodeList.Count; i++)
                {
                    Detail = "";
                    DataRow[] drSelect = dtSSRItinerary.Select("Code='" + ArSSRcodeList[i].ToString() + "' And AirSegment='" + AirSegment + "' And CodeType='" + "M" + "'");
                    if (drSelect.Length > 0)
                    {
                        foreach (DataRow dr in drSelect.CopyToDataTable().Rows)
                        {
                            RowID = (i + 1);
                            CarrierCode = dr["CarrierCode"].ToString();
                            Code = dr["Code"].ToString();
                            FltType = dr["FltType"].ToString();
                            CodeType = dr["CodeType"].ToString();
                            Amount = dr["Amount"].ToString();
                            AirSegment2 = dr["AirSegment"].ToString();
                            Description = dr["Description"].ToString();

                            DepartureStation = dr["DepartureStation"].ToString();
                            ArrivalStation = dr["ArrivalStation"].ToString();

                            Detail += dr["Detail"].ToString();
                            Detail += ":";
                        }
                        Detail = Detail.Substring(0, Detail.Length - 1);

                        DataRow drAdd = dtSSRItineraryUpdated.NewRow();
                        drAdd["RowID"] = RowID;
                        drAdd["CarrierCode"] = CarrierCode;
                        drAdd["Code"] = Code;
                        drAdd["FltType"] = FltType;
                        drAdd["CodeType"] = CodeType;
                        drAdd["Amount"] = Amount;
                        drAdd["Description"] = Description;
                        drAdd["Detail"] = Detail;
                        drAdd["AirSegment"] = AirSegment2;

                        drAdd["DepartureStation"] = DepartureStation;
                        drAdd["ArrivalStation"] = ArrivalStation;

                        dtSSRItineraryUpdated.Rows.Add(drAdd);
                    }
                }

                for (int i = 0; i < ArSSRcodeList.Count; i++)
                {
                    Detail = "";
                    DataRow[] drSelect = dtSSRItinerary.Select("Code='" + ArSSRcodeList[i].ToString() + "' And CodeType='" + "B" + "'");
                    if (drSelect.Length > 0)
                    {
                        foreach (DataRow dr in drSelect.CopyToDataTable().Rows)
                        {
                            RowID = (i + 1);
                            CarrierCode = dr["CarrierCode"].ToString();
                            Code = dr["Code"].ToString();
                            FltType = dr["FltType"].ToString();
                            CodeType = dr["CodeType"].ToString();
                            Amount = dr["Amount"].ToString();
                            Description = dr["Description"].ToString();
                            AirSegment2 = dr["AirSegment"].ToString();

                            DepartureStation = dr["DepartureStation"].ToString();
                            ArrivalStation = dr["ArrivalStation"].ToString();

                            Detail += dr["Detail"].ToString();
                            Detail += ":";
                        }
                        Detail = Detail.Substring(0, Detail.Length - 1);

                        DataRow drAdd = dtSSRItineraryUpdated.NewRow();
                        drAdd["RowID"] = RowID;
                        drAdd["CarrierCode"] = CarrierCode;
                        drAdd["Code"] = Code;
                        drAdd["FltType"] = FltType;
                        drAdd["CodeType"] = CodeType;
                        drAdd["Amount"] = Amount;
                        drAdd["Description"] = Description;
                        drAdd["Detail"] = Detail;
                        drAdd["AirSegment"] = AirSegment2;

                        drAdd["DepartureStation"] = DepartureStation;
                        drAdd["ArrivalStation"] = ArrivalStation;
                        dtSSRItineraryUpdated.Rows.Add(drAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "RemoveConnectingForMeal", "GetFareQuoteFunctions", "", SearchID, ex.Message);
            }
            return dtSSRItineraryUpdated;
        }
        //=================================================================================================================================
        public void UpdateFlightTypeInSSR(string SearchID, string CompanyID, int BookingRef, DataTable dtSSRItinerary, DataTable dtBound)
        {
            try
            {
                DataRow[] drResult = dtBound.Select("FltType='" + "O" + "'");

                ArrayList ArInboundKey = new ArrayList();
                ArrayList ArOutboundKey = new ArrayList();
                if (drResult.Length > 0)
                {
                    ArOutboundKey = DBCommon.CommonFunction.DataTable2ArrayList(drResult.CopyToDataTable(), "JourneySellKey", true);
                }

                drResult = dtBound.Select("FltType='" + "I" + "'");
                if (drResult.Length > 0)
                {
                    ArInboundKey = DBCommon.CommonFunction.DataTable2ArrayList(drResult.CopyToDataTable(), "JourneySellKey", true);
                }

                if (dtSSRItinerary != null && dtSSRItinerary.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSSRItinerary.Rows)
                    {
                        if (ArOutboundKey.Contains(dr["AirSegment"].ToString()))
                        {
                            dr["FltType"] = "O";
                        }
                        else if (ArInboundKey != null && ArInboundKey.Count > 0 && ArInboundKey.Contains(dr["AirSegment"].ToString()))
                        {
                            dr["FltType"] = "I";
                        }
                    }
                    dtSSRItinerary.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "UpdateFlightTypeInSSR", "GetFareQuoteFunctions", "", SearchID, ex.Message);
            }
        }
        //==================================================================================================================================
    }
}

