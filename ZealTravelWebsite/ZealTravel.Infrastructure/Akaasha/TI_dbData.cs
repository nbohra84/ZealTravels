using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using ZealTravel.Domain.Interfaces.Akasha;
using ZealTravel.Domain.Models;
using ZealTravelWebsite.Infrastructure.Context;

namespace ZealTravel.Infrastructure.Akaasha
{
    public class TI_dbData : ITI_DBData
    {
        private ZealdbNContext _context;
        public TI_dbData(ZealdbNContext context)
        {
            _context = context;
        }
        public static DataTable GetBlackOutGDSAirline()
        {
            DataTable dtBlackOutGDSAirline = new DataTable();
            dtBlackOutGDSAirline.Clear();
            dtBlackOutGDSAirline.Columns.Add("GDS");
            dtBlackOutGDSAirline.Columns.Add("IncludeGDS");
            dtBlackOutGDSAirline.Columns.Add("ExcludeGDS");
            dtBlackOutGDSAirline.Columns.Add("IncludeAirline");
            dtBlackOutGDSAirline.Columns.Add("ExcludeAirline");
            dtBlackOutGDSAirline.Columns.Add("PCC");

            DataRow drAdd = dtBlackOutGDSAirline.NewRow();
            drAdd["GDS"] = "1A";
            drAdd["IncludeGDS"] = "";
            drAdd["ExcludeGDS"] = "";
            drAdd["IncludeAirline"] = "";
            //drAdd["ExcludeAirline"] = "4Z,8Q,AH,B6,CZ,EI,FI,G9,GP,H9,IC,ID,MK,NX,PS,S7,SD,SK,SQ,SU,SW,TR,TZ,UR,US,UT,UX,VA,VS,W2,WE,WF";
            drAdd["ExcludeAirline"] = "";
            drAdd["PCC"] = "";
            dtBlackOutGDSAirline.Rows.Add(drAdd);
            return dtBlackOutGDSAirline;
        }
        public static string DateChangeForSearch(string Date)
        {
            string nDate = string.Empty;

            string dd = Date.Substring(6, 2);
            string mm = Date.Substring(4, 2);
            string yy = Date.Substring(0, 4);
            nDate = yy + "-" + mm + "-" + dd;

            return nDate;
        }
        public static int TimeToMinustes(string Time)
        {
            try
            {
                DateTime dtTime = Convert.ToDateTime(Time);
                return dtTime.Hour * 60 + dtTime.Minute;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public List<AkashaAirLineModel> GetAirlineData(string DepartureStation, string ArrivalStation)
        {
            List<AkashaAirLineModel> dtAirline = new List<AkashaAirLineModel>();
            try
            {
                //using (SqlConnection dbCon = new SqlConnection(DBCommon.ConnectionString.dbConnect))
                //{
                //    SqlCommand dbCmd = new SqlCommand("TI_Airline_Proc", dbCon);
                //    dbCmd.CommandType = CommandType.StoredProcedure;
                //    dbCmd.Parameters.Add(@"ProcNo", SqlDbType.Int).Value = 1;
                //    dbCmd.Parameters.Add(@"DepartureStation", SqlDbType.VarChar).Value = DepartureStation;
                //    dbCmd.Parameters.Add(@"ArrivalStation", SqlDbType.VarChar).Value = ArrivalStation;
                //    SqlDataAdapter dbDa = new SqlDataAdapter(dbCmd);
                //    dbDa.Fill(dtAirline);
                //}

                var data = _context.Database.SqlQuery<AkashaAirLineModel>($"EXEC TI_Airline_Proc @ProcNo=1, @DepartureStation={DepartureStation}, @ArrivalStation={ArrivalStation}");
                dtAirline = data.ToList();
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg("", 0, "GetAirlineData", "TI_dbData", "", "", ex.Message);
            }
            return dtAirline;
        }
        public static string GetAvailabilityRequest(ArrayList CarrierList, string DepartureStation, string ArrivalStation, string Cabin, string StartDate, string EndDate, string Adult, string Child, string Infant)
        {
            StringBuilder ReqXml = new StringBuilder();
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<DepartureStation>" + DepartureStation + "</DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + ArrivalStation + "</ArrivalStation>");
            ReqXml.Append("<Cabin>" + Cabin + "</Cabin>");

            ReqXml.Append("<AirVAry>");
            for (int i = 0; i < CarrierList.Count; i++)
            {
                ReqXml.Append("<AirVInfo>");
                ReqXml.Append("<AirV>" + CarrierList[i].ToString() + "</AirV>");
                ReqXml.Append("</AirVInfo>");
            }
            ReqXml.Append("</AirVAry>");

            ReqXml.Append("<StartDate>" + StartDate + "</StartDate>");
            ReqXml.Append("<EndDate>" + EndDate + "</EndDate>");
            ReqXml.Append("<Adult>" + Adult + "</Adult>");
            ReqXml.Append("<Child>" + Child + "</Child>");
            ReqXml.Append("<Infant>" + Infant + "</Infant>");
            ReqXml.Append("</AvailabilityRequest>");

            return ReqXml.ToString();
        }
        public static string GetAvailabilityRequestMC(ArrayList ArayDepartureStation, ArrayList ArayArrivalStation, ArrayList ArayStartDate, string Adult, string Child, string Infant, ArrayList CarrierList, string Cabin)
        {
            StringBuilder ReqXml = new StringBuilder();
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<Cabin>" + Cabin + "</Cabin>");

            ReqXml.Append("<AirSearch>");
            for (int i = 0; i < ArayDepartureStation.Count; i++)
            {
                ReqXml.Append("<AirSrchInfo>");
                ReqXml.Append("<DepartureStation>" + ArayDepartureStation[i].ToString() + "</DepartureStation>");
                ReqXml.Append("<ArrivalStation>" + ArayArrivalStation[i].ToString() + "</ArrivalStation>");
                ReqXml.Append("<StartDate>" + ArayStartDate[i].ToString() + "</StartDate>");
                ReqXml.Append("</AirSrchInfo>");
            }
            ReqXml.Append("</AirSearch>");


            ReqXml.Append("<AirVAry>");
            for (int i = 0; i < CarrierList.Count; i++)
            {
                ReqXml.Append("<AirVInfo>");
                ReqXml.Append("<AirV>" + CarrierList[i].ToString() + "</AirV>");
                ReqXml.Append("</AirVInfo>");
            }
            ReqXml.Append("</AirVAry>");


            ReqXml.Append("<Adult>" + Adult + "</Adult>");
            ReqXml.Append("<Child>" + Child + "</Child>");
            ReqXml.Append("<Infant>" + Infant + "</Infant>");
            ReqXml.Append("</AvailabilityRequest>");

            return ReqXml.ToString();
        }
        public static DataTable CompanyInfo(string CompanyName, string PostalCode, string StateCode, string CountryCode, string CountryName, string Email, string CityName, string MobileNo, string PhoneNo, string Address)
        {
            DataTable dtCompanyInfo = DBCommon.Schema.SchemaCompanyInfo;
            DataRow drAdd = dtCompanyInfo.NewRow();
            drAdd["CompanyName"] = CompanyName;
            drAdd["PostalCode"] = PostalCode;
            drAdd["StateCode"] = StateCode;
            drAdd["CountryCode"] = CountryCode;
            drAdd["CountryName"] = CountryName;
            drAdd["Email"] = Email;
            drAdd["CityName"] = CityName;
            drAdd["MobileNo"] = MobileNo;
            drAdd["PhoneNo"] = PhoneNo;
            drAdd["Address"] = Address;
            dtCompanyInfo.Rows.Add(drAdd);

            return dtCompanyInfo;
        }
        public static DataTable GstInfo(string GSTNumber, string GSTCompanyName, string GSTCompanyEmail, string GSTCompanyContactNumber, string GSTCompanyAddress)
        {
            DataTable dtGstInfo = DBCommon.Schema.SchemaGstInfo;
            DataRow drAdd = dtGstInfo.NewRow();
            drAdd["GSTNumber"] = GSTNumber;
            drAdd["GSTCompanyName"] = GSTCompanyName;
            drAdd["GSTCompanyEmail"] = GSTCompanyEmail;
            drAdd["GSTCompanyContactNumber"] = GSTCompanyContactNumber;
            drAdd["GSTCompanyAddress"] = GSTCompanyAddress;
            dtGstInfo.Rows.Add(drAdd);

            return dtGstInfo;
        }
        public static DataTable GetPassengerData(int Adt, int Chd, int Inf)
        {
            DataTable dtPassenger = new DataTable();
            dtPassenger = SchemaPassengers;

            int Passengerid = 1;
            for (int i = 0; i < Adt; i++)
            {
                DataRow drAdd = dtPassenger.NewRow();

                drAdd["RowID"] = Passengerid;
                drAdd["PaxType"] = "ADT";
                drAdd["Title"] = "MR";

                drAdd["First_Name"] = "AV" + RandomString(5 + i);

                drAdd["Last_Name"] = "Gupta";
                drAdd["DOB"] = "1980-01-01";


                drAdd["Email"] = "mailgupta17@gmail.com";
                drAdd["MobileNo"] = "8802598779";


                drAdd["PPIssueDate"] = "2000-01-01";
                drAdd["PPExpirayDate"] = "2027-01-01";
                drAdd["PpNumber"] = "A123456";
                drAdd["PpCountry"] = "IN";
                drAdd["Nationality"] = "IN";

                drAdd["MealDesc_O"] = "";
                drAdd["MealCode_O"] = "";
                drAdd["MealChg_O"] = 0;
                drAdd["MealDetail_O"] = "";

                drAdd["MealDesc_I"] = "";
                drAdd["MealCode_I"] = "";
                drAdd["MealChg_I"] = 0;
                drAdd["MealDetail_I"] = "";

                drAdd["BaggageDesc_O"] = "";
                drAdd["BaggageCode_O"] = "";
                drAdd["BaggageChg_O"] = 0;
                drAdd["BaggageDetail_O"] = "";

                drAdd["BaggageDesc_I"] = "";
                drAdd["BaggageCode_I"] = "";
                drAdd["BaggageChg_I"] = 0;
                drAdd["BaggageDetail_I"] = "";

                dtPassenger.Rows.Add(drAdd);
                Passengerid++;
            }
            for (int i = 0; i < Chd; i++)
            {
                DataRow drAdd = dtPassenger.NewRow();

                drAdd["RowID"] = Passengerid;
                drAdd["PaxType"] = "CHD";
                drAdd["Title"] = "MSTR";

                drAdd["First_Name"] = "CV" + RandomString(6 + i);

                drAdd["Last_Name"] = "Gupta";
                drAdd["DOB"] = "2012-02-01";

                drAdd["Email"] = "mailgupta17@gmail.com";
                drAdd["MobileNo"] = "8802598779";

                drAdd["PPIssueDate"] = "2015-01-01";
                drAdd["PPExpirayDate"] = "2026-01-01";
                drAdd["PpNumber"] = "V123456";
                drAdd["PpCountry"] = "IN";
                drAdd["Nationality"] = "IN";
                drAdd["MealDesc_O"] = "";
                drAdd["MealCode_O"] = "";
                drAdd["MealChg_O"] = 0;
                drAdd["MealDetail_O"] = "";

                drAdd["MealDesc_I"] = "";
                drAdd["MealCode_I"] = "";
                drAdd["MealChg_I"] = 0;
                drAdd["MealDetail_I"] = "";

                drAdd["BaggageDesc_O"] = "";
                drAdd["BaggageCode_O"] = "";
                drAdd["BaggageChg_O"] = 0;
                drAdd["BaggageDetail_O"] = "";

                drAdd["BaggageDesc_I"] = "";
                drAdd["BaggageCode_I"] = "";
                drAdd["BaggageChg_I"] = 0;
                drAdd["BaggageDetail_I"] = "";

                dtPassenger.Rows.Add(drAdd);
                Passengerid++;
            }
            for (int i = 0; i < Inf; i++)
            {
                DataRow drAdd = dtPassenger.NewRow();

                drAdd["RowID"] = Passengerid;
                drAdd["PaxType"] = "INF";
                drAdd["Title"] = "MSTR";
                drAdd["First_Name"] = "IV" + RandomString(4 + i);

                drAdd["Last_Name"] = "Gupta";
                drAdd["DOB"] = "2022-11-05";

                drAdd["Email"] = "mailgupta17@gmail.com";
                drAdd["MobileNo"] = "8802598779";

                drAdd["PPIssueDate"] = "2022-03-01";
                drAdd["PPExpirayDate"] = "2024-01-01";
                drAdd["PpNumber"] = "S123456";
                drAdd["PpCountry"] = "IN";
                drAdd["Nationality"] = "IN";
                drAdd["MealDesc_O"] = "";
                drAdd["MealCode_O"] = "";
                drAdd["MealChg_O"] = 0;
                drAdd["MealDetail_O"] = "";

                drAdd["MealDesc_I"] = "";
                drAdd["MealCode_I"] = "";
                drAdd["MealChg_I"] = 0;
                drAdd["MealDetail_I"] = "";

                drAdd["BaggageDesc_O"] = "";
                drAdd["BaggageCode_O"] = "";
                drAdd["BaggageChg_O"] = 0;
                drAdd["BaggageDetail_O"] = "";

                drAdd["BaggageDesc_I"] = "";
                drAdd["BaggageCode_I"] = "";
                drAdd["BaggageChg_I"] = 0;
                drAdd["BaggageDetail_I"] = "";

                dtPassenger.Rows.Add(drAdd);
                Passengerid++;
            }

            return dtPassenger;
        }
        public static string RandomString(int length)
        {
            const string src = "abcdefghijklmnopqrstuvwxyz";
            var sb = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                sb.Append(c);
            }
            return sb.ToString().ToUpper();
        }
        private static DataTable SchemaPassengers
        {
            get
            {
                DataTable Table = new DataTable();
                Table.TableName = "PassengerInfo";
                Table.Columns.Add("RowID", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("PaxType", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Title", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("First_Name", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Middle_Name", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Last_Name", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("DOB", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("MealDesc_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealCode_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealChg_O", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("MealDetail_O", typeof(string)).DefaultValue = 0;

                Table.Columns.Add("MealDesc_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealCode_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MealChg_I", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("MealDetail_I", typeof(string)).DefaultValue = 0;

                Table.Columns.Add("BaggageDesc_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageCode_O", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageChg_O", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("BaggageDetail_O", typeof(string)).DefaultValue = 0;

                Table.Columns.Add("BaggageDesc_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageCode_I", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("BaggageChg_I", typeof(Int32)).DefaultValue = 0;
                Table.Columns.Add("BaggageDetail_I", typeof(string)).DefaultValue = 0;

                //Table.Columns.Add("SeatDesc_O", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatCode_O", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatChg_O", typeof(Int32)).DefaultValue = 0;

                //Table.Columns.Add("SeatDesc_I", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatCode_I", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("SeatChg_I", typeof(Int32)).DefaultValue = 0;

                Table.Columns.Add("Email", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("MobileNo", typeof(string)).DefaultValue = string.Empty;

                //Table.Columns.Add("City", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("State", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("LandLine", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("Address", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("Nationality", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("FFN", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("TourCode", typeof(string)).DefaultValue = string.Empty;

                Table.Columns.Add("PpNumber", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PPIssueDate", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PPExpirayDate", typeof(string)).DefaultValue = string.Empty;
                Table.Columns.Add("PpCountry", typeof(string)).DefaultValue = string.Empty;

                //Table.Columns.Add("GSTCompanyName", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTNumber", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTCompanyEmail", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTCompanyContactNumber", typeof(string)).DefaultValue = string.Empty;
                //Table.Columns.Add("GSTCompanyAddress", typeof(string)).DefaultValue = string.Empty;

                return Table;
            }
        }
    }
}
