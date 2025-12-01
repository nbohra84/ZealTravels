using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.TBO
{
    public class GetDateTimeFormatter
    {
        public static bool IsMoreThan10Hours(string DepartureDate)
        {
            DateTime dt = Convert.ToDateTime(DepartureDate);
            TimeSpan ts = dt.Subtract(DateTime.Now);
            if (ts.TotalHours > 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int BetweenTime(string DepartureDateTime)
        {
            string Date = Convert.ToDateTime(DepartureDateTime).ToString("yyyy-MM-dd");
            //startDate <= dateToCheck && dateToCheck < endDate
            //bool T0301_1130 = false;
            //bool T1131_1500 = false;
            //bool T1501_1900 = false;
            //bool T1901_2300 = false;
            //bool T2301_0300 = false;

            if (Convert.ToDateTime(Date + " " + "03:01") <= Convert.ToDateTime(DepartureDateTime) && Convert.ToDateTime(DepartureDateTime) < Convert.ToDateTime(Date + " " + "11:30"))
            {
                return 1;
            }
            else if (Convert.ToDateTime(Date + " " + "11:31") <= Convert.ToDateTime(DepartureDateTime) && Convert.ToDateTime(DepartureDateTime) < Convert.ToDateTime(Date + " " + "15:00"))
            {
                return 2;
            }
            else if (Convert.ToDateTime(Date + " " + "15:01") <= Convert.ToDateTime(DepartureDateTime) && Convert.ToDateTime(DepartureDateTime) < Convert.ToDateTime(Date + " " + "19:00"))
            {
                return 3;
            }
            else if (Convert.ToDateTime(Date + " " + "19:01") <= Convert.ToDateTime(DepartureDateTime) && Convert.ToDateTime(DepartureDateTime) < Convert.ToDateTime(Date + " " + "23:00"))
            {
                return 4;
            }
            else if (Convert.ToDateTime(Date + " " + "23:01") <= Convert.ToDateTime(DepartureDateTime) && Convert.ToDateTime(DepartureDateTime) < Convert.ToDateTime(Date + " " + "03:00"))
            {
                return 5;
            }

            return 0;
        }
        public static bool Is48Hours(string DepartureDate)
        {
            bool bValid = false;

            DateTime transdate = Convert.ToDateTime(DepartureDate);
            TimeSpan t = transdate.Subtract(System.DateTime.Now);

            if (t.TotalHours >= 48)
            {
                bValid = true;
            }
            else
            {
                bValid = false;
            }
            return bValid;
        }
        public static bool Is24Hours(string DepartureDate)
        {
            bool bValid = false;

            DateTime transdate = Convert.ToDateTime(DepartureDate);
            TimeSpan t = transdate.Subtract(System.DateTime.Now);

            if (t.TotalHours >= 24)
            {
                bValid = true;
            }
            else
            {
                bValid = false;
            }
            return bValid;
        }
        public static bool Is6Hours(string DepartureDate)
        {
            bool bValid = false;

            DateTime transdate = Convert.ToDateTime(DepartureDate);
            TimeSpan t = transdate.Subtract(System.DateTime.Now);

            if (t.TotalHours >= 6)
            {
                bValid = true;
            }
            else
            {
                bValid = false;
            }
            return bValid;
        }
        public static string DateChangeForSearch(string Date)
        {
            string nDate = string.Empty;

            string dd = Date.Substring(6, 2);
            string mm = Date.Substring(4, 2);
            string yy = Date.Substring(0, 4);
            nDate = yy + "-" + mm + "-" + dd;

            return nDate;
        }
        public static string DateChange(string Date)
        {
            return Convert.ToDateTime(Date).ToString("yyyy-MM-ddTHH:mm:ss");
        }
        public static int GeJrnyTime(string DepDateDesc, string DepTimeDesc, string ArrDateDesc, string ArrTimeDesc)
        {
            int TotalMin = 0;
            try
            {
                CultureInfo cultEnUs = new CultureInfo("en-US");
                DateTime DDate = Convert.ToDateTime(DepDateDesc, cultEnUs.DateTimeFormat).Date;
                DateTime ADate = Convert.ToDateTime(ArrDateDesc, cultEnUs.DateTimeFormat).Date;
                DateTime DDateTime = DDate.Add(TimeSpan.Parse(DepTimeDesc));
                DateTime ADateTime = ADate.Add(TimeSpan.Parse(ArrTimeDesc));

                TimeSpan Ts = ADateTime.Subtract(DDateTime);
                TotalMin = (int)Ts.TotalMinutes;
            }
            catch (Exception ex)
            {

            }

            return TotalMin;
        }
        public static string ConvertTmDesc(string JrnyTm)
        {
            int iJrnyTime = 0;

            try
            {
                int.TryParse(JrnyTm, out iJrnyTime);
                TimeSpan ts = TimeSpan.FromMinutes(iJrnyTime);
                int day = ts.Days;
                int hr = ts.Hours;
                int mm = ts.Minutes;
                hr += day * 24;

                string hour = hr.ToString();
                string minute = mm.ToString();

                if (hr.ToString().Length.Equals(1))
                {
                    hour = "0" + hr.ToString();
                }
                if (mm.ToString().Length.Equals(1))
                {
                    minute = "0" + mm.ToString();
                }

                if (hr.ToString().Length.Equals(0))
                {
                    hour = "00";
                }
                if (mm.ToString().Length.Equals(0))
                {
                    minute = "00";
                }

                if (hr > 0)
                {
                    if (hr > 0 && mm > 0)
                    {
                        return hour + "H " + minute + "M "; ;
                    }
                    if (hr > 0 && mm.Equals(0))
                    {
                        return hour + "H " + minute + "M "; ;
                    }
                    else
                    {
                        return hour + "H ";
                    }
                }
                else
                {
                    return minute + "M ";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string GetFltTmDesc(string DepTimeDesc, string ArrTimeDesc)
        {
            string sDuration = "0";

            try
            {
                DateTime D = Convert.ToDateTime(ArrTimeDesc);
                DateTime A = Convert.ToDateTime(DepTimeDesc);

                TimeSpan Duration;

                if (D < A)
                {
                    D = D.AddHours(24);
                    Duration = D - A;
                }
                else
                {
                    Duration = D - A;
                }

                sDuration = Duration.TotalMinutes.ToString();
            }
            catch (Exception ex)
            {

            }

            return sDuration;
        }
        public static Int32 GetDuration(string DepTimeDesc, string ArrTimeDesc)
        {
            Int32 iduration = 0;

            try
            {
                DateTime D = Convert.ToDateTime(ArrTimeDesc);
                DateTime A = Convert.ToDateTime(DepTimeDesc);

                TimeSpan Duration;

                if (D < A)
                {
                    D = D.AddHours(24);
                    Duration = D - A;
                }
                else
                {
                    Duration = D - A;
                }

                iduration = Convert.ToInt32(Duration.TotalMinutes);
            }
            catch (Exception ex)
            {

            }

            return iduration;
        }
        public static string GetDurationDesc(string DepTimeDesc, string ArrTimeDesc)
        {
            string sDuration = "0 Min";

            try
            {
                DateTime D = Convert.ToDateTime(ArrTimeDesc);
                DateTime A = Convert.ToDateTime(DepTimeDesc);

                TimeSpan Duration;

                if (D < A)
                {
                    D = D.AddHours(24);
                    Duration = D - A;
                }
                else
                {
                    Duration = D - A;
                }

                int day = Duration.Days;
                int hr = Duration.Hours;
                int mm = Duration.Minutes;
                hr += day * 24;

                if (hr > 0)
                {
                    sDuration = hr.ToString() + " Hr " + mm.ToString() + " Min";
                }
                else
                {
                    sDuration = mm.ToString() + " Min";
                }
            }
            catch (Exception ex)
            {

            }

            return sDuration;
        }
        public static string DateFormat(string Dt)
        {
            string sDate = Dt;

            try
            {
                DateTime New_Date = DateTime.Now;

                if (Dt.IndexOf("-") != -1 && Dt.IndexOf("T") == -1)
                {
                    New_Date = Convert.ToDateTime(Dt);
                }
                else if (Dt.IndexOf("T") != -1 && Dt.IndexOf("-") != -1 && Dt.IndexOf(",") != -1)
                {
                    New_Date = Convert.ToDateTime(Dt);
                }
                else if (Dt.IndexOf("T") != -1 && Dt.IndexOf("-") != -1)
                {
                    New_Date = Convert.ToDateTime(Dt.Substring(0, Dt.IndexOf("T")));
                }
                else if (Dt.Length == 6)
                {
                    string dd = Dt.Substring(0, 2);
                    string mm = Dt.Substring(2, 2);
                    string yy = Dt.Substring(4, 2);
                    yy = "20" + yy;
                    Dt = yy + "-" + mm + "-" + dd;
                    New_Date = Convert.ToDateTime(Dt);
                }
                else if (Dt.Length == 8)
                {
                    string dd = Dt.Substring(6, 2);
                    string mm = Dt.Substring(4, 2);
                    string yy = Dt.Substring(0, 4);
                    Dt = yy + "-" + mm + "-" + dd;
                    New_Date = Convert.ToDateTime(Dt);
                }
                else
                {
                    New_Date = Convert.ToDateTime(Dt);
                }

                sDate = New_Date.ToString("ddd,dd-MMM-yyyy");
            }
            catch (Exception ex)
            {

            }

            return sDate;
        }
        public static string TimeFormat(string Tm)
        {
            string sReturnTime = Tm;

            try
            {
                string New_Time = string.Empty;
                if (Tm.Length == 1)
                {
                    string hh = "00";
                    string mm = "0" + Tm.Substring(0, 1);
                    New_Time = hh + ":" + mm;
                }
                else if (Tm.Length == 2)
                {
                    string hh = "00";
                    string mm = Tm.Substring(0, 2);
                    New_Time = hh + ":" + mm;
                }
                else if (Tm.Length == 3)
                {
                    string hh = "0" + Tm.Substring(0, 1);
                    string mm = Tm.Substring(1, 2);
                    New_Time = hh + ":" + mm;
                }
                else if (Tm.Length == 4)
                {
                    string hh = Tm.Substring(0, 2);
                    string mm = Tm.Substring(2, 2);
                    New_Time = hh + ":" + mm;
                }
                else if (Tm.Length == 5)
                {
                    New_Time = Tm;
                }
                else if (Tm.Length == 8)
                {
                    New_Time = Tm.Substring(0, 5);
                }
                else if (Tm.IndexOf("T") != -1)
                {
                    New_Time = Tm.Substring(Tm.IndexOf("T") + 1, 5);
                }
                else
                {
                    DateTime dd = Convert.ToDateTime(Tm);
                    New_Time = dd.ToString("HH:mm", CultureInfo.CurrentCulture);
                }

                DateTime DateTimeFormat = Convert.ToDateTime(New_Time);
                sReturnTime = DateTimeFormat.ToString("HH:mm", CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {

            }

            return sReturnTime;
        }
        public static string GetArrivalDateDesc(string sDepDateDesc, string sDepTimeDesc, string sArrTimeDesc)
        {
            string sArrDate = sDepDateDesc;
            try
            {
                DateTime D = Convert.ToDateTime(sArrTimeDesc);
                DateTime A = Convert.ToDateTime(sDepTimeDesc);

                DateTime ArrDate = Convert.ToDateTime(sDepDateDesc);

                if (D < A)
                {
                    ArrDate = ArrDate.AddDays(1);
                    sArrDate = ArrDate.ToString("ddd,dd-MMM-yyyy", CultureInfo.CurrentCulture);
                }
                else
                {
                    sArrDate = ArrDate.ToString("ddd,dd-MMM-yyyy", CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {

            }

            return sArrDate;
        }
        public static string GetArrivalDate(string sArrDateDesc)
        {
            string sArrDate = sArrDateDesc;

            try
            {
                DateTime ArrDate = Convert.ToDateTime(sArrDateDesc);
                sArrDate = ArrDate.ToString("yyyyMMdd");
            }
            catch (Exception ex)
            {

            }

            return sArrDate;
        }
        public static string GetDateDesc(string sDateEightDigit)
        {
            string sReturnDate = sDateEightDigit;
            try
            {
                string dd = sDateEightDigit.Substring(6, 2);
                string mm = sDateEightDigit.Substring(4, 2);
                string yy = sDateEightDigit.Substring(0, 4);
                sDateEightDigit = yy + "-" + mm + "-" + dd;

                DateTime ddd = Convert.ToDateTime(sDateEightDigit);
                sReturnDate = ddd.ToString("ddd,dd-MMM-yyyy", CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {

            }

            return sReturnDate;
        }
        public static string DepDateDB(string DateofDep)
        {
            CultureInfo cultEnUs = new CultureInfo("en-US");
            DateofDep = Convert.ToDateTime(DateofDep, cultEnUs.DateTimeFormat).ToString("yyyyMMdd");
            return DateofDep;
        }
        public static string ConverDate8ToDesc(string sDate)
        {
            if (sDate.Length.Equals(8))
            {
                string month = sDate.Substring(4, 2);
                string date = sDate.Substring(6, 2);
                string year = sDate.Substring(0, 4);

                sDate = Convert.ToDateTime(month + "/" + date + "/" + year).ToString("ddd,dd-MMM-yyyy");
            }

            return sDate;
        }
        public static string DepDateDescTo8(string DateofDep)
        {
            CultureInfo cultEnUs = new CultureInfo("en-US");
            DateofDep = Convert.ToDateTime(DateofDep, cultEnUs.DateTimeFormat).ToString("yyyyMMdd");
            return DateofDep;
        }
    }
}
