using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Application.DBCommonManagement.Commands
{
    public class AddDBSearchLogCommand
    {
        public string CompanyID { get; set; }
        public Int32 BookingRef { get; set; }
        public string StaffID { get; set; }
        public string SearchID { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Place { get; set; }
        public string Remark { get; set; }
        public string Remark2 { get; set; }
        public string Host { get; set; }
    }
}
