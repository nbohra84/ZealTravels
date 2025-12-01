using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiAvailabilityFare
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiAvailabilityFare(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public bool GetRTFares(DataTable dtBound, bool IsfareMethod)
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

                    bool IsFareAvailable = GetRTPriceRequest(OutSelectDt, InSelectDt, dtBound, ArRefid[i].ToString());
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
        private bool GetRTPriceRequest(DataTable OutSelectDt, DataTable InSelectDt, DataTable dtBound, string Refid)
        {
            bool IsFareAvailable = false;
            string Supplierid = OutSelectDt.Rows[0]["AirlineID"].ToString();
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string Signature = OutSelectDt.Rows[0]["Api_SessionID"].ToString();

                Int32 Adt = Convert.ToInt32(OutSelectDt.Rows[0]["Adt"].ToString());
                Int32 Chd = Convert.ToInt32(OutSelectDt.Rows[0]["Chd"].ToString());
                Int32 Inf = Convert.ToInt32(OutSelectDt.Rows[0]["Inf"].ToString());

                svc_booking.PriceItineraryRequest objPriceApiRequest = new svc_booking.PriceItineraryRequest();
                objPriceApiRequest.Signature = Signature;
                objPriceApiRequest.ContractVersion = ContractVersion;

                objPriceApiRequest.ItineraryPriceRequest = new svc_booking.ItineraryPriceRequest();
                objPriceApiRequest.ItineraryPriceRequest.PriceItineraryBy = svc_booking.PriceItineraryBy.JourneyBySellKey;
                objPriceApiRequest.ItineraryPriceRequest.PriceItineraryBySpecified = true;

                int iPaxCount = Chd + Adt;
                if (Inf > 0)
                {
                    DataTable dtMerge = OutSelectDt.Clone();
                    dtMerge.Merge(MakeFlightCombination(OutSelectDt));
                    dtMerge.Merge(MakeFlightCombination(InSelectDt));
                    dtMerge.AcceptChanges();

                    objPriceApiRequest.ItineraryPriceRequest.SSRRequest = new svc_booking.SSRRequest();
                    objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[dtMerge.Rows.Count];

                    for (int j = 0; j < dtMerge.Rows.Count; j++)
                    {
                        DataRow dr = dtMerge.Rows[j];

                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr["ArrivalStation"].ToString();
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr["DepartureStation"].ToString();

                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr["DepDate"].ToString());
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                        for (short i = 0; i < Inf; i++)
                        {
                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";
                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;


                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;


                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                            objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();
                        }
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.CurrencyCode = "INR";
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSell = false;
                        objPriceApiRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;
                    }
                }


                svc_booking.SellJourneyByKeyRequestData srkd = new svc_booking.SellJourneyByKeyRequestData();
                srkd.ActionStatusCode = "NN";

                srkd.JourneySellKeys = new svc_booking.SellKeyList[2];
                srkd.JourneySellKeys[0] = new svc_booking.SellKeyList();
                srkd.JourneySellKeys[0].JourneySellKey = OutSelectDt.Rows[0]["JourneySellKey"].ToString();
                srkd.JourneySellKeys[0].FareSellKey = OutSelectDt.Rows[0]["FareSellKey"].ToString();

                srkd.JourneySellKeys[1] = new svc_booking.SellKeyList();
                srkd.JourneySellKeys[1].JourneySellKey = InSelectDt.Rows[0]["JourneySellKey"].ToString();
                srkd.JourneySellKeys[1].FareSellKey = InSelectDt.Rows[0]["FareSellKey"].ToString();

                srkd.PaxPriceType = new svc_booking.PaxPriceType[iPaxCount];
                for (short i = 0; i < Adt; i++)
                {
                    srkd.PaxPriceType[i] = new svc_booking.PaxPriceType();
                    srkd.PaxPriceType[i].PaxType = "ADT";
                }

                for (short i = 0; i < Chd; i++)
                {
                    srkd.PaxPriceType[Adt + i] = new svc_booking.PaxPriceType();
                    srkd.PaxPriceType[Adt + i].PaxType = "CHD";
                }

                srkd.CurrencyCode = "INR";
                srkd.PaxCount = short.Parse(iPaxCount.ToString());
                srkd.PaxCountSpecified = true;

                srkd.IsAllotmentMarketFare = false;
                srkd.IsAllotmentMarketFareSpecified = false;

                srkd.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                srkd.LoyaltyFilterSpecified = true;
                objPriceApiRequest.ItineraryPriceRequest.SellByKeyRequest = srkd;


                GetApiRequest = GetCommonFunctions.Serialize(objPriceApiRequest);

                svc_booking.IBookingManager objBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.PriceItineraryResponse objApiPriceItineraryResponse = objBookingManager.GetItineraryPrice(objPriceApiRequest);

                GetApiResponse = GetCommonFunctions.Serialize(objApiPriceItineraryResponse);

                if (objApiPriceItineraryResponse != null && objApiPriceItineraryResponse.Booking != null && objApiPriceItineraryResponse.Booking.Journeys != null)
                {
                    decimal BookingSum = objApiPriceItineraryResponse.Booking.BookingSum.BalanceDue;

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

                    IsFareAvailable = SetRTPriceModifier(ArOutbound, objApiPriceItineraryResponse.Booking.Passengers, objApiPriceItineraryResponse.Booking.Journeys[0].Segments, Inf, BookingSum, dtBound, Refid);
                    IsFareAvailable = SetRTPriceModifier(ArInbound, objApiPriceItineraryResponse.Booking.Passengers, objApiPriceItineraryResponse.Booking.Journeys[1].Segments, Inf, BookingSum, dtBound, Refid);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetRTPriceRequest", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }

            //dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetRTPriceRequest-air_spicejet", "FARE", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
            return IsFareAvailable;
        }
        private bool SetRTPriceModifier(ArrayList ArSectors, svc_booking.Passenger[] objPassenger, svc_booking.Segment[] objSegment, int iInf, decimal BookingSum, DataTable dtBound, string Refid)
        {
            bool IsFareAvailable = false;

            try
            {
                if (iInf > 0)
                {
                    int iBASIC = 0;
                    int iEXT = 0;
                    string ChargeCode = string.Empty;

                    for (int i = 0; i < objPassenger.Length;)
                    {
                        svc_booking.PassengerFee[] pfee = objPassenger[0].PassengerFees;
                        for (int j = 0; j < pfee.Length; j++)
                        {
                            if (ValidSectorForInfant(ArSectors, pfee[j].FlightReference).Equals(true))
                            {
                                svc_booking.BookingServiceCharge[] bsc = pfee[j].ServiceCharges;
                                for (int n = 0; n < bsc.Length; n++)
                                {
                                    if (bsc[n].ChargeType.ToString().ToUpper() != "IncludedTax".ToUpper())
                                    {
                                        int iValue = Decimal.ToInt32(bsc[n].Amount);
                                        ChargeCode = bsc[n].ChargeCode;
                                        if (bsc[n].ChargeCode.Equals(string.Empty) || bsc[n].ChargeCode.Equals("INFT"))
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
                for (int j = 0; j < objSegment.Length; j++)
                {
                    svc_booking.Fare[] fares = objSegment[j].Fares;
                    for (int k = 0; k < fares.Length; k++)
                    {
                        svc_booking.PaxFare[] pfare = fares[k].PaxFares;
                        for (int l = 0; l < pfare.Length; l++)
                        {
                            svc_booking.BookingServiceCharge[] bsc = pfare[l].ServiceCharges;
                            if (pfare[l].PaxType == "ADT")
                            {
                                for (int n = 0; n < bsc.Length; n++)
                                {
                                    iVal = Convert.ToDecimal(bsc[n].Amount);

                                    if (bsc[n].ChargeCode.Equals(string.Empty))
                                    {
                                        BASIC += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("TRF"))
                                    {
                                        TRF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.IndexOf("CGST") != -1)
                                    {
                                        GST += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.IndexOf("SGST") != -1)
                                    {
                                        GST += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.IndexOf("IGST") != -1)
                                    {
                                        GST += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("PSF"))
                                    {
                                        PSF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("YQ"))
                                    {
                                        YQ += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("ASF"))
                                    {
                                        UDF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("RCS"))
                                    {
                                        AUDF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("TF"))
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

                                for (int n = 0; n < bsc.Length; n++)
                                {
                                    iVal = Convert.ToDecimal(bsc[n].Amount);

                                    if (bsc[n].ChargeCode.Equals(string.Empty))
                                    {
                                        BASIC += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("TRF"))
                                    {
                                        TRF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.IndexOf("CGST") != -1)
                                    {
                                        GST += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.IndexOf("SGST") != -1)
                                    {
                                        GST += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.IndexOf("IGST") != -1)
                                    {
                                        GST += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("PSF"))
                                    {
                                        PSF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("YQ"))
                                    {
                                        YQ += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("ASF"))
                                    {
                                        UDF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("RCS"))
                                    {
                                        AUDF += Decimal.ToInt32(iVal);
                                    }
                                    else if (bsc[n].ChargeCode.Equals("TF"))
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
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "SetRTPriceModifier", "air_spicejet", "", Searchid, ex.Message + "," + ex.StackTrace);
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
        public bool GetOneWayFares(DataTable dtBound, bool IsfareMethod)
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


                    bool IsFareAvailable = GetOneWayPriceRequest(SelectDt, dtBound, ArRefid[i].ToString());
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
        private bool GetOneWayPriceRequest(DataTable SelectDt, DataTable dtBound, string Refid)
        {
            bool IsFareAvailable = false;
            string Supplierid = SelectDt.Rows[0]["AirlineID"].ToString();
            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                string Signature = SelectDt.Rows[0]["Api_SessionID"].ToString();

                Int32 Adt = Convert.ToInt32(SelectDt.Rows[0]["Adt"].ToString());
                Int32 Chd = Convert.ToInt32(SelectDt.Rows[0]["Chd"].ToString());
                Int32 Inf = Convert.ToInt32(SelectDt.Rows[0]["Inf"].ToString());

                svc_booking.PriceItineraryRequest objApiPriceRequest = new svc_booking.PriceItineraryRequest();
                objApiPriceRequest.Signature = Signature;
                objApiPriceRequest.ContractVersion = ContractVersion;

                objApiPriceRequest.ItineraryPriceRequest = new svc_booking.ItineraryPriceRequest();
                objApiPriceRequest.ItineraryPriceRequest.PriceItineraryBy = svc_booking.PriceItineraryBy.JourneyBySellKey;
                objApiPriceRequest.ItineraryPriceRequest.PriceItineraryBySpecified = true;

                int iPaxCount = Chd + Adt;

                if (Inf > 0)
                {
                    bool bSameCls = false;

                    if (SelectDt.Rows.Count.Equals(1))
                    {
                        bSameCls = true;
                    }
                    else if (SelectDt.Rows.Count.Equals(2))
                    {
                        if (SelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDt.Rows[1]["FlightNumber"].ToString().Trim()))
                        {
                            bSameCls = true;
                        }
                    }
                    else if (SelectDt.Rows.Count.Equals(3))
                    {
                        if (SelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDt.Rows[1]["FlightNumber"].ToString().Trim()) && SelectDt.Rows[0]["FlightNumber"].ToString().Trim().Equals(SelectDt.Rows[2]["FlightNumber"].ToString().Trim()))
                        {
                            bSameCls = true;
                        }
                    }

                    if (bSameCls.Equals(true))
                    {
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest = new svc_booking.SSRRequest();
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[1];

                        DataRow dr = SelectDt.Rows[0];
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0] = new svc_booking.SegmentSSRRequest();
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].FlightDesignator = new svc_booking.FlightDesignator();
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].FlightDesignator.OpSuffix = " ";
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].ArrivalStation = dr["Destination"].ToString();
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].DepartureStation = dr["Origin"].ToString();

                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].STD = Convert.ToDateTime(dr["DepDate"].ToString());
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].STDSpecified = true;


                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs = new svc_booking.PaxSSR[Inf];

                        for (short i = 0; i < Inf; i++)
                        {
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i] = new svc_booking.PaxSSR();

                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].State = svc_booking.MessageState.New;
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].StateSpecified = true;


                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ActionStatusCode = "NN";


                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumber = i;
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].PassengerNumberSpecified = true;


                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRCode = "INFT";


                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumber = i;
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRNumberSpecified = true;


                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValue = 0;
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].SSRValueSpecified = true;

                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].ArrivalStation = dr["Destination"].ToString();
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[0].PaxSSRs[i].DepartureStation = dr["Origin"].ToString();
                        }

                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.CurrencyCode = "INR";
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSell = false;
                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;
                    }
                    else
                    {
                        if (SelectDt.Rows.Count.Equals(3))
                        {
                            string sFlightNum1 = SelectDt.Rows[0]["FlightNumber"].ToString().Trim();
                            string sFlightNum2 = SelectDt.Rows[1]["FlightNumber"].ToString().Trim();
                            string sFlightNum3 = SelectDt.Rows[2]["FlightNumber"].ToString().Trim();

                            if (sFlightNum1.Equals(sFlightNum2) && sFlightNum1 != sFlightNum3)
                            {
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest = new svc_booking.SSRRequest();
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[2];

                                DataRow dr1 = SelectDt.Rows[0];
                                DataRow dr2 = SelectDt.Rows[1];
                                DataRow dr3 = SelectDt.Rows[2];

                                for (int j = 0; j < 2; j++)
                                {
                                    if (j.Equals(0))
                                    {
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr1["CarrierCode"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr1["FlightNumber"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr2["ArrivalStation"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr1["DepartureStation"].ToString();


                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr1["DepDate"].ToString());
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                        for (short i = 0; i < Inf; i++)
                                        {
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr2["ArrivalStation"].ToString();
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr1["DepartureStation"].ToString();
                                        }
                                    }
                                    else if (j.Equals(1))
                                    {
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr3["CarrierCode"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr3["FlightNumber"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr3["ArrivalStation"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr3["DepartureStation"].ToString();

                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr3["DepDate"].ToString());
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;


                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                        for (short i = 0; i < Inf; i++)
                                        {
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr3["ArrivalStation"].ToString();
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr3["DepartureStation"].ToString();
                                        }
                                    }

                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.CurrencyCode = "INR";
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSell = false;
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSellSpecified = false;
                                }
                            }
                            else if (sFlightNum1 != sFlightNum2 && sFlightNum2.Equals(sFlightNum3))
                            {
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest = new svc_booking.SSRRequest();
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[2];

                                DataRow dr1 = SelectDt.Rows[0];
                                DataRow dr2 = SelectDt.Rows[1];
                                DataRow dr3 = SelectDt.Rows[2];

                                for (int j = 0; j < 2; j++)
                                {
                                    if (j.Equals(0))
                                    {
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr1["CarrierCode"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr1["FlightNumber"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr1["ArrivalStation"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr1["DepartureStation"].ToString();

                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr1["DepDate"].ToString());
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                        for (short i = 0; i < Inf; i++)
                                        {
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";


                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;


                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;


                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr1["ArrivalStation"].ToString();
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr1["DepartureStation"].ToString();
                                        }
                                    }
                                    else if (j.Equals(1))
                                    {
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr2["CarrierCode"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr2["FlightNumber"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr3["ArrivalStation"].ToString();
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr2["DepartureStation"].ToString();

                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr2["DepDate"].ToString());
                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;

                                        objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                        for (short i = 0; i < Inf; i++)
                                        {
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;


                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;


                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";

                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;


                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;


                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr3["ArrivalStation"].ToString();
                                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr2["DepartureStation"].ToString();
                                        }
                                    }
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.CurrencyCode = "INR";
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSell = false;
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;
                                }
                            }
                        }
                        else
                        {
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest = new svc_booking.SSRRequest();
                            objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests = new svc_booking.SegmentSSRRequest[SelectDt.Rows.Count];

                            for (int j = 0; j < SelectDt.Rows.Count; j++)
                            {
                                DataRow dr = SelectDt.Rows[j];

                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j] = new svc_booking.SegmentSSRRequest();
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator = new svc_booking.FlightDesignator();
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.CarrierCode = dr["CarrierCode"].ToString();
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.FlightNumber = dr["FlightNumber"].ToString();
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].FlightDesignator.OpSuffix = " ";
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].ArrivalStation = dr["ArrivalStation"].ToString();
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].DepartureStation = dr["DepartureStation"].ToString();

                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STD = Convert.ToDateTime(dr["DepDate"].ToString());
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].STDSpecified = true;


                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs = new svc_booking.PaxSSR[Inf];

                                for (short i = 0; i < Inf; i++)
                                {
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i] = new svc_booking.PaxSSR();

                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].State = svc_booking.MessageState.New;
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].StateSpecified = true;


                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ActionStatusCode = "NN";

                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumber = i;
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].PassengerNumberSpecified = true;


                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRCode = "INFT";


                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumber = i;
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRNumberSpecified = true;


                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValue = 0;
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].SSRValueSpecified = true;


                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].ArrivalStation = dr["ArrivalStation"].ToString();
                                    objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SegmentSSRRequests[j].PaxSSRs[i].DepartureStation = dr["DepartureStation"].ToString();
                                }

                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.CurrencyCode = "INR";
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSell = false;
                                objApiPriceRequest.ItineraryPriceRequest.SSRRequest.SSRFeeForceWaiveOnSellSpecified = true;
                            }
                        }
                    }
                }

                svc_booking.SellJourneyByKeyRequestData srkd = new svc_booking.SellJourneyByKeyRequestData();
                srkd.ActionStatusCode = "NN";
                srkd.JourneySellKeys = new svc_booking.SellKeyList[1];
                srkd.JourneySellKeys[0] = new svc_booking.SellKeyList();
                srkd.JourneySellKeys[0].JourneySellKey = SelectDt.Rows[0]["JourneySellKey"].ToString();
                srkd.JourneySellKeys[0].FareSellKey = SelectDt.Rows[0]["FareSellKey"].ToString();

                srkd.PaxPriceType = new svc_booking.PaxPriceType[iPaxCount];
                for (short i = 0; i < Adt; i++)
                {
                    srkd.PaxPriceType[i] = new svc_booking.PaxPriceType();
                    srkd.PaxPriceType[i].PaxType = "ADT";
                }

                for (short i = 0; i < Chd; i++)
                {
                    srkd.PaxPriceType[Adt + i] = new svc_booking.PaxPriceType();
                    srkd.PaxPriceType[Adt + i].PaxType = "CHD";
                }

                srkd.CurrencyCode = "INR";
                srkd.PaxCount = short.Parse(iPaxCount.ToString());
                srkd.PaxCountSpecified = true;

                srkd.IsAllotmentMarketFare = false;
                srkd.IsAllotmentMarketFareSpecified = true;

                srkd.LoyaltyFilter = svc_booking.LoyaltyFilter.MonetaryOnly;
                srkd.LoyaltyFilterSpecified = true;

                objApiPriceRequest.ItineraryPriceRequest.SellByKeyRequest = srkd;


                GetApiRequest = GetCommonFunctions.Serialize(objApiPriceRequest);

                svc_booking.IBookingManager objBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.PriceItineraryResponse objApiPriceItineraryResponse = objBookingManager.GetItineraryPrice(objApiPriceRequest);
                decimal BookingSum = objApiPriceItineraryResponse.Booking.BookingSum.BalanceDue;

                GetApiResponse = GetCommonFunctions.Serialize(objApiPriceItineraryResponse);

                if (objApiPriceItineraryResponse != null && objApiPriceItineraryResponse.Booking != null && objApiPriceItineraryResponse.Booking.Journeys != null)
                {
                    ArrayList ArSectors = new ArrayList();

                    if (Inf > 0)
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

                    svc_booking.Booking bb = objApiPriceItineraryResponse.Booking;
                    svc_booking.Journey[] Seg = bb.Journeys;

                    if (Inf > 0)
                    {
                        int iBASIC = 0;
                        int iEXT = 0;
                        string ChargeCode = string.Empty;

                        svc_booking.Passenger[] infpax = bb.Passengers;
                        for (int i = 0; i < infpax.Length;)
                        {
                            svc_booking.PassengerFee[] pfee = infpax[i].PassengerFees;
                            for (int j = 0; j < pfee.Length; j++)
                            {
                                if (ValidSectorForInfant(ArSectors, pfee[j].FlightReference).Equals(true))
                                {
                                    svc_booking.BookingServiceCharge[] bsc = pfee[j].ServiceCharges;
                                    for (int n = 0; n < bsc.Length; n++)
                                    {
                                        int iValue = Decimal.ToInt32(bsc[n].Amount);
                                        ChargeCode = bsc[n].ChargeCode;
                                        if (bsc[n].ChargeCode.Equals(string.Empty) || bsc[n].ChargeCode.Equals("INFT"))
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
                    for (int i = 0; i < Seg.Length; i++)
                    {
                        svc_booking.Segment[] segkk = Seg[i].Segments;
                        for (int j = 0; j < segkk.Length; j++)
                        {
                            svc_booking.Fare[] fares = segkk[j].Fares;
                            for (int k = 0; k < fares.Length; k++)
                            {
                                svc_booking.PaxFare[] pfare = fares[k].PaxFares;
                                for (int l = 0; l < pfare.Length; l++)
                                {
                                    svc_booking.BookingServiceCharge[] bsc = pfare[l].ServiceCharges;
                                    if (pfare[l].PaxType == "ADT")
                                    {
                                        for (int n = 0; n < bsc.Length; n++)
                                        {
                                            iVal = Convert.ToDecimal(bsc[n].Amount);

                                            if (bsc[n].ChargeCode.Equals(string.Empty))
                                            {
                                                BASIC += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("TRF"))
                                            {
                                                TRF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.IndexOf("CGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.IndexOf("SGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.IndexOf("IGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("PSF"))
                                            {
                                                PSF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("YQ"))
                                            {
                                                YQ += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("ASF"))
                                            {
                                                UDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("RCS"))
                                            {
                                                AUDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("TF"))
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

                                        for (int n = 0; n < bsc.Length; n++)
                                        {
                                            iVal = Convert.ToDecimal(bsc[n].Amount);

                                            if (bsc[n].ChargeCode.Equals(string.Empty))
                                            {
                                                BASIC += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("TRF"))
                                            {
                                                TRF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.IndexOf("CGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.IndexOf("SGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.IndexOf("IGST") != -1)
                                            {
                                                GST += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("PSF"))
                                            {
                                                PSF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("YQ"))
                                            {
                                                YQ += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("ASF"))
                                            {
                                                UDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("RCS"))
                                            {
                                                AUDF += Decimal.ToInt32(iVal);
                                            }
                                            else if (bsc[n].ChargeCode.Equals("TF"))
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
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetOneWayPriceRequest-" + Refid, "air_spicejet", "", Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetOneWayPriceRequest-air_spicejet", "FARE", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);
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

