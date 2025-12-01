using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZealTravel.Domain.Interfaces.AirCalculation;
using ZealTravel.Infrastructure.AirCalculations;

namespace ZealTravel.Infrastructure.TBO
{
    public class GetAvailabilityThread
    {
        public string errorMessage;
        public DataTable dtOutbound;
        //-----------------------------------------------------------------------------------------------
        private string Supplierid;
        private string Password;
        private string Searchid;
        private string Companyid;
        private string EndUserIp;
        private string Request;
        IRR_Layer _rr_Layer;
        //-----------------------------------------------------------------------------------------------
        public GetAvailabilityThread(string Supplierid, string Password, string Searchid, string Companyid, string Request, string EndUserIp)
        {
            this.Supplierid = Supplierid;
            this.Password = Password;
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.EndUserIp = EndUserIp;
            this.Request = Request;
            _rr_Layer=new rr_Layer();
        }
        public string Thread(string JourneyType)
        {
            string Trip = "R";
            GetAvailabilityThreadMethod objThreadMethod = new GetAvailabilityThreadMethod(JourneyType, Supplierid, Password, Searchid, Companyid, Request, EndUserIp);

            if (JourneyType.Equals("OW"))
            {
                Trip = "O";

                try
                {
                    //objThreadMethod.GetOutbound();

                    Thread ThreadOW1 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                    ThreadOW1.Start();
                    if (ThreadOW1.IsAlive.Equals(true))
                    {
                        ThreadOW1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadOW2 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                        ThreadOW2.Start();
                        if (ThreadOW2.IsAlive.Equals(true))
                        {
                            ThreadOW2.Priority = ThreadPriority.Highest;
                        }
                    }
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_O = "D";
                }

                while (objThreadMethod.Done_O != "D")
                {

                }
            }
            else if (JourneyType.Equals("RW"))
            {
                try
                {
                    Thread ThreadOW1 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                    ThreadOW1.Start();
                    if (ThreadOW1.IsAlive.Equals(true))
                    {
                        ThreadOW1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadOW2 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                        ThreadOW2.Start();
                        if (ThreadOW2.IsAlive.Equals(true))
                        {
                            ThreadOW2.Priority = ThreadPriority.Highest;
                        }
                    }
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_O = "D";
                }

                try
                {
                    Thread ThreadRW1 = new Thread(new ThreadStart(objThreadMethod.GetInbound));
                    ThreadRW1.Start();
                    if (ThreadRW1.IsAlive.Equals(true))
                    {
                        ThreadRW1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadRW2 = new Thread(new ThreadStart(objThreadMethod.GetInbound));
                        ThreadRW2.Start();
                        if (ThreadRW2.IsAlive.Equals(true))
                        {
                            ThreadRW2.Priority = ThreadPriority.Highest;
                        }
                    }
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_I = "D";
                }

                while (objThreadMethod.Done_O != "D")
                {

                }
                while (objThreadMethod.Done_I != "D")
                {

                }
            }
            else if (JourneyType.Equals("RT") || JourneyType.Equals("RTLCC"))
            {
                if (objThreadMethod.Sector.Equals("D"))
                {
                    try
                    {
                        Thread ThreadRT1 = new Thread(new ThreadStart(objThreadMethod.GetRTLCC));
                        ThreadRT1.Start();
                        if (ThreadRT1.IsAlive.Equals(true))
                        {
                            ThreadRT1.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread ThreadRT2 = new Thread(new ThreadStart(objThreadMethod.GetRTLCC));
                            ThreadRT2.Start();
                            if (ThreadRT2.IsAlive.Equals(true))
                            {
                                ThreadRT2.Priority = ThreadPriority.Highest;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        objThreadMethod.Done_O = "D";
                    }

                    while (objThreadMethod.Done_O != "D")
                    {

                    }
                }
                else
                {
                    try
                    {
                        Thread ThreadRT1 = new Thread(new ThreadStart(objThreadMethod.GetRT));
                        ThreadRT1.Start();
                        if (ThreadRT1.IsAlive.Equals(true))
                        {
                            ThreadRT1.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread ThreadRT2 = new Thread(new ThreadStart(objThreadMethod.GetRT));
                            ThreadRT2.Start();
                            if (ThreadRT2.IsAlive.Equals(true))
                            {
                                ThreadRT2.Priority = ThreadPriority.Highest;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        objThreadMethod.Done_O = "D";
                    }

                    while (objThreadMethod.Done_O != "D")
                    {

                    }
                }
            }
            else if (JourneyType.Equals("MC"))
            {
                Trip = "M";

                try
                {
                    Thread ThreadRT1 = new Thread(new ThreadStart(objThreadMethod.GetMC));
                    ThreadRT1.Start();
                    if (ThreadRT1.IsAlive.Equals(true))
                    {
                        ThreadRT1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadRT2 = new Thread(new ThreadStart(objThreadMethod.GetMC));
                        ThreadRT2.Start();
                        if (ThreadRT2.IsAlive.Equals(true))
                        {
                            ThreadRT2.Priority = ThreadPriority.Highest;
                        }
                    }
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_O = "D";
                }

                while (objThreadMethod.Done_O != "D")
                {

                }
            }

            //-----------------------------------------------------------------------------------------------------------------------

            dtOutbound = objThreadMethod.dtBound;
            errorMessage = objThreadMethod.errorMessage;

            //-----------------------------------------------------------------------------------------------------------------------

            if (dtOutbound != null && dtOutbound.Rows.Count > 0)
            {
                dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                GetCommonFunctions.SetTrip(Trip, dtOutbound);
                dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);

                //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);

                objThreadMethod.dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtOutbound.Copy());
                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return "";
            }
        }
        public async Task<string> ThreadAsync(string JourneyType)
        {
            string Trip = "R";
            GetAvailabilityThreadMethod objThreadMethod = new GetAvailabilityThreadMethod(JourneyType, Supplierid, Password, Searchid, Companyid, Request, EndUserIp);

            if (JourneyType.Equals("OW"))
            {
                Trip = "O";

                try
                {
                    objThreadMethod.Done_O =await objThreadMethod.GetOutboundAsync();

                    /*Thread ThreadOW1 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                    ThreadOW1.Start();
                    if (ThreadOW1.IsAlive.Equals(true))
                    {
                        ThreadOW1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadOW2 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                        ThreadOW2.Start();
                        if (ThreadOW2.IsAlive.Equals(true))
                        {
                            ThreadOW2.Priority = ThreadPriority.Highest;
                        }
                    }*/
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_O = "D";
                }

                /*while (objThreadMethod.Done_O != "D")
                {

                }*/
            }
            else if (JourneyType.Equals("RW"))
            {
                try
                {
                    objThreadMethod.Done_O=await objThreadMethod.GetOutboundAsync();
                    /*Thread ThreadOW1 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                    ThreadOW1.Start();
                    if (ThreadOW1.IsAlive.Equals(true))
                    {
                        ThreadOW1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadOW2 = new Thread(new ThreadStart(objThreadMethod.GetOutbound));
                        ThreadOW2.Start();
                        if (ThreadOW2.IsAlive.Equals(true))
                        {
                            ThreadOW2.Priority = ThreadPriority.Highest;
                        }
                    }*/
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_O = "D";
                }

                try
                {
                    objThreadMethod.Done_I=await objThreadMethod.GetInboundAsync();
                    /*Thread ThreadRW1 = new Thread(new ThreadStart(objThreadMethod.GetInbound));
                    ThreadRW1.Start();
                    if (ThreadRW1.IsAlive.Equals(true))
                    {
                        ThreadRW1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadRW2 = new Thread(new ThreadStart(objThreadMethod.GetInbound));
                        ThreadRW2.Start();
                        if (ThreadRW2.IsAlive.Equals(true))
                        {
                            ThreadRW2.Priority = ThreadPriority.Highest;
                        }
                    }*/
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_I = "D";
                }

                /*while (objThreadMethod.Done_O != "D")
                {

                }
                while (objThreadMethod.Done_I != "D")
                {

                }*/
            }
            else if (JourneyType.Equals("RT") || JourneyType.Equals("RTLCC"))
            {
                if (objThreadMethod.Sector.Equals("D"))
                {
                    try
                    {
                        objThreadMethod.Done_O =await  objThreadMethod.GetRTLCC_Async();
                        /*Thread ThreadRT1 = new Thread(new ThreadStart(objThreadMethod.GetRTLCC));
                        ThreadRT1.Start();
                        if (ThreadRT1.IsAlive.Equals(true))
                        {
                            ThreadRT1.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread ThreadRT2 = new Thread(new ThreadStart(objThreadMethod.GetRTLCC));
                            ThreadRT2.Start();
                            if (ThreadRT2.IsAlive.Equals(true))
                            {
                                ThreadRT2.Priority = ThreadPriority.Highest;
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        objThreadMethod.Done_O = "D";
                    }

                    /*while (objThreadMethod.Done_O != "D")
                    {

                    }*/
                }
                else
                {
                    try
                    {
                        objThreadMethod.Done_O=await  objThreadMethod.GetRT_Async();
                        /*Thread ThreadRT1 = new Thread(new ThreadStart(objThreadMethod.GetRT));
                        ThreadRT1.Start();
                        if (ThreadRT1.IsAlive.Equals(true))
                        {
                            ThreadRT1.Priority = ThreadPriority.Highest;
                        }
                        else
                        {
                            Thread ThreadRT2 = new Thread(new ThreadStart(objThreadMethod.GetRT));
                            ThreadRT2.Start();
                            if (ThreadRT2.IsAlive.Equals(true))
                            {
                                ThreadRT2.Priority = ThreadPriority.Highest;
                            }
                        }*/
                    }
                    catch (Exception ex)
                    {
                        objThreadMethod.Done_O = "D";
                    }

                    /*while (objThreadMethod.Done_O != "D")
                    {

                    }*/
                }
            }
            else if (JourneyType.Equals("MC"))
            {
                Trip = "M";

                try
                {
                    objThreadMethod.Done_O=await objThreadMethod.GetMC_Async();

                    /*Thread ThreadRT1 = new Thread(new ThreadStart(objThreadMethod.GetMC));
                    ThreadRT1.Start();
                    if (ThreadRT1.IsAlive.Equals(true))
                    {
                        ThreadRT1.Priority = ThreadPriority.Highest;
                    }
                    else
                    {
                        Thread ThreadRT2 = new Thread(new ThreadStart(objThreadMethod.GetMC));
                        ThreadRT2.Start();
                        if (ThreadRT2.IsAlive.Equals(true))
                        {
                            ThreadRT2.Priority = ThreadPriority.Highest;
                        }
                    }*/
                }
                catch (Exception ex)
                {
                    objThreadMethod.Done_O = "D";
                }

                /*while (objThreadMethod.Done_O != "D")
                {

                }*/
            }

            //-----------------------------------------------------------------------------------------------------------------------

            dtOutbound = objThreadMethod.dtBound;
            errorMessage = objThreadMethod.errorMessage;

            //-----------------------------------------------------------------------------------------------------------------------

            if (dtOutbound != null && dtOutbound.Rows.Count > 0)
            {
                dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                GetCommonFunctions.SetTrip(Trip, dtOutbound);
                dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);

                //air_db_cal.rr_Layer objC = new air_db_cal.rr_Layer();
                dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);

                objThreadMethod.dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtOutbound.Copy());
                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return "";
            }
        }
    }
}
