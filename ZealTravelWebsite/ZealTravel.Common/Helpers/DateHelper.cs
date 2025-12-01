using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Common.Helpers
{
    public class DateHelper
    {
        public static int DayDiff(string departureDateStr, string arrivalDateStr)
        {
            DateTime departureDate = DateTime.Parse(departureDateStr);
            DateTime arrivalDate = DateTime.Parse(arrivalDateStr);

            return (arrivalDate - departureDate).Days;
        }


        public static string EightDIgit2DateFormat(string date)
        {
            string[] split = date.Split('/');
            string day = split[0].ToString();
            string month = split[1].ToString();
            string year = split[2].ToString();
            date = (year + "-" + month + "-" + day);
            return date;
        }

        public static bool TimeDiffCurrentTime(DateTime DepartureDate, DateTime DepartureTime)
        {
            bool b = true;
            if (DepartureDate == DateTime.Today)
            {
                DateTime DTime = DateTime.Now.AddHours(2.5);
                if (DepartureTime < DTime)
                {
                    b = false;
                }
                else
                {
                    b = true;
                }
            }
            return b;
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
    }
}
