using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class EventsComm
    {
        public EventsComm()
        {
            this.Amount = 0.0;
            this.IsFixed = false;
        }

        public string EventType { get; set; }

        public double Amount { get; set; }

        public bool IsFixed { get; set; }
    }
}