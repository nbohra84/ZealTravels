using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Data.Models
{
    public class DashboardNotification
    {
        public string Description { get; set; }
        public string? Subject { get; set; }
        public string? Link { get; set; }
    }
}
