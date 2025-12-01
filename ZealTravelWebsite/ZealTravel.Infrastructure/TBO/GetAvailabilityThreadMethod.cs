using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ZealTravel.Domain.Interfaces.TBO;

namespace ZealTravel.Infrastructure.TBO
{
    public class GetAvailabilityThreadMethod: IGetAvailabilityThread
    {
        public string errorMessage;
        public DataTable dtBound;
        //-----------------------------------------------------------------------------------------------
        string Searchid { get; set; }
        string Companyid { get; set; }
        string Supplierid { get; set; }
        string Password { get; set; }
        string EndUserIp { get; set; }
        string RQ_Flight { get; set; }

        //-----------------------------------------------------------------------------------------------
        private string Origin;
        private string Destination;
        private string BeginDate;
        private string EndDate;
        private int Adt;
        private int Chd;
        private int Inf;
        private ArrayList CarrierList;
        public string Sector;
        private string Cabin;
        //-----------------------------------------------------------------------------------------------
        public string Done_O;
        public string Done_I;
        public GetAvailabilityThreadMethod(string JourneyType, string Supplierid, string Password, string Searchid, string Companyid, string RQ_Flight, string EndUserIp)
        {
            this.Supplierid = Supplierid;
            this.Password = Password;
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.EndUserIp = EndUserIp;
            this.RQ_Flight = RQ_Flight;
            this.dtBound = DBCommon.Schema.SchemaFlights;

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(RQ_Flight);

            if (JourneyType.Equals("MC"))
            {
                DataSet dsRequest = DBCommon.CommonFunction.StringToDataSet(RQ_Flight);
                Sector = "";// GetDataBaseData.GetMulticitySector(RQ_Flight);

                CarrierList = new ArrayList();
                foreach (DataRow dr in dsRequest.Tables["AirVInfo"].Rows)
                {
                    CarrierList.Add(dr["AirV"].ToString());
                }
                CarrierList = DBCommon.CommonFunction.RemoveDuplicates(CarrierList);
                Cabin = dsRequest.Tables["AvailabilityRequest"].Rows[0]["Cabin"].ToString();
            }
            else
            {
                DBCommon.GetRequestData objtm = new DBCommon.GetRequestData();
                Sector = objtm.GetSector(this.RQ_Flight);

                Origin = xmldoc.SelectSingleNode("AvailabilityRequest/DepartureStation").InnerText;
                Destination = xmldoc.SelectSingleNode("AvailabilityRequest/ArrivalStation").InnerText;
                BeginDate = xmldoc.SelectSingleNode("AvailabilityRequest/StartDate").InnerText;
                EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;
                Adt = Convert.ToInt32(xmldoc.SelectSingleNode("AvailabilityRequest/Adult").InnerText);
                Chd = Convert.ToInt32(xmldoc.SelectSingleNode("AvailabilityRequest/Child").InnerText);
                Inf = Convert.ToInt32(xmldoc.SelectSingleNode("AvailabilityRequest/Infant").InnerText);
                Cabin = xmldoc.SelectSingleNode("AvailabilityRequest/Cabin").InnerText;
              

                CarrierList = new ArrayList();
                XmlNodeList nodeList = xmldoc.SelectNodes("AvailabilityRequest/AirVAry/AirVInfo");
                foreach (XmlNode no in nodeList)
                {
                    CarrierList.Add(no.FirstChild.InnerText);
                }
                CarrierList = GetCommonFunctions.RemoveDuplicates(CarrierList);
            }
        }
        public void GetOutbound()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            obj.GetOneWay(Origin, Destination, BeginDate, Adt, Chd, Inf, Cabin, CarrierList, Sector, "O");
            dtBound.Merge(obj.dtBound);
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
        }
        public async Task<string> GetOutboundAsync()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            dtBound.Merge(await obj.GetOneWayAsync(Origin, Destination, BeginDate, Adt, Chd, Inf, Cabin, CarrierList, Sector, "O"));
            //dtBound.Merge(obj.dtBound);
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
            return this.Done_O;
        }
        public void GetInbound()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            obj.GetOneWay(Destination, Origin, EndDate, Adt, Chd, Inf, Cabin, CarrierList, Sector, "I");
            GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
            DataTable dtData = objCommonFunctions.SetFltType(obj.dtBound);
            dtBound.Merge(dtData);
            this.errorMessage = obj.errorMessage;
            this.Done_I = "D";
        }
        public async Task<string> GetInboundAsync()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            DataTable dtData_= await obj.GetOneWayAsync(Destination, Origin, EndDate, Adt, Chd, Inf, Cabin, CarrierList, Sector, "I");
            GetApiCommonFunctions objCommonFunctions = new GetApiCommonFunctions();
            //DataTable dtData = objCommonFunctions.SetFltType(obj.dtBound);
            DataTable dtData = objCommonFunctions.SetFltType(dtData_);
            dtBound.Merge(dtData);
            this.errorMessage = obj.errorMessage;
            this.Done_I = "D";
            return this.Done_I;
        }

        public void GetRT()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            obj.GetRT(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, Sector);
            dtBound = obj.dtBound;
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
        }
        public async Task<string> GetRT_Async()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            dtBound=await obj.GetRT_Async(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, Sector);
            //dtBound = obj.dtBound;
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
            return this.Done_O;
        }

        public void GetRTLCC()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            obj.GetRTLCC(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, Sector);
            dtBound = obj.dtBound;
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
        }
        public async Task<string> GetRTLCC_Async()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            dtBound=await obj.GetRTLCC_Async(Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Cabin, CarrierList, Sector);
            //dtBound = obj.dtBound;
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
            return this.Done_O;
        }

        public void GetMC()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            obj.GetMC(RQ_Flight, Cabin, Sector);
            dtBound = obj.dtBound;
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
        }
        public async Task<string> GetMC_Async()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Supplierid, Password, Searchid, Companyid, EndUserIp);
            dtBound=await obj.GetMC_Async(RQ_Flight, Cabin, Sector);
            //dtBound = obj.dtBound;
            this.errorMessage = obj.errorMessage;
            this.Done_O = "D";
            return this.Done_O;
        }

    }
}
