using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Common;
using ZealTravel.Common.Models;
using ZealTravel.Front.Web.Models;
using ZealTravel.Front.Web.Models.Flight;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public static class SearchFlightRequestHelper
    {
       
        public static string PSQXMLString(List<clsPSQBO> objclsPSQBO)
        {
            clsPSQBO obj = objclsPSQBO[0];

            string[] arrdep = new string[2];
            string[] arrArv = new string[2];
            string ArrivalDate = "";
            arrdep = CommonFunction.GetStringInBetween(obj._DepartureStation, "(", ")", false, false);
            string DepartureStation = arrdep[0];

            arrArv = CommonFunction.GetStringInBetween(obj._ArrivalStation, "(", ")", false, false);
            string ArrivalStation = arrArv[0];
            if (ArrivalStation.Length != 3)
            {
                arrArv = CommonFunction.GetStringInBetween(arrArv[1], "(", ")", false, false);
                ArrivalStation = arrArv[0];
            }


            string startdate = obj._BeginDate;
            string[] depdate = startdate.Split('/');
            string BeginDate = depdate[2].ToString() + depdate[1].ToString() + depdate[0].ToString();
            if (obj._SearchType == "R")
            {
                string Enddate = obj._EndDate;
                string[] EnddateArr = Enddate.Split('/');
                ArrivalDate = EnddateArr[2].ToString() + EnddateArr[1].ToString() + EnddateArr[0].ToString();
            }
            if (obj._SearchType == "DRT")
            {
                string Enddate = obj._EndDate;
                string[] EnddateArr = Enddate.Split('/');
                ArrivalDate = EnddateArr[2].ToString() + EnddateArr[1].ToString() + EnddateArr[0].ToString();
            }
            StringBuilder ReqXml = new StringBuilder();
            string strPreferedAirline = obj._PreferedAirlines;
            if (strPreferedAirline == "" || strPreferedAirline == null)
            {
                obj._AirlineList.Add("GDS");
                obj._AirlineList.Add("SG");
                obj._AirlineList.Add("6E");
                obj._AirlineList.Add("G8");
                obj._AirlineList.Add("AK");
                obj._AirlineList.Add("IX");
                obj._AirlineList.Add("2T");
                obj._AirlineList.Add("TR");
                obj._AirlineList.Add("ZO");
                obj._AirlineList.Add("W5");
                obj._AirlineList.Add("FZ");
                obj._AirlineList.Add("G9");
                obj._AirlineList.Add("TZ");
            }
            else
            {
                //obj._AirlineList.Add("GDS");
                //obj._AirlineList.Add("SG");
                //obj._AirlineList.Add("6E");
                List<string> AirlineList = strPreferedAirline.Split(',').ToList();
                foreach (string Airline in AirlineList)
                {
                    obj._AirlineList.Add(Airline);
                }
            }
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<DepartureStation>" + DepartureStation + "</DepartureStation>");
            ReqXml.Append("<ArrivalStation>" + ArrivalStation + "</ArrivalStation>");

            ReqXml.Append("<Cabin>" + obj._TravelClass + "</Cabin>");
            ReqXml.Append("<AirVAry>");
            for (int i = 0; i < obj._AirlineList.Count; i++)
            {
                ReqXml.Append("<AirVInfo>");
                ReqXml.Append("<AirV>" + obj._AirlineList[i].ToString() + "</AirV>");
                ReqXml.Append("</AirVInfo>");
            }

            ReqXml.Append("</AirVAry>");
            ReqXml.Append("<StartDate>" + BeginDate + "</StartDate>");
            ReqXml.Append("<EndDate>" + ArrivalDate + "</EndDate>");
            ReqXml.Append("<Adult>" + obj._NoOfAdult + "</Adult>");
            ReqXml.Append("<Child>" + obj._NoOfChild + "</Child>");
            ReqXml.Append("<Infant>" + obj._NoOfInfant + "</Infant>");
            ReqXml.Append("<SplFare>" + obj._SpecialFare + "</SplFare>");
            ReqXml.Append("</AvailabilityRequest>");
            return ReqXml.ToString();
        }
        static int _Index = 0;
        public static string PSQXMLString(List<clsPSQBO> objclsPSQBO, bool _IsMultiCity)
        {
            //clsPSQBO obj = objclsPSQBO[0];
            StringBuilder ReqXml = new StringBuilder();
            ReqXml.Append("<AvailabilityRequest>");
            ReqXml.Append("<AirSearch>");
            foreach (clsPSQBO obj in objclsPSQBO)
            {
                _Index++;

                string[] arrdep = new string[2];
                string[] arrArv = new string[2];
                string ArrivalDate = "";
                arrdep = CommonFunction.GetStringInBetween(obj._DepartureStation, "(", ")", false, false);
                string DepartureStation = arrdep[0];

                arrArv = CommonFunction.GetStringInBetween(obj._ArrivalStation, "(", ")", false, false);
                string ArrivalStation = arrArv[0];
                if (ArrivalStation.Length != 3)
                {
                    arrArv = CommonFunction.GetStringInBetween(arrArv[1], "(", ")", false, false);
                    ArrivalStation = arrArv[0];
                }

                string startdate = obj._BeginDate;
                string[] depdate = startdate.Split('/');
                string BeginDate = depdate[2].ToString() + depdate[1].ToString() + depdate[0].ToString();
                if (obj._SearchType == "R")
                {
                    string Enddate = obj._EndDate;
                    string[] EnddateArr = Enddate.Split('/');
                    ArrivalDate = EnddateArr[2].ToString() + EnddateArr[1].ToString() + EnddateArr[0].ToString();
                }
                if (obj._SearchType == "DRT")
                {
                    string Enddate = obj._EndDate;
                    string[] EnddateArr = Enddate.Split('/');
                    ArrivalDate = EnddateArr[2].ToString() + EnddateArr[1].ToString() + EnddateArr[0].ToString();
                }



                ReqXml.Append("<AirSrchInfo>");
                ReqXml.Append("<DepartureStation>" + DepartureStation + "</DepartureStation>");
                ReqXml.Append("<ArrivalStation>" + ArrivalStation + "</ArrivalStation>");
                ReqXml.Append("<StartDate>" + BeginDate + "</StartDate>");
                ReqXml.Append("<EndDate>" + BeginDate + "</EndDate>");


                ReqXml.Append("</AirSrchInfo>");



                if (_Index == objclsPSQBO.Count())
                {
                    ReqXml.Append("</AirSearch>");
                    string strPreferedAirline = obj._PreferedAirlines;
                    if (strPreferedAirline == "" || strPreferedAirline == null)
                    {
                        obj._AirlineList.Add("GDS");
                        obj._AirlineList.Add("SG");
                        obj._AirlineList.Add("6E");
                        obj._AirlineList.Add("G8");
                        obj._AirlineList.Add("AK");
                        obj._AirlineList.Add("IX");
                        obj._AirlineList.Add("2T");
                        obj._AirlineList.Add("TR");
                        obj._AirlineList.Add("ZO");
                        obj._AirlineList.Add("W5");
                        obj._AirlineList.Add("FZ");
                        obj._AirlineList.Add("G9");
                        obj._AirlineList.Add("TZ");
                    }
                    else
                    {
                        //obj._AirlineList.Add("GDS");
                        //obj._AirlineList.Add("SG");
                        //obj._AirlineList.Add("6E");
                        List<string> AirlineList = strPreferedAirline.Split(',').ToList();
                        foreach (string Airline in AirlineList)
                        {
                            obj._AirlineList.Add(Airline);
                        }
                    }


                    ReqXml.Append("<Cabin>" + obj._TravelClass + "</Cabin>");
                    ReqXml.Append("<AirVAry>");
                    for (int i = 0; i < obj._AirlineList.Count; i++)
                    {
                        ReqXml.Append("<AirVInfo>");
                        ReqXml.Append("<AirV>" + obj._AirlineList[i].ToString() + "</AirV>");
                        ReqXml.Append("</AirVInfo>");
                    }

                    ReqXml.Append("</AirVAry>");
                    //ReqXml.Append("<StartDate>" + BeginDate + "</StartDate>");
                    //ReqXml.Append("<EndDate>" + ArrivalDate + "</EndDate>");
                    ReqXml.Append("<Adult>" + obj._NoOfAdult + "</Adult>");
                    ReqXml.Append("<Child>" + obj._NoOfChild + "</Child>");
                    ReqXml.Append("<Infant>" + obj._NoOfInfant + "</Infant>");
                    ReqXml.Append("<SplFare>" + obj._SpecialFare + "</SplFare>");
                }

            }
            ReqXml.Append("</AvailabilityRequest>");
            return ReqXml.ToString();
        }

        //public static bool InternationalSector(string Src, string Des)
        //{
        //    bool bDomestic = true;

        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(System.Web.HttpContext.Current.Server.MapPath(@"Xml\Airport_List_Full.xml"));
        //        DataTable dtCountry = new DataTable();
        //        dtCountry = ds.Tables["Table"];

        //        DataView dv = new DataView();
        //        DataTable FilterDt = new DataTable();
        //        string Compare = "'India'";
        //        dv = new DataView(dtCountry, dtCountry.DefaultView.RowFilter = "country IN (" + Compare.Trim() + ")", "", DataViewRowState.CurrentRows);
        //        if (dv.Count > 0)
        //        {
        //            FilterDt.Merge(dv.ToTable());
        //        }
        //        DataView dv1 = new DataView(FilterDt, FilterDt.DefaultView.RowFilter = "code IN ('" + Src.Trim() + "')", "", DataViewRowState.CurrentRows);
        //        DataView dv2 = new DataView(FilterDt, FilterDt.DefaultView.RowFilter = "code IN ('" + Des.Trim() + "')", "", DataViewRowState.CurrentRows);

        //        if (dv1.Count > 0 && dv2.Count > 0)
        //        {
        //            if (dv1.ToTable() != null && dv2.ToTable() != null)
        //            {
        //                if (dv1.ToTable().Rows.Count == 0 || dv2.ToTable().Rows.Count == 0)
        //                {
        //                    bDomestic = true;
        //                }
        //                else if (dv1.ToTable().Rows.Count == 0 && dv2.ToTable().Rows.Count == 0)
        //                {
        //                    bDomestic = true;
        //                }
        //                else
        //                {
        //                    bDomestic = false;
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        bDomestic = true;
        //    }

        //    return bDomestic;
        //}
    }
}
