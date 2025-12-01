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

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetAvailabilityThread
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string Companyid;
        private string AgentName;
        private string Password;
        IRR_Layer _rr_Layer;
        //-----------------------------------------------------------------------------------------------
        public GetAvailabilityThread(string Searchid, string Companyid, string Supplierid, string Password)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
            this.AgentName = Supplierid;
            this.Password = Password;
            _rr_Layer = new rr_Layer();
        }
        public DataTable Thread(string JourneyType, string PriceType, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            string Trip = "R";
            GetAvailabilityThreadMethod objThreadMethod = new GetAvailabilityThreadMethod(Searchid, Companyid, AgentName, Password, PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
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
            else if (JourneyType.Equals("DRT") || JourneyType.Equals("IRT"))
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

            //-----------------------------------------------------------------------------------------------------------------------

            DataTable dtOutbound = objThreadMethod.dtOutbound;
            DataTable dtInbound = objThreadMethod.dtInbound;

            if (Sector.Equals("D"))
            {
                if (dtOutbound != null && dtOutbound.Rows.Count > 0 && dtInbound != null && dtInbound.Rows.Count > 0)
                {
                    dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                    dtInbound = GetCommonFunctions.RemoveZeroFares(dtInbound);
                    dtOutbound.Merge(dtInbound);
                    GetCommonFunctions.SetTrip(Trip, dtOutbound);
                    dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);

                    dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);
                }
                else if (dtOutbound != null && dtOutbound.Rows.Count > 0)
                {
                    dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                    GetCommonFunctions.SetTrip(Trip, dtOutbound);
                    dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);
                    dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);
                }
                else if (dtInbound != null && dtInbound.Rows.Count > 0)
                {
                    dtInbound = GetCommonFunctions.RemoveZeroFares(dtInbound);
                    GetCommonFunctions.SetTrip(Trip, dtInbound);
                    dtInbound = GetCommonFunctions.SetRowid(dtInbound);
                    dtInbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtInbound);
                }
            }
            else
            {
                if (dtOutbound != null && dtOutbound.Rows.Count > 0)
                {
                    dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                    GetCommonFunctions.SetTrip(Trip, dtOutbound);
                    dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);
                    dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);
                }
            }

            return dtOutbound;
        }
        public async Task<DataTable> ThreadAsync(string JourneyType, string PriceType, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            string Trip = "R";
            GetAvailabilityThreadMethod objThreadMethod = new GetAvailabilityThreadMethod(Searchid, Companyid, AgentName, Password, PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            if (JourneyType.Equals("OW"))
            {
                Trip = "O";

                try
                {

                    objThreadMethod.Done_O = await objThreadMethod.GetOutboundAsync();

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

                try
                {
                    objThreadMethod.Done_I  = await  objThreadMethod.GetInboundAsync();
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
            else if (JourneyType.Equals("DRT") || JourneyType.Equals("IRT"))
            {
                try
                {

                    objThreadMethod.Done_O= await objThreadMethod.GetRT_Async();

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

            //-----------------------------------------------------------------------------------------------------------------------

            DataTable dtOutbound = objThreadMethod.dtOutbound;
            DataTable dtInbound = objThreadMethod.dtInbound;

            if (Sector.Equals("D"))
            {
                if (dtOutbound != null && dtOutbound.Rows.Count > 0 && dtInbound != null && dtInbound.Rows.Count > 0)
                {
                    dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                    dtInbound = GetCommonFunctions.RemoveZeroFares(dtInbound);
                    dtOutbound.Merge(dtInbound);
                    GetCommonFunctions.SetTrip(Trip, dtOutbound);
                    dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);

                    
                    dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);
                }
                else if (dtOutbound != null && dtOutbound.Rows.Count > 0)
                {
                    dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                    GetCommonFunctions.SetTrip(Trip, dtOutbound);
                    dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);
                    dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);
                }
                else if (dtInbound != null && dtInbound.Rows.Count > 0)
                {
                    dtInbound = GetCommonFunctions.RemoveZeroFares(dtInbound);
                    GetCommonFunctions.SetTrip(Trip, dtInbound);
                    dtInbound = GetCommonFunctions.SetRowid(dtInbound);
                    dtInbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtInbound);
                }
            }
            else
            {
                if (dtOutbound != null && dtOutbound.Rows.Count > 0)
                {
                    dtOutbound = GetCommonFunctions.RemoveZeroFares(dtOutbound);
                    GetCommonFunctions.SetTrip(Trip, dtOutbound);
                    dtOutbound = GetCommonFunctions.SetRowid(dtOutbound);
                    dtOutbound = _rr_Layer.GetAvailabilityCal(true, true, Searchid, Companyid, dtOutbound);
                }
            }

            return dtOutbound;
        }
    }
}
