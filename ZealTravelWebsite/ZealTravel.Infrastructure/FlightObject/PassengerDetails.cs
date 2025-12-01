using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class PassengerDetails
    {
        private bool propertyValue;

        public bool ShowPropertyWhileSerialize
        {
            get
            {
                return propertyValue;
            }
            set
            {
                propertyValue = value;
                Passenger[] passenger = Passenger;
                foreach (Passenger passenger2 in passenger)
                {
                    if (passenger2 != null)
                    {
                        passenger2.ShowPropertyWhileSerialize = propertyValue;
                    }
                }
            }
        }

        [DefaultValue(0)]
        public int Adults { get; set; }

        [DefaultValue(0)]
        public int Childs { get; set; }

        [DefaultValue(0)]
        public int Infant { get; set; }

        [DefaultValue(0)]
        public int Youth { get; set; }

        [DefaultValue(0)]
        public int Senior { get; set; }

        [XmlElement("Passenger")]
        public Passenger[] Passenger { get; set; }

        public bool ShouldSerializeShowPropertyWhileSerialize()
        {
            return false;
        }
    }
}
