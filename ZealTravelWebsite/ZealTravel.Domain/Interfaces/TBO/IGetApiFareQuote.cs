using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.TBO
{
    public interface IGetApiFareQuote
    {
        public string GetFare(DataTable dtBound);
    }
}
