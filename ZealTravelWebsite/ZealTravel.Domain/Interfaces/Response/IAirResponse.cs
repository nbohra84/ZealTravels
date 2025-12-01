using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Interfaces.Response
{
    public interface IAirResponse
    {
        string TBOResponse { get; set; }
        string UAPIResponse { get; set; }
        string _6EResponse { get; set; }
        string QPResponse { get; set; }

        //DataTable
    }
}
