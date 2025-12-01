using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class GetRequestData
    {
        public string DepartureStation;
        public string ArrivalStation;
        public string BeginDate;
        public string EndDate;
        public string Adt;
        public string Chd;
        public string Inf;
        public string Cabin;

        //public List<AvailabilityRequestVO> availabilityRequestVOList_;


        public ArrayList ArayCarrierList;
        public string JourneyType;

        public void getAvailabilityRequest(string AvailabilityRequest)

        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(AvailabilityRequest);

            DepartureStation = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
            ArrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;
            BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest/StartDate").InnerText;
            EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;
            Adt = (xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText);
            Chd = (xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText);
            Inf = (xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText);
            Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;
            /*€multicity*/
            //DepartureStation = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("DepartureStation").InnerText;
            //ArrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("ArrivalStation").InnerText;
            //BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("StartDate").InnerText;
            //EndDate = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("StartDate").InnerText;
            //Adt = xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText;
            //Chd = xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText;
            //Inf = xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText;
            //Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;

            ArayCarrierList = new ArrayList();
            XmlNodeList nodeList = xmldoc.SelectNodes("AvailabilityRequest/AirVAry/AirVInfo");
            foreach (XmlNode no in nodeList)
            {
                ArayCarrierList.Add(no.FirstChild.InnerText);
            }
        }



        public void getAvailabilityRequest(string AvailabilityRequest, string _Sector)

        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(AvailabilityRequest);
            DepartureStation = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
            ArrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;
            BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest/StartDate").InnerText;
            EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;
            Adt = (xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText);
            Chd = (xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText);
            Inf = (xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText);
            Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;

            //availabilityRequestVOList_ = new List<AvailabilityRequestVO>();
            //availabilityRequestVOList_ = GetAvailabilityRequestVOList(AvailabilityRequest, _Sector);


            /*€multicity*/
            /*
            DepartureStation = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("DepartureStation").InnerText;
            ArrivalStation = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("ArrivalStation").InnerText;
            BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("StartDate").InnerText;
            EndDate = xmldoc.SelectSingleNode("AvailabilityRequest").SelectSingleNode("AirSearch").SelectSingleNode("AirSrchInfo").SelectSingleNode("StartDate").InnerText;
            Adt = xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText;
            Chd = xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText;
            Inf = xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText;
            Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;
            */
            ArayCarrierList = new ArrayList();
            XmlNodeList nodeList = xmldoc.SelectNodes("AvailabilityRequest/AirVAry/AirVInfo");
            foreach (XmlNode no in nodeList)
            {
                ArayCarrierList.Add(no.FirstChild.InnerText);
            }
        }

        /*private List<AvailabilityRequestVO> GetAvailabilityRequestVOList(string _avlReq,string _Sector)
        {
            List<AvailabilityRequestVO>  l_ = new List<AvailabilityRequestVO>();
            AvailabilityRequestVO availabilityRequestVO;

            DataSet dsRequest = new DataSet();
            dsRequest.ReadXml(new System.IO.StringReader(_avlReq));


            foreach (DataRow dr in dsRequest.Tables["AirSrchInfo"].Rows)
            {
                availabilityRequestVO = new AvailabilityRequestVO();
                availabilityRequestVO.Origin = dr["DepartureStation"].ToString();
                availabilityRequestVO.Destination = dr["ArrivalStation"].ToString();
                availabilityRequestVO.BeginDate = dr["StartDate"].ToString();
                availabilityRequestVO.EndDate = dr["EndDate"].ToString();


                availabilityRequestVO.Adult = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Adult"].ToString());
                availabilityRequestVO.Child = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Child"].ToString());
                availabilityRequestVO.Infant = int.Parse(dsRequest.Tables["AvailabilityRequest"].Rows[0]["Infant"].ToString());
                availabilityRequestVO.Cabin = dsRequest.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();
                availabilityRequestVO.Sector = _Sector;
                l_.Add(availabilityRequestVO);
            }

            return l_;
        }*/

        public string GetSector(string AvailabilityRequest)
        {
            string Sector = "D";
            getAvailabilityRequest(AvailabilityRequest);

           // DBCommon.Airline_Detail objad = new DBCommon.Airline_Detail();
            //if (objad.IsDomestic(DepartureStation, ArrivalStation).Equals(false))
            //{
            //    Sector = "I";
            //}
            return Sector;
        }
        /*public string GetSector(string AvailabilityRequest, bool IsMultiCity)
        {
            string Sector = "D";
            if (IsMultiCity == false)
            {
                getAvailabilityRequest(AvailabilityRequest);
            }
            else
            {
                getAvailabilityRequest(AvailabilityRequest, IsMultiCity);
            }

            DBCommon.Airline_Detail objad = new DBCommon.Airline_Detail();
            if (objad.IsDomestic(DepartureStation, ArrivalStation).Equals(false))
            {
                Sector = "I";
            }
            return Sector;
        }*/

        public static DataTable Add_RowID(DataTable dtBound)
        {
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                int RowID = 1;
                foreach (DataRow dr in dtBound.Rows)
                {
                    dr["RowID"] = RowID;
                    RowID++;
                }
            }
            return dtBound;
        }
        //public static DataTable ASC_Filter_RowID(DataTable dtBound)
        //{
        //    if (dtBound != null && dtBound.Rows.Count > 0)
        //    {
        //        if (dtBound != null && dtBound.Columns.Contains("ForSort").Equals(true))
        //        {
        //            DBCommon.CommonFunction.RemoveColumn(dtBound, "ForSort");
        //            dtBound.AcceptChanges();
        //        }

        //        dtBound.Columns.Add("ForSort", typeof(int), "Convert(RowID, 'System.Int32')");
        //        DataView dv = new DataView(dtBound);
        //        dv.Sort = "ForSort ASC";
        //        dtBound = dv.ToTable();
        //        dtBound.Columns.Remove("ForSort");
        //    }
        //    return dtBound;
        //}
        public static void AddTrip(string Trip, DataTable dtBound)
        {
            if (dtBound != null && dtBound.Columns.Contains("Trip").Equals(true))
            {
                DBCommon.CommonFunction.RemoveColumn(dtBound, "Trip");
            }
            DBCommon.CommonFunction.AddColumn(dtBound, "Trip", Trip);
        }
        public static void AddSector(string Sector, DataTable dtBound)
        {
            if (dtBound != null && dtBound.Columns.Contains("Sector").Equals(true))
            {
                DBCommon.CommonFunction.RemoveColumn(dtBound, "Sector");
            }
            DBCommon.CommonFunction.AddColumn(dtBound, "Sector", Sector);
        }
    }
}
