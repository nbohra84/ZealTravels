using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class SpicejetSSRDTO
    {
        public Int64 Rowid { get; set; }
        public string? Code { get; set; }
        public string? Ssrtype { get; set; }
        public string? Description { get; set; }
        public Int32? Amount { get; set; }
    }
}
