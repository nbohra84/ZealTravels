using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Domain.Models
{
    public class SearchModel: TokenRequestModel
    {
        public string Origin { get; set; } 
        public string Destination{get;set;} 
        public string BeginDate{get;set;} 
        public int Adt{get;set;} 
        public int Chd{get;set;} 
        public int Inf{get;set;} 
        public string Cabin{get;set;} 
        public ArrayList CarrierList{get;set;} 
        public string Sector{get;set;}
        public string FltType{get;set;}
    }
}
