using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ZealTravel.Domain.Data.Entities;

namespace ZealTravel.Backoffice.Web.Models
{
    public class SearchAgencyResponse
    {
        public string? CompanyName { get; set; }
        public int? AccountId { get; set; }
        public string? City { get; set; }

        public string? State { get; set; }
        public bool ShowNoInputError { get; set; }


        public List<SearchAgencyData> AgencyData { get; set; } = new List<SearchAgencyData>();


    }
}
