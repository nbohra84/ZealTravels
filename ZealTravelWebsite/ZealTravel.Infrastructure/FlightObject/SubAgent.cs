using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    public class SubAgent
    {
        public SubAgent()
        {
            this.Id = this.UserId = this.BranchId = this.SaBranchId = 0;
            this.IsCreditCust = "NO";
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public int BranchId { get; set; }

        public int SaBranchId { get; set; }

        public string IsCreditCust { get; set; }

        public string AgentRef { get; set; }
    }
}