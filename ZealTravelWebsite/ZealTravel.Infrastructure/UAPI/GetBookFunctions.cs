using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetBookFunctions
    {
        public static bool IsValidSSR(DataTable dtPassenger)
        {
            bool IsFind = false;
            foreach (DataRow dr in dtPassenger.Rows)
            {
                if (dr["MealCode_O"].ToString().Trim().Length > 2)
                {
                    IsFind = true;
                    break;
                }
                if (dr["BaggageCode_O"].ToString().Trim().Length > 2)
                {
                    IsFind = true;
                    break;
                }
                if (dr["MealCode_I"].ToString().Trim().Length > 2)
                {
                    IsFind = true;
                    break;
                }
                if (dr["BaggageCode_I"].ToString().Trim().Length > 2)
                {
                    IsFind = true;
                    break;
                }
            }
            return IsFind;
        }
        public static int GetJourneyTime(string journey)
        {
            try
            {
                //P0DT2H15M0S
                int iDay = 0;
                string day = journey.Substring(1, journey.IndexOf("DT") - 1);
                int.TryParse(day, out iDay);

                journey = journey.Substring(journey.IndexOf("DT"), (journey.Length - journey.IndexOf("DT")));
                journey = journey.Replace("DT", "").Trim();

                string hh = journey.Substring(0, journey.IndexOf("H"));
                string mm = journey.Substring(journey.IndexOf("H") + 1, journey.IndexOf("M"));
                mm = mm.Substring(0, mm.IndexOf("M"));
                //string ss = journey.Substring(journey.IndexOf("M") + 1, journey.Length - (journey.IndexOf("M") + 1));
                //ss = ss.Substring(0, ss.IndexOf("S"));

                int Hours = 0;
                int.TryParse(hh, out Hours);

                int Minutes = 0;
                int.TryParse(mm, out Minutes);

                return ((iDay * 24) * 60) + (Hours * 60) + Minutes;
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetJourneyTime", "air_uapi", journey, "", ex.Message);
            }
            return 0;
        }
        public void SetBookingTravelerRefPassenegers(string SearchID, DataTable dtPassenger, string BookingTravelerRef)
        {
            try {
                if (!dtPassenger.Columns.Contains("BookingTravelerRef"))
                {
                    dtPassenger.Columns.Add("BookingTravelerRef", typeof(string));
                }

                foreach (DataRow dr in dtPassenger.Rows)
                {
                    dr["BookingTravelerRef"] = CommonUapi.GetBookingTravelerRef(BookingTravelerRef, int.Parse(dr["RowID"].ToString()));
                }
                dtPassenger.AcceptChanges();
            }
            catch(Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "SetBookingTravelerRefPassenegers", "air_uapi", BookingTravelerRef, SearchID, ex.Message);
            }
        }
        public DataTable SetReArrangePassenegers(DataTable dtPassenger)
        {
            DataTable dtUpdatedPassenegers = dtPassenger.Clone();

            DataRow[] dtAdults = dtPassenger.Select("PaxType='" + "ADT" + "'");
            for (int i = 0; i < dtAdults.CopyToDataTable().Rows.Count; i++)
            {
                DataRow drAdd = dtAdults.CopyToDataTable().Rows[i];
                dtUpdatedPassenegers.ImportRow(drAdd);
            }

            DataRow[] dtInfants = dtPassenger.Select("PaxType='" + "INF" + "'");
            if (dtInfants.Length > 0)
            {
                for (int i = 0; i < dtInfants.CopyToDataTable().Rows.Count; i++)
                {
                    DataRow drAdd = dtInfants.CopyToDataTable().Rows[i];
                    dtUpdatedPassenegers.ImportRow(drAdd);
                }
            }

            DataRow[] dtChilds = dtPassenger.Select("PaxType='" + "CHD" + "'");
            if (dtChilds.Length > 0)
            {
                for (int i = 0; i < dtChilds.CopyToDataTable().Rows.Count; i++)
                {
                    DataRow drAdd = dtChilds.CopyToDataTable().Rows[i];
                    dtUpdatedPassenegers.ImportRow(drAdd);
                }
            }

            int rowid = 1;
            foreach (DataRow dr in dtUpdatedPassenegers.Rows)
            {
                dr["RowID"] = rowid;
                rowid++;
            }
            dtUpdatedPassenegers.AcceptChanges();

            return dtUpdatedPassenegers;
        }
        public static string GetOptionalServices(string strOptionalServices, int RowID, string BookingTravelerRef, out string OptionalServicesRuleRef)
        {
            //string BookingTravelerRef = dtSelect.Rows[0]["API_AirlineID"].ToString();
            //BTF-fIuqYz4R2BKA3mxKBAAAAA==?fIuqYz4R2BKAFA3KBAAAAA==?fIuqYz4R2BKAIA3KBAAAAA==:BTF-fIuqYz4R2BKA4mxKBAAAAA==?fIuqYz4R2BKAJA3KBAAAAA==?fIuqYz4R2BKAIA3KBAAAAA==:BTF-fIuqYz4R2BKA5mxKBAAAAA==?fIuqYz4R2BKAMA3KBAAAAA==?fIuqYz4R2BKAIA3KBAAAAA==:BTF-fIuqYz4R2BKA6mxKBAAAAA==?fIuqYz4R2BKAPA3KBAAAAA==?fIuqYz4R2BKAIA3KBAAAAA==:BTF-fIuqYz4R2BKA7mxKBAAAAA==?fIuqYz4R2BKASA3KBAAAAA==?fIuqYz4R2BKAIA3KBAAAAA==

            OptionalServicesRuleRef = "";
            string PassenegerBookingTravelerRef = CommonUapi.GetBookingTravelerRef(BookingTravelerRef, RowID);
            string[] split1 = strOptionalServices.Split(':');
            for (int i = 0; i < split1.Length; i++)
            {
                string text = split1[i].ToString();

                string[] split = text.Split('?');
                if (split[0].ToString() == ("BTF-" + PassenegerBookingTravelerRef))
                {
                    OptionalServicesRuleRef = split[2].ToString();
                    return split[1].ToString();
                }
            }
            return "";
        }
        public static string GetHostToken(string strHostToken, out string HostToken_Text, int rowid)
        {
            HostToken_Text = "";
            if(strHostToken.IndexOf("?")!=-1)
            {
                string[] split = strHostToken.Split('?');
                HostToken_Text = split[rowid].ToString();
                return split[rowid].ToString();
            }
            else
            {
                return strHostToken;
            }            
        }
        public static string GetHostToken(string strHostToken, out string HostToken_Text)
        {
            HostToken_Text = "";
            string[] split = strHostToken.Split('?');
            HostToken_Text = split[1].ToString();
            return split[0].ToString();
        }
        public static string GetFeeInfo(string strFeeInfo, string PaxType, out string Code)
        {
            //AADT-TfrPb3BAAA/BqKswLXAAAA==,IAG?CCHD-TfrPb3BAAA/BvKswLXAAAA==,IAG
            Code = "";
            string[] split = strFeeInfo.Split('?');
            for (int i = 0; i < split.Length; i++)
            {
                if (PaxType.IndexOf("ADT") != -1 && split[i].ToString().IndexOf("AADT-") != -1)
                {
                    string[] split1 = split[i].ToString().Split(',');
                    Code = split1[1].ToString();
                    return split1[0].ToString().Replace("AADT-", "").Trim();
                }
                if (PaxType.IndexOf("CHD") != -1 && split[i].ToString().IndexOf("CCHD-") != -1)
                {
                    string[] split1 = split[i].ToString().Split(',');
                    Code = split1[1].ToString();
                    return split1[0].ToString().Replace("CCHD-", "").Trim();
                }
            }
            return "";
        }
        public static string GetFareFamily(string PriceType)
        {
            if (PriceType.Equals("####"))
            {
                return "Corporate";
            }

            string FareFamily = "";
            if (PriceType.IndexOf("|") != -1)
            {
                string[] split = PriceType.Split('|');
                FareFamily = split[0].ToString().Trim();
            }
            else
            {
                FareFamily = PriceType;
            }

            //if (drFareInfo.CopyToDataTable().Columns.Contains("FareFamily"))
            //{
            //    drAdd["PriceType"] = drFareInfo.CopyToDataTable().Rows[0]["FareFamily"].ToString();
            //}
            //if (drFareInfo.CopyToDataTable().Columns.Contains("PrivateFare"))
            //{
            //    if (drFareInfo.CopyToDataTable().Rows[0]["PrivateFare"].ToString().Trim().Length > 0)
            //    {
            //        drAdd["PriceType"] = drAdd["PriceType"].ToString() + "|" + drFareInfo.CopyToDataTable().Rows[0]["PrivateFare"].ToString();
            //    }
            //}

            return FareFamily;
        }
        public static string GetCodeshareInfo(string RuleNumber, out string OperatingFlightNumber)
        {
            string OperatingCarrier = "";
            OperatingFlightNumber = "";

            if (RuleNumber.IndexOf("|") != -1)
            {
                string[] split = RuleNumber.Split('|');
                if (split[0].ToString().IndexOf("-") != -1)
                {
                    string[] split2 = split[0].ToString().Trim().Split('-');
                    OperatingCarrier = split2[0].ToString().Trim();
                    OperatingFlightNumber = split2[1].ToString().Trim();
                }
                else
                {
                    OperatingCarrier = split[0].ToString().Trim();
                }
            }
            else
            {
                OperatingCarrier = RuleNumber;
            }

            //if (drCodeShareInfo.CopyToDataTable().Columns.Contains("OperatingFlightNumber") && drCodeShareInfo.CopyToDataTable().Columns.Contains("CodeShareInfo_Text"))
            //{
            //    drAdd["RuleNumber"] = drCodeShareInfo.CopyToDataTable().Rows[0]["OperatingCarrier"].ToString() + "-" + drCodeShareInfo.CopyToDataTable().Rows[0]["OperatingFlightNumber"].ToString() + "|" + drCodeShareInfo.CopyToDataTable().Rows[0]["CodeShareInfo_Text"].ToString();
            //}
            //else if (drCodeShareInfo.CopyToDataTable().Columns.Contains("CodeShareInfo_Text"))
            //{
            //    drAdd["RuleNumber"] = drCodeShareInfo.CopyToDataTable().Rows[0]["OperatingCarrier"].ToString() + "|" + drCodeShareInfo.CopyToDataTable().Rows[0]["CodeShareInfo_Text"].ToString();
            //}
            //else if (drCodeShareInfo.CopyToDataTable().Columns.Contains("OperatingCarrier"))
            //{
            //    drAdd["RuleNumber"] = drCodeShareInfo.CopyToDataTable().Rows[0]["OperatingCarrier"].ToString();
            //}

            return OperatingCarrier;
        }
        public static string GetKey(string KeyData, bool IsAdt, bool IsChd, bool IsInf)
        {
            if (KeyData.IndexOf("?") != -1)
            {
                string Key = "";
                string[] split = KeyData.Split('?');
                if (IsAdt)
                {
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i].ToString().IndexOf("AADT-") != -1)
                        {
                            Key = split[i].ToString();
                            Key = Key.Replace("AADT-", "").Trim();
                            break;
                        }
                    }
                }
                else if (IsChd)
                {
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i].ToString().IndexOf("CCHD-") != -1)
                        {
                            Key = split[i].ToString();
                            Key = Key.Replace("CCHD-", "").Trim();
                            break;
                        }
                    }
                }
                else if (IsInf)
                {
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i].ToString().IndexOf("IINF-") != -1)
                        {
                            Key = split[i].ToString();
                            Key = Key.Replace("IINF-", "").Trim();
                            break;
                        }
                    }
                }
                return Key;
            }

            KeyData = KeyData.Replace("AADT-", "").Trim();
            KeyData = KeyData.Replace("CCHD-", "").Trim();
            KeyData = KeyData.Replace("IINF-", "").Trim();

            return KeyData;
        }
        public static int CalculateAge(DateTime dateOfBirth)
        {
            return DateTime.Now.Year - dateOfBirth.Year;
        }
        public static void GetFFN(string CompanyID, int BookingRef, string FFN, string FltType, out string Code, out string Number)
        {
            Code = string.Empty;
            Number = string.Empty;

            try
            {
                if (FFN.IndexOf("#") != -1)
                {
                    string[] s = FFN.Split('#');

                    if (FltType.Equals("O"))
                    {
                        string[] s1 = s[0].ToString().Split('-');
                        Code = s1[0].ToString();
                        Number = s1[1].ToString();
                    }
                    else
                    {
                        string[] s1 = s[1].ToString().Split('-');
                        Code = s1[0].ToString();
                        Number = s1[1].ToString();
                    }
                }
                else
                {
                    if (FFN.IndexOf("-") != -1)
                    {
                        string[] s1 = FFN.Split('-');
                        Code = s1[0].ToString();
                        Number = s1[1].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "Ticketing", "PNR", FltType, FFN, ex.Message);
            }
        }
    }
}
