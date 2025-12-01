using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Models;

namespace ZealTravel.Domain.Interfaces.Akasha
{
    public interface ITI_DBData
    {
        //public DataTable GetBlackOutGDSAirline();
        //public string DateChangeForSearch(string Date);
        //public int TimeToMinustes(string Time);
        public List<AkashaAirLineModel> GetAirlineData(string DepartureStation, string ArrivalStation);
        //public  string GetAvailabilityRequest(ArrayList CarrierList, string DepartureStation, string ArrivalStation, string Cabin, string StartDate, string EndDate, string Adult, string Child, string Infant);
    }
    public interface ITI_Search
    {
        public string GetFlightSearchRequest(string AirRQ, string SearchType, string SearchID, string SupplierCode, ref string Sector);
    }
}
