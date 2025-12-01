using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.CommonComponents
{
    public class clsTextWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public clsTextWriter(StringBuilder sb)
            : base(sb)
        {
        }
    }
}
