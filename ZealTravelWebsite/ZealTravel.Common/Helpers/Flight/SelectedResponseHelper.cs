using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZealTravel.Common.Helpers.Flight
{
    public class SelectedResponseHelper
    {
        public static string GETSelectedResponse(string RefID_O, string RefID_I, string TResponse, bool IsCombi)
        {
            string SelectedResponse = string.Empty;

            if (IsCombi.Equals(true))
            {
                if (TResponse.Trim() != "")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(TResponse);
                    XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_O + "]");
                    SelectedResponse = "<root>";
                    if (xnList1 != null)
                    {
                        foreach (XmlNode node in xnList1)
                        {
                            SelectedResponse += node.OuterXml;
                        }
                    }
                    SelectedResponse += "</root>";
                }
            }
            else if (RefID_O.Length > 0 && RefID_I.Length > 0)
            {
                if (TResponse.Trim() != "")
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(TResponse);
                    XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_O + " and FltType='O']");
                    XmlNodeList xnList2 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_I + " and FltType='I']");

                    SelectedResponse = "<root>";
                    if (xnList1 != null)
                    {
                        foreach (XmlNode node in xnList1)
                        {
                            SelectedResponse += node.OuterXml;
                        }
                    }
                    if (xnList2 != null)
                    {
                        foreach (XmlNode node in xnList2)
                        {
                            SelectedResponse += node.OuterXml;
                        }
                    }
                    SelectedResponse += "</root>";
                }
            }
            else
            {
                if (RefID_O.Length > 0)
                {
                    if (TResponse.Trim() != "")
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(TResponse);

                        XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_O + " and FltType='O']");
                        SelectedResponse = "<root>";
                        if (xnList1 != null)
                        {
                            foreach (XmlNode node in xnList1)
                            {
                                SelectedResponse += node.OuterXml;
                            }
                        }
                        SelectedResponse += "</root>";
                    }
                }
                else if (RefID_I.Length > 0)
                {
                    if (TResponse.Trim() != "")
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.LoadXml(TResponse);

                        XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[RefID=" + RefID_I + " and FltType='I']");
                        SelectedResponse = "<root>";
                        if (xnList1 != null)
                        {
                            foreach (XmlNode node in xnList1)
                            {
                                SelectedResponse += node.OuterXml;
                            }
                        }
                        SelectedResponse += "</root>";
                    }
                }
            }

            return SelectedResponse;
        }

        public static string GetSelectedResponse(string AirRS, string FltType)
        {
            string SelectedResponse = string.Empty;
            if (FltType.Equals("O"))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(AirRS);

                XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[FltType='O']");
                SelectedResponse = "<root>";
                if (xnList1 != null)
                {
                    foreach (XmlNode node in xnList1)
                    {
                        SelectedResponse += node.OuterXml;
                    }
                }
                SelectedResponse += "</root>";
            }
            else if (FltType.Equals("I"))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(AirRS);

                XmlNodeList xnList1 = xmldoc.SelectNodes("/root/AvailabilityInfo[FltType='I']");
                SelectedResponse = "<root>";
                if (xnList1 != null)
                {
                    foreach (XmlNode node in xnList1)
                    {
                        SelectedResponse += node.OuterXml;
                    }
                }
                SelectedResponse += "</root>";
            }
            return SelectedResponse;
        }
    }
}
