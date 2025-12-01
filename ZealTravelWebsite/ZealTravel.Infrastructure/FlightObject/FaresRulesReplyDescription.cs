using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class FaresRulesReplyDescription
    {
        public FaresRulesReplyDescription()
        {
            this.Title = this.Dep = this.Arr = this.Description = "";
            this.Group = (byte)0;
        }

        public string Title { get; set; }

        public string Dep { get; set; }

        public string Arr { get; set; }

        public byte Group { get; set; }

        public string Description { get; set; }
    }
}