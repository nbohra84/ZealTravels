using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ZealTravel.Infrastructure.DBCommon;
using ZealTravel.Common.Helpers;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetSsrAvailability
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        //-----------------------------------------------------------------------------------------------
        public GetSsrAvailability(string Searchid)
        {
            this.Searchid = Searchid;
        }
        //================================================================================================================================================
        public DataTable GetRoundWaySSR(DataTable dtOutbound, DataTable dtInbound)
        {
            //Outbound------------------------------------------------------------------------------------------------------------------------
            DataTable dtAddOnFlights = Schema.SchemaSSR;

            try
            {
                bool Is48Hour = false;
                bool Is24Hour = false;
                bool Is6Hour = false;

                bool IsBoeing = false;
                bool IsQ400 = false;

                bool T0301_1130 = false;
                bool T1131_1500 = false;
                bool T1501_1900 = false;
                bool T1901_2300 = false;
                bool T2301_0300 = false;

                GetDataBaseData objDataBaseData = new GetDataBaseData();
                DataTable dtBaggages = objDataBaseData.GetSSR(Searchid, "B", dtOutbound.Rows[0]["Sector"].ToString(), Is48Hour, Is24Hour, Is6Hour, IsBoeing, IsQ400, T0301_1130, T1131_1500, T1501_1900, T1901_2300, T2301_0300);

                int iEquipmentType = 0;
                string EquipmentType = dtOutbound.Rows[0]["EquipmentType"].ToString();
                bool ForBoeing = int.TryParse(dtOutbound.Rows[0]["EquipmentType"].ToString(), out iEquipmentType);
                if (ForBoeing)
                {
                    IsBoeing = true;
                }
                else
                {
                    IsQ400 = true;
                }

                Is48Hour = DateHelper.Is48Hours(dtOutbound.Rows[0]["DepartureTime"].ToString());
                if (Is48Hour)
                {
                    Is24Hour = false;
                    Is6Hour = false;
                }
                else
                {
                    Is24Hour = DateHelper.Is24Hours(dtOutbound.Rows[0]["DepartureTime"].ToString());
                    if (Is24Hour)
                    {
                        Is6Hour = false;
                    }
                    else
                    {
                        Is6Hour = DateHelper.Is6Hours(dtOutbound.Rows[0]["DepartureTime"].ToString());
                    }
                }

                int iBetweenTime = DateHelper.BetweenTime(dtOutbound.Rows[0]["DepartureDate"].ToString());
                if (iBetweenTime.Equals(1))
                {
                    T0301_1130 = true;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(2))
                {
                    T0301_1130 = false;
                    T1131_1500 = true;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(3))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = true;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(4))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = true;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(5))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = true;
                }

                DataTable dtMeals = objDataBaseData.GetSSR(Searchid, "M", dtOutbound.Rows[0]["Sector"].ToString(), Is48Hour, Is24Hour, Is6Hour, IsBoeing, IsQ400, T0301_1130, T1131_1500, T1501_1900, T1901_2300, T2301_0300);
                if (dtBaggages != null && dtBaggages.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBaggages.Rows)
                    {
                        DataRow drAdd = dtAddOnFlights.NewRow();
                        drAdd["Rowid"] = dr["Rowid"];
                        drAdd["DepartureStation"] = dtOutbound.Rows[0]["Origin"].ToString();
                        drAdd["ArrivalStation"] = dtOutbound.Rows[0]["Destination"].ToString();
                        drAdd["Code"] = dr["Code"];
                        drAdd["CodeType"] = dr["SSRType"];
                        drAdd["Amount"] = dr["Amount"];
                        drAdd["Description"] = dr["Description"];
                        drAdd["FltType"] = dtOutbound.Rows[0]["FltType"].ToString();
                        dtAddOnFlights.Rows.Add(drAdd);
                    }
                }
                if (dtMeals != null && dtMeals.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMeals.Rows)
                    {
                        DataRow drAdd = dtAddOnFlights.NewRow();
                        drAdd["Rowid"] = dr["Rowid"];
                        drAdd["DepartureStation"] = dtOutbound.Rows[0]["DepartureStation"].ToString();
                        drAdd["ArrivalStation"] = dtOutbound.Rows[0]["ArrivalStation"].ToString();
                        drAdd["Code"] = dr["Code"];
                        drAdd["CodeType"] = dr["SSRType"];
                        drAdd["Amount"] = dr["Amount"];
                        drAdd["Description"] = dr["Description"];
                        drAdd["FltType"] = dtOutbound.Rows[0]["FltType"].ToString();
                        dtAddOnFlights.Rows.Add(drAdd);
                    }
                }
                //Inbound----------------------------------------------------------------------------------------------------------------------------------------------------------

                Is48Hour = false;
                Is24Hour = false;
                Is6Hour = false;

                IsBoeing = false;
                IsQ400 = false;

                T0301_1130 = false;
                T1131_1500 = false;
                T1501_1900 = false;
                T1901_2300 = false;
                T2301_0300 = false;

                dtBaggages = objDataBaseData.GetSSR(Searchid, "B", dtInbound.Rows[0]["Sector"].ToString(), Is48Hour, Is24Hour, Is6Hour, IsBoeing, IsQ400, T0301_1130, T1131_1500, T1501_1900, T1901_2300, T2301_0300);

                iEquipmentType = 0;
                EquipmentType = dtInbound.Rows[0]["EquipmentType"].ToString();
                ForBoeing = int.TryParse(dtInbound.Rows[0]["EquipmentType"].ToString(), out iEquipmentType);
                if (ForBoeing)
                {
                    IsBoeing = true;
                }
                else
                {
                    IsQ400 = true;
                }

                Is48Hour = DateHelper.Is48Hours(dtInbound.Rows[0]["DepartureTime"].ToString());
                if (Is48Hour)
                {
                    Is24Hour = true;
                    Is6Hour = true;
                }
                else
                {
                    Is24Hour = DateHelper.Is24Hours(dtInbound.Rows[0]["DepartureTime"].ToString());
                    if (Is24Hour)
                    {
                        Is6Hour = true;
                    }
                    else
                    {
                        Is6Hour = DateHelper.Is6Hours(dtInbound.Rows[0]["DepartureTime"].ToString());
                    }
                }

                iBetweenTime = DateHelper.BetweenTime(dtInbound.Rows[0]["DepartureDate"].ToString());
                if (iBetweenTime.Equals(1))
                {
                    T0301_1130 = true;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(2))
                {
                    T0301_1130 = false;
                    T1131_1500 = true;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(3))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = true;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(4))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = true;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(5))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = true;
                }

                dtMeals = objDataBaseData.GetSSR(Searchid, "M", dtInbound.Rows[0]["Sector"].ToString(), Is48Hour, Is24Hour, Is6Hour, IsBoeing, IsQ400, T0301_1130, T1131_1500, T1501_1900, T1901_2300, T2301_0300);
                if (dtBaggages != null && dtBaggages.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBaggages.Rows)
                    {
                        DataRow drAdd = dtAddOnFlights.NewRow();
                        drAdd["Rowid"] = dr["Rowid"];
                        drAdd["DepartureStation"] = dtInbound.Rows[0]["Origin"].ToString();
                        drAdd["ArrivalStation"] = dtInbound.Rows[0]["Destination"].ToString();
                        drAdd["Code"] = dr["Code"];
                        drAdd["CodeType"] = dr["SSRType"];
                        drAdd["Amount"] = dr["Amount"];
                        drAdd["Description"] = dr["Description"];
                        drAdd["FltType"] = dtInbound.Rows[0]["FltType"].ToString();
                        dtAddOnFlights.Rows.Add(drAdd);
                    }
                }
                if (dtMeals != null && dtMeals.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMeals.Rows)
                    {
                        DataRow drAdd = dtAddOnFlights.NewRow();
                        drAdd["Rowid"] = dr["Rowid"];
                        drAdd["DepartureStation"] = dtInbound.Rows[0]["DepartureStation"].ToString();
                        drAdd["ArrivalStation"] = dtInbound.Rows[0]["ArrivalStation"].ToString();
                        drAdd["Code"] = dr["Code"];
                        drAdd["CodeType"] = dr["SSRType"];
                        drAdd["Amount"] = dr["Amount"];
                        drAdd["Description"] = dr["Description"];
                        drAdd["FltType"] = dtInbound.Rows[0]["FltType"].ToString();
                        dtAddOnFlights.Rows.Add(drAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg("", 0, "GetRoundWaySSR", "air_spicejet", "", Searchid, ex.Message + "," + ex.StackTrace);
            }
            return dtAddOnFlights;
        }
        public DataTable GetOneWaySSR(DataTable dtBound)
        {
            DataTable dtAddOnFlights = Schema.SchemaSSR;

            try
            {
                bool Is48Hour = false;
                bool Is24Hour = false;
                bool Is6Hour = false;

                bool IsBoeing = false;
                bool IsQ400 = false;

                bool T0301_1130 = false;
                bool T1131_1500 = false;
                bool T1501_1900 = false;
                bool T1901_2300 = false;
                bool T2301_0300 = false;

                GetDataBaseData objDataBaseData = new GetDataBaseData();
                DataTable dtBaggages = objDataBaseData.GetSSR(Searchid, "B", dtBound.Rows[0]["Sector"].ToString(), Is48Hour, Is24Hour, Is6Hour, IsBoeing, IsQ400, T0301_1130, T1131_1500, T1501_1900, T1901_2300, T2301_0300);

                int iEquipmentType = 0;
                string EquipmentType = dtBound.Rows[0]["EquipmentType"].ToString();
                bool ForBoeing = int.TryParse(dtBound.Rows[0]["EquipmentType"].ToString(), out iEquipmentType);
                if (ForBoeing)
                {
                    IsBoeing = true;
                }
                else
                {
                    IsQ400 = true;
                }

                Is48Hour = DateHelper.Is48Hours(dtBound.Rows[0]["DepartureTime"].ToString());
                if (Is48Hour)
                {
                    Is24Hour = false;
                    Is6Hour = false;
                }
                else
                {
                    Is24Hour = DateHelper.Is24Hours(dtBound.Rows[0]["DepartureTime"].ToString());
                    if (Is24Hour)
                    {
                        Is6Hour = false;
                    }
                    else
                    {
                        Is6Hour = DateHelper.Is6Hours(dtBound.Rows[0]["DepartureTime"].ToString());
                    }
                }

                int iBetweenTime = DateHelper.BetweenTime(dtBound.Rows[0]["DepartureDate"].ToString());
                if (iBetweenTime.Equals(1))
                {
                    T0301_1130 = true;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(2))
                {
                    T0301_1130 = false;
                    T1131_1500 = true;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(3))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = true;
                    T1901_2300 = false;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(4))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = true;
                    T2301_0300 = false;
                }
                else if (iBetweenTime.Equals(5))
                {
                    T0301_1130 = false;
                    T1131_1500 = false;
                    T1501_1900 = false;
                    T1901_2300 = false;
                    T2301_0300 = true;
                }

                DataTable dtMeals = objDataBaseData.GetSSR(Searchid, "M", dtBound.Rows[0]["Sector"].ToString(), Is48Hour, Is24Hour, Is6Hour, IsBoeing, IsQ400, T0301_1130, T1131_1500, T1501_1900, T1901_2300, T2301_0300);
                if (dtBaggages != null && dtBaggages.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBaggages.Rows)
                    {
                        DataRow drAdd = dtAddOnFlights.NewRow();
                        drAdd["Rowid"] = dr["Rowid"];
                        drAdd["DepartureStation"] = dtBound.Rows[0]["Origin"].ToString();
                        drAdd["ArrivalStation"] = dtBound.Rows[0]["Destination"].ToString();
                        drAdd["Code"] = dr["Code"];
                        drAdd["CodeType"] = dr["SSRType"];
                        drAdd["Amount"] = dr["Amount"];
                        drAdd["Description"] = dr["Description"];
                        drAdd["FltType"] = dtBound.Rows[0]["FltType"].ToString();
                        dtAddOnFlights.Rows.Add(drAdd);
                    }
                }
                if (dtMeals != null && dtMeals.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMeals.Rows)
                    {
                        DataRow drAdd = dtAddOnFlights.NewRow();
                        drAdd["Rowid"] = dr["Rowid"];
                        drAdd["DepartureStation"] = dtBound.Rows[0]["DepartureStation"].ToString();
                        drAdd["ArrivalStation"] = dtBound.Rows[0]["ArrivalStation"].ToString();
                        drAdd["Code"] = dr["Code"];
                        drAdd["CodeType"] = dr["SSRType"];
                        drAdd["Amount"] = dr["Amount"];
                        drAdd["Description"] = dr["Description"];
                        drAdd["FltType"] = dtBound.Rows[0]["FltType"].ToString();
                        dtAddOnFlights.Rows.Add(drAdd);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg("", 0, "GetOneWaySSR", "air_spicejet", "", Searchid, ex.Message + "," + ex.StackTrace);
            }
            return dtAddOnFlights;
        }

        //if (EquipmentType.Equals("737") || EquipmentType.Equals("707") || EquipmentType.Equals("727") || EquipmentType.Equals("747")
        //    || EquipmentType.Equals("717") || EquipmentType.Equals("717"))
        //{
        //    IsBoeing = true;
        //}
        //if (EquipmentType.Equals("Q400") || EquipmentType.Equals("400") || EquipmentType.Equals("D400"))
        //{
        //    IsQ400 = true;
        //}
    }
}
