using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class GDSDetails
    {
        public GDSDetails() => this.AgentPCC = this.AgentSignIn = "";

        public string AgentPCC { get; set; }

        public string AgentSignIn { get; set; }
    }
}