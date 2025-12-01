using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using ZealTravel.Infrastructure.Akaasa;
using ZealTravel.Domain.Interfaces.FlightManagement.Akasaa;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Domain.Interfaces.DBCommon;
using ZealTravel.Domain.Interfaces.AirlineManagement.Akasaa;
using Newtonsoft.Json;
using System.Xml;
using ZealTravel.Domain.Models;

namespace ZealTravel.Infrastructure.Akaasha
{
    public class GetApiAvailabilityFare : IGetApiAvailabilityFare
    {
        public string errorMessage;
        ICredential _credential;
        public GetApiAvailabilityFare(ICredential credential)
        {
            _credential = credential;
        }
        //================================================================================================================================================
        public bool GetRTFares( DataTable dtBound, bool IsfareMethod, string Searchid,string CompanyID)
        {
            ArrayList ArRefid = new ArrayList();
            ArRefid = GetCommonFunctions.DataTable2ArrayList(dtBound, "RefID", true);

            DataTable OutSelectDt = dtBound.Clone();
            DataTable InSelectDt = dtBound.Clone();
            for (int i = 0; i < ArRefid.Count; i++)
            {
                OutSelectDt = dtBound.Select("RefID='" + ArRefid[i].ToString() + "' And FltType='" + "O" + "'").CopyToDataTable();
                InSelectDt = dtBound.Select("RefID='" + ArRefid[i].ToString() + "' And FltType='" + "I" + "'").CopyToDataTable();
                if (OutSelectDt.Rows.Count > 0 && InSelectDt.Rows.Count > 0)
                {
                    if (IsfareMethod)
                    {
                        DataRow[] rows = dtBound.Select("RefID='" + ArRefid[i].ToString() + "'");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow dr in rows)
                            {
                                dr["Adt_BASIC"] = 0;
                                dr["Adt_YQ"] = 0;
                                dr["Adt_PSF"] = 0;
                                dr["Adt_UDF"] = 0;
                                dr["Adt_AUDF"] = 0;
                                dr["Adt_CUTE"] = 0;
                                dr["Adt_GST"] = 0;
                                dr["Adt_TF"] = 0;
                                dr["Adt_CESS"] = 0;
                                dr["Adt_EX"] = 0;

                                dr["Chd_BASIC"] = 0;
                                dr["Chd_YQ"] = 0;
                                dr["Chd_PSF"] = 0;
                                dr["Chd_UDF"] = 0;
                                dr["Chd_AUDF"] = 0;
                                dr["Chd_CUTE"] = 0;
                                dr["Chd_GST"] = 0;
                                dr["Chd_TF"] = 0;
                                dr["Chd_CESS"] = 0;
                                dr["Chd_EX"] = 0;

                                dr["Inf_BASIC"] = 0;
                                dr["Inf_TAX"] = 0;

                                dr["FareStatus"] = false;

                                dr.AcceptChanges();
                                dtBound.AcceptChanges();
                            }
                        }
                    }

                    bool IsFareAvailable = GetRTPriceRequest(Searchid, OutSelectDt, InSelectDt, dtBound, ArRefid[i].ToString());
                    if (!IsFareAvailable)
                    {
                        DataRow[] rows = dtBound.Select("RefID='" + ArRefid[i].ToString() + "'");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow dr in rows)
                            {
                                dr["FareStatus"] = false;
                                dr.AcceptChanges();
                                dtBound.AcceptChanges();
                            }
                        }
                    }
                    if (IsfareMethod)
                    {
                        return IsFareAvailable;
                    }
                }
            }
            dtBound.AcceptChanges();
            return true;
        }
        private bool GetRTPriceRequest(string SearchID, DataTable OutSelectDt, DataTable InSelectDt, DataTable dtBound, string Refid)
        {
            bool IsFareAvailable = false;
            string Supplierid = OutSelectDt.Rows[0]["AirlineID"].ToString();
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                CS_FARE_SSR objfare = new CS_FARE_SSR();
                string JsonFlightFareRulesRQ = objfare.GetFlightFareRulesRQ_V2(SearchID, dtBound);

                var commonQP = new CommonQP(_credential);
                string _Token = commonQP.GetTokenAsync("", "").GetAwaiter().GetResult();
                //Task<string> jsonSearchRSAw = Task.Run(() => CommonQP.GetResponseQpV1Async(SearchID, "", "", JsonFlightFareRulesRQ, _Token));
                Task<string> jsonSearchRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonFlightFareRulesRQ, "Quote", _Token));

                string JsonFlightFareRulesRS = jsonSearchRSAw.GetAwaiter().GetResult();

                FareQpRS objApiPriceItineraryResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FareQpRS>(JsonFlightFareRulesRS);


                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string Signature = OutSelectDt.Rows[0]["Api_SessionID"].ToString();

                Int32 Adt = Convert.ToInt32(OutSelectDt.Rows[0]["Adt"].ToString());
                Int32 Chd = Convert.ToInt32(OutSelectDt.Rows[0]["Chd"].ToString());
                Int32 Inf = Convert.ToInt32(OutSelectDt.Rows[0]["Inf"].ToString());


                
                    if (objApiPriceItineraryResponse != null && objApiPriceItineraryResponse.Data.Journeys != null && objApiPriceItineraryResponse.Data.Passengers != null)
                    {
                        decimal BookingSum = objApiPriceItineraryResponse.Data.Breakdown.balanceDue; //objApiPriceItineraryResponse.Booking.BookingSum.BalanceDue;

                        ArrayList ArOutbound = new ArrayList();
                        ArrayList ArInbound = new ArrayList();

                        if (Inf > 0)
                        {
                            if (OutSelectDt.Rows.Count.Equals(1))
                            {
                                ArOutbound.Add(OutSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[0]["DepartureStation"].ToString().Trim() + OutSelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                            }
                            else if (OutSelectDt.Rows.Count.Equals(2))
                            {
                                if (OutSelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(OutSelectDt.Rows[1]["FlightNumber"].ToString().Trim()))
                                {
                                    ArOutbound.Add(OutSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[0]["Origin"].ToString().Trim() + OutSelectDt.Rows[0]["Destination"].ToString().Trim());
                                }
                                else
                                {
                                    ArOutbound.Add(OutSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[0]["DepartureStation"].ToString().Trim() + OutSelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                                    ArOutbound.Add(OutSelectDt.Rows[1]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[1]["DepartureStation"].ToString().Trim() + OutSelectDt.Rows[1]["ArrivalStation"].ToString().Trim());
                                }
                            }
                            else if (OutSelectDt.Rows.Count.Equals(3))
                            {
                                if (OutSelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(OutSelectDt.Rows[1]["FlightNumber"].ToString().Trim()).Equals(OutSelectDt.Rows[2]["FlightNumber"].ToString().Trim()))
                                {
                                    ArOutbound.Add(OutSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[0]["Origin"].ToString().Trim() + OutSelectDt.Rows[0]["Destination"].ToString().Trim());
                                }
                                else
                                {
                                    ArOutbound.Add(OutSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[0]["DepartureStation"].ToString().Trim() + OutSelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                                    ArOutbound.Add(OutSelectDt.Rows[1]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[1]["DepartureStation"].ToString().Trim() + OutSelectDt.Rows[1]["ArrivalStation"].ToString().Trim());
                                    ArOutbound.Add(OutSelectDt.Rows[2]["FlightNumber"].ToString().Trim() + " " + OutSelectDt.Rows[2]["DepartureStation"].ToString().Trim() + OutSelectDt.Rows[2]["ArrivalStation"].ToString().Trim());
                                }
                            }
                        }
                        if (Inf > 0)
                        {
                            if (InSelectDt.Rows.Count.Equals(1))
                            {
                                ArInbound.Add(InSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[0]["DepartureStation"].ToString().Trim() + InSelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                            }
                            else if (InSelectDt.Rows.Count.Equals(2))
                            {
                                if (InSelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(InSelectDt.Rows[1]["FlightNumber"].ToString().Trim()))
                                {
                                    ArInbound.Add(InSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[0]["Origin"].ToString().Trim() + InSelectDt.Rows[0]["Destination"].ToString().Trim());
                                }
                                else
                                {
                                    ArInbound.Add(InSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[0]["DepartureStation"].ToString().Trim() + InSelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                                    ArInbound.Add(InSelectDt.Rows[1]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[1]["DepartureStation"].ToString().Trim() + InSelectDt.Rows[1]["ArrivalStation"].ToString().Trim());
                                }
                            }
                            else if (InSelectDt.Rows.Count.Equals(3))
                            {
                                if (InSelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(InSelectDt.Rows[1]["FlightNumber"].ToString().Trim()).Equals(InSelectDt.Rows[2]["FlightNumber"].ToString().Trim()))
                                {
                                    ArInbound.Add(InSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[0]["Origin"].ToString().Trim() + InSelectDt.Rows[0]["Destination"].ToString().Trim());
                                }
                                else
                                {
                                    ArInbound.Add(InSelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[0]["DepartureStation"].ToString().Trim() + InSelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                                    ArInbound.Add(InSelectDt.Rows[1]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[1]["DepartureStation"].ToString().Trim() + InSelectDt.Rows[1]["ArrivalStation"].ToString().Trim());
                                    ArInbound.Add(InSelectDt.Rows[2]["FlightNumber"].ToString().Trim() + " " + InSelectDt.Rows[2]["DepartureStation"].ToString().Trim() + InSelectDt.Rows[2]["ArrivalStation"].ToString().Trim());
                                }
                            }
                        }

                        IsFareAvailable = SetRTPriceModifier(ArOutbound, objApiPriceItineraryResponse.Data.Passengers.FirstOrDefault().Value.PassengerList, objApiPriceItineraryResponse.Data.Journeys[0], Inf, BookingSum, dtBound, Refid);
                        IsFareAvailable = SetRTPriceModifier(ArInbound, objApiPriceItineraryResponse.Data.Passengers.FirstOrDefault().Value.PassengerList, objApiPriceItineraryResponse.Data.Journeys[1], Inf, BookingSum, dtBound, Refid);

                    }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetRTPriceRequest", "air_AkasaAir", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }

            //DBCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetRTPriceRequest-air_AkasaAir", "FARE", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return IsFareAvailable;
        }
        //private bool SetRTPriceModifier(ArrayList ArSectors, svc_booking.Passenger[] objPassenger, svc_booking.Segment[] objSegment, int iInf, decimal BookingSum, DataTable dtBound, string Refid)
        private bool SetRTPriceModifier(ArrayList ArSectors, List<Passenger4Fare> objPassenger, JourneyDtl4Fare objJournyList, int iInf, decimal BookingSum, DataTable dtBound, string Refid)
        {
            bool IsFareAvailable = false;

            try
            {
                if (iInf > 0)
                {
                    int iBASIC = 0;
                    int iEXT = 0;
                    string ChargeCode = string.Empty;
                    foreach (Passenger4Fare _PassengerDtl in objPassenger)
                    //for (int i = 0; i < objPassenger.Length;)
                    {
                        //svc_booking.PassengerFee[] pfee = objPassenger[0].PassengerFees;
                        List<ServiceCharge4Fare> pfees = _PassengerDtl.Infant.InfantFees.FirstOrDefault().ServiceCharges;
                        foreach (ServiceCharge4Fare _pfee in pfees)
                        //for (int j = 0; j < pfee.Length; j++)
                        {
                            if (ValidSectorForInfant(ArSectors, _PassengerDtl.Infant.InfantFees.FirstOrDefault().FlightReference).Equals(true))
                            //    if (ValidSectorForInfant(ArSectors, pfee[j].FlightReference).Equals(true))
                            {

                                List<ServiceCharge4Fare> _bscs = _PassengerDtl.Infant.InfantFees.FirstOrDefault().ServiceCharges; //svc_booking.BookingServiceCharge[] bsc = pfee[j].ServiceCharges;
                                foreach (ServiceCharge4Fare _bsc in _bscs) //for (int n = 0; n < bsc.Length; n++)
                                {

                                    //if (bsc[n].ChargeType.ToString().ToUpper() != "IncludedTax".ToUpper())
                                    //{
                                    int iValue = Decimal.ToInt32(_bsc.Amount);
                                    ChargeCode = _bsc.Code; //bsc[n].ChargeCode;
                                    if (_bsc.Code == null || _bsc.Code.Equals(string.Empty) || _bsc.Code.Equals("INFT"))
                                    {
                                        ChargeCode = "BASIC";
                                        iBASIC += iValue;
                                    }
                                    else
                                    {
                                        ChargeCode = "EX";
                                        iEXT += iValue;
                                    }
                                    //}
                                }

                                if (iBASIC > 0 || iEXT > 0)
                                {
                                    DataRow[] rows = dtBound.Select("RefID='" + Refid + "'");
                                    if (rows.Length > 0)
                                    {
                                        foreach (DataRow dr in rows)
                                        {
                                            dr["Inf_BASIC"] = Convert.ToInt32(dr["Inf_BASIC"].ToString()) + iBASIC;
                                            dr["Inf_TAX"] = Convert.ToInt32(dr["Inf_TAX"].ToString()) + iEXT;
                                            dr.AcceptChanges();
                                            dtBound.AcceptChanges();
                                        }
                                    }

                                    iBASIC = 0;
                                    iEXT = 0;
                                }
                            }
                        }
                        break;
                    }
                }

                int BASIC = 0;
                int YQ = 0;
                int TRF = 0;
                int UDF = 0;
                int PSF = 0;
                int AUDF = 0;
                int TF = 0;
                int GST = 0;
                int EXT = 0;

                Decimal iVal = 0;
                JourneyDtl4Fare _JourneyDtl = objJournyList;
                //foreach (JourneyDtl4Fare _JourneyDtl in objJournyList)  //for (int j = 0; j < objSegment.Length; j++)
                //{
                List<Segment> segkk = _JourneyDtl.Segments;  //svc_booking.Fare[] fares = objSegment[j].Fares;
                foreach (Segment _Segment in segkk)  //for (int k = 0; k < fares.Length; k++)
                {
                    List<FareObject4FareQuote> fares = _Segment.Fares;  //svc_booking.PaxFare[] pfare = fares[k].PaxFares;
                    foreach (FareObject4FareQuote _Fare in fares)  //for (int l = 0; l < pfare.Length; l++)
                    {
                        List<PassengerFare> pfare = _Fare.PassengerFares; //svc_booking.BookingServiceCharge[] bsc = pfare[l].ServiceCharges;
                        PassengerFare _PassengerFare = pfare.FirstOrDefault();
                        List<ServiceCharge> _bscs = _PassengerFare.ServiceCharges;
                        if (_PassengerFare.PassengerType == "ADT") //if (pfare[l].PaxType == "ADT")
                        {
                            foreach (ServiceCharge _ServiceCharge in _bscs) //for (int n = 0; n < bsc.Length; n++)
                            {
                                iVal = Convert.ToDecimal(_ServiceCharge.Amount);

                                if (_ServiceCharge.Code == null || _ServiceCharge.Code.Equals(string.Empty))//if (bsc[n].ChargeCode==null || bsc[n].ChargeCode.Equals(string.Empty))
                                {
                                    BASIC += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("TRF"))//else if (bsc[n].ChargeCode.Equals("TRF"))
                                {
                                    TRF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("CGST") != -1)//else if (bsc[n].ChargeCode.IndexOf("CGST") != -1)
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("SGST") != -1)//else if (bsc[n].ChargeCode.IndexOf("SGST") != -1)
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("IGST") != -1) //else if (bsc[n].ChargeCode.IndexOf("IGST") != -1)
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("C33") != -1) // C refers to CGST
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("S33") != -1) //S refers to SGST
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("PSF"))//else if (bsc[n].ChargeCode.Equals("PSF"))
                                {
                                    PSF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("YQ"))//else if (bsc[n].ChargeCode.Equals("YQ"))
                                {
                                    YQ += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("ASF"))//else if (bsc[n].ChargeCode.Equals("ASF"))
                                {
                                    UDF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("RCS"))//else if (bsc[n].ChargeCode.Equals("RCS"))
                                {
                                    AUDF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("TF"))//else if (bsc[n].ChargeCode.Equals("TF"))
                                {
                                    TF += Decimal.ToInt32(iVal);
                                }
                                else
                                {
                                    EXT += Decimal.ToInt32(iVal);
                                }
                            }

                            if (BASIC > 0 || TRF > 0 || GST > 0 || PSF > 0 || YQ > 0 || UDF > 0 || EXT > 0 || AUDF > 0 || TF > 0)
                            {
                                DataRow[] rows = dtBound.Select("RefID='" + Refid + "'");
                                if (rows.Length > 0)
                                {
                                    foreach (DataRow dr in rows)
                                    {
                                        dr["Adt_BASIC"] = Convert.ToInt32(dr["Adt_BASIC"].ToString()) + BASIC;
                                        dr["Adt_YQ"] = Convert.ToInt32(dr["Adt_YQ"].ToString()) + YQ;
                                        dr["Adt_PSF"] = Convert.ToInt32(dr["Adt_PSF"].ToString()) + PSF;
                                        dr["Adt_TF"] = Convert.ToInt32(dr["Adt_TF"].ToString()) + TF;
                                        dr["Adt_CUTE"] = Convert.ToInt32(dr["Adt_CUTE"].ToString()) + TRF;
                                        dr["Adt_GST"] = Convert.ToInt32(dr["Adt_GST"].ToString()) + GST;
                                        dr["Adt_UDF"] = Convert.ToInt32(dr["Adt_UDF"].ToString()) + UDF;
                                        dr["Adt_AUDF"] = Convert.ToInt32(dr["Adt_AUDF"].ToString()) + AUDF;
                                        dr["Adt_EX"] = Convert.ToInt32(dr["Adt_EX"].ToString()) + EXT;
                                        dr["FareQuote"] = "Updated";
                                        dr["Chd_Import"] = BookingSum;

                                        dr.AcceptChanges();
                                        dtBound.AcceptChanges();

                                        IsFareAvailable = true;
                                    }
                                }

                                BASIC = 0;
                                YQ = 0;
                                TRF = 0;
                                UDF = 0;
                                PSF = 0;
                                AUDF = 0;
                                TF = 0;
                                GST = 0;
                                EXT = 0;
                            }
                        }
                        else
                        {
                            BASIC = 0;
                            YQ = 0;
                            TRF = 0;
                            UDF = 0;
                            PSF = 0;
                            AUDF = 0;
                            TF = 0;
                            GST = 0;
                            EXT = 0;

                            foreach (ServiceCharge _ServiceCharge in _bscs) //for (int n = 0; n < bsc.Length; n++)
                            {
                                iVal = Convert.ToDecimal(_ServiceCharge.Amount); //iVal = Convert.ToDecimal(bsc[n].Amount);

                                if (_ServiceCharge.Code == null || _ServiceCharge.Code.Equals(string.Empty))
                                {
                                    BASIC += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("TRF"))
                                {
                                    TRF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("CGST") != -1)
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("SGST") != -1)
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("IGST") != -1)
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("C33") != -1) // C refers to CGST
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.IndexOf("S33") != -1) //S refers to SGST
                                {
                                    GST += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("PSF"))
                                {
                                    PSF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("YQ"))
                                {
                                    YQ += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("ASF"))
                                {
                                    UDF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("RCS"))
                                {
                                    AUDF += Decimal.ToInt32(iVal);
                                }
                                else if (_ServiceCharge.Code.Equals("TF"))
                                {
                                    TF += Decimal.ToInt32(iVal);
                                }
                                else
                                {
                                    EXT += Decimal.ToInt32(iVal);
                                }
                            }
                            if (BASIC > 0 || TRF > 0 || GST > 0 || PSF > 0 || YQ > 0 || UDF > 0 || EXT > 0 || AUDF > 0 || TF > 0)
                            {
                                DataRow[] rows = dtBound.Select("RefID='" + Refid + "'");
                                if (rows.Length > 0)
                                {
                                    foreach (DataRow dr in rows)
                                    {
                                        dr["Chd_BASIC"] = Convert.ToInt32(dr["Chd_BASIC"].ToString()) + BASIC;
                                        dr["Chd_YQ"] = Convert.ToInt32(dr["Chd_YQ"].ToString()) + YQ;
                                        dr["Chd_PSF"] = Convert.ToInt32(dr["Chd_PSF"].ToString()) + PSF;
                                        dr["Chd_TF"] = Convert.ToInt32(dr["Chd_TF"].ToString()) + TF;
                                        dr["Chd_CUTE"] = Convert.ToInt32(dr["Chd_CUTE"].ToString()) + TRF;
                                        dr["Chd_GST"] = Convert.ToInt32(dr["Chd_GST"].ToString()) + GST;
                                        dr["Chd_UDF"] = Convert.ToInt32(dr["Chd_UDF"].ToString()) + UDF;
                                        dr["Chd_AUDF"] = Convert.ToInt32(dr["Chd_AUDF"].ToString()) + AUDF;
                                        dr["Chd_EX"] = Convert.ToInt32(dr["Chd_EX"].ToString()) + EXT;
                                        dr["FareQuote"] = "Updated";
                                        dr.AcceptChanges();
                                        dtBound.AcceptChanges();
                                    }
                                }

                                BASIC = 0;
                                YQ = 0;
                                TRF = 0;
                                UDF = 0;
                                PSF = 0;
                                AUDF = 0;
                                TF = 0;
                                GST = 0;
                                EXT = 0;
                            }
                        }
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               // DBCommon.Logger.dbLogg(CompanyID, BookingRef, "SetRTPriceModifier", "air_AkasaAir", "", Searchid, ex.Message + "," + ex.StackTrace);
            }
            return IsFareAvailable;
        }
        private DataTable MakeFlightCombination(DataTable SelectDT)
        {
            DataTable CombineDT = new DataTable();

            try
            {
                bool bSameCls = false;
                if (SelectDT.Rows.Count.Equals(2))
                {
                    if (SelectDT.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDT.Rows[1]["FlightNumber"].ToString().Trim()))
                    {
                        bSameCls = true;
                    }
                }
                else if (SelectDT.Rows.Count.Equals(3))
                {
                    if (SelectDT.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDT.Rows[1]["FlightNumber"].ToString().Trim()) && SelectDT.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDT.Rows[2]["FlightNumber"].ToString().Trim()))
                    {
                        bSameCls = true;
                    }
                }
                else if (SelectDT.Rows.Count.Equals(1))
                {
                    bSameCls = true;
                }

                if (bSameCls.Equals(true))
                {
                    CombineDT = SelectDT.Clone();
                    DataRow dr1 = SelectDT.Rows[0];
                    dr1["DepartureStation"] = dr1["Origin"];
                    dr1["ArrivalStation"] = dr1["Destination"];
                    CombineDT.ImportRow(dr1);
                    CombineDT.AcceptChanges();
                }
                else
                {
                    if (SelectDT.Rows.Count.Equals(3))
                    {
                        CombineDT = SelectDT.Clone();
                        string sFlightNum1 = SelectDT.Rows[0]["FlightNumber"].ToString().Trim();
                        string sFlightNum2 = SelectDT.Rows[1]["FlightNumber"].ToString().Trim();
                        string sFlightNum3 = SelectDT.Rows[2]["FlightNumber"].ToString().Trim();

                        if (sFlightNum1.Equals(sFlightNum2) && sFlightNum1 != sFlightNum3)
                        {
                            DataRow dr1 = SelectDT.Rows[0];
                            DataRow dr2 = SelectDT.Rows[1];
                            DataRow dr3 = SelectDT.Rows[2];

                            for (int j = 0; j < 2; j++)
                            {
                                if (j.Equals(0))
                                {
                                    CombineDT.ImportRow(dr1);
                                    CombineDT.Rows[0]["ArrivalStation"] = dr2["ArrivalStation"];
                                    CombineDT.AcceptChanges();
                                }
                                else if (j.Equals(1))
                                {
                                    CombineDT.ImportRow(dr3);
                                    CombineDT.AcceptChanges();
                                }
                            }
                        }
                        else if (sFlightNum1 != sFlightNum2 && sFlightNum2.Equals(sFlightNum3))
                        {
                            DataRow dr1 = SelectDT.Rows[0];
                            DataRow dr2 = SelectDT.Rows[1];
                            DataRow dr3 = SelectDT.Rows[2];

                            for (int j = 0; j < 2; j++)
                            {
                                if (j.Equals(0))
                                {
                                    CombineDT.ImportRow(dr1);
                                    CombineDT.AcceptChanges();
                                }
                                else if (j.Equals(1))
                                {
                                    CombineDT.ImportRow(dr2);
                                    CombineDT.Rows[1]["ArrivalStation"] = dr3["ArrivalStation"];
                                    CombineDT.AcceptChanges();
                                }
                            }
                        }
                        else
                        {
                            CombineDT = SelectDT;
                            CombineDT.AcceptChanges();
                        }
                    }
                    else
                    {
                        CombineDT = SelectDT;
                        CombineDT.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return CombineDT;
        }
        //================================================================================================================================================
        public bool GetOneWayFares(string SearchID, DataTable dtBound, bool IsfareMethod)
        {
            ArrayList ArRefid = new ArrayList();
            ArRefid = GetCommonFunctions.DataTable2ArrayList(dtBound, "RefID", true);

            DataTable SelectDt = new DataTable();
            for (int i = 0; i < ArRefid.Count; i++)
            {
                SelectDt = dtBound.Select("RefID='" + ArRefid[i].ToString() + "'").CopyToDataTable();
                if (SelectDt.Rows.Count > 0)
                {
                    if (IsfareMethod)
                    {
                        DataRow[] rows = dtBound.Select("RefID='" + ArRefid[i].ToString() + "'");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow dr in rows)
                            {
                                dr["Adt_BASIC"] = 0;
                                dr["Adt_YQ"] = 0;
                                dr["Adt_PSF"] = 0;
                                dr["Adt_UDF"] = 0;
                                dr["Adt_AUDF"] = 0;
                                dr["Adt_CUTE"] = 0;
                                dr["Adt_GST"] = 0;
                                dr["Adt_TF"] = 0;
                                dr["Adt_CESS"] = 0;
                                dr["Adt_EX"] = 0;

                                dr["Chd_BASIC"] = 0;
                                dr["Chd_YQ"] = 0;
                                dr["Chd_PSF"] = 0;
                                dr["Chd_UDF"] = 0;
                                dr["Chd_AUDF"] = 0;
                                dr["Chd_CUTE"] = 0;
                                dr["Chd_GST"] = 0;
                                dr["Chd_TF"] = 0;
                                dr["Chd_CESS"] = 0;
                                dr["Chd_EX"] = 0;

                                dr["Inf_BASIC"] = 0;
                                dr["Inf_TAX"] = 0;

                                dr["FareStatus"] = false;

                                dr.AcceptChanges();
                                dtBound.AcceptChanges();
                            }
                        }
                    }


                    bool IsFareAvailable = GetOneWayPriceRequest(SearchID, SelectDt, dtBound, ArRefid[i].ToString());
                    if (!IsFareAvailable)
                    {
                        DataRow[] rows = dtBound.Select("RefID='" + ArRefid[i].ToString() + "'");
                        if (rows.Length > 0)
                        {
                            foreach (DataRow dr in rows)
                            {
                                if (IsfareMethod)
                                {
                                    dr["FareQuote"] = "Updated";
                                }

                                dr["FareStatus"] = false;
                                dr.AcceptChanges();
                                dtBound.AcceptChanges();
                            }
                        }
                    }

                    if (IsfareMethod)
                    {
                        return IsFareAvailable;
                    }
                }
            }
            dtBound.AcceptChanges();
            return true;
        }
        private bool GetOneWayPriceRequest(string SearchID,DataTable SelectDt, DataTable dtBound, string Refid)
        {
            bool IsFareAvailable = false;
            string Supplierid = SelectDt.Rows[0]["AirlineID"].ToString();
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                CS_FARE_SSR objfare = new CS_FARE_SSR();
                string JsonFlightFareRulesRQ = objfare.GetFlightFareRulesRQ_V2(SearchID, dtBound);


                var commonQP = new CommonQP(_credential);
                string _Token = commonQP.GetTokenAsync("", "").GetAwaiter().GetResult();    
                Task<string> jsonSearchRSAw = Task.Run(() => CommonQP.GetResponseQpAsync(SearchID, JsonFlightFareRulesRQ, "Quote", _Token));

                string JsonFlightFareRulesRS = jsonSearchRSAw.GetAwaiter().GetResult();


                FareQpRS objApiPriceItineraryResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FareQpRS>(JsonFlightFareRulesRS);



                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string Signature = SelectDt.Rows[0]["Api_SessionID"].ToString();

                Int32 Adt = Convert.ToInt32(SelectDt.Rows[0]["Adt"].ToString());
                Int32 Chd = Convert.ToInt32(SelectDt.Rows[0]["Chd"].ToString());
                Int32 Inf = Convert.ToInt32(SelectDt.Rows[0]["Inf"].ToString());

                int iPaxCount = Chd + Adt;
               
                decimal BookingSum = objApiPriceItineraryResponse.Data.Breakdown.balanceDue; //dd this as temp amount  by  Rangaa // objApiPriceItineraryResponse.Booking.BookingSum.BalanceDue;


                if (objApiPriceItineraryResponse != null && objApiPriceItineraryResponse.Data.Journeys != null && objApiPriceItineraryResponse.Data.Passengers != null)
                {
                    ArrayList ArSectors = new ArrayList();

                    if (Inf > 0)  //(Adt > 0) //
                    {
                        if (SelectDt.Rows.Count.Equals(1))
                        {
                            ArSectors.Add(SelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[0]["DepartureStation"].ToString().Trim() + SelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                        }
                        else if (SelectDt.Rows.Count.Equals(2))
                        {
                            if (SelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDt.Rows[1]["FlightNumber"].ToString().Trim()))
                            {
                                ArSectors.Add(SelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[0]["Origin"].ToString().Trim() + SelectDt.Rows[0]["Destination"].ToString().Trim());
                            }
                            else
                            {
                                ArSectors.Add(SelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[0]["DepartureStation"].ToString().Trim() + SelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                                ArSectors.Add(SelectDt.Rows[1]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[1]["DepartureStation"].ToString().Trim() + SelectDt.Rows[1]["ArrivalStation"].ToString().Trim());
                            }
                        }
                        else if (SelectDt.Rows.Count.Equals(3))
                        {
                            if (SelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDt.Rows[1]["FlightNumber"].ToString().Trim()).Equals(SelectDt.Rows[2]["FlightNumber"].ToString().Trim()))
                            {
                                ArSectors.Add(SelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[0]["Origin"].ToString().Trim() + SelectDt.Rows[0]["Destination"].ToString().Trim());
                            }
                            else
                            {
                                ArSectors.Add(SelectDt.Rows[0]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[0]["DepartureStation"].ToString().Trim() + SelectDt.Rows[0]["ArrivalStation"].ToString().Trim());
                                ArSectors.Add(SelectDt.Rows[1]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[1]["DepartureStation"].ToString().Trim() + SelectDt.Rows[1]["ArrivalStation"].ToString().Trim());
                                ArSectors.Add(SelectDt.Rows[2]["FlightNumber"].ToString().Trim() + " " + SelectDt.Rows[2]["DepartureStation"].ToString().Trim() + SelectDt.Rows[2]["ArrivalStation"].ToString().Trim());
                            }
                        }
                    }

                    var bb = objApiPriceItineraryResponse.Data.Passengers.FirstOrDefault().Value;
                    List<JourneyDtl4Fare> Seg = objApiPriceItineraryResponse.Data.Journeys;

                    if (Inf > 0)  //(Adt > 0) //
                    {
                        int iBASIC = 0;
                        int iEXT = 0;
                        string ChargeCode = string.Empty;

                        List<Passenger4Fare> infpax = bb.PassengerList;
                        foreach (Passenger4Fare _PassengerDtl in infpax)
                        {
                            List<ServiceCharge4Fare> pfees = _PassengerDtl.Infant.InfantFees.FirstOrDefault().ServiceCharges; // infpax[i].PassengerFees;
                            //for (int j = 0; j < pfee.Length; j++)
                            foreach (ServiceCharge4Fare _pfee in pfees)
                            {
                                //if (ValidSectorForInfant(ArSectors, pfee[j].FlightReference).Equals(true))
                                //if (ValidSectorForInfant(ArSectors, _pfee.FlightReference).Equals(true))
                                if (ValidSectorForInfant(ArSectors, _PassengerDtl.Infant.InfantFees.FirstOrDefault().FlightReference).Equals(true))
                                {

                                    //svc_booking.BookingServiceCharge[] bsc = pfee[j].ServiceCharges;
                                    List<ServiceCharge4Fare> _bscs = _PassengerDtl.Infant.InfantFees.FirstOrDefault().ServiceCharges;   // _pfee.ServiceCharges;
                                    //for (int n = 0; n < bsc.Length; n++)
                                    foreach (ServiceCharge4Fare _bsc in _bscs)
                                    {
                                        int iValue = Decimal.ToInt32(_bsc.Amount);
                                        ChargeCode = _bsc.Code; // .ChargeCode;
                                        if (_bsc.Code == null || _bsc.Code.Equals(string.Empty) || _bsc.Code.Equals("INFT"))
                                        {
                                            ChargeCode = "BASIC";
                                            iBASIC += iValue;
                                        }
                                        else
                                        {
                                            ChargeCode = "EX";
                                            iEXT += iValue;
                                        }
                                    }

                                    if (iBASIC > 0 || iEXT > 0)
                                    {
                                        DataRow[] rows = dtBound.Select("RefID='" + Refid + "'");
                                        if (rows.Length > 0)
                                        {
                                            foreach (DataRow dr in rows)
                                            {
                                                dr["Inf_BASIC"] = Convert.ToInt32(dr["Inf_BASIC"].ToString()) + iBASIC;
                                                dr["Inf_TAX"] = Convert.ToInt32(dr["Inf_TAX"].ToString()) + iEXT;
                                                dr.AcceptChanges();
                                                dtBound.AcceptChanges();
                                            }
                                        }

                                        iBASIC = 0;
                                        iEXT = 0;
                                    }
                                }
                            }
                            break;
                        }
                    }

                    int BASIC = 0;
                    int YQ = 0;
                    int TRF = 0;
                    int UDF = 0;
                    int PSF = 0;
                    int AUDF = 0;
                    int TF = 0;
                    int GST = 0;
                    int EXT = 0;

                    Decimal iVal = 0;
                    //for (int i = 0; i < Seg.Length; i++)
                    foreach (JourneyDtl4Fare _JourneyDtl in Seg)
                    {
                        //svc_booking.Segment[] segkk = Seg[i].Segments;
                        List<Segment> segkk = _JourneyDtl.Segments;
                        //for (int j = 0; j < segkk.Length; j++)
                        foreach (Segment _Segment in segkk)
                        {
                            List<FareObject4FareQuote> fares = _Segment.Fares;
                            //for (int k = 0; k < fares.Length; k++)
                            foreach (FareObject4FareQuote _Fare in fares)
                            {
                                //svc_booking.PaxFare[] pfare = fares[k].PaxFares;
                                List<PassengerFare> pfare = _Fare.PassengerFares;
                                //for (int l = 0; l < pfare.Length; l++)
                                foreach (PassengerFare _PassengerFare in pfare)
                                {
                                    List<ServiceCharge> _bscs = _PassengerFare.ServiceCharges;
                                    if (_PassengerFare.PassengerType == "ADT")
                                    {
                                        //for (int n = 0; n < bsc.Length; n++)
                                        foreach (ServiceCharge _ServiceCharge in _bscs)
                                        {
                                            iVal = Convert.ToDecimal(_ServiceCharge.Amount);

                                            if (_ServiceCharge.Code == null || _ServiceCharge.Code.Equals(string.Empty))
                                            {
                                                BASIC += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("TRF"))
                                            {
                                                TRF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("CGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("SGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("IGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("C33") != -1) // C refers to CGST
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("S33") != -1) //S refers to SGST
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }

                                            else if (_ServiceCharge.Code.Equals("PSF"))
                                            {
                                                PSF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("YQ"))
                                            {
                                                YQ += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("ASF"))
                                            {
                                                UDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("RCS"))
                                            {
                                                AUDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("TF"))
                                            {
                                                TF += Decimal.ToInt32(iVal);
                                            }
                                            else
                                            {
                                                EXT += Decimal.ToInt32(iVal);
                                            }
                                        }

                                        if (BASIC > 0 || TRF > 0 || GST > 0 || PSF > 0 || YQ > 0 || UDF > 0 || EXT > 0 || AUDF > 0 || TF > 0)
                                        {
                                            DataRow[] rows = dtBound.Select("RefID='" + Refid + "'");
                                            if (rows.Length > 0)
                                            {
                                                foreach (DataRow dr in rows)
                                                {
                                                    dr["Chd_Import"] = BookingSum;
                                                    dr["FareStatus"] = true;

                                                    dr["Adt_BASIC"] = Convert.ToInt32(dr["Adt_BASIC"].ToString()) + BASIC;
                                                    dr["Adt_YQ"] = Convert.ToInt32(dr["Adt_YQ"].ToString()) + YQ;
                                                    dr["Adt_PSF"] = Convert.ToInt32(dr["Adt_PSF"].ToString()) + PSF;
                                                    dr["Adt_TF"] = Convert.ToInt32(dr["Adt_TF"].ToString()) + TF;
                                                    dr["Adt_CUTE"] = Convert.ToInt32(dr["Adt_CUTE"].ToString()) + TRF;
                                                    dr["Adt_GST"] = Convert.ToInt32(dr["Adt_GST"].ToString()) + GST;
                                                    dr["Adt_UDF"] = Convert.ToInt32(dr["Adt_UDF"].ToString()) + UDF;
                                                    dr["Adt_AUDF"] = Convert.ToInt32(dr["Adt_AUDF"].ToString()) + AUDF;
                                                    dr["Adt_EX"] = Convert.ToInt32(dr["Adt_EX"].ToString()) + EXT;
                                                    dr["FareQuote"] = "Updated";
                                                    dr.AcceptChanges();
                                                    dtBound.AcceptChanges();

                                                    IsFareAvailable = true;
                                                }
                                            }



                                            BASIC = 0;
                                            YQ = 0;
                                            TRF = 0;
                                            UDF = 0;
                                            PSF = 0;
                                            AUDF = 0;
                                            TF = 0;
                                            GST = 0;
                                            EXT = 0;
                                        }
                                    }
                                    else
                                    {
                                        BASIC = 0;
                                        YQ = 0;
                                        TRF = 0;
                                        UDF = 0;
                                        PSF = 0;
                                        AUDF = 0;
                                        TF = 0;
                                        GST = 0;
                                        EXT = 0;

                                        //for (int n = 0; n < bsc.Length; n++)
                                        foreach (ServiceCharge _ServiceCharge in _bscs)
                                        {
                                            iVal = Convert.ToDecimal(_ServiceCharge.Amount);

                                            if (_ServiceCharge.Code == null || _ServiceCharge.Code.Equals(string.Empty))
                                            {
                                                BASIC += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("TRF"))
                                            {
                                                TRF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("CGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("SGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("IGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("C33") != -1) // C refers to CGST
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.IndexOf("S33") != -1) //S refers to SGST
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("PSF"))
                                            {
                                                PSF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("YQ"))
                                            {
                                                YQ += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("ASF"))
                                            {
                                                UDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("RCS"))
                                            {
                                                AUDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (_ServiceCharge.Code.Equals("TF"))
                                            {
                                                TF += Decimal.ToInt32(iVal);
                                            }
                                            else
                                            {
                                                EXT += Decimal.ToInt32(iVal);
                                            }
                                        }

                                        if (BASIC > 0 || TRF > 0 || GST > 0 || PSF > 0 || YQ > 0 || UDF > 0 || EXT > 0 || AUDF > 0 || TF > 0)
                                        {
                                            DataRow[] rows = dtBound.Select("RefID='" + Refid + "'");
                                            if (rows.Length > 0)
                                            {
                                                foreach (DataRow dr in rows)
                                                {
                                                    dr["Chd_BASIC"] = Convert.ToInt32(dr["Chd_BASIC"].ToString()) + BASIC;
                                                    dr["Chd_YQ"] = Convert.ToInt32(dr["Chd_YQ"].ToString()) + YQ;
                                                    dr["Chd_PSF"] = Convert.ToInt32(dr["Chd_PSF"].ToString()) + PSF;
                                                    dr["Chd_TF"] = Convert.ToInt32(dr["Chd_TF"].ToString()) + TF;
                                                    dr["Chd_CUTE"] = Convert.ToInt32(dr["Chd_CUTE"].ToString()) + TRF;
                                                    dr["Chd_GST"] = Convert.ToInt32(dr["Chd_GST"].ToString()) + GST;
                                                    dr["Chd_UDF"] = Convert.ToInt32(dr["Chd_UDF"].ToString()) + UDF;
                                                    dr["Chd_AUDF"] = Convert.ToInt32(dr["Chd_AUDF"].ToString()) + AUDF;
                                                    dr["Chd_EX"] = Convert.ToInt32(dr["Chd_EX"].ToString()) + EXT;
                                                    dr["FareQuote"] = "Updated";
                                                    dr.AcceptChanges();
                                                    dtBound.AcceptChanges();
                                                }
                                            }

                                            BASIC = 0;
                                            YQ = 0;
                                            TRF = 0;
                                            UDF = 0;
                                            PSF = 0;
                                            AUDF = 0;
                                            TF = 0;
                                            GST = 0;
                                            EXT = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    IsFareAvailable = true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //DBCommon.Logger.dbLogg(CompanyID, BookingRef, "GetOneWayPriceRequest-" + Refid, "air_AkasaAir", "", Searchid, ex.Message + "," + ex.StackTrace);
            }
           // DBCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetOneWayPriceRequest-air_AkasaAir", "FARE", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return IsFareAvailable;
        }
        private bool ValidSectorForInfant(ArrayList Ar, string FlightReference)
        {
            bool bValid = false;
            for (int i = 0; i < Ar.Count; i++)
            {
                if (FlightReference.IndexOf(Ar[i].ToString().Trim()) != -1)
                {
                    bValid = true;
                    break;
                }
            }

            return bValid;
        }
        //================================================================================================================================================
    }
}


