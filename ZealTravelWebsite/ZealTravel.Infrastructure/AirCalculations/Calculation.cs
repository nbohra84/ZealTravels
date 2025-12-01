using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

namespace ZealTravel.Infrastructure.AirCalculations
{
    class Calculation
    {
        public string Calculate_Fare_Commission(bool IsOnline, bool IsDeal, string SearchID, string CompanyID, string AvailabilityResponse)
        {
            string SearchCriteria = string.Empty;
            try
            {
                DataSet dsResponse = Common.CommonFunction.StringToDataSet(AvailabilityResponse);
                if (dsResponse.Tables["AvailabilityInfo"] != null)
                {
                    Calculate_Fare_Commission(IsOnline, IsDeal, SearchID, CompanyID, dsResponse.Tables["AvailabilityInfo"]);
                }

                AvailabilityResponse = Common.CommonFunction.DataSetToString(dsResponse, "root");
            }
            catch (Exception ex)
            {
                //DBCommon.Logger.dbLogg(CompanyID, 0, "Calculate_Fare_Commission", "CAL", SearchCriteria, SearchID, ex.Message);
            }

            return AvailabilityResponse;
        }
        public DataTable Calculate_Fare_Commission(bool IsOnline, bool IsDeal, string SearchID, string CompanyID, DataTable dtBound)
        {
            string SearchCriteria = "";
            try
            {


                short Adt = short.Parse(dtBound.Rows[0]["Adt"].ToString().Trim());
                short Chd = short.Parse(dtBound.Rows[0]["Chd"].ToString().Trim());
                short Inf = short.Parse(dtBound.Rows[0]["Inf"].ToString().Trim());
                string Sector = dtBound.Rows[0]["Sector"].ToString().Trim().ToUpper();
                string AirlineID = dtBound.Rows[0]["AirlineID"].ToString().Trim().ToUpper();
                string Origin = dtBound.Rows[0]["Origin"].ToString().Trim().ToUpper();
                string Destination = dtBound.Rows[0]["Destination"].ToString().Trim().ToUpper();
                string BeginDate = dtBound.Rows[0]["DepDate"].ToString().Trim();
                string EndDate = "";
                if (dtBound.Rows[0]["Trip"].ToString().Trim().Equals("R"))
                {
                    EndDate = dtBound.Rows[dtBound.Rows.Count - 1]["DepDate"].ToString().Trim();
                }
                SearchCriteria = DBCommon.Logger.getSearchQuery(AirlineID, Origin, Destination, BeginDate, EndDate, Adt, Chd, Inf);

                //---------------------------------------------------------------------------------------------------------------------------

                CommissionCalC.Calculate_decimal_to_int_fare(SearchID, CompanyID, SearchCriteria, dtBound);

                //---------------------------------------------------------------------------------------------------------------------------
                bool IsApiUser = false;
                if (CompanyID.Length.Equals(0))
                {
                    CompanyID = dbCommission.getCustomerAdminID(SearchID);
                    if (CompanyID.IndexOf("AD-") != -1)
                    {
                        string UserType = dbCommission.getUserType(CompanyID);
                        Calculate_Fare_Commission_B2C(SearchCriteria, IsOnline, IsDeal, SearchID, CompanyID, dtBound, UserType);
                    }
                    else
                    {
                        string UserType = dbCommission.getUserType(CompanyID);
                        Calculate_Fare_Commission(SearchCriteria, IsOnline, IsDeal, SearchID, CompanyID, dtBound, UserType, IsApiUser);
                    }
                }
                else if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
                {
                    string UserType = dbCommission.getUserType(CompanyID);
                    Calculate_Fare_Commission_B2C(SearchCriteria, IsOnline, IsDeal, SearchID, CompanyID, dtBound, UserType);
                }
                else
                {
                    string UserType = dbCommission.getUserType(CompanyID);
                    if (UserType.Equals("B2B"))
                    {
                        IsApiUser = dbCommission.IsApiAgent(CompanyID);
                    }

                    Calculate_Fare_Commission(SearchCriteria, IsOnline, IsDeal, SearchID, CompanyID, dtBound, UserType, IsApiUser);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "Calculate_Fare_Commission", "CAL", SearchCriteria, SearchID, ex.Message);
            }
            dtBound.AcceptChanges();
            return dtBound;
        }
        private DataTable Calculate_Fare_Commission(string SearchCriteria, bool IsOnline, bool IsDeal, string SearchID, string CompanyID, DataTable dtBound, string UserType, bool IsApiUser)
        {
            try
            {
                string Sector = dtBound.Rows[0]["Sector"].ToString().Trim().ToUpper();
                string Origin = dtBound.Rows[0]["Origin"].ToString().Trim().ToUpper();
                string Destination = dtBound.Rows[0]["Destination"].ToString().Trim().ToUpper();

                bool IsSOTO = false;
                if (Sector.Equals("I"))
                {
                    IsSOTO = dbCommission.IsSotoSector(Origin);
                }

                DataTable dtCommission = dbCommission.CompanyCommissionRule(Sector, DBCommon.CommonFunction.getCompany_by_SubCompany_Customer(CompanyID));
                DataTable dtPriceTypeCommission = dbCommission.CompanyCommissionRule_PriceType(Sector);
                DataTable dtCommission_C = new DataTable();
                DataTable dtCommission_SA = new DataTable();
                if (CompanyID.IndexOf("C-") != -1)
                {
                    dtCommission_C = dbCommission.CompanyCommissionRule_C(Sector, DBCommon.CommonFunction.getCompany_by_SubCompany_Customer(CompanyID));
                }
                if (CompanyID.IndexOf("-SA-") != -1)
                {
                    dtCommission_SA = dbCommission.CompanyCommissionRule_SA(CompanyID, Sector);
                }

                DataTable dtAGmarkup = new DataTable();
                if (CompanyID.IndexOf("-SA-") != -1)
                {
                    dtAGmarkup = dbCommission.AgencyToSubAgencyMarkup(CompanyID, Sector);
                }


                DataTable dtCfee = new DataTable();
                if (CompanyID.IndexOf("C-") != -1 || CompanyID.IndexOf("-SA-") != -1)
                {
                    dtCfee = dbCommission.Cfee(CompanyID, "AIRLINE", Sector);
                }

                DataTable dtTDS = dbCommission.CompanyTDSDetail(DBCommon.CommonFunction.getCompany_by_SubCompany_Customer(CompanyID));
                DataTable dtST = dbCommission.CompanyServiceTaxDetail(DBCommon.CommonFunction.getCompany_by_SubCompany_Customer(CompanyID));
                DataTable dtMU = dbCommission.CompanyMarkupDetail(CompanyID, Sector);
                DataTable dtCountry = new DataTable();
                if (Sector.Equals("I"))
                {
                    dtCountry = dbCommission.CountryCode(Origin, Destination);
                }

                Calculation_all objall = new Calculation_all();
                objall.CalculateDeal(SearchCriteria, IsOnline, IsDeal, dtBound, dtCommission, dtST, dtTDS, dtCountry, dtMU, UserType, dtCommission_C, IsSOTO, dtPriceTypeCommission, dtCfee, dtAGmarkup, IsApiUser);

                CalculatePasseneger(SearchCriteria, dtBound);

                if (CompanyID.IndexOf("-SA-") != -1)
                {
                    objall.getSAdeal(SearchCriteria, CompanyID, dtBound, dtCommission_SA, dtTDS);
                }
                else if (CompanyID.IndexOf("C-") != -1)
                {
                    objall.getCdeal(SearchCriteria, CompanyID, dtBound, dtCommission_C);
                }
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "Calculate_Fare_Commission", "CAL", SearchCriteria, SearchID, ex.Message);
            }
            dtBound.AcceptChanges();
            return dtBound;
        }
        private DataTable Calculate_Fare_Commission_B2C(string SearchCriteria, bool IsOnline, bool IsDeal, string SearchID, string CompanyID, DataTable dtBound, string UserType)
        {
            try
            {
                DataTable dtCommission = dbCommission.CompanyCommissionRule(dtBound.Rows[0]["Sector"].ToString().Trim().ToUpper(), "AD-101");
                DataTable dtST = dbCommission.CompanyServiceTaxDetail("AD-101");
                DataTable dtCfee = dbCommission.Cfee("AD-101", "AIRLINE", dtBound.Rows[0]["Sector"].ToString().Trim().ToUpper());
                DataTable dtPriceTypeCommission = dbCommission.CompanyCommissionRule_PriceType(dtBound.Rows[0]["Sector"].ToString().Trim().ToUpper());

                Calculation_b2c objb2c = new Calculation_b2c();
                objb2c.CalculateDeal(SearchCriteria, IsOnline, IsDeal, dtBound, dtCommission, dtST, dtCfee, UserType, dtPriceTypeCommission);

                CalculatePasseneger(SearchCriteria, dtBound);
            }
            catch (Exception ex)
            {
                DBCommon.Logger.dbLogg(CompanyID, 0, "Calculate_Fare_Commission_B2C", "CAL", SearchCriteria, SearchID, ex.Message);
            }
            dtBound.AcceptChanges();
            return dtBound;
        }
        private void CalculatePasseneger(string SearchCriteria, DataTable dtBound)
        {
            short Adt = short.Parse(dtBound.Rows[0]["Adt"].ToString().Trim());
            short Chd = short.Parse(dtBound.Rows[0]["Chd"].ToString().Trim());
            short Inf = short.Parse(dtBound.Rows[0]["Inf"].ToString().Trim());

            foreach (DataRow dr in dtBound.Rows)
            {
                dr["AdtTotalBasic"] = int.Parse(dr["Adt_BASIC"].ToString());
                dr["AdtTotalTax"] = (int.Parse(dr["Adt_YQ"].ToString()) + int.Parse(dr["Adt_PSF"].ToString()) + int.Parse(dr["Adt_UDF"].ToString()) + int.Parse(dr["Adt_AUDF"].ToString()) + int.Parse(dr["Adt_CUTE"].ToString()) + int.Parse(dr["Adt_GST"].ToString()) + int.Parse(dr["Adt_TF"].ToString()) + int.Parse(dr["Adt_CESS"].ToString()) + int.Parse(dr["Adt_EX"].ToString()));
                dr["AdtTotalFare"] = int.Parse(dr["AdtTotalBasic"].ToString()) + int.Parse(dr["AdtTotalTax"].ToString());

                if (Chd > 0)
                {
                    dr["ChdTotalBasic"] = int.Parse(dr["Chd_BASIC"].ToString());
                    dr["ChdTotalTax"] = (int.Parse(dr["Chd_YQ"].ToString()) + int.Parse(dr["Chd_PSF"].ToString()) + int.Parse(dr["Chd_UDF"].ToString()) + int.Parse(dr["Chd_AUDF"].ToString()) + int.Parse(dr["Chd_CUTE"].ToString()) + int.Parse(dr["Chd_GST"].ToString()) + int.Parse(dr["Chd_TF"].ToString()) + int.Parse(dr["Chd_CESS"].ToString()) + int.Parse(dr["Chd_EX"].ToString()));
                    dr["ChdTotalFare"] = int.Parse(dr["ChdTotalBasic"].ToString()) + int.Parse(dr["ChdTotalTax"].ToString());
                }

                if (Inf > 0)
                {
                    dr["InfTotalBasic"] = int.Parse(dr["Inf_BASIC"].ToString());
                    dr["InfTotalTax"] = int.Parse(dr["Inf_TAX"].ToString());
                    dr["InfTotalFare"] = int.Parse(dr["InfTotalBasic"].ToString()) + int.Parse(dr["InfTotalTax"].ToString());
                }

                dr["TotalServiceTax"] = Adt * Convert.ToDecimal(dr["Adt_ST"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_ST"].ToString());
                dr["TotalServiceFee"] = Adt * Convert.ToDecimal(dr["Adt_SF"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_SF"].ToString());
                dr["TotalMarkup"] = Adt * Convert.ToDecimal(dr["Adt_MU"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_MU"].ToString());
                dr["TotalTds"] = Adt * Convert.ToDecimal(dr["Adt_TDS"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_TDS"].ToString());

                Decimal iTotalCommission = Adt * Convert.ToDecimal(dr["Adt_BAS"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_BAS"].ToString());
                iTotalCommission += Adt * Convert.ToDecimal(dr["Adt_Y"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_Y"].ToString());
                iTotalCommission += Adt * Convert.ToDecimal(dr["Adt_CB"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_CB"].ToString());
                iTotalCommission += Adt * Convert.ToDecimal(dr["Adt_PR"].ToString()) + Chd * Convert.ToDecimal(dr["Chd_PR"].ToString());

                dr["TotalCommission"] = iTotalCommission;

                Int32 iTotalAgmarkup = 0;
                if (dr.Table.Columns.Contains("AG_Markup"))
                {
                    iTotalAgmarkup = Adt * Convert.ToInt32(dr["AG_Markup"].ToString()) + Chd * Convert.ToInt32(dr["AG_Markup"].ToString());
                }

                dr["TotalBasic"] = Adt * (Convert.ToDecimal(dr["AdtTotalBasic"].ToString())) + Chd * (Convert.ToDecimal(dr["ChdTotalBasic"].ToString())) + Inf * (Convert.ToDecimal(dr["InfTotalBasic"].ToString()));
                dr["TotalTax"] = Adt * (Convert.ToDecimal(dr["AdtTotalTax"].ToString())) + Chd * (Convert.ToDecimal(dr["ChdTotalTax"].ToString())) + Inf * (Convert.ToDecimal(dr["InfTotalTax"].ToString()));
                dr["TotalFare"] = Convert.ToDecimal(dr["TotalBasic"].ToString()) + Convert.ToDecimal(dr["TotalTax"].ToString()) + Convert.ToDecimal(dr["TotalServiceTax"].ToString()) + Convert.ToDecimal(dr["TotalServiceFee"].ToString()) + Convert.ToDecimal(dr["TotalMarkup"].ToString()) + Convert.ToDecimal(dr["TotalTds"].ToString()) + iTotalAgmarkup;
            }

            dtBound.AcceptChanges();
        }
        //=======================================================OWN COMMISSION==========================================================================================
    }
}
