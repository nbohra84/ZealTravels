using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class CredModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string SupplierCode { get; set; }
    }

    public class SupplierCREDGalileoAirline
    {
        public int Id { get; set; }

        public string? HAP { get; set; }

        public string? Userid { get; set; }

        public string? Password { get; set; }

        public string? Soap_Url { get; set; }

        public string? TargetBranch { get; set; }

        public string? PCC { get; set; }

        public int? Import_Queue { get; set; }

        public int? Tktd_Queue { get; set; }

        public bool? TicketIfFareGaurantee { get; set; }

        public bool? Status { get; set; }
        public string NetworkUserName { get; set; }
        public string NetworkPassword { get; set; }
    }
    public partial class SupplierCREDDetailLccAirline
    {
        public int Id { get; set; }

        public string? Supplier_Name { get; set; }

        public string? SupplierID { get; set; }

        public string? Supplier_Code { get; set; }

        public string? TargetBranch { get; set; }

        public string? OrganizationCode { get; set; }

        public string? CarrierCode { get; set; }

        public string? AgentID { get; set; }

        public string? AgentDomain { get; set; }

        public string? Password { get; set; }

        public string? LocationCode { get; set; }

        public int? ContractVersion { get; set; }

        public string? PromoCode { get; set; }

        public string? CorporateCode { get; set; }

        public string? Currency { get; set; }

        public string? LoginID { get; set; }

        public string? Pwd { get; set; }

        public string? SessionURL { get; set; }

        public string? BookingURL { get; set; }

        public string? FareURL { get; set; }

        public string? ContentURL { get; set; }

        public string? AgentURL { get; set; }

        public string? LookupURL { get; set; }

        public string? OperationURL { get; set; }
    }
}
