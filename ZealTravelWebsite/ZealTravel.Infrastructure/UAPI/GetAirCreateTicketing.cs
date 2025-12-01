using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using System.Xml;
using ZealTravel.Domain.BookingManagement;
namespace ZealTravel.Infrastructure.UAPI
{
    class GetAirCreateTicketing
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string UserName;
        private string Password;

        private string SearchID;
        private string CompanyID;
        private Int32 BookingRef;
        private IBookingManagementService _bookingService;
        public GetAirCreateTicketing(string NetworkUserName, string NetworkPassword, string TargetBranch, string UserName, string Password, string SearchID, string CompanyID, int BookingRef, IBookingManagementService bookingService)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.UserName = UserName;
            this.Password = Password;

            this.SearchID = SearchID;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
            this._bookingService = bookingService;
        }
        private string CreateTicketing(string CarrierCode, string AirReservationLocatorCode)
        {
            string Request = "";
            try
            {
                CommonUapi objcu = new CommonUapi();
                Request += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">";
                Request += @"<soapenv:Header/>";
                Request += @"<soapenv:Body>";
                Request += @"<air:AirTicketingReq ReturnInfoOnFail=""true"" xmlns:air=""http://www.travelport.com/schema/air_v51_0"" TraceId=" + "\"" + SearchID + "\"" + " TargetBranch=" + "\"" + TargetBranch + "\"" + ">";
                Request += @"<com:BillingPointOfSaleInfo OriginApplication=""uapi"" xmlns:com=""http://www.travelport.com/schema/common_v51_0"" />";
                Request += @"<air:AirReservationLocatorCode>" + AirReservationLocatorCode + "</air:AirReservationLocatorCode>";
            
                //Commenting out this code as its not required since added SME fare account code
                //if (objcu.GetAccountCommissionAirlineWise(CarrierCode).Length > 0)
                //{
                //    Request += @"<air:AirTicketingModifiers>";
                //    Request += @"<com:Commission xmlns:com=""http://www.travelport.com/schema/common_v51_0"" Level=""Fare"" Type=""PercentBase"" Percentage=" + "\"" + objcu.GetAccountCommissionAirlineWise(CarrierCode) + "\"" + "/>";
                //    Request += @"</air:AirTicketingModifiers>";
                //}
                Request += @"</air:AirTicketingReq>";
                Request += @"</soapenv:Body>";
                Request += @"</soapenv:Envelope>";
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "CreateTicketing", "air_uapi", Request, SearchID, ex.Message);
            }
            return Request;
        }
        public async Task<bool> CreateTicket(string GalileoCode, string AirlinePNR, string AirReservationLocatorCode, string CarrierCode)
        {
            string RQ = "";
            string RS = "";

            ArrayList ArayTicketNumber = new ArrayList();

            try
            {
                RQ = CreateTicketing(CarrierCode, AirReservationLocatorCode);
                CommonUapi cs = new CommonUapi();
                RS = cs.GetResponseUapi(NetworkUserName, NetworkPassword, SearchID, RQ, "AirService", "TKT");

                if (RS != null && RS.IndexOf("ETR") != -1)
                {
                    XmlDocument xDocResponse1 = new XmlDocument();
                    xDocResponse1.LoadXml(RS);

                    XmlNodeList objNodeList1 = xDocResponse1.LastChild.LastChild.FirstChild.ChildNodes;
                    foreach (XmlNode objNode1 in objNodeList1)
                    {
                        if (objNode1.OuterXml.IndexOf("ETR") != -1)
                        {
                            XmlDocument xDocResponse2 = new XmlDocument();
                            xDocResponse2.LoadXml(objNode1.OuterXml);

                            XmlNodeList objNodeList2 = xDocResponse2.LastChild.ChildNodes;
                            foreach (XmlNode objNode2 in objNodeList2)
                            {
                                if (objNode2.OuterXml.IndexOf("air:Ticket") != -1)
                                {
                                    XmlDocument xDoc = new XmlDocument();
                                    xDoc.LoadXml(objNode2.OuterXml);

                                    XmlNodeList XmlNodeListAirPricePointList = xDoc.ChildNodes;
                                    foreach (XmlNode AllFares in XmlNodeListAirPricePointList)
                                    {
                                        ArayTicketNumber.Add(AllFares.Attributes["TicketNumber"].Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "CreateTicket-air_uapi", RS, RQ, SearchID, ex.Message);
            }

            DBCommon.Logger.dbLoggAPI(SearchID, CompanyID, BookingRef, "", "", "TKT", RQ, RS, GalileoCode + "-" + AirlinePNR + "-" + AirReservationLocatorCode);
            DBCommon.Logger.WriteLogg(CompanyID, BookingRef, "PNR", "PNR", RQ + Environment.NewLine + RS, GalileoCode + "-" + AirlinePNR + "-" + AirReservationLocatorCode, SearchID);

            if (ArayTicketNumber != null && ArayTicketNumber.Count > 0)
            {
                return await _bookingService.UpdateTicketNumber(BookingRef, ArayTicketNumber, false);
            }
            return false;
        }
    }
}
