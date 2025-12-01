using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Infrastructure.AirCalculations;

namespace ZealTravel.Infrastructure.UAPI
{
    class GetAvailabilityThread
    {
        private string NetworkUserName;
        private string NetworkPassword;
        private string TargetBranch;
        private string Searchid;
        private string Companyid;
        private string AirRQ;
        private string Sector;
        private bool UAPISME;
        IRR_Layer _rr_Layer;
        public GetAvailabilityThread(string NetworkUserName, string NetworkPassword, string TargetBranch, string Searchid, string Companyid, string AirRQ, string Sector, bool uapiSME = false)
        {
            this.NetworkUserName = NetworkUserName;
            this.NetworkPassword = NetworkPassword;
            this.TargetBranch = TargetBranch;
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.AirRQ = AirRQ;
            this.Sector = Sector;
            this.UAPISME = uapiSME;
            _rr_Layer =new rr_Layer();

        }
        public string Thread(string JourneyType)
        {
            GetAvailabilityThreadMethod Call = new GetAvailabilityThreadMethod(NetworkUserName, NetworkPassword, TargetBranch, Searchid, Companyid, AirRQ, Sector, UAPISME);

            try
            {
                if (JourneyType.Equals("OW") || JourneyType.Equals("MC") || JourneyType.Equals("RT"))
                {
                    Thread Thread_Call;
                    Thread Thread_nxtCall;

                    try
                    {
                        if (JourneyType.Equals("OW"))
                        {
                            Thread_Call = new Thread(new ThreadStart(Call.GET_FLT));
                        }
                        else if (JourneyType.Equals("MC"))
                        {
                            Thread_Call = new Thread(new ThreadStart(Call.GetFlightMultiCity));
                        }
                        else
                        {
                            Thread_Call = new Thread(new ThreadStart(Call.GetFlightRT));
                        }

                        Thread_Call.Start();
                        if (Thread_Call.IsAlive.Equals(true))
                        {
                            Thread_Call.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            if (JourneyType.Equals("OW"))
                            {

                                Thread_nxtCall = new Thread(new ThreadStart(Call.GET_FLT));
                            }
                            else if (JourneyType.Equals("MC"))
                            {
                                Thread_nxtCall = new Thread(new ThreadStart(Call.GetFlightMultiCity));
                            }
                            else
                            {
                                Thread_nxtCall = new Thread(new ThreadStart(Call.GetFlightRT));
                            }

                            Thread_nxtCall.Start();
                            if (Thread_nxtCall.IsAlive.Equals(true))
                            {
                                Thread_nxtCall.Priority = ThreadPriority.Highest;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Call.Done = "D";
                    }

                    while (Call.Done != "D")
                    {

                    }
                }
                else if (JourneyType.Equals("RW"))
                {
                    Thread Thread_Call;
                    Thread Thread_nxtCall;
                    Thread Thread_Inb;
                    Thread Thread_nxtInb;

                    try
                    {
                        Thread_Call = new Thread(new ThreadStart(Call.GetOutbound));
                        Thread_Call.Start();
                        if (Thread_Call.IsAlive.Equals(true))
                        {
                            Thread_Call.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread_nxtCall = new Thread(new ThreadStart(Call.GetOutbound));
                            Thread_nxtCall.Start();
                            if (Thread_nxtCall.IsAlive.Equals(true))
                            {
                                Thread_nxtCall.Priority = ThreadPriority.Highest;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Call.Done_O = "D";
                    }

                    try
                    {
                        Thread_Inb = new Thread(new ThreadStart(Call.GetInbound));
                        Thread_Inb.Start();
                        if (Thread_Inb.IsAlive.Equals(true))
                        {
                            Thread_Inb.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread_nxtInb = new Thread(new ThreadStart(Call.GetInbound));
                            Thread_nxtInb.Start();
                            if (Thread_nxtInb.IsAlive.Equals(true))
                            {
                                Thread_nxtInb.Priority = ThreadPriority.Highest;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Call.Done_I = "D";
                    }

                    while (Call.Done_O != "D")
                    {

                    }
                    while (Call.Done_I != "D")
                    {

                    }
                }

                //return Call.GetResponse();

                DataTable dtBound = Call.GetResponse();
                string Sector = Call.Sector;
                //=====================================================================================================================

                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    //air_db_cal.rr_Layer objdbCal = new air_db_cal.rr_Layer();
                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

                    if (JourneyType.Equals("RT") || JourneyType.Equals("RW"))
                    {
                        DBCommon.GetRequestData.AddTrip("R", dtBound);
                    }
                    else if (JourneyType.Equals("MC"))
                    {
                        DBCommon.GetRequestData.AddTrip("M", dtBound);
                    }
                    else if (JourneyType.Equals("OW"))
                    {
                        DBCommon.GetRequestData.AddTrip("O", dtBound);
                    }

                    DBCommon.GetRequestData.AddSector(Sector, dtBound);
                    dtBound = DBCommon.GetRequestData.Add_RowID(dtBound);

                    //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

                    dtBound.TableName = "AvailabilityInfo";
                    DataSet dsBound = new DataSet();
                    dsBound.Tables.Add(dtBound.Copy());
                    dsBound.DataSetName = "root";

                    return DBCommon.CommonFunction.DataSetToString(dsBound);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "Thread", "GetAvailabilityThread", JourneyType, Searchid, ex.Message);
            }
            return string.Empty;
        }

        public async Task<string> ThreadAsync(string JourneyType)
        {
            GetAvailabilityThreadMethod Call = new GetAvailabilityThreadMethod(NetworkUserName, NetworkPassword, TargetBranch, Searchid, Companyid, AirRQ, Sector, UAPISME);
            //DataTable dtBound = new DataTable();
            try
            {
                if (JourneyType.Equals("OW") || JourneyType.Equals("MC") || JourneyType.Equals("RT"))
                {
                    //Thread Thread_Call;
                    //Thread Thread_nxtCall;

                    try
                    {
                        if (JourneyType.Equals("OW"))
                        {
                            //dtBound = new DataTable();
                            Call.Done=await Call.GET_FLT_Async();
                            //Func<Task> asyncDelegate = async () => { Call.GET_FLT_Async(); };
                            //await asyncDelegate();

                            //Thread_Call = new Thread(new ThreadStart(Call.GET_FLT_Async));
                        }
                        else if (JourneyType.Equals("MC"))
                        {
                            Call.GetFlightMultiCity();
                            //Thread_Call = new Thread(new ThreadStart(Call.GetFlightMultiCity));
                        }
                        else
                        {
                            //dtBound = new DataTable();
                            Call.Done=await Call.GetFlightRT_Async();
                            //Thread_Call = new Thread(new ThreadStart(Call.GetFlightRT));
                        }

                        /*Thread_Call.Start();
                        if (Thread_Call.IsAlive.Equals(true))
                        {
                            Thread_Call.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            if (JourneyType.Equals("OW"))
                            {

                                Call.GET_FLT_Async();
                                //Thread_nxtCall = new Thread(new ThreadStart(Call.GET_FLT_Async));
                            }
                            else if (JourneyType.Equals("MC"))
                            {
                                Call.GetFlightMultiCity();
                                //Thread_nxtCall = new Thread(new ThreadStart(Call.GetFlightMultiCity));
                            }
                            else
                            {

                                Call.GetFlightRT();
                                //Thread_nxtCall = new Thread(new ThreadStart(Call.GetFlightRT));
                            }

                            Thread_nxtCall.Start();
                            if (Thread_nxtCall.IsAlive.Equals(true))
                            {
                                Thread_nxtCall.Priority = ThreadPriority.Highest;
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        Call.Done = "D";
                    }

                    /*while (Call.Done != "D")
                    {

                    }*/
                }
                else if (JourneyType.Equals("RW"))
                {
                    //Thread Thread_Call;
                    //Thread Thread_nxtCall;
                    //Thread Thread_Inb;
                    //Thread Thread_nxtInb;

                    try
                    {
                        //dtBound = new DataTable();
                        Call.Done_O=await Call.GetOutboundAsync();

                        /*Thread_Call = new Thread(new ThreadStart(Call.GetOutbound));
                        Thread_Call.Start();
                        if (Thread_Call.IsAlive.Equals(true))
                        {
                            Thread_Call.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread_nxtCall = new Thread(new ThreadStart(Call.GetOutbound));
                            Thread_nxtCall.Start();
                            if (Thread_nxtCall.IsAlive.Equals(true))
                            {
                                Thread_nxtCall.Priority = ThreadPriority.Highest;
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        Call.Done_O = "D";
                    }

                    try
                    {
                        //dtBound = new DataTable();
                        Call.Done_I=await Call.GetInboundAsync();
                        /*Thread_Inb = new Thread(new ThreadStart(Call.GetInbound));
                        Thread_Inb.Start();
                        if (Thread_Inb.IsAlive.Equals(true))
                        {
                            Thread_Inb.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread_nxtInb = new Thread(new ThreadStart(Call.GetInbound));
                            Thread_nxtInb.Start();
                            if (Thread_nxtInb.IsAlive.Equals(true))
                            {
                                Thread_nxtInb.Priority = ThreadPriority.Highest;
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        Call.Done_I = "D";
                    }

                    /*while (Call.Done_O != "D")
                    {

                    }
                    while (Call.Done_I != "D")
                    {

                    }*/
                }

                //return Call.GetResponse();

                DataTable dtBound = Call.GetResponse();
                string Sector = Call.Sector;
                //=====================================================================================================================

                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    //air_db_cal.rr_Layer objdbCal = new air_db_cal.rr_Layer();
                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

                    if (JourneyType.Equals("RT") || JourneyType.Equals("RW"))
                    {
                        DBCommon.GetRequestData.AddTrip("R", dtBound);
                    }
                    else if (JourneyType.Equals("MC"))
                    {
                        DBCommon.GetRequestData.AddTrip("M", dtBound);
                    }
                    else if (JourneyType.Equals("OW"))
                    {
                        DBCommon.GetRequestData.AddTrip("O", dtBound);
                    }

                    DBCommon.GetRequestData.AddSector(Sector, dtBound);
                    dtBound = DBCommon.GetRequestData.Add_RowID(dtBound);

                    //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                    dtBound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtBound);

                    dtBound.TableName = "AvailabilityInfo";
                    DataSet dsBound = new DataSet();
                    dsBound.Tables.Add(dtBound.Copy());
                    dsBound.DataSetName = "root";

                    return DBCommon.CommonFunction.DataSetToString(dsBound);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "Thread", "GetAvailabilityThread", JourneyType, Searchid, ex.Message);
            }
            return string.Empty;
        }
    }
}
