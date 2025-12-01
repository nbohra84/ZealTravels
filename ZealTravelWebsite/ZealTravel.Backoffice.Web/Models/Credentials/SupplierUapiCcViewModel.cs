using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Backoffice.Web.Models.Credentials
{
    public class SupplierUapiCcViewModel
    {
        public List<UapiCcDetail>? UapiCcDetails { get; set; }

        public UapiCcDetail? NewUapiCc { get; set; }
    }

}
