using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetAvailabilityThreadMethod
    {
        public string errorMessage;
        public DataTable dtOutbound;
        public DataTable dtInbound;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string Companyid;
        private string Supplierid;
        private string Password;
        private string Sector;
        //-----------------------------------------------------------------------------------------------
        private string PriceType;
        private string Origin;
        private string Destination;
        private string BeginDate;
        private string EndDate;
        private int Adt;
        private int Chd;
        private int Inf;
        //-----------------------------------------------------------------------------------------------
        public string Done_O;
        public string Done_I;
        public GetAvailabilityThreadMethod(string Searchid, string Companyid, string Supplierid, string Password, string PriceType, string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;

            this.Supplierid = Supplierid;
            this.Password = Password;
            this.Sector = Sector;

            this.PriceType = PriceType;
            this.Origin = Origin;
            this.Destination = Destination;
            this.BeginDate = BeginDate;
            this.EndDate = EndDate;
            this.Adt = Adt;
            this.Chd = Chd;
            this.Inf = Inf;
        }
        public void GetOutbound()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Searchid, Companyid, Supplierid, Password);
            obj.GetOneWay(PriceType, Origin, Destination, BeginDate, Adt, Chd, Inf, "O", Sector);
            dtOutbound = obj.dtOutbound;
            this.Done_O = "D";
        }
        public async Task<string> GetOutboundAsync()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Searchid, Companyid, Supplierid, Password);
            dtOutbound= await obj.GetOneWayAsync(PriceType, Origin, Destination, BeginDate, Adt, Chd, Inf, "O", Sector);
            //dtOutbound = obj.dtOutbound;
            this.Done_O = "D";
            return this.Done_O;

        }


        public void GetInbound()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Searchid, Companyid, Supplierid, Password);
            obj.GetOneWay(PriceType, Destination, Origin, EndDate, Adt, Chd, Inf, "I", Sector);
            dtInbound = obj.dtOutbound;
            this.Done_I = "D";
        }
        public async Task<string> GetInboundAsync()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Searchid, Companyid, Supplierid, Password);
            dtInbound=await obj.GetOneWayAsync(PriceType, Destination, Origin, EndDate, Adt, Chd, Inf, "I", Sector);
            //dtInbound = obj.dtOutbound;
            this.Done_I = "D";
            return this.Done_I;
        }

        public void GetRT()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Searchid, Companyid, Supplierid, Password);
            obj.GetRT(PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            dtOutbound = obj.dtOutbound;
            dtInbound = obj.dtInbound;
            this.Done_O = "D";
        }
        public async Task<string> GetRT_Async()
        {
            GetApiAvailabilityFlight obj = new GetApiAvailabilityFlight(Searchid, Companyid, Supplierid, Password);
            List<DataTable> _l = new List<DataTable>();
            _l =await obj.GetRT_Async(PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);
            dtOutbound = _l[0];
            dtInbound = _l[1];
            //dtOutbound = obj.dtOutbound;
            //dtInbound = obj.dtInbound;
            this.Done_O = "D";
            return this.Done_O;
        }

    }
}
