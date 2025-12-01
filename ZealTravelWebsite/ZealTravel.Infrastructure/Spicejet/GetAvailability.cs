using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.Spicejet
{
    class GetAvailability
    {
        private string Companyid;
        private string Searchid;
        private string Supplierid;
        private string Password;
        private string PriceType;
        private string RequestList;

        public GetAvailability(string Supplierid, string Password, string PriceType, string Searchid, string Companyid)
        {
            this.Companyid = Companyid;
            this.Searchid = Searchid;
            this.Supplierid = Supplierid;
            this.Password = Password;
            this.PriceType = PriceType;
        }
        /*
        public string GetOneWayMC(List<AvailabilityRequestVO> _AvailabilityRequestVOList)
        {
            int _index=0;
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataSet dsBound = new DataSet();
            foreach (AvailabilityRequestVO _o in _AvailabilityRequestVOList)
            {
                DataTable dtBound = objthrd.Thread("OW", PriceType, _o.Origin, _o.Destination, _o.BeginDate, "", _o.Adult, _o.Child, _o.Infant, _o.Sector);
                if (dtBound != null && dtBound.Rows.Count > 0)
                {
                    _index++;
                    dtBound.TableName = "AvailabilityInfo" + _index.ToString();

                    dsBound.Tables.Add(dtBound.Copy());

                    GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                    objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);
                }
            }
            if (_index > 0)
            {
                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }
        */
        public string GetOneWay(string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound = objthrd.Thread("OW", PriceType, Origin, Destination, BeginDate, "", Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());

                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }
        public async Task<string> GetOneWayAsync(string Origin, string Destination, string BeginDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound =await objthrd.ThreadAsync("OW", PriceType, Origin, Destination, BeginDate, "", Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());

                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }
        public string GetDomesticRoundWay(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound = objthrd.Thread("RW", PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());

                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }
        public async Task<string> GetDomesticRoundWayAsync(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound =await objthrd.ThreadAsync("RW", PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());

                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetDomesticRT(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound = objthrd.Thread("DRT", PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());


                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }
        public async Task<string> GetDomesticRTAsync(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound =await objthrd.ThreadAsync("DRT", PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());


                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetInternationalRT(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound = objthrd.Thread("IRT", PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());

                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }
        public async Task<string> GetInternationalRTAsync(string Origin, string Destination, string BeginDate, string EndDate, int Adt, int Chd, int Inf, string Sector)
        {
            GetAvailabilityThread objthrd = new GetAvailabilityThread(Searchid, Companyid, Supplierid, Password);
            DataTable dtBound =await objthrd.ThreadAsync("IRT", PriceType, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf, Sector);

            if (dtBound != null && dtBound.Rows.Count > 0)
            {
                dtBound.TableName = "AvailabilityInfo";
                DataSet dsBound = new DataSet();
                dsBound.Tables.Add(dtBound.Copy());

                GetCommonFunctions objCommonFunctions = new GetCommonFunctions();
                objCommonFunctions.Add_Journey_Duration_TimeDetail(dsBound);

                dsBound.DataSetName = "root";
                return GetCommonFunctions.DataSetToString(dsBound);
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
