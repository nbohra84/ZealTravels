using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class SearchRequestModel:SearchModel
    {
        public string TokenId { get; set; }
    }
}
