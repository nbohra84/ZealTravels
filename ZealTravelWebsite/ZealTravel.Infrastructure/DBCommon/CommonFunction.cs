using System;
using System.Collections;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Xml.Schema;
using System.Text.RegularExpressions;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class CommonFunction
    { 
        public static bool CheckInteger(string value)
        {
            bool b = false;
            string expression = @"^[0-9]+$";
            Match match = Regex.Match(value.Trim(), expression, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                b = true;
            }
            else
            {
                b = false;
            }
            return b;
        }
        public static Decimal getPercentValue(Decimal iValue, Decimal dPercent)
        {
            Decimal dValue = 0;
            dValue = (iValue * dPercent / 100);
            dValue = DBCommon.CommonFunction.AvgAmount(dValue.ToString());
            return dValue;
        }
        public static Decimal getFixedValue(Decimal iValue, Decimal dFixed)
        {
            Decimal dValue = 0;
            dValue = (iValue + dFixed);
            return dValue;
        }
        public static string getCompany_by_SubCompany_Customer(string CompanyID)
        {
            if (CompanyID.IndexOf("-SA-") != -1)
            {
                CompanyID = CompanyID.Substring(0, CompanyID.IndexOf("-SA"));
            }
            if (CompanyID.IndexOf("-C-") != -1)
            {
                CompanyID = CompanyID.Substring(0, CompanyID.IndexOf("-C"));
            } 
            return CompanyID;
        }
        public static int AvgAmount(string Fees)
        {
            try
            {
                double Fees1 = Convert.ToDouble(Fees);

                int GetFees = 0;

                if (Fees.ToString().IndexOf(".") != -1)
                {
                    string k3 = Fees.ToString().Substring(0, Fees.ToString().IndexOf("."));

                    double k4 = Convert.ToDouble(k3) + .5;

                    if (Fees1 >= k4)
                    {
                        GetFees = int.Parse(k3) + 1;
                    }
                    else
                    {
                        GetFees = int.Parse(k3);
                    }
                }
                else
                {
                    GetFees = int.Parse(Fees1.ToString());
                }

                return GetFees;
            }
            catch
            {
                return int.Parse(Fees.ToString().Substring(0, Fees.ToString().IndexOf(".")));
            }
        }
        public static int AvgAmount1(string Fees)
        {
            try
            {
                double Fees1 = Convert.ToDouble(Fees);

                int GetFees = 0;

                if (Fees.ToString().IndexOf(".") != -1)
                {
                    string k3 = Fees.ToString().Substring(0, Fees.ToString().IndexOf("."));

                    double k4 = Convert.ToDouble(k3) + .1;

                    if (Fees1 >= k4)
                    {
                        GetFees = int.Parse(k3) + 1;
                    }
                    else
                    {
                        GetFees = int.Parse(k3);
                    }
                }
                else
                {
                    GetFees = int.Parse(Fees1.ToString());
                }

                return GetFees;
            }
            catch
            {
                return int.Parse(Fees.ToString().Substring(0, Fees.ToString().IndexOf(".")));
            }
        }
        public static bool IsLCC(string CarrierCode)
        {
            CarrierCode = CarrierCode.Trim().ToUpper();
            if (CarrierCode.IndexOf("SG") != -1 || CarrierCode.IndexOf("G8") != -1 || CarrierCode.IndexOf("6E") != -1 || CarrierCode.IndexOf("IX") != -1
                            && CarrierCode.IndexOf("I5") != -1 || CarrierCode.IndexOf("LB") != -1 || CarrierCode.IndexOf("D7") != -1 || CarrierCode.IndexOf("FD") != -1
                            && CarrierCode.IndexOf("AK") != -1 || CarrierCode.IndexOf("XJ") != -1 || CarrierCode.IndexOf("XT") != -1 || CarrierCode.IndexOf("QZ") != -1
                            && CarrierCode.IndexOf("Z2") != -1 || CarrierCode.IndexOf("ZO") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Int32 GenerateRefID(string AirlineID, Int32 iAdd, string FltType)
        {
            int iRefID = 0;

            try
            {
                byte[] asciiValue = Encoding.ASCII.GetBytes(AirlineID);
                for (int i = 0; i < asciiValue.Length; i++)
                {
                    iRefID += asciiValue[i];
                }

                byte[] asciiValue1 = Encoding.ASCII.GetBytes(FltType);
                for (int i = 0; i < asciiValue1.Length; i++)
                {
                    iRefID += asciiValue1[i];
                }

                iRefID += iAdd;
                if (FltType.Equals("I"))
                {
                    iRefID += 70000;
                }

            }
            catch
            {

            }

            return iRefID;
        }
        public static Int32 GenerateRefID(string CarrierCode, string AirlineID)
        {
            int iRefID = 0;

            try
            {
                byte[] asciiCarrierCode = Encoding.ASCII.GetBytes(CarrierCode);
                byte[] asciiAirlineID = Encoding.ASCII.GetBytes(AirlineID);

                for (int i = 0; i < asciiCarrierCode.Length; i++)
                {
                    iRefID += asciiCarrierCode[i];
                }
                for (int i = 0; i < asciiAirlineID.Length; i++)
                {
                    iRefID += asciiAirlineID[i];
                }
            }
            catch
            {

            }

            return iRefID;
        }

        public DataTable SetRowid(DataTable dtBound)
        {
            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                int RowID = 1;
                foreach (DataRow dr in dtBound.Rows)
                {
                    dr["RowID"] = RowID;
                    RowID++;
                }
            }
            return dtBound;
        }
        //------------------------------------------------------
        //System.IO.StreamReader myFile22 = new System.IO.StreamReader(@"E:\02.AvailabilityResponse.xml");
        //string AvailabilityRequest = myFile22.ReadToEnd();
        //myFile22.Close();
        //------------------------------------------------------
        //XmlDocument xmldoc = new XmlDocument();
        //xmldoc.LoadXml(AvailabilityRequest);
        //string RefID="3302";
        //XmlNodeList xnList = xmldoc.SelectNodes("/AvailabilityResponse/OutboundAvailabilityResponse[RefID=" + RefID + "]");

        //String returnStr = "<AvailabilityResponse>";
        //if (xnList != null)
        //{
        //    foreach (XmlNode node in xnList)
        //    {
        //        returnStr += node.OuterXml;
        //    }

        //}
        //returnStr += "</AvailabilityResponse>";
        //------------------------------------------------------
        //XmlDocument xml = new XmlDocument();
        // xml.LoadXml(FinalResult);
        //XmlNodeList xnList = xml.SelectNodes("/AvailabilityResponse/OutboundAvailabilityResponse[RefID=" + RefID + "]");
        //------------------------------------------------------

        //    XmlDocument xmldoc = new XmlDocument();
        //    xmldoc.LoadXml(AvailabilityRequest);
        //    string EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;

        //------------------------------------------------------

        //XmlDocument xmldoc = new XmlDocument();
        //xmldoc.LoadXml(AvailabilityRequest);

        //string EndDate = xmldoc.SelectSingleNode("AvailabilityRequest/EndDate").InnerText;

        //XmlNodeList nodeList = xmldoc.SelectNodes("AvailabilityRequest/AirVAry/AirVInfo");
        //foreach (XmlNode no in nodeList)
        //{
        //}

        //------------------------------------------------------
        // ss = ss.Replace("\"", "").Trim(); remove ""
        //------------------------------------------------------
        //DataSet ResponseDt = new DataSet();
        //ResponseDt.ReadXml(new System.IO.StringReader(XmlResponse));
        //------------------------------------------------------
        //FileInfo f1 = new FileInfo(@"C:\Request.xml");
        //StreamWriter sw1 = f1.CreateText();
        //sw1.Write(XMLResponse);
        //sw1.Close();
        //------------------------------------------------------
        //  string[] Orginallist1 = original.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        //------------------------------------------------------

        public static string[] ArrayList2Object(ArrayList Ar)
        {
            String[] myArr = (String[])Ar.ToArray(typeof(string));
            return myArr;
        }
        public static string ArrayListToString(ArrayList ar, string delim)
        {
            return string.Join(delim, (string[])ar.ToArray(typeof(string)));
        }

        public static string[] GetStringInBetween(string strSource, string strBegin, string strEnd, bool includeBegin, bool includeEnd)
        {
            string[] result = { "", "" };
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired

                if (includeBegin)
                {
                    iIndexOfBegin -= strBegin.Length;
                }
                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);
                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {  // include the End string if desired
                    if (includeEnd)
                    { iEnd += strEnd.Length; }
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment
                    if (iEnd + strEnd.Length < strSource.Length)
                    { result[1] = strSource.Substring(iEnd + strEnd.Length); }
                }
            }
            else
            // stay where we are
            { result[1] = strSource; }
            return result;
        }

        public static DataTable FilterRowID(DataTable dtBound)
        {
            if (dtBound.Columns.Contains("ForSort").Equals(true))
            {
                dtBound.Columns.Remove("ForSort");
                dtBound.AcceptChanges();
            }

            dtBound.Columns.Add("ForSort", typeof(int), "Convert(RowID, 'System.Int32')");
            DataView dv = new DataView(dtBound);
            dv.Sort = "ForSort ASC";
            dtBound = dv.ToTable();
            dtBound.Columns.Remove("ForSort");

            return dtBound;
        }
        public static DataTable RemoveConnectionsFlight(DataTable Dt, int NoOfConn)
        {
            DataTable MainDt = new DataTable();
            MainDt = Schema.SchemaFlights;

            try
            {
                ArrayList Ar_RefId = new ArrayList();
                Ar_RefId = DataTable2ArrayList(Dt, "RefId", true);

                DataRow[] result;

                for (int i = 0; i < Ar_RefId.Count; i++)
                {
                    result = Dt.Select("RefId='" + Ar_RefId[i].ToString() + "'");
                    if (result.Length != NoOfConn && result.Length > 0)
                    {
                        MainDt.Merge(result.CopyToDataTable());
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return MainDt;
        }
        public static ArrayList DataTable2ArrayList(DataTable Dt, string CloumnName, bool Distinct)
        {
            List<string> cs = new List<string>();
            ArrayList ar = new ArrayList(cs);

            if (Dt != null)
            {
                if (Dt.Rows.Count > 0)
                {

                    if (Distinct == true)
                    {
                        cs = Dt.Select().Select(ROW => ROW[CloumnName].ToString()).Distinct().ToList();
                    }
                    else
                    {
                        cs = Dt.Select().Select(ROW => ROW[CloumnName].ToString()).ToList();
                    }

                    ar = new ArrayList(cs);
                }
            }
            return ar;
        }
        public static ArrayList RemoveDuplicates(ArrayList items)
        {
            ArrayList noDups = new ArrayList();
            foreach (string strItem in items)
            {
                if (!noDups.Contains(strItem.Trim()))
                {
                    noDups.Add(strItem.Trim());
                }
            }
            return noDups;
        }

        public static string ReadFileToString(string Path)
        {
            System.IO.StreamReader myFile = new System.IO.StreamReader(Path);
            string StrRead = myFile.ReadToEnd();
            myFile.Close();

            return StrRead;
        }
        public static void WriteFileToTxt(string Response, string FileName)
        {
            try
            {
                string Path = @"E://Logs//" + FileName + ".txt";
                System.IO.StreamWriter file = new System.IO.StreamWriter(Path);
                file.WriteLine(Response);
                file.Close();
            }
            catch
            {

            }
        }

        public static string DataTableToStringFormat(DataTable Dt, string ColoumnName, string delim)
        {
            return string.Join(delim, (from DataRow row in Dt.Rows
                                       select (string)row[ColoumnName]).ToArray());
        }

        public static string DataTableToString(DataTable Dt1, string strDataTableName1, DataTable Dt2, string strDataTableName2, string strDataSetname)
        {
            string Request = string.Empty;

            try
            {
                Dt1.TableName = strDataTableName1;
                Dt2.TableName = strDataTableName2;

                DataSet Ds = new DataSet();
                Ds.Tables.Add(Dt1);
                Ds.Tables.Add(Dt2);
                Ds.DataSetName = strDataSetname;

                MemoryStream ms = new MemoryStream();
                Ds.WriteXml(ms);
                XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
                ms = (MemoryStream)xtw.BaseStream;
                UTF8Encoding u8e = new UTF8Encoding();
                Request = u8e.GetString(ms.ToArray());

                return Request;
            }
            catch
            {
                return Request;
            }
        }
        public static string DataTableToString(DataTable Dt, string strDataTableName, string strDataSetname)
        {
            string Request = string.Empty;

            try
            {
                Dt.TableName = strDataTableName;

                DataSet Ds = new DataSet();
                Ds.Tables.Add(Dt);
                Ds.DataSetName = strDataSetname;

                MemoryStream ms = new MemoryStream();
                Ds.WriteXml(ms);
                XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
                ms = (MemoryStream)xtw.BaseStream;
                UTF8Encoding u8e = new UTF8Encoding();
                Request = u8e.GetString(ms.ToArray());

                return Request;
            }
            catch
            {
                return Request;
            }
        }
        public static string DataTableToString(DataTable Dt)
        {
            string Request = string.Empty;

            try
            {
                DataSet Ds = new DataSet();
                Ds.Tables.Add(Dt);

                MemoryStream ms = new MemoryStream();
                Ds.WriteXml(ms);
                XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
                ms = (MemoryStream)xtw.BaseStream;
                UTF8Encoding u8e = new UTF8Encoding();
                Request = u8e.GetString(ms.ToArray());

                return Request;
            }
            catch
            {
                return Request;
            }
        }
        public static string DataTableToString(DataTable Dt1, DataTable Dt2)
        {
            string Request = string.Empty;

            try
            {
                DataSet Ds = new DataSet();
                Ds.Tables.Add(Dt1);
                Ds.Tables.Add(Dt2);

                MemoryStream ms = new MemoryStream();
                Ds.WriteXml(ms);
                XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
                ms = (MemoryStream)xtw.BaseStream;
                UTF8Encoding u8e = new UTF8Encoding();
                Request = u8e.GetString(ms.ToArray());

                return Request;
            }
            catch
            {
                return Request;
            }
        }
        public static string DataSetToString(DataSet Ds, string strDataSetname)
        {
            string Request = string.Empty;

            try
            {
                Ds.DataSetName = strDataSetname;

                MemoryStream ms = new MemoryStream();
                Ds.WriteXml(ms);
                XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
                ms = (MemoryStream)xtw.BaseStream;
                UTF8Encoding u8e = new UTF8Encoding();
                Request = u8e.GetString(ms.ToArray());
                return Request;
            }
            catch
            {
                return Request;
            }
        }
        public static string DataSetToString(DataSet Ds)
        {
            string Request = string.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                Ds.WriteXml(ms);
                XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
                ms = (MemoryStream)xtw.BaseStream;
                UTF8Encoding u8e = new UTF8Encoding();
                Request = u8e.GetString(ms.ToArray());
                return Request;
            }
            catch
            {
                return Request;
            }
        }

        public static DataSet XmlToDataSet(string Response)
        {
            DataSet ResponseDt = new DataSet();
            ResponseDt.ReadXml(new System.IO.StringReader(Response));

            return ResponseDt;
        }
        public static DataSet StringToDataSet(string Request)
        {
            DataSet ds = new DataSet();
            try
            {
                StringReader sr = new StringReader(Request);
                ds.ReadXml(sr);
                return ds;
            }
            catch
            {
                return ds;
            }
        }
        public static DataTable CopyDataDT(DataTable Dt)
        {
            DataTable MainDt = new DataTable();
            MainDt = Dt.Clone();

            foreach (DataRow dr in Dt.Rows)
            {
                MainDt.ImportRow(dr);
            }

            return MainDt;
        }

        public static int SearchDateDiffCurrentDate(string SearchDate)
        {
            try
            {
                SearchDate = SearchDate.Substring(0, 4) + "-" + SearchDate.Substring(4, 2) + "-" + SearchDate.Substring(6, 2);
                DateTime transdate = Convert.ToDateTime(SearchDate);
                TimeSpan t = transdate.Subtract(System.DateTime.Today);
                return Convert.ToInt32(t.TotalDays);
            }
            catch
            {
                return -1;
            }
        }
        public static bool IsNumber(string value)
        {
            Int64 number1;
            return Int64.TryParse(value, out number1);
        }

        public static bool ValidSector(string SectorCode, DataTable AirportDt)
        {
            bool b = false;
            try
            {
                DataView dv = new DataView();
                for (int i = 0; i < AirportDt.Rows.Count; )
                {
                    dv = new DataView(AirportDt, AirportDt.DefaultView.RowFilter = "code = '" + SectorCode + "'", "", DataViewRowState.CurrentRows);
                    break;
                }

                if (dv.Count > 0)
                {
                    if (dv.ToTable() != null)
                    {
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            b = true;
                        }
                    }
                }
            }
            catch
            {

            }
            return b;
        }

        public static DataTable RemoveDupliCate(DataTable tbl, string Coloumn1, string Coloumn2)
        {
            DataColumn[] keyColumns = new DataColumn[] { tbl.Columns[Coloumn1], tbl.Columns[Coloumn2] };

            RemoveDuplicates(tbl, keyColumns);
            return tbl;
        }
        public static void RemoveDuplicates(DataTable tbl, DataColumn[] keyColumns)
        {
            int rowNdx = 0;
            while (rowNdx < tbl.Rows.Count - 1)
            {
                DataRow[] dups = FindDups(tbl, rowNdx, keyColumns);
                if (dups.Length > 0)
                {
                    foreach (DataRow dup in dups)
                    {
                        tbl.Rows.Remove(dup);
                    }
                }
                else
                {
                    rowNdx++;
                }
            }
        }
        private static DataRow[] FindDups(DataTable tbl, int sourceNdx, DataColumn[] keyColumns)
        {
            ArrayList retVal = new ArrayList();
            DataRow sourceRow = tbl.Rows[sourceNdx];
            for (int i = sourceNdx + 1; i < tbl.Rows.Count; i++)
            {
                DataRow targetRow = tbl.Rows[i];
                if (IsDup(sourceRow, targetRow, keyColumns))
                {
                    retVal.Add(targetRow);
                }
            }
            return (DataRow[])retVal.ToArray(typeof(DataRow));
        }
        private static bool IsDup(DataRow sourceRow, DataRow targetRow, DataColumn[] keyColumns)
        {
            bool retVal = true;
            foreach (DataColumn column in keyColumns)
            {
                retVal = retVal && sourceRow[column].Equals(targetRow[column]);
                if (!retVal) break;
            }
            return retVal;
        }
        private static void PrintRows(DataTable tbl)
        {
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                Console.WriteLine("row: {0}, ColumnA: {1}, ColumnB: {2}", i, tbl.Rows[i]["ColumnA"], tbl.Rows[i]["ColumnB"]);
            }
        }

        // REMOVE NAME SPACES       
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        public static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        public static string Serialize(object objectToSerialize)
        {
            try
            {
                MemoryStream mem = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(objectToSerialize.GetType());
                ser.Serialize(mem, objectToSerialize);
                ASCIIEncoding ascii = new ASCIIEncoding();
                return ascii.GetString(mem.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void RemoveColumn(DataTable dtBound, string ColumnName)
        {
            if (dtBound.Columns.Contains(ColumnName))
            {
                dtBound.Columns.Remove(ColumnName);
                dtBound.AcceptChanges();
            }
        }
        public static void AddColumn(DataTable dtBound, string ColumnName, string DefaultValue)
        {
            if (dtBound.Columns.Contains(ColumnName).Equals(false))
            {
                DataColumn col = new DataColumn();
                col.ColumnName = ColumnName;
                col.DataType = typeof(string);
                col.DefaultValue = DefaultValue;
                dtBound.Columns.Add(col);
                dtBound.AcceptChanges();
            }
        }

        public static bool InternationalSector(DataTable dtCountry, string DepartureStation, string ArrivalStation)
        {
            bool bDomestic = true;

            try
            {
                DataView dv = new DataView();
                DataTable FilterDt = new DataTable();
                string Compare = "'India'";
                dv = new DataView(dtCountry, dtCountry.DefaultView.RowFilter = "country IN (" + Compare.Trim() + ")", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    FilterDt.Merge(dv.ToTable());
                }
                DataView dv1 = new DataView(FilterDt, FilterDt.DefaultView.RowFilter = "code IN ('" + DepartureStation.Trim() + "')", "", DataViewRowState.CurrentRows);
                DataView dv2 = new DataView(FilterDt, FilterDt.DefaultView.RowFilter = "code IN ('" + ArrivalStation.Trim() + "')", "", DataViewRowState.CurrentRows);

                if (dv1.Count > 0 && dv2.Count > 0)
                {
                    if (dv1.ToTable() != null && dv2.ToTable() != null)
                    {
                        if (dv1.ToTable().Rows.Count == 0 || dv2.ToTable().Rows.Count == 0)
                        {
                            bDomestic = true;
                        }
                        else if (dv1.ToTable().Rows.Count == 0 && dv2.ToTable().Rows.Count == 0)
                        {
                            bDomestic = true;
                        }
                        else
                        {
                            bDomestic = false;
                        }
                    }
                }
            }
            catch
            {
                bDomestic = true;
            }

            return bDomestic;
        }
        public static void SelectedUpdateRow(DataTable table, string ColumnValue, string UpdateColumnName, string UpdateColumnValue)
        {
            DataRow[] dr = table.Select("FltType='" + ColumnValue + "'"); // finds all rows with id==2 and selects first or null if haven't found any
            if (dr != null)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    dr[i][UpdateColumnName] = UpdateColumnValue; //changes the Product_name
                }
            }

            table.AcceptChanges();
        }
        private void SelectedUpdateRow(DataTable table)
        {
            DataRow dr = table.Select("Product_id=2").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
            if (dr != null)
            {
                dr["Product_name"] = "cde"; //changes the Product_name
            }
        }
        private void SelectedUpdateRow1(DataTable table)
        {
            DataRow row = table.Select("Product_id=2").FirstOrDefault();
            row["Product_name"] = "cde";
        }
        private void SelectedUpdateRow2(DataTable table)
        {
            DataRow[] dr = table.Select("ItemCode =" + "44");
            {
                dr[0]["ItemName"] = "erse";
                dr[0]["ItemName"] = "erse";
                dr[0]["GroupCode"] = "erse";
                dr[0]["Price"] = "erse";
                dr[0]["Descriptions"] = "erse";
            }
        }
        private void SelectedUpdateRow3(DataTable table)
        {
            DataRow[] rows = table.Select("name 'nameValue' AND code = 'codeValue'");
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i]["color"] = "red"; ;
            }
        }
        private void SelectedUpdateRow4(DataTable table)
        {
            DataRow[] rows = table.Select("name 'nameValue' AND code = 'codeValue'");
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i]["color"] = "red"; ;
            }
        }
        private void SelectedUpdateRow5(DataTable table)
        {
            DataRow[] rows = table.Select("UpdateStatus =" + 0);
            if (rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    row["Name "] = "wetwetwe";
                    row["Address "] = "safwds";
                    row["Phone "] = "888888888";
                    row["UpdateStatus "] = "0";
                    table.AcceptChanges();
                    row.SetModified();
                }
            }
        }
        //LOCK METHOD FOR SIMULTANEOUSLY CALL
        public string LockMethod(string MethodName_, string errorMsg_, string Searchtype, string newguid)
        {
            string logpath = string.Empty;

            try
            {
                object lockIndex = new object();
                lock (lockIndex)
                {
                    // code here
                }
            }
            catch (Exception _ex) //if error comes in log writing
            {
                return "Error: Failed to Write Log" + _ex.Message;
            }

            return logpath;
        }
        //public static DataTable ASC_Filter_RowID(DataTable Dt)
        //{
        //    if (Dt != null && Dt.Rows.Count > 0)
        //    {
        //        if (Dt != null && Dt.Columns.Contains("ForSort").Equals(true))
        //        {
        //            DBCommon.CommonFunction.RemoveColumn(Dt, "ForSort");
        //        }

        //        Dt.Columns.Add("ForSort", typeof(int), "Convert(RowID, 'System.Int32')");
        //        DataView dv = new DataView(Dt);
        //        dv.Sort = "ForSort ASC";
        //        Dt = dv.ToTable();
        //        Dt.Columns.Remove("ForSort");
        //    }
        //    return Dt;
        //}
    }
}
