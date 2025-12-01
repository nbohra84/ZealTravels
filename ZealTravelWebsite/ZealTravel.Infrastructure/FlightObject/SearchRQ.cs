using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZealTravel.Infrastructure.FlightObject
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class SearchRQ
    {
        private bool propertyValue;

        public bool ShouldSerializeShowPropertyWhileSerialize() => false;

        public bool ShowPropertyWhileSerialize
        {
            get => this.propertyValue;
            set
            {
                this.propertyValue = value;
                if (this.GeneralInfo != null)
                    this.GeneralInfo.ShowPropertyWhileSerialize = this.propertyValue;
                if (this.Search == null)
                    return;
                foreach (Search search in this.Search)
                    search.ShowPropertyWhileSerialize = this.propertyValue;
            }
        }

        [XmlElement("Authentication")]
        public Authentication Authentication { get; set; }

        [XmlElement("GeneralInfo")]
        public GeneralInfo GeneralInfo { get; set; }

        [XmlElement("Search")]
        public Search[] Search { get; set; }

        [XmlElement("PaxDetails")]
        public PaxDetails PaxDetails { get; set; }

        [XmlElement("AdvanceSearch")]
        public AdvanceSearch AdvanceSearch { get; set; }

        [XmlElement("CompanySetting")]
        public CompanySetting CompanySetting { get; set; }

        public bool ShouldSerializeCompanySetting() => this.propertyValue;

        public SplitInfo SplitInfo { get; set; }

        public bool ShouldSerializeSplitInfo() => this.propertyValue;

        public IncludeGDS IncludeGDS { get; set; }

        public bool ShouldSerializeIncludeGDS() => this.propertyValue;
    }
}
