using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.DBCommonManagement.Commands
{
    public class AddDBLogCommand
    {
       public string CompanyID { get; set; }
       public Int32 BookingRef { get; set; }
       public string MethodName { get; set; }
       public string Location { get; set; }
       public string SearchCriteria { get; set; }
       public string SearchID { get; set; }
       public string ErrorMessage { get; set; }
    }
}
