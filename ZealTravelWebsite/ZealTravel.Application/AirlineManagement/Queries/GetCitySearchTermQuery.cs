using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.AirlineManagement.Queries
{
    public class GetCitySearchTermQuery//:IQuery<List<string>>
    {
        public string SearchTerm { get; set; }=String.Empty;
    }
}
