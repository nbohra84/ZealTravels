using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AgencyManagement.Queries
{
    public class CompanyAfterLoginDetails
    {
        public decimal? Available_Balance { get; set; }
        public decimal? Temporary_Balance { get; set; }
        public DateTime? Temporary_Balance_Time { get; set; }
        public string CompanyID { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string AdminID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool DistributorAgent { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNo { get; set; }
        public string UserType { get; set; }
        public string IP { get; set; }
        public DateTime EventTime { get; set; }
        public string Pan_Name { get; set; }
        public string Pan_No { get; set; }
        public string GST { get; set; }
        public string SalesPerson { get; set; }
        public string SalesContactNumber { get; set; }
        public string SalesEmail { get; set; }
        public string Staff_Email { get; set; }
        public string Staff_Mobile { get; set; }
        public string Staff_AccountID { get; set; }
        public bool WhitelabelAgent { get; set; }
        public bool CorporateAgent { get; set; }
        public int AccountID { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Company_Email { get; set; }
        public string Company_Mobile { get; set; }
        public string Company_PhoneNo { get; set; }
        public string CreditUser { get; set; }
        public string StaffType { get; set; }
    }
}
