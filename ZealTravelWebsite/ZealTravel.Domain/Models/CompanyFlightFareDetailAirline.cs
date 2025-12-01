using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class CompanyFlightFareDetailAirline
    {
        private string sSupplierID;
        private string sSector;
        private string sTrip;
        private string sOrigin;
        private string sDestination;
        private string sCarrierCode1;
        private string sDepartureDate1;
        private string sCarrierCode2;
        private string sDepartureDate2;
        private Int32 iAdt;
        private Int32 iChd;
        private Int32 iInf;
        private Decimal dTotalTax;
        private Decimal dTotalBasic;
        private Decimal dTotalYq;
        private Decimal dTotalFare;
        private Decimal dTotalServiceTax;
        private Decimal dTotalMarkup;
        private Decimal dTotalBasic_deal;
        private Decimal dTotalYQ_deal;
        private Decimal dTotalCB_deal;
        private Decimal dTotalPromo_deal;
        private Decimal dTotalServiceFee;
        private Decimal dTotalCommission;
        private Decimal dTotalTds;

        private Decimal dTotalCfee;


        private Decimal dTotalMarkup1;
        private Decimal dTotalBasic_deal1;
        private Decimal dTotalYQ_deal1;
        private Decimal dTotalCB_deal1;
        private Decimal dTotalPromo_deal1;
        private Decimal dTotalCommission1;
        private Decimal dTotalTds1;
        private Decimal dTotalFare1;

        private Decimal dTotalMeal;
        private Decimal dTotalBaggage;

        private Decimal dTotalImport;

        private string sPriceType_D;
        private string sPriceType_A;

        private Int32 iAG_Markup_D;
        private Int32 iAG_Markup_A;

        public Int32 AG_Markup_D
        {
            get
            {
                return iAG_Markup_D;
            }
            set
            {
                iAG_Markup_D = value;
            }
        }
        public Int32 AG_Markup_A
        {
            get
            {
                return iAG_Markup_A;
            }
            set
            {
                iAG_Markup_A = value;
            }
        }


        public string PriceType_D
        {
            get
            {
                return sPriceType_D;
            }
            set
            {
                sPriceType_D = value;
            }
        }
        public string PriceType_A
        {
            get
            {
                return sPriceType_A;
            }
            set
            {
                sPriceType_A = value;
            }
        }

        public string SupplierID
        {
            get
            {
                return sSupplierID;
            }
            set
            {
                sSupplierID = value;
            }
        }
        public string Sector
        {
            get
            {
                return sSector;
            }
            set
            {
                sSector = value;
            }
        }
        public string Trip
        {
            get
            {
                return sTrip;
            }
            set
            {
                sTrip = value;
            }
        }
        public string Origin
        {
            get
            {
                return sOrigin;
            }
            set
            {
                sOrigin = value;
            }
        }
        public string Destination
        {
            get
            {
                return sDestination;
            }
            set
            {
                sDestination = value;
            }
        }

        public Int32 Adt
        {
            get
            {
                return iAdt;
            }
            set
            {
                iAdt = value;
            }
        }
        public Int32 Chd
        {
            get
            {
                return iChd;
            }
            set
            {
                iChd = value;
            }
        }
        public Int32 Inf
        {
            get
            {
                return iInf;
            }
            set
            {
                iInf = value;
            }
        }

        public string CarrierCode_D
        {
            get
            {
                return sCarrierCode1;
            }
            set
            {
                sCarrierCode1 = value;
            }
        }
        public string DepartureDate_D
        {
            get
            {
                return sDepartureDate1;
            }
            set
            {
                sDepartureDate1 = value;
            }
        }
        public string CarrierCode_A
        {
            get
            {
                return sCarrierCode2;
            }
            set
            {
                sCarrierCode2 = value;
            }
        }
        public string DepartureDate_A
        {
            get
            {
                return sDepartureDate2;
            }
            set
            {
                sDepartureDate2 = value;
            }
        }

        public Decimal TotalTax
        {
            get
            {
                return dTotalTax;
            }
            set
            {
                dTotalTax = value;
            }
        }
        public Decimal TotalBasic
        {
            get
            {
                return dTotalBasic;
            }
            set
            {
                dTotalBasic = value;
            }
        }
        public Decimal TotalYq
        {
            get
            {
                return dTotalYq;
            }
            set
            {
                dTotalYq = value;
            }
        }
        public Decimal TotalFare
        {
            get
            {
                return dTotalFare;
            }
            set
            {
                dTotalFare = value;
            }
        }
        public Decimal TotalServiceTax
        {
            get
            {
                return dTotalServiceTax;
            }
            set
            {
                dTotalServiceTax = value;
            }
        }
        public Decimal TotalMarkup
        {
            get
            {
                return dTotalMarkup;
            }
            set
            {
                dTotalMarkup = value;
            }
        }

        public Decimal TotalServiceFee
        {
            get
            {
                return dTotalServiceFee;
            }
            set
            {
                dTotalServiceFee = value;
            }
        }
        public Decimal TotalCommission
        {
            get
            {
                return dTotalCommission;
            }
            set
            {
                dTotalCommission = value;
            }
        }

        public Decimal TotalTds
        {
            get
            {
                return dTotalTds;
            }
            set
            {
                dTotalTds = value;
            }
        }

        public Decimal TotalMeal
        {
            get
            {
                return dTotalMeal;
            }
            set
            {
                dTotalMeal = value;
            }
        }
        public Decimal TotalBaggage
        {
            get
            {
                return dTotalBaggage;
            }
            set
            {
                dTotalBaggage = value;
            }
        }

        public Decimal TotalQueue
        {
            get
            {
                return dTotalImport;
            }
            set
            {
                dTotalImport = value;
            }
        }

        public Decimal TotalBasic_deal
        {
            get
            {
                return dTotalBasic_deal;
            }
            set
            {
                dTotalBasic_deal = value;
            }
        }
        public Decimal TotalYQ_deal
        {
            get
            {
                return dTotalYQ_deal;
            }
            set
            {
                dTotalYQ_deal = value;
            }
        }
        public Decimal TotalCB_deal
        {
            get
            {
                return dTotalCB_deal;
            }
            set
            {
                dTotalCB_deal = value;
            }
        }
        public Decimal TotalPromo_deal
        {
            get
            {
                return dTotalPromo_deal;
            }
            set
            {
                dTotalPromo_deal = value;
            }
        }

        public Decimal TotalMarkup_SA
        {
            get
            {
                return dTotalMarkup1;
            }
            set
            {
                dTotalMarkup1 = value;
            }
        }
        public Decimal TotalCommission_SA
        {
            get
            {
                return dTotalCommission1;
            }
            set
            {
                dTotalCommission1 = value;
            }
        }
        public Decimal TotalTds_SA
        {
            get
            {
                return dTotalTds1;
            }
            set
            {
                dTotalTds1 = value;
            }
        }
        public Decimal TotalBasic_deal_SA
        {
            get
            {
                return dTotalBasic_deal1;
            }
            set
            {
                dTotalBasic_deal1 = value;
            }
        }
        public Decimal TotalYQ_deal_SA
        {
            get
            {
                return dTotalYQ_deal1;
            }
            set
            {
                dTotalYQ_deal1 = value;
            }
        }
        public Decimal TotalCB_deal_SA
        {
            get
            {
                return dTotalCB_deal1;
            }
            set
            {
                dTotalCB_deal1 = value;
            }
        }
        public Decimal TotalPromo_deal_SA
        {
            get
            {
                return dTotalPromo_deal1;
            }
            set
            {
                dTotalPromo_deal1 = value;
            }
        }

        public Decimal TotalFare_SA
        {
            get
            {
                return dTotalFare1;
            }
            set
            {
                dTotalFare1 = value;
            }
        }

        public Decimal Totalcfee
        {
            get
            {
                return dTotalCfee;
            }
            set
            {
                dTotalCfee = value;
            }
        }
    }
}

