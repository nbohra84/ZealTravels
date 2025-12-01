using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZealTravel.Infrastructure.TBO
{
    class SetSSRModifier
    {
        public string errorMessage;
        //-----------------------------------------------------------------------------------------------
        private string Searchid;
        private string Companyid;
        //-----------------------------------------------------------------------------------------------
        public SetSSRModifier(string Searchid, string Companyid)
        {
            this.Searchid = Searchid;
            this.Companyid = Companyid;
        }
        public DataTable GetSSRModifierRTLCC(string OuterXml, string Origin, string Destination, string FltType, bool IsCombi, string CarrierCode)
        {
            DataTable dtAddOn = DBCommon.Schema.SchemaSSR;

            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(OuterXml);
                int iRow = 0;

                XmlNodeList nodeGDS = xmlflt.SelectNodes("Response/Meal"); // You can also use XPath here
                if (nodeGDS != null && nodeGDS.Count > 0)
                {
                    foreach (XmlNode node in nodeGDS)
                    {
                        DataRow drAdd = dtAddOn.NewRow();
                        drAdd["RowID"] = iRow;
                        drAdd["CarrierCode"] = CarrierCode;
                        drAdd["CodeType"] = "M";
                        drAdd["Code"] = node["Code"].InnerText;
                        drAdd["Description"] = node["Description"].InnerText;
                        drAdd["Detail"] = "";
                        drAdd["DepartureStation"] = Origin;
                        drAdd["ArrivalStation"] = Destination;
                        dtAddOn.Rows.Add(drAdd);
                        iRow++;
                    }

                    if (IsCombi.Equals(false))
                    {
                        foreach (DataRow dr in dtAddOn.Rows)
                        {
                            dr["FltType"] = FltType;
                        }
                        dtAddOn.AcceptChanges();
                    }
                    else
                    {
                        foreach (DataRow dr in dtAddOn.Rows)
                        {
                            dr["FltType"] = "O";
                        }
                        dtAddOn.AcceptChanges();

                        DataTable ndtSSR = dtAddOn.Copy();
                        foreach (DataRow dr in dtAddOn.Rows)
                        {
                            DataRow drAdd = ndtSSR.NewRow();
                            drAdd["RowID"] = iRow;
                            drAdd["CarrierCode"] = CarrierCode;
                            drAdd["CodeType"] = "M";
                            drAdd["Code"] = dr["Code"].ToString();
                            drAdd["Description"] = dr["Description"].ToString();
                            drAdd["FltType"] = "I";
                            drAdd["Detail"] = "";
                            drAdd["DepartureStation"] = Origin;
                            drAdd["ArrivalStation"] = Destination;
                            ndtSSR.Rows.Add(drAdd);
                            iRow++;
                        }
                        ndtSSR.AcceptChanges();
                        dtAddOn.Clear();
                        dtAddOn = ndtSSR.Copy();
                    }
                }
                else
                {
                    string FlightNumber = "";
                    string AirlineCode = "";

                    XmlNodeList nodeO = xmlflt.SelectNodes("Response/Baggage"); // You can also use XPath here
                    if (nodeO != null && nodeO.Count > 0)
                    {
                        foreach (XmlNode node in nodeO)
                        {
                            XmlNodeList cc = node.ChildNodes;
                            foreach (XmlNode nodeI in cc)
                            {
                                if (Convert.ToDecimal(nodeI["Price"].InnerText) > 0)
                                {
                                    DataRow drAdd = dtAddOn.NewRow();
                                    drAdd["RowID"] = iRow;
                                    drAdd["CodeType"] = "B";
                                    drAdd["Code"] = nodeI["Code"].InnerText;
                                    drAdd["Description"] = nodeI["Weight"].InnerText + " Kg";
                                    drAdd["Amount"] = GetCommonFunctions.AvgAmount1(Convert.ToDecimal(nodeI["Price"].InnerText).ToString());
                                    drAdd["DepartureStation"] = nodeI["Origin"].InnerText;
                                    drAdd["ArrivalStation"] = nodeI["Destination"].InnerText;

                                    FlightNumber = "";
                                    AirlineCode = "";
                                    if (nodeI["FlightNumber"] != null)
                                    {
                                        FlightNumber = nodeI["FlightNumber"].InnerText;
                                    }
                                    if (nodeI["AirlineCode"] != null)
                                    {
                                        AirlineCode = nodeI["AirlineCode"].InnerText;
                                    }

                                    if (nodeI["Origin"].InnerText.Equals(Origin))
                                    {
                                        drAdd["FltType"] = "O";
                                    }
                                    else if (nodeI["Origin"].InnerText.Equals(Destination))
                                    {
                                        drAdd["FltType"] = "I";
                                    }

                                    drAdd["CarrierCode"] = AirlineCode;
                                    drAdd["Detail"] = "DESC:" + nodeI["Description"].InnerText + "-WT:" + nodeI["WayType"].InnerText + "-W:" + nodeI["Weight"].InnerText + "-CU:" + nodeI["Currency"].InnerText + "-OR:" + nodeI["Origin"].InnerText + "-DE:" + nodeI["Destination"].InnerText + "-AC:" + AirlineCode + "-FN:" + FlightNumber;

                                    dtAddOn.Rows.Add(drAdd);
                                    iRow++;
                                }
                            }
                        }
                    }

                    nodeO = xmlflt.SelectNodes("Response/MealDynamic"); // You can also use XPath here
                    if (nodeO != null && nodeO.Count > 0)
                    {
                        foreach (XmlNode node in nodeO)
                        {
                            XmlNodeList cc = node.ChildNodes;
                            foreach (XmlNode nodeI in cc)
                            {
                                string AD = string.Empty;
                                if (nodeI["AirlineDescription"] != null)
                                {
                                    AD = nodeI["AirlineDescription"].InnerText.Trim();
                                    if (AD.IndexOf("&amp;") != -1)
                                    {
                                        AD = AD.Replace("&amp;", "-");
                                    }
                                }

                                DataRow drAdd = dtAddOn.NewRow();
                                drAdd["RowID"] = iRow;

                                drAdd["CodeType"] = "M";
                                drAdd["Code"] = nodeI["Code"].InnerText;

                                if (AD.Equals(string.Empty))
                                {
                                    drAdd["Description"] = nodeI["Code"].InnerText;
                                }
                                else
                                {
                                    drAdd["Description"] = AD;
                                }

                                drAdd["Amount"] = GetCommonFunctions.AvgAmount1(Convert.ToDecimal(nodeI["Price"].InnerText).ToString());

                                drAdd["DepartureStation"] = nodeI["Origin"].InnerText;
                                drAdd["ArrivalStation"] = nodeI["Destination"].InnerText;

                                FlightNumber = "";
                                AirlineCode = "";
                                if (nodeI["FlightNumber"] != null)
                                {
                                    FlightNumber = nodeI["FlightNumber"].InnerText;
                                }
                                if (nodeI["AirlineCode"] != null)
                                {
                                    AirlineCode = nodeI["AirlineCode"].InnerText;
                                }

                                drAdd["CarrierCode"] = AirlineCode;
                                drAdd["Detail"] = "DESC:" + nodeI["Description"].InnerText + "-WT:" + nodeI["WayType"].InnerText + "-AD:" + AD + "-CU:" + nodeI["Currency"].InnerText + "-OR:" + nodeI["Origin"].InnerText + "-DE:" + nodeI["Destination"].InnerText + "-AC:" + AirlineCode + "-FN:" + FlightNumber;

                                if (nodeI["Origin"].InnerText.Equals(Origin))
                                {
                                    drAdd["FltType"] = "O";
                                }
                                else if (nodeI["Origin"].InnerText.Equals(Destination))
                                {
                                    drAdd["FltType"] = "I";
                                }

                                if (nodeI["Code"].InnerText.Trim().IndexOf("No Meal") == -1)
                                {
                                    dtAddOn.Rows.Add(drAdd);
                                }
                                iRow++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "GetSSRModifier", "air_tbo-SetSSRModifier", "", Searchid, errorMessage);
            }

            return dtAddOn;
        }
        public DataTable GetSSRModifier(string OuterXml, string Origin, string Destination, string FltType, bool IsCombi, string CarrierCode)
        {
            DataTable dtAddOn = DBCommon.Schema.SchemaSSR;

            try
            {
                XmlDocument xmlflt = new XmlDocument();
                xmlflt.LoadXml(OuterXml);
                int iRow = 0;
                XmlNodeList nodeGDS = xmlflt.SelectNodes("Response/Meal"); // You can also use XPath here
                if (nodeGDS != null && nodeGDS.Count > 0)
                {
                    foreach (XmlNode node in nodeGDS)
                    {
                        DataRow drAdd = dtAddOn.NewRow();
                        drAdd["RowID"] = iRow;
                        drAdd["CarrierCode"] = CarrierCode;
                        drAdd["CodeType"] = "M";
                        drAdd["Code"] = node["Code"].InnerText;
                        drAdd["Description"] = node["Description"].InnerText;
                        drAdd["Detail"] = "";
                        drAdd["DepartureStation"] = Origin;
                        drAdd["ArrivalStation"] = Destination;
                        dtAddOn.Rows.Add(drAdd);
                        iRow++;
                    }

                    if (IsCombi.Equals(false))
                    {
                        foreach (DataRow dr in dtAddOn.Rows)
                        {
                            dr["FltType"] = FltType;
                        }
                        dtAddOn.AcceptChanges();
                    }
                    else
                    {
                        foreach (DataRow dr in dtAddOn.Rows)
                        {
                            dr["FltType"] = "O";
                        }
                        dtAddOn.AcceptChanges();

                        DataTable ndtSSR = dtAddOn.Copy();
                        foreach (DataRow dr in dtAddOn.Rows)
                        {
                            DataRow drAdd = ndtSSR.NewRow();
                            drAdd["RowID"] = iRow;
                            drAdd["CarrierCode"] = CarrierCode;
                            drAdd["CodeType"] = "M";
                            drAdd["Code"] = dr["Code"].ToString();
                            drAdd["Description"] = dr["Description"].ToString();
                            drAdd["FltType"] = "I";
                            drAdd["Detail"] = "";
                            drAdd["DepartureStation"] = Origin;
                            drAdd["ArrivalStation"] = Destination;
                            ndtSSR.Rows.Add(drAdd);
                            iRow++;
                        }
                        ndtSSR.AcceptChanges();
                        dtAddOn.Clear();
                        dtAddOn = ndtSSR.Copy();
                    }
                }
                else
                {
                    string FlightNumber = "";
                    string AirlineCode = "";

                    XmlNodeList nodeO = xmlflt.SelectNodes("Response/Baggage"); // You can also use XPath here
                    if (nodeO != null && nodeO.Count > 0)
                    {
                        foreach (XmlNode node in nodeO)
                        {
                            XmlNodeList cc = node.ChildNodes;
                            foreach (XmlNode nodeI in cc)
                            {
                                if (Convert.ToDecimal(nodeI["Price"].InnerText) > 0)
                                {
                                    DataRow drAdd = dtAddOn.NewRow();
                                    drAdd["RowID"] = iRow;
                                    drAdd["CodeType"] = "B";
                                    drAdd["Code"] = nodeI["Code"].InnerText;
                                    drAdd["Description"] = nodeI["Weight"].InnerText + " Kg";
                                    drAdd["Amount"] = GetCommonFunctions.AvgAmount1(Convert.ToDecimal(nodeI["Price"].InnerText).ToString());

                                    FlightNumber = "";
                                    AirlineCode = "";
                                    if (nodeI["FlightNumber"] != null)
                                    {
                                        FlightNumber = nodeI["FlightNumber"].InnerText;
                                    }
                                    if (nodeI["AirlineCode"] != null)
                                    {
                                        AirlineCode = nodeI["AirlineCode"].InnerText;
                                    }



                                    if (IsCombi.Equals(false))
                                    {
                                        drAdd["FltType"] = FltType;
                                    }
                                    else
                                    {
                                        if (nodeI["Origin"].InnerText.Equals(Origin))
                                        {
                                            drAdd["FltType"] = "O";
                                        }
                                        else if (nodeI["Origin"].InnerText.Equals(Destination))
                                        {
                                            drAdd["FltType"] = "I";
                                        }
                                        else
                                        {
                                            if (nodeI["Destination"].InnerText.Equals(Origin))
                                            {
                                                drAdd["FltType"] = "I";
                                            }
                                            else if (nodeI["Destination"].InnerText.Equals(Destination))
                                            {
                                                drAdd["FltType"] = "O";
                                            }
                                        }
                                    }

                                    drAdd["CarrierCode"] = AirlineCode;
                                    drAdd["DepartureStation"] = nodeI["Origin"].InnerText;
                                    drAdd["ArrivalStation"] = nodeI["Destination"].InnerText;

                                    drAdd["Detail"] = "DESC:" + nodeI["Description"].InnerText + "-WT:" + nodeI["WayType"].InnerText + "-W:" + nodeI["Weight"].InnerText + "-CU:" + nodeI["Currency"].InnerText + "-OR:" + nodeI["Origin"].InnerText + "-DE:" + nodeI["Destination"].InnerText + "-AC:" + AirlineCode + "-FN:" + FlightNumber;

                                    dtAddOn.Rows.Add(drAdd);
                                    iRow++;
                                }
                            }
                        }
                    }

                    nodeO = xmlflt.SelectNodes("Response/MealDynamic"); // You can also use XPath here
                    if (nodeO != null && nodeO.Count > 0)
                    {
                        foreach (XmlNode node in nodeO)
                        {
                            XmlNodeList cc = node.ChildNodes;
                            foreach (XmlNode nodeI in cc)
                            {
                                string AD = string.Empty;
                                if (nodeI["AirlineDescription"] != null)
                                {
                                    AD = nodeI["AirlineDescription"].InnerText.Trim();
                                    if (AD.IndexOf("&amp;") != -1)
                                    {
                                        AD = AD.Replace("&amp;", "-");
                                    }
                                }

                                DataRow drAdd = dtAddOn.NewRow();
                                drAdd["RowID"] = iRow;
                                drAdd["CodeType"] = "M";
                                drAdd["Code"] = nodeI["Code"].InnerText;

                                if (AD.Equals(string.Empty))
                                {
                                    drAdd["Description"] = nodeI["Code"].InnerText;
                                }
                                else
                                {
                                    drAdd["Description"] = AD;
                                }

                                drAdd["Amount"] = GetCommonFunctions.AvgAmount1(Convert.ToDecimal(nodeI["Price"].InnerText).ToString());


                                FlightNumber = "";
                                AirlineCode = "";
                                if (nodeI["FlightNumber"] != null)
                                {
                                    FlightNumber = nodeI["FlightNumber"].InnerText;
                                }
                                if (nodeI["AirlineCode"] != null)
                                {
                                    AirlineCode = nodeI["AirlineCode"].InnerText;
                                }

                                drAdd["CarrierCode"] = AirlineCode;
                                drAdd["DepartureStation"] = nodeI["Origin"].InnerText;
                                drAdd["ArrivalStation"] = nodeI["Destination"].InnerText;

                                drAdd["Detail"] = "DESC:" + nodeI["Description"].InnerText + "-WT:" + nodeI["WayType"].InnerText + "-AD:" + AD + "-CU:" + nodeI["Currency"].InnerText + "-OR:" + nodeI["Origin"].InnerText + "-DE:" + nodeI["Destination"].InnerText + "-AC:" + AirlineCode + "-FN:" + FlightNumber;



                                if (IsCombi.Equals(false))
                                {
                                    drAdd["FltType"] = FltType;
                                }
                                else
                                {
                                    if (nodeI["Origin"].InnerText.Equals(Origin))
                                    {
                                        drAdd["FltType"] = "O";
                                    }
                                    else if (nodeI["Origin"].InnerText.Equals(Destination))
                                    {
                                        drAdd["FltType"] = "I";
                                    }
                                    else
                                    {
                                        if (nodeI["Destination"].InnerText.Equals(Origin))
                                        {
                                            drAdd["FltType"] = "I";
                                        }
                                        else if (nodeI["Destination"].InnerText.Equals(Destination))
                                        {
                                            drAdd["FltType"] = "O";
                                        }
                                    }
                                }

                                if (nodeI["Code"].InnerText.Trim().IndexOf("No Meal") == -1)
                                {
                                    dtAddOn.Rows.Add(drAdd);
                                }

                                iRow++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(Companyid, 0, "GetSSRModifier", "air_tbo-SetSSRModifier", "", Searchid, errorMessage);
            }

            return dtAddOn;
        }
    }
}
