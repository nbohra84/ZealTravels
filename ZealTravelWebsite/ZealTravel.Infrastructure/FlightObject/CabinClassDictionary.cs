using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.FlightObject
{
    public static class CabinClassDictionary
    {
        public static Dictionary<string, string> CabinCodeClassAsKey = new Dictionary<string, string>()
    {
      {
        "EC",
        "Y"
      },
      {
        "FR",
        "F"
      },
      {
        "BU",
        "C"
      },
      {
        "PE",
        "W"
      }
    };
        public static Dictionary<string, string> CabinClassesCodeAsKey = new Dictionary<string, string>()
    {
      {
        "ALL",
        "ALL"
      },
      {
        "EC",
        "Economy"
      },
      {
        "FR",
        "First Class"
      },
      {
        "BU",
        "Business"
      },
      {
        "PE",
        "Premium Economy"
      }
    };
        public static Dictionary<string, string> CabinClassesNameAsKey = new Dictionary<string, string>()
    {
      {
        "Economy",
        "EC"
      },
      {
        "First Class",
        "FR"
      },
      {
        "Business",
        "BU"
      },
      {
        "Premium Economy",
        "PE"
      },
      {
        "PremiumEconomy",
        "PE"
      }
    };
    }
}
