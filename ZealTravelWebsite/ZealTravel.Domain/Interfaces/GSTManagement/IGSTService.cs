using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Domain.Interfaces.GSTManagement
{
    public interface IGSTService
    {
        Task<bool> SETGSTdetail(string CompanyID, string PassengerResponse, string GSTInfo);
        Task<bool> SetGSTAirline(string CompanyID, int BookingRef, DataTable dtPassenger);
        Task<CompanyRegisterGst> GetGSTdetail(string CompanyID);
    }
}
