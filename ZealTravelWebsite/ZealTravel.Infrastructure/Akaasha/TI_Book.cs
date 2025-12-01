using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIRequestResponse;

namespace ZealTravel.Infrastructure.Akaasha
{
    public class TI_Book
    {
        public string GetBookingXML(string JourneyType, string SearchID, string CompanyID, int BookingRef, DataTable dtBound, DataTable dtPassenegr, DataTable dtGST, string Idx)
        {
            string strBookingXML = "";
            try
            {
                BookingXML objBookingXML = new BookingXML();
                objBookingXML.ReservationId = BookingRef;
                objBookingXML.ReservationRef = BookingRef.ToString();

                string[] splitIdx = Idx.Split('_');
                string flightJson = string.Empty;
                string searchJson = string.Empty;
                string fareruleJson = string.Empty;

                CommonComponents.FlightDBOperation flightDBOperation = new CommonComponents.FlightDBOperation("FTI");
                flightDBOperation.GetFlightFromCache(SearchID, splitIdx[0].ToString().Trim(), ref flightJson, ref searchJson, ref fareruleJson);

                if (flightJson.Length > 0 && searchJson.Length > 0 && fareruleJson.Length > 0)
                {
                    FaresRulesReply faresRulesReply = null;
                    Flight fFlight = null;
                    SearchRQ searchRQ = null;
                    fFlight = CommonComponents.SerializeDeserialize.DeserializeObject<Flight>(Convert.ToString(flightJson), true);
                    faresRulesReply = CommonComponents.SerializeDeserialize.DeserializeObject<FaresRulesReply>(Convert.ToString(fareruleJson), true);
                    searchRQ = CommonComponents.SerializeDeserialize.DeserializeObject<SearchRQ>(Convert.ToString(searchJson), true);

                    #region create product
                    objBookingXML.Product = new TIRequestResponse.Product()
                    {
                        Item = new List<TIRequestResponse.Item>()
                    {
                        new TIRequestResponse.Item() {
                        Code="AIR", CrossSell = "false", Index = "0", Value = "YES"
                        }

                    }.ToArray()
                    };
                    #endregion

                    #region flightinfo
                    objBookingXML.FlightInfo = new TIRequestResponse.FlightInfo()
                    {
                        Flight = fFlight
                    };

                    objBookingXML.FlightInfo.SearchRQ = searchRQ;
                    #endregion

                    #region FaresRule
                    //Assign fare rule when fare rule is not on booking xml
                    if (objBookingXML.FlightInfo.Flight.FaresRulesReply == null && faresRulesReply != null)
                    {
                        objBookingXML.FlightInfo.Flight.FaresRulesReply = faresRulesReply;
                    }
                    #endregion

                    #region create general info
                    objBookingXML.GeneralInfo = new BookingGeneralInfo()
                    {
                        Channel = "B2B",
                        CompanyId = "FTI",
                        CultureCode = "en",
                        Currency = "INR",
                        LangCode = "en",
                        DecimalPreference = 2,
                        ClientIP = "",
                        Customer = null,
                        IsCustomerOnCash = false,
                        //comment by shailendra, As per disccussed with Anurag and Rizwan Sir in B2B ticket issue when both company and agent auto ticketing true.
                        //IsPaymentRecieved = Convert.ToBoolean(GeneralVariable.AgentDetails().IsAllowAutoTicketing) || isLCC || !bookingXML.FlightInfo.Flight.Refundable,
                        IsPaymentRecieved = true,
                        IsRefundable = objBookingXML.FlightInfo.Flight.Refundable,
                        LPOETONumber = "",
                        OnBehalfBooking = false,
                        PaymentReceiptId = "",
                        ReasonCode = "",
                        TravelerId = 0,
                        ProcessedBy = 0,
                        AgentId = 0,
                        BranchId = 0,
                        SubAgent = new BookingSubAgent()
                        {
                            Id = 0,
                            UserId = 0,
                            BranchId = 0,
                            SaBranchId = 0
                        },
                        TTSPrimaryChannel = "",
                        IsCanxRefund = false,
                        IsCeiling = false,
                        IsChargeable = false,
                        IsEMD = false,
                        IsCCLogic = false, //should get this value from db
                        ChannelInterface = "",
                        DomainUrl = "", //HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority + HttpContext.Request.ApplicationPath.TrimEnd('/') + "/",
                                        //comment by shailendra, As per disccussed with Anurag and Rizwan Sir in B2B ticket issue when both company and agent auto ticketing true.
                                        //IsAutoTicket = CompanySettings.CompanySetting().IsFlightAutoticket && !isLCC && bookingXML.FlightInfo.Flight.Refundable,
                        IsAutoTicket = true,
                        AgencyLogo = "",
                        IsWhiteLabel = "FALSE"
                    };
                    #endregion

                    #region Fill seatmap segments Change by Avinash on 9 SEP 2022
                    List<SeatMapSegments> listSeatMapSegment = new List<SeatMapSegments>();
                    foreach (var rSeg in fFlight.Details.RSeg.Select((x, i) => new { value = x, index = i }))
                    {
                        foreach (var fSeg in rSeg.value.FSeg.Select((x, i) => new { value = x, index = i }))
                        {
                            if (fSeg.value.CFBC.Equals("VIA"))
                            {
                                if (fSeg.index.Equals(0))
                                {
                                    listSeatMapSegment.Add(new SeatMapSegments()
                                    {
                                        SegmentIndex = (rSeg.index + 1) + "_" + (fSeg.index + 1),
                                        DepartureAirport = rSeg.value.Dep,
                                        ArrivalAirport = rSeg.value.Arr,
                                        SegmentTattoo = fSeg.value.GdsSegmentTatoo
                                    });
                                }
                            }
                            else
                            {
                                listSeatMapSegment.Add(new SeatMapSegments()
                                {
                                    SegmentIndex = (rSeg.index + 1) + "_" + (fSeg.index + 1),
                                    DepartureAirport = fSeg.value.DApt,
                                    ArrivalAirport = fSeg.value.AApt,
                                    SegmentTattoo = fSeg.value.GdsSegmentTatoo
                                });
                            }
                        }
                    }
                    #endregion Change by Avinash on 9 SEP 2022

                    #region create passenger details

                    List<Passenger> passengerList = new List<Passenger>();
                    objBookingXML.PassengerDetails = new PassengerDetails()
                    {
                        Adults = Convert.ToInt32(searchRQ.PaxDetails.Adults),
                        Childs = Convert.ToInt32(searchRQ.PaxDetails.Childs),
                        Infant = Convert.ToInt32(searchRQ.PaxDetails.Infant)
                    };

                    #region create adult

                    if (searchRQ.PaxDetails.Adults > 0)
                    {
                        DataRow[] drPassenger = dtPassenegr.Select("PaxType='" + "ADT" + "'");
                        for (int i = 0; i < searchRQ.PaxDetails.Adults; i++)
                        {
                            Passenger paxItem = new Passenger();
                            paxItem.PaxId = 0;
                            paxItem.PaxType = "ADT";
                            //paxItem.Title = "Mr";
                            //paxItem.Gender = "MALE";
                            paxItem.PaxIndex = i + 1;

                            paxItem.Title = drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().Trim().ToUpper();
                            paxItem.FirstName = drPassenger.CopyToDataTable().Rows[i]["First_Name"].ToString().Trim().ToUpper();
                            paxItem.MiddleName = "";
                            paxItem.LastName = drPassenger.CopyToDataTable().Rows[i]["Last_Name"].ToString().Trim().ToUpper();
                            paxItem.DOB = drPassenger.CopyToDataTable().Rows[i]["DOB"].ToString().Trim();
                            paxItem.FFQNo = drPassenger.CopyToDataTable().Rows[i]["FFN"].ToString().Trim();
                            paxItem.MealPref = "";
                            paxItem.SeatPref = "";
                            paxItem.PassportNo = drPassenger.CopyToDataTable().Rows[i]["PpNumber"].ToString().Trim();
                            paxItem.PassportIssueDate = drPassenger.CopyToDataTable().Rows[i]["PPIssueDate"].ToString().Trim();
                            paxItem.PassportExpiryDate = drPassenger.CopyToDataTable().Rows[i]["PPExpirayDate"].ToString().Trim();
                            paxItem.PassportIssuingCountry = drPassenger.CopyToDataTable().Rows[i]["PpCountry"].ToString().Trim();

                            if (drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().ToUpper().Trim().Equals("MR") || drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().ToUpper().Trim().Equals("MSTR"))
                            {
                                paxItem.Gender = "MALE";
                            }
                            else
                            {
                                paxItem.Gender = "FEMALE";
                            }

                            paxItem.Nationalty = drPassenger.CopyToDataTable().Rows[i]["Nationality"].ToString().Trim();
                            paxItem.FrequentFlyer = drPassenger.CopyToDataTable().Rows[i]["FFN"].ToString().Trim();
                            paxItem.MobileNumber = drPassenger.CopyToDataTable().Rows[i]["MobileNo"].ToString().Trim();
                            paxItem.EMailID = drPassenger.CopyToDataTable().Rows[i]["Email"].ToString().Trim();
                            paxItem.IdentityType = "";

                            if (JourneyType.Equals("OW") || JourneyType.Equals("RW"))
                            {
                                if (dtBound.Rows[0]["FltType"].ToString().Equals("O") && drPassenger.CopyToDataTable().Rows[i]["MealCode_O"].ToString().Length > 0 ||
                                    dtBound.Rows[0]["FltType"].ToString().Equals("I") && drPassenger.CopyToDataTable().Rows[i]["MealCode_I"].ToString().Length > 0)
                                {
                                    paxItem.OBMealIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Meal", "ADT", dtBound.Rows[0]["FltType"].ToString(), "", "", i);
                                }

                                if (dtBound.Rows[0]["FltType"].ToString().Equals("O") && drPassenger.CopyToDataTable().Rows[i]["BaggageCode_O"].ToString().Length > 0 ||
                                    dtBound.Rows[0]["FltType"].ToString().Equals("I") && drPassenger.CopyToDataTable().Rows[i]["BaggageCode_I"].ToString().Length > 0)
                                {
                                    paxItem.OutboundBaggIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Bag", "ADT", dtBound.Rows[0]["FltType"].ToString(), "", "", i);
                                }
                            }
                            else
                            {
                                if (drPassenger.CopyToDataTable().Rows[i]["MealCode_O"].ToString().Length > 0)
                                {
                                    paxItem.OBMealIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Meal", "ADT", "O", dtBound.Rows[0]["Origin"].ToString(), dtBound.Rows[0]["Destination"].ToString(), i);
                                }
                                if (drPassenger.CopyToDataTable().Rows[i]["MealCode_I"].ToString().Length > 0)
                                {
                                    paxItem.IBMealIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Meal", "ADT", "I", dtBound.Rows[0]["Destination"].ToString(), dtBound.Rows[0]["Origin"].ToString(), i);
                                }
                                if (drPassenger.CopyToDataTable().Rows[i]["BaggageCode_O"].ToString().Length > 0)
                                {
                                    paxItem.OutboundBaggIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Bag", "ADT", "O", dtBound.Rows[0]["Origin"].ToString(), dtBound.Rows[0]["Destination"].ToString(), i);
                                }
                                if (drPassenger.CopyToDataTable().Rows[i]["BaggageCode_I"].ToString().Length > 0)
                                {
                                    paxItem.InboundBaggIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Bag", "ADT", "I", dtBound.Rows[0]["Destination"].ToString(), dtBound.Rows[0]["Origin"].ToString(), i);
                                }
                            }


                            paxItem.Baggage = "";
                            paxItem.DailingCode = "";

                            paxItem.SeatMapSegments = listSeatMapSegment.Count > 0 ? listSeatMapSegment : null;
                            passengerList.Add(paxItem);
                        }
                    }
                    #endregion

                    #region create child
                    if (searchRQ.PaxDetails.Childs > 0)
                    {
                        DataRow[] drPassenger = dtPassenegr.Select("PaxType='" + "CHD" + "'");
                        for (int i = 0; i < searchRQ.PaxDetails.Childs; i++)
                        {
                            Passenger paxItem = new Passenger();
                            paxItem.PaxId = 0;
                            paxItem.PaxType = "CHD";
                            //paxItem.Title = "Mstr";
                            //paxItem.Gender = "MALE";
                            paxItem.PaxIndex = i + 1;

                            paxItem.Title = drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().Trim().ToUpper();
                            paxItem.FirstName = drPassenger.CopyToDataTable().Rows[i]["First_Name"].ToString().Trim().ToUpper();
                            paxItem.MiddleName = "";
                            paxItem.LastName = drPassenger.CopyToDataTable().Rows[i]["Last_Name"].ToString().Trim().ToUpper();
                            paxItem.DOB = drPassenger.CopyToDataTable().Rows[i]["DOB"].ToString().Trim();
                            paxItem.FFQNo = drPassenger.CopyToDataTable().Rows[i]["FFN"].ToString().Trim();
                            paxItem.MealPref = "";
                            paxItem.SeatPref = "";
                            paxItem.PassportNo = drPassenger.CopyToDataTable().Rows[i]["PpNumber"].ToString().Trim();
                            paxItem.PassportIssueDate = drPassenger.CopyToDataTable().Rows[i]["PPIssueDate"].ToString().Trim();
                            paxItem.PassportExpiryDate = drPassenger.CopyToDataTable().Rows[i]["PPExpirayDate"].ToString().Trim();
                            paxItem.PassportIssuingCountry = drPassenger.CopyToDataTable().Rows[i]["PpCountry"].ToString().Trim();

                            if (drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().ToUpper().Trim().Equals("MR") || drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().ToUpper().Trim().Equals("MSTR"))
                            {
                                paxItem.Gender = "MALE";
                            }
                            else
                            {
                                paxItem.Gender = "FEMALE";
                            }

                            paxItem.Nationalty = drPassenger.CopyToDataTable().Rows[i]["Nationality"].ToString().Trim();
                            paxItem.FrequentFlyer = drPassenger.CopyToDataTable().Rows[i]["FFN"].ToString().Trim();
                            paxItem.MobileNumber = drPassenger.CopyToDataTable().Rows[i]["MobileNo"].ToString().Trim();
                            paxItem.EMailID = drPassenger.CopyToDataTable().Rows[i]["Email"].ToString().Trim();
                            paxItem.IdentityType = "";

                            if (JourneyType.Equals("OW") || JourneyType.Equals("RW"))
                            {
                                if (dtBound.Rows[0]["FltType"].ToString().Equals("O") && drPassenger.CopyToDataTable().Rows[i]["MealCode_O"].ToString().Length > 0 ||
                                       dtBound.Rows[0]["FltType"].ToString().Equals("I") && drPassenger.CopyToDataTable().Rows[i]["MealCode_I"].ToString().Length > 0)
                                {
                                    paxItem.OBMealIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Meal", "CHD", dtBound.Rows[0]["FltType"].ToString(), "", "", i);
                                }

                                if (dtBound.Rows[0]["FltType"].ToString().Equals("O") && drPassenger.CopyToDataTable().Rows[i]["BaggageCode_O"].ToString().Length > 0 ||
                                    dtBound.Rows[0]["FltType"].ToString().Equals("I") && drPassenger.CopyToDataTable().Rows[i]["BaggageCode_I"].ToString().Length > 0)
                                {
                                    paxItem.OutboundBaggIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Bag", "CHD", dtBound.Rows[0]["FltType"].ToString(), "", "", i);
                                }
                            }
                            else
                            {
                                if (drPassenger.CopyToDataTable().Rows[i]["MealCode_O"].ToString().Length > 0)
                                {
                                    paxItem.OBMealIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Meal", "CHD", "O", dtBound.Rows[0]["Origin"].ToString(), dtBound.Rows[0]["Destination"].ToString(), i);
                                }
                                if (drPassenger.CopyToDataTable().Rows[i]["MealCode_I"].ToString().Length > 0)
                                {
                                    paxItem.IBMealIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Meal", "CHD", "I", dtBound.Rows[0]["Destination"].ToString(), dtBound.Rows[0]["Origin"].ToString(), i);
                                }
                                if (drPassenger.CopyToDataTable().Rows[i]["BaggageCode_O"].ToString().Length > 0)
                                {
                                    paxItem.OutboundBaggIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Bag", "CHD", "O", dtBound.Rows[0]["Origin"].ToString(), dtBound.Rows[0]["Destination"].ToString(), i);
                                }
                                if (drPassenger.CopyToDataTable().Rows[i]["BaggageCode_I"].ToString().Length > 0)
                                {
                                    paxItem.InboundBaggIdx = GetMealBaggageIdx(JourneyType, SearchID, CompanyID, BookingRef, drPassenger.CopyToDataTable().Rows[i], fFlight, "Bag", "CHD", "I", dtBound.Rows[0]["Destination"].ToString(), dtBound.Rows[0]["Origin"].ToString(), i);
                                }
                            }

                            paxItem.Baggage = "";
                            paxItem.DailingCode = "";

                            paxItem.SeatMapSegments = listSeatMapSegment.Count > 0 ? listSeatMapSegment : null;
                            passengerList.Add(paxItem);
                        }
                    }
                    #endregion

                    #region create infant
                    if (searchRQ.PaxDetails.Infant > 0)
                    {
                        DataRow[] drPassenger = dtPassenegr.Select("PaxType='" + "INF" + "'");
                        for (int i = 0; i < searchRQ.PaxDetails.Infant; i++)
                        {
                            Passenger paxItem = new Passenger();
                            paxItem.PaxId = 0;
                            paxItem.PaxType = "INF";
                            //paxItem.Title = "Mstr";
                            //paxItem.Gender = "MALE";
                            paxItem.PaxIndex = i + 1;
                            paxItem.AssociatedOrderID = "";

                            paxItem.Title = drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().Trim().ToUpper();
                            paxItem.FirstName = drPassenger.CopyToDataTable().Rows[i]["First_Name"].ToString().Trim().ToUpper();
                            paxItem.MiddleName = "";
                            paxItem.LastName = drPassenger.CopyToDataTable().Rows[i]["Last_Name"].ToString().Trim().ToUpper();
                            paxItem.DOB = drPassenger.CopyToDataTable().Rows[i]["DOB"].ToString().Trim();
                            paxItem.FFQNo = drPassenger.CopyToDataTable().Rows[i]["FFN"].ToString().Trim();
                            paxItem.MealPref = "";
                            paxItem.SeatPref = "";
                            paxItem.PassportNo = drPassenger.CopyToDataTable().Rows[i]["PpNumber"].ToString().Trim();
                            paxItem.PassportIssueDate = drPassenger.CopyToDataTable().Rows[i]["PPIssueDate"].ToString().Trim();
                            paxItem.PassportExpiryDate = drPassenger.CopyToDataTable().Rows[i]["PPExpirayDate"].ToString().Trim();
                            paxItem.PassportIssuingCountry = drPassenger.CopyToDataTable().Rows[i]["PpCountry"].ToString().Trim();

                            if (drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().ToUpper().Trim().Equals("MR") || drPassenger.CopyToDataTable().Rows[i]["Title"].ToString().ToUpper().Trim().Equals("MSTR"))
                            {
                                paxItem.Gender = "MALE";
                            }
                            else
                            {
                                paxItem.Gender = "FEMALE";
                            }

                            paxItem.Nationalty = drPassenger.CopyToDataTable().Rows[i]["Nationality"].ToString().Trim();
                            paxItem.FrequentFlyer = drPassenger.CopyToDataTable().Rows[i]["FFN"].ToString().Trim();
                            paxItem.MobileNumber = drPassenger.CopyToDataTable().Rows[i]["MobileNo"].ToString().Trim();
                            paxItem.EMailID = drPassenger.CopyToDataTable().Rows[i]["Email"].ToString().Trim();
                            paxItem.IdentityType = "";
                            paxItem.OBMealIdx = "";
                            paxItem.IBMealIdx = "";
                            paxItem.OutboundBaggIdx = "";
                            paxItem.InboundBaggIdx = "";
                            paxItem.Baggage = "";
                            paxItem.DailingCode = "";

                            paxItem.SeatMapSegments = listSeatMapSegment.Count > 0 ? listSeatMapSegment : null;
                            passengerList.Add(paxItem);
                        }
                    }
                    #endregion

                    objBookingXML.PassengerDetails.Passenger = passengerList.ToArray();

                    #endregion

                    #region payment
                    List<Payment> listPayment = new List<Payment>();
                    if (searchRQ.GeneralInfo.CompanyId == "FTI")
                    {
                        listPayment.Add(new Payment()
                        {
                            PayMode = "WLT",
                            PayAmount = 0
                        });
                    }
                    objBookingXML.PaymentDetail = new PaymentDetail()
                    {
                        Payment = listPayment.ToArray()
                    };
                    #endregion

                    #region create client billing delivery
                    objBookingXML.Client = new Client()
                    {
                        Title = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().Title,
                        FirstName = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().FirstName,
                        MiddleName = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().MiddleName,
                        LastName = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().LastName,
                        DOB = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().DOB,
                        Email = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().EMailID,
                        Mobile = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().MobileNumber,
                        PassportNo = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().PassportNo,
                        AreaCode = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().DailingCode,
                        Country = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().Nationalty,
                        Address = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().CompanyAddress,
                        City = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().CompanyAddress,//city
                        PostCode = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().CompanyAddress,//PostCode
                        State = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().CompanyAddress,//State
                        Street = objBookingXML.PassengerDetails.Passenger.Where(x => x.PaxType == "ADT").FirstOrDefault().CompanyAddress,//Street
                        ClientId = 0,
                        Password = ""
                    };
                    objBookingXML.Billing = objBookingXML.Client;
                    objBookingXML.Delivery = objBookingXML.Client;
                    #endregion

                    #region Update SeatInfo
                    if (objBookingXML.PassengerDetails.Passenger.Any(x => x.SeatMapSegments != null))
                    {
                        decimal _TotalSeatAmt = 0;
                        objBookingXML.PassengerDetails.Passenger.All(g =>
                        {
                            if (objBookingXML.PassengerDetails.Passenger != null && objBookingXML.PassengerDetails.Passenger.Count() > 0)
                            {
                                var Passenger = objBookingXML.PassengerDetails.Passenger.Where(m => m.PaxIndex == g.PaxIndex && m.PaxType == g.PaxType).FirstOrDefault();
                                if (Passenger != null && Passenger.SeatMapSegments != null)
                                {
                                    g.SeatMapSegments = Passenger.SeatMapSegments;
                                    if (Passenger.SeatMapSegments.Any(x => x.Amount > 0))
                                        _TotalSeatAmt += Passenger.SeatMapSegments.Sum(x => x.Amount);
                                }
                            }
                            return true;
                        });
                        if (_TotalSeatAmt > 0)
                            objBookingXML.FlightInfo.Flight.TAmt += Convert.ToDouble(_TotalSeatAmt);
                    }
                    #endregion

                    #region Extra Baggage Amt Calculation
                    if (objBookingXML.FlightInfo.Flight.SSRDetails != null && objBookingXML.FlightInfo.Flight.SSRDetails.PaxSSRs != null && objBookingXML.FlightInfo.Flight.SSRDetails.PaxSSRs.Count() > 0)
                    {
                        int ADTIdx = 0; int CHDIdx = 0; int INFIdx = 0; int ssrIDX = 0; string origin = ""; string destination = "";
                        int PaxIdx = 0; decimal _baggamt = 0; string[] arrSSR = null;
                        foreach (var _pax in objBookingXML.PassengerDetails.Passenger)
                        {
                            #region pax idx
                            if (_pax.PaxType == "ADT")
                            {
                                PaxIdx = ADTIdx;
                                ADTIdx++;
                            }
                            else if (_pax.PaxType == "CHD")
                            {
                                PaxIdx = CHDIdx;
                                CHDIdx++;
                            }
                            else if (_pax.PaxType == "INF")
                            {
                                PaxIdx = INFIdx;
                                INFIdx++;
                            }
                            #endregion

                            #region Outbound SSR
                            if (!string.IsNullOrEmpty(_pax.OutboundBaggIdx) || !string.IsNullOrEmpty(_pax.OBMealIdx))
                            {
                                #region Baggage
                                if (!string.IsNullOrEmpty(_pax.OutboundBaggIdx))
                                {
                                    arrSSR = _pax.OutboundBaggIdx.Split(',');
                                    foreach (string strSSR in arrSSR)
                                    {
                                        if (strSSR.Contains("#"))
                                        {
                                            origin = strSSR.Split('#')[0].Split('-')[0];
                                            destination = strSSR.Split('#')[0].Split('-')[1];
                                            ssrIDX = Convert.ToInt32(strSSR.Split('#')[1]);
                                            var outbagginfo = objBookingXML.FlightInfo.Flight.SSRDetails.PaxSSRs.Where(s => s.PaxType == _pax.PaxType && s.SSRType == "Bag" && s.Origin == origin && s.Destination == destination);
                                            if (outbagginfo != null)
                                            {
                                                var _Bagginfo = outbagginfo.ToArray()[PaxIdx];
                                                if (_Bagginfo != null)
                                                {
                                                    _Bagginfo.Idx = _pax.PaxIndex;
                                                    var _ssrdetail = _Bagginfo.SSRs.Where(x => x.SSRIdx == ssrIDX).FirstOrDefault();
                                                    if (_ssrdetail != null)
                                                    {
                                                        _ssrdetail.Isselected = true;
                                                        _baggamt += _ssrdetail.Amount;
                                                        _Bagginfo.SSRs = new List<SSRs>();
                                                        _Bagginfo.SSRs.Add(_ssrdetail);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Meal
                                if (!string.IsNullOrEmpty(_pax.OBMealIdx))
                                {
                                    arrSSR = _pax.OBMealIdx.Split(',');
                                    foreach (string strSSR in arrSSR)
                                    {
                                        if (strSSR.Contains("#"))
                                        {
                                            origin = strSSR.Split('#')[0].Split('-')[0];
                                            destination = strSSR.Split('#')[0].Split('-')[1];
                                            ssrIDX = Convert.ToInt32(strSSR.Split('#')[1]);
                                            var outmealinfo = objBookingXML.FlightInfo.Flight.SSRDetails.PaxSSRs.Where(s => s.PaxType == _pax.PaxType && s.SSRType == "Meal" && s.Origin == origin && s.Destination == destination);
                                            if (outmealinfo != null)
                                            {
                                                var _Mealinfo = outmealinfo.ToArray()[PaxIdx];
                                                if (_Mealinfo != null)
                                                {
                                                    _Mealinfo.Idx = _pax.PaxIndex;
                                                    var _ssrdetail = _Mealinfo.SSRs.Where(x => x.SSRIdx == ssrIDX).FirstOrDefault();
                                                    if (_ssrdetail != null)
                                                    {
                                                        _ssrdetail.Isselected = true;
                                                        _baggamt += _ssrdetail.Amount;
                                                        _Mealinfo.SSRs = new List<SSRs>();
                                                        _Mealinfo.SSRs.Add(_ssrdetail);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion

                            #region Inbound SSR
                            if (objBookingXML.FlightInfo.Flight.Details.RSeg.Count() > 0 && (!string.IsNullOrEmpty(_pax.InboundBaggIdx) || !string.IsNullOrEmpty(_pax.IBMealIdx)))
                            {
                                #region Baggage
                                if (!string.IsNullOrEmpty(_pax.InboundBaggIdx))
                                {
                                    arrSSR = _pax.InboundBaggIdx.Split(',');
                                    foreach (string strSSR in arrSSR)
                                    {
                                        if (strSSR.Contains("#"))
                                        {
                                            origin = strSSR.Split('#')[0].Split('-')[0];
                                            destination = strSSR.Split('#')[0].Split('-')[1];
                                            ssrIDX = Convert.ToInt32(strSSR.Split('#')[1]);
                                            var inbagginfo = objBookingXML.FlightInfo.Flight.SSRDetails.PaxSSRs.Where(s => s.PaxType == _pax.PaxType && s.SSRType == "Bag" && s.Origin == origin && s.Destination == destination);
                                            if (inbagginfo != null)
                                            {
                                                var _Bagginfo = inbagginfo.ToArray()[PaxIdx];
                                                if (_Bagginfo != null)
                                                {
                                                    _Bagginfo.Idx = _pax.PaxIndex;
                                                    var _ssrdetail = _Bagginfo.SSRs.Where(x => x.SSRIdx == ssrIDX).FirstOrDefault();
                                                    if (_ssrdetail != null)
                                                    {
                                                        _ssrdetail.Isselected = true;
                                                        _baggamt += _ssrdetail.Amount;
                                                        _Bagginfo.SSRs = new List<SSRs>();
                                                        _Bagginfo.SSRs.Add(_ssrdetail);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Meal
                                if (!string.IsNullOrEmpty(_pax.IBMealIdx))
                                {
                                    arrSSR = _pax.IBMealIdx.Split(',');
                                    foreach (string strSSR in arrSSR)
                                    {
                                        if (strSSR.Contains("#"))
                                        {
                                            origin = strSSR.Split('#')[0].Split('-')[0];
                                            destination = strSSR.Split('#')[0].Split('-')[1];
                                            ssrIDX = Convert.ToInt32(strSSR.Split('#')[1]);
                                            var inmealinfo = objBookingXML.FlightInfo.Flight.SSRDetails.PaxSSRs.Where(s => s.PaxType == _pax.PaxType && s.SSRType == "Meal" && s.Origin == origin && s.Destination == destination);
                                            if (inmealinfo != null)
                                            {
                                                var _Mealinfo = inmealinfo.ToArray()[PaxIdx];
                                                if (_Mealinfo != null)
                                                {
                                                    _Mealinfo.Idx = _pax.PaxIndex;
                                                    var _ssrdetail = _Mealinfo.SSRs.Where(x => x.SSRIdx == ssrIDX).FirstOrDefault();
                                                    if (_ssrdetail != null)
                                                    {
                                                        _ssrdetail.Isselected = true;
                                                        _baggamt += _ssrdetail.Amount;
                                                        _Mealinfo.SSRs = new List<SSRs>();
                                                        _Mealinfo.SSRs.Add(_ssrdetail);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        if (_baggamt > 0)
                            objBookingXML.FlightInfo.Flight.TAmt += Convert.ToDouble(_baggamt);
                    }
                    #endregion

                    string GSTCompanyAddress = string.Empty;
                    string GSTCompanyContactNumber = string.Empty;
                    string GSTCompanyName = string.Empty;
                    string GSTNumber = string.Empty;
                    string GSTCompanyEmail = string.Empty;
                    if (dtGST != null && dtGST.Rows.Count > 0)
                    {
                        GSTCompanyAddress = dtGST.Rows[0]["GSTCompanyAddress"].ToString().Trim().ToUpper();
                        GSTCompanyContactNumber = dtGST.Rows[0]["GSTCompanyContactNumber"].ToString().Trim().ToUpper();
                        GSTCompanyName = dtGST.Rows[0]["GSTCompanyName"].ToString().Trim().ToUpper();
                        GSTNumber = dtGST.Rows[0]["GSTNumber"].ToString().Trim().ToUpper();
                        GSTCompanyEmail = dtGST.Rows[0]["GSTCompanyEmail"].ToString().Trim().ToUpper();
                    }

                    #region GSTDetails 
                    GSTDetails objGST = new GSTDetails();
                    objGST.City = "";
                    objGST.Country = "INDIA";
                    objGST.GSTNumber = GSTNumber;
                    objGST.isMyCompanyPay = false;
                    objGST.IsRegisteredCustomer = false;
                    objGST.RegisteredCompanyAddress = GSTCompanyAddress;
                    objGST.RegisteredCompanyName = GSTCompanyName;
                    objGST.State = "";
                    objBookingXML.GSTDetails = objGST;
                    #endregion

                    objBookingXML.ShowPropertyWhileSerialize = true;

                    strBookingXML = CommonComponents.SerializeDeserialize.SerializeInJsonString(objBookingXML, "BookingXML");
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookingXML-TI_Book", strBookingXML, Idx, SearchID, ex.Message + "," + ex.StackTrace);
            }
            return strBookingXML;
        }
        private string GetMealBaggageIdx(string JourneyType, string SearchID, string CompanyID, int BookingRef, DataRow dr, Flight fFlight, string SSRType, string PaxType, string FltType, string Origin, string Destination, int idx)
        {
            try
            {
                //int idx = Convert.ToInt32(dr["RowID"].ToString()) - 1;
                if (JourneyType.Equals("RT"))
                {
                    var ss2r = fFlight.SSRDetails.PaxSSRs.Where(x => x.PaxType == PaxType && x.SSRType == SSRType && x.Idx == idx);// iska loop chalana padega for rt ke liye
                    foreach (var data2 in ss2r.ToList())
                    {
                        foreach (var data in data2.SSRs)
                        {
                            if (SSRType.Equals("Bag"))
                            {
                                if (Origin.Equals(data2.Origin) && Destination.Equals(data2.Destination))
                                {
                                    if (FltType.Equals("O"))
                                    {
                                        if (Convert.ToDecimal(dr["BaggageChg_O"].ToString()).Equals(data.Amount))
                                        {
                                            string desc1 = dr["BaggageDesc_O"].ToString().Trim().ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return data2.Origin + "-" + data2.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                    else if (FltType.Equals("I"))
                                    {
                                        if (Convert.ToDecimal(dr["BaggageChg_I"].ToString()).Equals(data.Amount))
                                        {
                                            string desc1 = dr["BaggageDesc_I"].ToString().Trim().ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return data2.Origin + "-" + data2.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                }
                            }
                            else if (SSRType.Equals("Meal"))
                            {
                                string MealDesc = dr["MealDesc_O"].ToString().Trim();
                                if (FltType.Equals("I"))
                                {
                                    MealDesc = dr["MealDesc_I"].ToString().Trim();
                                }

                                string[] Split1 = MealDesc.Split('(');
                                string[] Split2 = Split1[Split1.Length - 1].ToString().Split('-');
                                string mOrigin = Split2[0].ToString().Replace("(", "").Replace(")", "").Trim();
                                string mDestination = Split2[1].ToString().Replace("(", "").Replace(")", "").Trim();

                                if (mOrigin.Equals(data2.Origin) && mDestination.Equals(data2.Destination))
                                {
                                    if (FltType.Equals("O"))
                                    {
                                        if (Convert.ToDecimal(dr["MealChg_O"].ToString()).Equals(data.Amount))
                                        {
                                            //string kk = "Upma(DEL-BLR)";
                                            string desc1 = dr["MealDesc_O"].ToString().Trim().Substring(0, dr["MealDesc_O"].ToString().Trim().IndexOf("(")).ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return data2.Origin + "-" + data2.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                    else if (FltType.Equals("I"))
                                    {
                                        if (Convert.ToDecimal(dr["MealChg_I"].ToString()).Equals(data.Amount))
                                        {
                                            //string kk = "Upma(DEL-BLR)";
                                            string desc1 = dr["MealDesc_I"].ToString().Trim().Substring(0, dr["MealDesc_I"].ToString().Trim().IndexOf("(")).ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return data2.Origin + "-" + data2.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {

                    var ssr = fFlight.SSRDetails.PaxSSRs.Where(x => x.PaxType == PaxType && x.SSRType == SSRType && x.Idx == idx).FirstOrDefault();// iska loop chalana padega for rt ke liye
                    if (ssr != null && ssr.SSRs.Count() > 0)
                    {
                        foreach (var data in ssr.SSRs)
                        {
                            if (JourneyType.Equals("OW") || JourneyType.Equals("RW"))
                            {
                                if (FltType.Equals("O"))
                                {
                                    if (SSRType.Equals("Meal"))
                                    {
                                        if (Convert.ToDecimal(dr["MealChg_O"].ToString()).Equals(data.Amount))
                                        {
                                            //string kk = "Upma(DEL-BLR)";
                                            string desc1 = dr["MealDesc_O"].ToString().Trim().Substring(0, dr["MealDesc_O"].ToString().Trim().IndexOf("(")).ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return ssr.Origin + "-" + ssr.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                    else if (SSRType.Equals("Bag"))
                                    {
                                        if (Convert.ToDecimal(dr["BaggageChg_O"].ToString()).Equals(data.Amount))
                                        {
                                            string desc1 = dr["BaggageDesc_O"].ToString().Trim().ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return ssr.Origin + "-" + ssr.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (SSRType.Equals("Meal"))
                                    {
                                        if (Convert.ToDecimal(dr["MealChg_I"].ToString()).Equals(data.Amount))
                                        {
                                            //string kk = "Upma(DEL-BLR)";
                                            string desc1 = dr["MealDesc_I"].ToString().Trim().Substring(0, dr["MealDesc_I"].ToString().Trim().IndexOf("(")).ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return ssr.Origin + "-" + ssr.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                    else if (SSRType.Equals("Bag"))
                                    {
                                        if (Convert.ToDecimal(dr["BaggageChg_I"].ToString()).Equals(data.Amount))
                                        {
                                            string desc1 = dr["BaggageDesc_I"].ToString().Trim().ToUpper();
                                            string desc2 = data.Description.Trim().ToUpper();
                                            if (desc1.Equals(desc2))
                                            {
                                                return ssr.Origin + "-" + ssr.Destination + "#" + data.SSRIdx;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetBookingXML-TI_Book-GetMealBaggageIdx", PaxType + "," + FltType, SSRType, SearchID, ex.Message + "," + ex.StackTrace);
            }
            return "";
        }
    }
}
