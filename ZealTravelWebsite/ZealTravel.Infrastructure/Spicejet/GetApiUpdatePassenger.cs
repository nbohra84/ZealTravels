using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetApiUpdatePassenger
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string CompanyID;
        private Int32 BookingRef;
        private Int32 ContractVersion;
        //-----------------------------------------------------------------------------------------------
        public GetApiUpdatePassenger(string Searchid, string CompanyID, Int32 BookingRef)
        {
            this.ContractVersion = 420;
            this.Searchid = Searchid;
            this.CompanyID = CompanyID;
            this.BookingRef = BookingRef;
        }
        //================================================================================================================================================
        public Decimal GetUpdatePassenger(DataTable dtBound, DataTable dtPassenger)
        {
            Decimal iTotalCost = 0;

            string GetApiRequest = string.Empty;
            string GetApiResponse = string.Empty;
            string Supplierid = string.Empty;

            try
            {
                Supplierid = dtBound.Rows[0]["AirlineID"].ToString().Trim();
                string Signature = dtBound.Rows[0]["Api_SessionID"].ToString().Trim();
                int Adt = int.Parse(dtBound.Rows[0]["Adt"].ToString());
                int Chd = int.Parse(dtBound.Rows[0]["Chd"].ToString());
                int Inf = int.Parse(dtBound.Rows[0]["Inf"].ToString());
                int PaxCount = Chd + Adt;

                DataTable dtInfants = new DataTable();
                DataRow[] drResults = dtPassenger.Select("PaxType='" + "INF" + "'");
                if (drResults.Length > 0)
                {
                    dtInfants = drResults.CopyToDataTable();
                }

                DataTable dtAdults = new DataTable();
                drResults = dtPassenger.Select("PaxType='" + "ADT" + "'");
                if (drResults.Length > 0)
                {
                    dtAdults = drResults.CopyToDataTable();
                }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                svc_booking.UpdatePassengersRequestData objUpdatePassengersRequestData = new svc_booking.UpdatePassengersRequestData();
                objUpdatePassengersRequestData.Passengers = new svc_booking.Passenger[PaxCount];

                int i = 0;
                for (; i < dtAdults.Rows.Count; i++)
                {
                    DataRow dr = dtAdults.Rows[i];

                    if (dr["PaxType"].ToString().Trim().Equals("ADT"))
                    {
                        objUpdatePassengersRequestData.Passengers[i] = new svc_booking.Passenger();

                        objUpdatePassengersRequestData.Passengers[i].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].PassengerPrograms = new svc_booking.PassengerProgram[1];
                        objUpdatePassengersRequestData.Passengers[i].PassengerPrograms[0] = new svc_booking.PassengerProgram();

                        objUpdatePassengersRequestData.Passengers[i].PassengerPrograms[0].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i].PassengerPrograms[0].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].PassengerNumber = Int16.Parse(i.ToString());
                        objUpdatePassengersRequestData.Passengers[i].PassengerNumberSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].Names = new svc_booking.BookingName[1];
                        objUpdatePassengersRequestData.Passengers[i].PassengerTypeInfos = new svc_booking.PassengerTypeInfo[1];
                        objUpdatePassengersRequestData.Passengers[i].PassengerPrograms[0] = new svc_booking.PassengerProgram();

                        objUpdatePassengersRequestData.Passengers[i].PassengerPrograms[0].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i].PassengerPrograms[0].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].PassengerNumber = Int16.Parse(i.ToString());
                        objUpdatePassengersRequestData.Passengers[i].PassengerNumberSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].FamilyNumber = Int16.Parse(i.ToString());
                        objUpdatePassengersRequestData.Passengers[i].FamilyNumberSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].PassengerID = Int16.Parse(i.ToString());
                        objUpdatePassengersRequestData.Passengers[i].PassengerIDSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].Names[0] = new svc_booking.BookingName();
                        objUpdatePassengersRequestData.Passengers[i].Names[0].FirstName = dr["First_Name"].ToString().Trim();
                        objUpdatePassengersRequestData.Passengers[i].Names[0].MiddleName = "";
                        objUpdatePassengersRequestData.Passengers[i].Names[0].LastName = dr["Last_Name"].ToString().Trim();
                        objUpdatePassengersRequestData.Passengers[i].Names[0].Title = dr["Title"].ToString().Trim();

                        string AdtDob = "1980-01-01";
                        if (dr["DOB"].ToString().Trim().Length > 0)
                        {
                            AdtDob = Convert.ToDateTime(dr["DOB"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        //if (dr["PPNumber"].ToString().Trim().Length > 0 && dr["DateOfExpiry"].ToString().Trim().Length > 0)
                        //{
                        //    string AdtDateOfExpiry = Convert.ToDateTime(dr["DateOfExpiry"].ToString().Trim()).ToString("yyyy-MM-dd");

                        //    //------------------ as per passport detail 
                        //    if (iInf > 0)
                        //    {
                        //        if (InfPaxDt.Rows.Count > i)
                        //        {
                        //            psgrequestdata.Passengers[i].PassengerTravelDocuments = new PassengerTravelDocument[2];
                        //        }
                        //        else
                        //        {
                        //            psgrequestdata.Passengers[i].PassengerTravelDocuments = new PassengerTravelDocument[1];
                        //        }
                        //    }
                        //    else
                        //    {
                        //        psgrequestdata.Passengers[i].PassengerTravelDocuments = new PassengerTravelDocument[1];
                        //    }

                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0] = new PassengerTravelDocument();
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].State = ServiceBookingSG.MessageState.New;
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Names = new BookingName[1];
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Names[0] = new BookingName();
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Names[0].FirstName = dr["FName"].ToString();
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Names[0].MiddleName = dr["MName"].ToString();
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Names[0].LastName = dr["LName"].ToString();
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Names[0].Title = dr["Title"].ToString();

                        //    if (dr["Title"].ToString().ToUpper().IndexOf("MRS") != -1 || dr["Title"].ToString().ToUpper().IndexOf("MS") != -1)
                        //    {
                        //        psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Gender = Gender.Female;
                        //    }
                        //    else if (dr["Title"].ToString().ToUpper().IndexOf("DR") != -1)
                        //    {
                        //        psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Gender = Gender.Unmapped;
                        //    }
                        //    else if (dr["Title"].ToString().ToUpper().IndexOf("MR") != -1)
                        //    {
                        //        psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Gender = Gender.Male;
                        //    }

                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].DocTypeCode = "P";
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].IssuedByCode = dr["Nationality"].ToString();     //"IN";
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].DocNumber = dr["PPNumber"].ToString();           //"A1234567";
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].DOB = DateTime.Parse(AdtDob);                    // as per passport
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].Nationality = dr["Nationality"].ToString();      //  "IN";//as per passport
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].ExpirationDate = DateTime.Parse(AdtDateOfExpiry); //DateTime.Parse(dr["DateOfExpiry"].ToString());     
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].BirthCountry = string.Empty;//dr["Nationality"].ToString();    //"IN"; //as per passport
                        //    psgrequestdata.Passengers[i].PassengerTravelDocuments[0].IssuedDate = DateTime.Parse("0001-01-01T00:00:00");    //DateTime.Parse(dr["DateOfIssue"].ToString());

                        //    //------------------ as per passport detail 
                        //}



                        objUpdatePassengersRequestData.Passengers[i].PassengerInfos = new svc_booking.PassengerInfo[1];
                        objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0] = new svc_booking.PassengerInfo();
                        objUpdatePassengersRequestData.Passengers[i].PassengerInfo = new svc_booking.PassengerInfo();

                        if (dr["Title"].ToString().ToUpper().Trim().Equals("MRS") || dr["Title"].ToString().ToUpper().Trim().Equals("MS"))
                        {
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].Nationality = "IN";

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].Gender = svc_booking.Gender.Female;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].WeightCategory = svc_booking.WeightCategory.Female;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Gender = svc_booking.Gender.Female;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.WeightCategory = svc_booking.WeightCategory.Female;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Nationality = "IN";
                        }
                        else if (dr["Title"].ToString().ToUpper().Trim().Equals("DR"))
                        {
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].Nationality = "IN";

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].Gender = svc_booking.Gender.Unmapped;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].WeightCategory = svc_booking.WeightCategory.Unmapped;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Gender = svc_booking.Gender.Unmapped;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.WeightCategory = svc_booking.WeightCategory.Unmapped;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.WeightCategorySpecified = true;


                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Nationality = "IN";
                        }
                        else if (dr["Title"].ToString().ToUpper().Trim().Equals("MR"))
                        {
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].Nationality = "IN";
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].Gender = svc_booking.Gender.Male;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].WeightCategory = svc_booking.WeightCategory.Male;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfos[0].WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Gender = svc_booking.Gender.Male;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.WeightCategory = svc_booking.WeightCategory.Male;
                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Nationality = "IN";
                        }

                        objUpdatePassengersRequestData.Passengers[i].PassengerTypeInfos[0] = new svc_booking.PassengerTypeInfo();

                        objUpdatePassengersRequestData.Passengers[i].PassengerTypeInfos[0].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i].PassengerTypeInfos[0].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].PassengerTypeInfos[0].DOB = DateTime.Parse(AdtDob);
                        objUpdatePassengersRequestData.Passengers[i].PassengerTypeInfos[0].DOBSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i].PassengerTypeInfos[0].PaxType = "ADT";

                        if (dtInfants.Rows.Count > 0)
                        {
                            if (dtInfants.Rows.Count > i)
                            {
                                DataRow dr1 = dtInfants.Rows[i];
                                string sInfDob = Convert.ToDateTime(dr1["DOB"].ToString().Trim()).ToString("yyyy-MM-dd");

                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants = new svc_booking.PassengerInfant[1];
                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0] = new svc_booking.PassengerInfant();

                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Nationality = "IN";

                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].DOB = DateTime.Parse(sInfDob);
                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].DOBSpecified = true;

                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].State = svc_booking.MessageState.New;
                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].StateSpecified = true;

                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Names = new svc_booking.BookingName[1];
                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Names[0] = new svc_booking.BookingName();
                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Names[0].FirstName = dr1["First_Name"].ToString().Trim();
                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Names[0].MiddleName = "";
                                objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Names[0].LastName = dr1["Last_Name"].ToString().Trim();

                                if (dr1["Title"].ToString().Trim().Equals("MSTR") || dr1["Title"].ToString().Trim().Equals("MR"))
                                {
                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Names[0].Title = "MR";

                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Gender = svc_booking.Gender.Male;
                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfo.GenderSpecified = true;

                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Nationality = "IN";
                                }
                                else
                                {
                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfants[0].Names[0].Title = "MS";

                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Gender = svc_booking.Gender.Female;
                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfo.GenderSpecified = true;

                                    objUpdatePassengersRequestData.Passengers[i].PassengerInfo.Nationality = "IN";
                                }

                                //if (dr1["PPNumber"].ToString().Trim().Length > 0 && dr1["DateOfExpiry"].ToString().Trim().Length > 0)
                                //{
                                //    string InfDateOfExpiry = Convert.ToDateTime(dr1["DateOfExpiry"].ToString().Trim()).ToString("yyyy-MM-dd");
                                //    //------------------ Infant as per passport detail 

                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1] = new PassengerTravelDocument();
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].State = ServiceBookingSG.MessageState.New;
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Names = new BookingName[1];
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Names[0] = new BookingName();
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Names[0].FirstName = dr1["FName"].ToString();
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Names[0].MiddleName = dr1["MName"].ToString();
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Names[0].LastName = dr1["LName"].ToString();
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Names[0].Title = dr1["Title"].ToString();

                                //    if (dr1["Title"].ToString().ToUpper().IndexOf("MSTR") != -1)
                                //    {
                                //        psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Gender = Gender.Male;
                                //    }
                                //    else if (dr1["Title"].ToString().ToUpper().IndexOf("MISS") != -1 || dr1["Title"].ToString().ToUpper().IndexOf("MS") != -1)
                                //    {
                                //        psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Gender = Gender.Female;
                                //    }

                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].DocTypeCode = "P";
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].IssuedByCode = dr1["Nationality"].ToString();     //"IN"
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].DocSuffix = "I";
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].DocNumber = dr1["PPNumber"].ToString();    // "I1234567";//
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].DOB = DateTime.Parse(sInfDob);
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].Nationality = dr1["Nationality"].ToString();       //"IN"; // //as per passport
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].ExpirationDate = DateTime.Parse(InfDateOfExpiry);  //DateTime.Parse(dr["DateOfExpiry"].ToString());     
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].BirthCountry = string.Empty;//dr1["Nationality"].ToString();      //"IN"; //as per passport
                                //    psgrequestdata.Passengers[i].PassengerTravelDocuments[1].IssuedDate = DateTime.Parse("0001-01-01T00:00:00");     //DateTime.Parse(dr["DateOfIssue"].ToString());

                                //    //------------------ as per passport detail 
                                //}
                            }
                        }
                    }
                }

                DataTable dtChilds = new DataTable();
                drResults = dtPassenger.Select("PaxType='" + "CHD" + "'");

                if (drResults.Length > 0)
                {
                    dtChilds = drResults.CopyToDataTable();

                    for (i = 0; i < dtChilds.Rows.Count; i++)
                    {
                        DataRow dr = dtChilds.Rows[i];
                        string sChdDob = Convert.ToDateTime(dr["DOB"].ToString().Trim()).ToString("yyyy-MM-dd");

                        objUpdatePassengersRequestData.Passengers[i + Adt] = new svc_booking.Passenger();

                        objUpdatePassengersRequestData.Passengers[i + Adt].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i + Adt].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerPrograms = new svc_booking.PassengerProgram[1];
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerPrograms[0] = new svc_booking.PassengerProgram();

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerPrograms[0].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerPrograms[0].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerNumber = Int16.Parse((i + Adt).ToString());
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerNumberSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].Names = new svc_booking.BookingName[1];
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerTypeInfos = new svc_booking.PassengerTypeInfo[1];
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerPrograms[0] = new svc_booking.PassengerProgram();

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerPrograms[0].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerPrograms[0].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerNumber = Int16.Parse((i + Adt).ToString());
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerNumberSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].FamilyNumber = Int16.Parse((i + Adt).ToString());
                        objUpdatePassengersRequestData.Passengers[i + Adt].FamilyNumberSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerID = Int16.Parse((i + Adt).ToString());
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerIDSpecified = true;

                        //if (dr["PPNumber"].ToString().Trim().Length > 0 && dr["DateOfExpiry"].ToString().Trim().Length > 0)
                        //{
                        //    string ChdDateOfExpiry = Convert.ToDateTime(dr["DateOfExpiry"].ToString().Trim()).ToString("yyyy-MM-dd");
                        //    //------------------ as per passport detail 
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments = new PassengerTravelDocument[1];
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0] = new PassengerTravelDocument();
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].State = ServiceBookingSG.MessageState.New;
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Names = new BookingName[1];
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Names[0] = new BookingName();
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Names[0].FirstName = dr["FName"].ToString();
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Names[0].MiddleName = dr["MName"].ToString();
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Names[0].LastName = dr["LName"].ToString();
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Names[0].Title = dr["Title"].ToString();

                        //    if (dr["Title"].ToString().ToUpper().IndexOf("MSTR") != -1)
                        //    {
                        //        psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Gender = Gender.Male;
                        //    }
                        //    else if (dr["Title"].ToString().ToUpper().IndexOf("MISS") != -1 || dr["Title"].ToString().ToUpper().IndexOf("MS") != -1)
                        //    {
                        //        psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Gender = Gender.Female;
                        //    }

                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].DocTypeCode = "P";
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].IssuedByCode = dr["Nationality"].ToString();  //"IN";
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].DocNumber = dr["PPNumber"].ToString();    //"C1234567";//dr["PPNumber"].ToString();
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].DOB = DateTime.Parse(sChdDob);         // as per passport
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].Nationality = dr["Nationality"].ToString();    //as per passport
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].ExpirationDate = DateTime.Parse(ChdDateOfExpiry);//DateTime.Parse(dr["DateOfExpiry"].ToString());     
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].BirthCountry = string.Empty;// dr["Nationality"].ToString();  //"IN"; //as per passport
                        //    psgrequestdata.Passengers[i + iAdt].PassengerTravelDocuments[0].IssuedDate = DateTime.Parse("0001-01-01T00:00:00"); //DateTime.Parse(dr["DateOfIssue"].ToString());

                        //    //------------------ as per passport detail 
                        //}


                        objUpdatePassengersRequestData.Passengers[i + Adt].Names[0] = new svc_booking.BookingName();
                        objUpdatePassengersRequestData.Passengers[i + Adt].Names[0].FirstName = dr["First_Name"].ToString().Trim();
                        objUpdatePassengersRequestData.Passengers[i + Adt].Names[0].MiddleName = "";
                        objUpdatePassengersRequestData.Passengers[i + Adt].Names[0].LastName = dr["Last_Name"].ToString().Trim();

                        if (dr["Title"].ToString().ToUpper().Trim().Equals("MSTR") || dr["Title"].ToString().ToUpper().Trim().Equals("MR"))
                        {
                            objUpdatePassengersRequestData.Passengers[i + Adt].Names[0].Title = "MR";
                        }
                        else if (dr["Title"].ToString().ToUpper().Trim().Equals("MISS") || dr["Title"].ToString().ToUpper().Trim().Equals("MS"))
                        {
                            objUpdatePassengersRequestData.Passengers[i + Adt].Names[0].Title = "MS";
                        }


                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos = new svc_booking.PassengerInfo[1];
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0] = new svc_booking.PassengerInfo();
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo = new svc_booking.PassengerInfo();

                        if (dr["Title"].ToString().ToUpper().Trim().Equals("MSTR") || dr["Title"].ToString().ToUpper().Trim().Equals("MR"))
                        {
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].Nationality = "IN";

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].Gender = svc_booking.Gender.Male;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].WeightCategory = svc_booking.WeightCategory.Child;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.Gender = svc_booking.Gender.Male;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.WeightCategory = svc_booking.WeightCategory.Child;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.Nationality = "IN";
                        }
                        else if (dr["Title"].ToString().ToUpper().Trim().Equals("MISS") || dr["Title"].ToString().ToUpper().Trim().Equals("MS"))
                        {
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].Nationality = "IN";

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].Gender = svc_booking.Gender.Female;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].WeightCategory = svc_booking.WeightCategory.Child;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfos[0].WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.Gender = svc_booking.Gender.Female;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.GenderSpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.WeightCategory = svc_booking.WeightCategory.Child;
                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.WeightCategorySpecified = true;

                            objUpdatePassengersRequestData.Passengers[i + Adt].PassengerInfo.Nationality = "IN";
                        }

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerTypeInfos[0] = new svc_booking.PassengerTypeInfo();

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerTypeInfos[0].State = svc_booking.MessageState.New;
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerTypeInfos[0].StateSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerTypeInfos[0].DOB = DateTime.Parse(sChdDob);
                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerTypeInfos[0].DOBSpecified = true;

                        objUpdatePassengersRequestData.Passengers[i + Adt].PassengerTypeInfos[0].PaxType = "CHD";
                    }
                }

                objUpdatePassengersRequestData.WaiveNameChangeFee = false;
                objUpdatePassengersRequestData.WaiveNameChangeFeeSpecified = false;

                svc_booking.UpdatePassengersRequest objUpdatePassengersRequest = new svc_booking.UpdatePassengersRequest();
                objUpdatePassengersRequest.updatePassengersRequestData = objUpdatePassengersRequestData;
                objUpdatePassengersRequest.ContractVersion = ContractVersion;
                objUpdatePassengersRequest.Signature = Signature;

                GetApiRequest = GetCommonFunctions.Serialize(objUpdatePassengersRequest);

                svc_booking.IBookingManager ObjIBookingManager = new svc_booking.BookingManagerClient();
                svc_booking.UpdatePassengersResponse objUpdatePassengersResponse = ObjIBookingManager.UpdatePassengers(objUpdatePassengersRequest);
                if (objUpdatePassengersResponse.BookingUpdateResponseData != null && objUpdatePassengersResponse.BookingUpdateResponseData.Success != null)
                {
                    iTotalCost = objUpdatePassengersResponse.BookingUpdateResponseData.Success.PNRAmount.TotalCost;
                }
                GetApiResponse = GetCommonFunctions.Serialize(objUpdatePassengersResponse);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                //dbCommon.Logger.dbLogg(CompanyID, BookingRef, "GetUpdatePassenger", "air_spicejet", Supplierid, Searchid, ex.Message + "," + ex.StackTrace);
            }
           // dbCommon.Logger.WriteLogg(CompanyID, BookingRef, "GetUpdatePassenger-air_spicejet", "PNR", GetApiRequest + Environment.NewLine + GetApiResponse, Supplierid, Searchid);

            return iTotalCost;
        }
    }
}
