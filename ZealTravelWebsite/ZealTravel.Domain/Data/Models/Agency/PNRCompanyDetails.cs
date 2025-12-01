using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models.Agency
{
    public class PNRCompanyDetails
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public int AccountID { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public string UserType { get; set; }
        public string Pan_Name { get; set; }
        public string Pan_No { get; set; }
        public int StaffType { get; set; }
        public DateTime EventTime { get; set; }
        public bool Access_Status { get; set; }
        public bool Active_Status { get; set; }
        public string Company_PhoneNo { get; set; }
        public string Company_Mobile { get; set; }
        public string Company_Email { get; set; }
        public string GST { get; set; }
        public string Host { get; set; }
        public string AdminID { get; set; }
        public int Admin_AccountID { get; set; }
        public string Admin_CompanyName { get; set; }
        public string Admin_Contact { get; set; }
    }
}
