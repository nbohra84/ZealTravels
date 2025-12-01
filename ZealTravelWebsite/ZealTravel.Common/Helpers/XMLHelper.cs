using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ZealTravel.Common.Helpers
{
    public class XMLHelper
    {
        public static void MapXmlToObject(XmlNode node, object target)
        {
            Type targetType = target.GetType();
            PropertyInfo[] properties = targetType.GetProperties();

            foreach (var property in properties)
            {
                string value = node.SelectSingleNode(property.Name)?.InnerText;
                if (!string.IsNullOrEmpty(value))
                {
                    property.SetValue(target, Convert.ChangeType(value, property.PropertyType));
                }
            }
        }

        public static string ConvertDatatableToXMLstring(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }

        public static DataSet SessionFilterResult(string resultCom)
        {
            DataSet ds = new DataSet();
            //string SelectedResponse = "<root>";
            string SelectedResponse1 = string.Empty;
            string SelectedResponse2 = string.Empty;

            XmlDocument xmldoc1 = new XmlDocument();
            xmldoc1.LoadXml(resultCom);
            XmlNodeList xnList1 = xmldoc1.SelectNodes("/root/AvailabilityInfo[FltType='O']");
            string FilteredResponse1 = "<root>";
            if (xnList1 != null)
            {
                foreach (XmlNode node in xnList1)
                {
                    SelectedResponse1 += node.OuterXml;
                    FilteredResponse1 += node.OuterXml;
                }
            }
            FilteredResponse1 += "</root>";

            //HttpContext.Current.Session["FinalResult"] = FilteredResponse1;

            DataTable _dt = new DataTable();
            DataTable dt1 = new DataTable();
            string outbondResponse = "<root>" + SelectedResponse1 + "</root>";
            _dt = BuildDataTableFromXml("Outbond", outbondResponse);
            dt1 = DatatableHelper.FilterRowID(_dt);
            ds.Tables.Add(dt1);

            SelectedResponse1 = SelectedResponse1.Replace("<root>", "");
            SelectedResponse1 = SelectedResponse1.Replace("</root>", "");
            XmlDocument xmldoc2 = new XmlDocument();
            xmldoc2.LoadXml(resultCom);

            XmlNodeList xnList2 = xmldoc2.SelectNodes("/root/AvailabilityInfo[FltType='I']");
            string FilteredResponse2 = "<root>";
            if (xnList2 != null)
            {
                foreach (XmlNode node in xnList2)
                {
                    SelectedResponse2 += node.OuterXml;
                    FilteredResponse2 += node.OuterXml;
                }
            }
            FilteredResponse2 += "</root>";
            //HttpContext.Current.Session["FinalResultInBond"] = FilteredResponse2;

            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            string InbondResponse = "<root>" + SelectedResponse2 + "</root>";
            dt2 = BuildDataTableFromXml("Inbond", InbondResponse);

            dt3 = DatatableHelper.FilterRowID(dt2);

            ds.Tables.Add(dt3);
            return ds;
        }

        public static DataTable BuildDataTableFromXml(string Name, string XMLString)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(XMLString));
            DataTable Dt = new DataTable(Name);
            try
            {

                XmlNode NodoEstructura = doc.FirstChild.FirstChild;
                //  Table structure (columns definition) 
                foreach (XmlNode columna in NodoEstructura.ChildNodes)
                {
                    Dt.Columns.Add(columna.Name, typeof(String));
                }

                XmlNode Filas = doc.FirstChild;
                //  Data Rows 
                foreach (XmlNode Fila in Filas.ChildNodes)
                {
                    List<string> Valores = new List<string>();
                    foreach (XmlNode Columna in Fila.ChildNodes)
                    {
                        Valores.Add(Columna.InnerText);
                    }
                    Dt.Rows.Add(Valores.ToArray());
                }
            }
            catch (Exception)
            {

            }

            return Dt;
        }

        public static string ConvertDatatableToXML(DataTable dt)
        {
            dt.TableName = "AvailabilityInfo";
            DataSet ds = new DataSet();
            ds.DataSetName = "root";

            ds.Tables.Add(dt);

            MemoryStream ms = new MemoryStream();
            ds.WriteXml(ms);
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
            ms = (MemoryStream)xtw.BaseStream;
            UTF8Encoding u8e = new UTF8Encoding();
            string Request = u8e.GetString(ms.ToArray());
            return Request;
            
        }

        /// <summary>
        /// Serializes a list of objects to an XML string.
        /// </summary>
        public static string ConvertListToXml<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List cannot be null or empty.", nameof(list));

            return Serialize(list, typeof(List<T>));
        }

        /// <summary>
        /// Serializes a single object to an XML string.
        /// </summary>
        public static string ConvertObjectToXml<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

            return Serialize(obj, typeof(T));
        }

        /// <summary>
        /// Common serialization logic used by both ConvertListToXml and ConvertObjectToXml.
        /// </summary>
        private static string Serialize(object data, Type dataType)
        {
            try
            {
                var serializer = new XmlSerializer(dataType);
                using (var stringWriter = new StringWriter())
                {
                    serializer.Serialize(stringWriter, data);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while converting to XML.", ex);
            }
        }



    }
}