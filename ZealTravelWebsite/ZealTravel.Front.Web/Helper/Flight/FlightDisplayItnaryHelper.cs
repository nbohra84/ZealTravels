using System.Data;
using System.Text;
using ZealTravel.Common.Helpers;

namespace ZealTravel.Front.Web.Helper.Flight
{
    public class FlightDisplayItnaryHelper
    {
        
        public static StringBuilder Itinary(DataTable dtItinary, out StringBuilder sbHtmlFare, out int totalFare,  bool IsPaymentPage)
        {
            return Itinary(dtItinary, out sbHtmlFare, out totalFare,  IsPaymentPage, false);
        }
        public static StringBuilder Itinary(DataTable dtItinary, out StringBuilder sbHtmlFare, out int totalFare,  bool IsPaymentPage, bool _IsMultiCity)
        {


            StringBuilder Itinary = new StringBuilder();
            sbHtmlFare = new StringBuilder();
            totalFare = 0;

             StringBuilder sbHtmlFareBasic = new StringBuilder();
            StringBuilder sbHtmlFareTax = new StringBuilder();
            StringBuilder sbHtmlFareTaxOther = new StringBuilder();
            int Adt = 0;
            int Chd = 0;
            int Inf = 0;

            int AdtTotalBasic = 0;
            int ChdTotalBasic = 0;
            int InfTotakBasic = 0;

            int AdtTotalTax = 0;
            int ChdTotalTax = 0;
            int InfTotakTax = 0;

            int AdtSTPlusSF = 0;
            int ChdSTPlusSF = 0;

            int AdtTotalTaxCharge = 0;
            int ChdTotalTaxCharge = 0;
            int InfTotakTaxCharge = 0;

            int AllBasicFare = 0;
            int AllTaxFare = 0;
            int AllOtherFare = 0;


            int TotalGST = 0;

            int outb = 0;
            int inb = 0;

            int totaltds = 0;
            int totalcomm = 0;

            string Currencytype = string.Empty;
            var companyID = UserHelper.GetCompanyID(HttpContextHelper.Current?.User);
            if (HttpContextHelper.Current.Session.GetString("Curr") != null)
            {
                Currencytype = HttpContextHelper.Current.Session.GetString("Curr");
            }
            else
            {
                Currencytype = "INR";
            }

            if (dtItinary != null && dtItinary.Rows.Count > 0)
            {
                var TripCode_ = string.Empty;

                for (int i = 0; i < dtItinary.Rows.Count; i++)
                {
                    //==========1 start======
                    Itinary.Append("<div class='detailboxre'>");

                    //==========2 start======
                    Itinary.Append("<div class='detailhadsec'>");

                    //==========3 start======
                    Itinary.Append("<div class='leftdetabx'>");

                    Itinary.Append("<span class='detabx'>");
                    Itinary.Append("<p class='peradv1'>");
                    if (dtItinary.Rows[i]["FltType"].ToString().Trim().ToUpper().Equals("O") && _IsMultiCity == false)
                    {


                        Itinary.Append("<span class='depatdvi'>DEPART </span>");
                        if (outb.Equals(0))
                        {

                            if (companyID.IndexOf("-SA-") != -1 || companyID.IndexOf("C-") != -1 || companyID.Equals(string.Empty))
                            {
                                totaltds += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalTds_SA"].ToString().Trim()));
                                totalcomm += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalCommission_SA"].ToString().Trim()));
                            }
                            else
                            {
                                totaltds += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalTds"].ToString().Trim()));
                                totalcomm += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalCommission"].ToString().Trim()));
                            }

                            Adt = Convert.ToInt32(dtItinary.Rows[i]["Adt"].ToString().Trim());
                            Chd = Convert.ToInt32(dtItinary.Rows[i]["Chd"].ToString().Trim());
                            Inf = Convert.ToInt32(dtItinary.Rows[i]["Inf"].ToString().Trim());

                            AdtSTPlusSF = (ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_ST"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_SF"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_MU"].ToString().Trim())));
                            AdtTotalTaxCharge = AdtSTPlusSF * Adt;

                            AdtTotalTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalTax"].ToString().Trim())) * Adt;
                            AdtTotalBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalBasic"].ToString().Trim())) * Adt;

                            sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                            sbHtmlFareBasic.Append("<span class='adtfare'>Adult(s) (" + Adt + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalBasic"].ToString().Trim())) + ")</span>");
                            sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalBasic + "</span>");
                            sbHtmlFareBasic.Append("</div>");

                            sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                            sbHtmlFareTax.Append("<span class='adtfare'>Adult(s) (" + Adt + ")</span>");
                            sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalTax + "</span>");
                            sbHtmlFareTax.Append("</div>");

                            sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                            sbHtmlFareTaxOther.Append("<span class='adtfare'>Adult(s) (" + Adt + ")</span>");
                            sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalTaxCharge + "</span>");
                            sbHtmlFareTaxOther.Append("</div>");

                            //TotalFare = Convert.ToInt32(dtItinary.Rows[i]["TotalFare"].ToString().Trim()) - Convert.ToInt32(dtItinary.Rows[i]["TotalTds"].ToString().Trim());
                            totalFare = ShowFlightDataHelper.ReturnFinalFare(dtItinary.Rows[i], companyID);

                            TotalGST = Adt * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_GST"].ToString().Trim())) + Chd * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_GST"].ToString().Trim()));

                            if (Convert.ToInt32(dtItinary.Rows[i]["Chd"].ToString().Trim()) > 0)
                            {
                                ChdSTPlusSF = (ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_ST"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_SF"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_MU"].ToString().Trim())));
                                ChdTotalTaxCharge = ChdSTPlusSF * Chd;

                                ChdTotalTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalTax"].ToString().Trim())) * Chd;
                                ChdTotalBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalBasic"].ToString().Trim())) * Chd;

                                sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                                sbHtmlFareBasic.Append("<span class='adtfare'>Children (" + Chd + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalBasic"].ToString().Trim())) + ")</span>");
                                sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalBasic + "</span>");
                                sbHtmlFareBasic.Append("</div>");

                                sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTax.Append("<span class='adtfare'>Children (" + Chd + ")</span>");
                                sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalTax + "</span>");
                                sbHtmlFareTax.Append("</div>");

                                sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTaxOther.Append("<span class='adtfare'>Children (" + Chd + ")</span>");
                                sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalTaxCharge + "</span>");
                                sbHtmlFareTaxOther.Append("</div>");
                            }
                            if (Convert.ToInt32(dtItinary.Rows[i]["Inf"].ToString().Trim()) > 0)
                            {
                                InfTotakTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalTax"].ToString().Trim())) * Inf;
                                InfTotakBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalBasic"].ToString().Trim())) * Inf;

                                sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                                sbHtmlFareBasic.Append("<span class='adtfare'>Infant (" + Inf + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalBasic"].ToString().Trim())) + ")</span>");
                                sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakBasic + "</span>");
                                sbHtmlFareBasic.Append("</div>");

                                sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTax.Append("<span class='adtfare'>Infant (" + Inf + ") </span>");
                                sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakTax + "</span>");
                                sbHtmlFareTax.Append("</div>");

                                sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTaxOther.Append("<span class='adtfare'>Infant (" + Inf + ")</span>");
                                sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakTaxCharge + "</span>");
                                sbHtmlFareTaxOther.Append("</div>");

                            }

                            AllBasicFare = AdtTotalBasic + ChdTotalBasic + InfTotakBasic;
                            AllTaxFare = AdtTotalTax + ChdTotalTax + InfTotakTax;
                            AllOtherFare = AdtTotalTaxCharge + ChdTotalTaxCharge + InfTotakTaxCharge;

                            outb++;
                        }

                    }
                    else if (dtItinary.Rows[i]["FltType"].ToString().Trim().ToUpper().Equals("O") && _IsMultiCity == true)
                    {

                        Itinary.Append("<span class='depatdvi'>DEPART </span>");
                        //if (outb.Equals(0))
                        if (TripCode_ != (dtItinary.Rows[i]["Origin"].ToString().Trim().ToUpper() + dtItinary.Rows[i]["Destination"].ToString().Trim().ToUpper())
                            || TripCode_ == "")
                        {
                            TripCode_ = (dtItinary.Rows[i]["Origin"].ToString().Trim().ToUpper() + dtItinary.Rows[i]["Destination"].ToString().Trim().ToUpper());


                            if (companyID.IndexOf("-SA-") != -1 || companyID.IndexOf("C-") != -1 || companyID.Equals(string.Empty))
                            {
                                totaltds += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalTds_SA"].ToString().Trim()));
                                totalcomm += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalCommission_SA"].ToString().Trim()));
                            }
                            else
                            {
                                totaltds += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalTds"].ToString().Trim()));
                                totalcomm += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalCommission"].ToString().Trim()));
                            }

                            Adt = Convert.ToInt32(dtItinary.Rows[i]["Adt"].ToString().Trim());
                            Chd = Convert.ToInt32(dtItinary.Rows[i]["Chd"].ToString().Trim());
                            Inf = Convert.ToInt32(dtItinary.Rows[i]["Inf"].ToString().Trim());

                            AdtSTPlusSF = (ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_ST"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_SF"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_MU"].ToString().Trim())));
                            AdtTotalTaxCharge = AdtSTPlusSF * Adt;

                            AdtTotalTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalTax"].ToString().Trim())) * Adt;
                            AdtTotalBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalBasic"].ToString().Trim())) * Adt;

                            sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                            sbHtmlFareBasic.Append("<span class='adtfare'>Adult(s) (" + Adt + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalBasic"].ToString().Trim())) + ")</span>");
                            sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalBasic + "</span>");
                            sbHtmlFareBasic.Append("</div>");

                            sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                            sbHtmlFareTax.Append("<span class='adtfare'>Adult(s) (" + Adt + ")</span>");
                            sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalTax + "</span>");
                            sbHtmlFareTax.Append("</div>");

                            sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                            sbHtmlFareTaxOther.Append("<span class='adtfare'>Adult(s) (" + Adt + ")</span>");
                            sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalTaxCharge + "</span>");
                            sbHtmlFareTaxOther.Append("</div>");

                            //TotalFare = Convert.ToInt32(dtItinary.Rows[i]["TotalFare"].ToString().Trim()) - Convert.ToInt32(dtItinary.Rows[i]["TotalTds"].ToString().Trim());   // remove commented 7Sep2023
                            totalFare += ShowFlightDataHelper.ReturnFinalFare(dtItinary.Rows[i], companyID);    // dont comment this 7Sep2023

                            //TotalFare += Convert.ToInt32(dtItinary.Rows[i]["TotalFare"].ToString().Trim()); // if you  uncomment this some time it givin error

                            TotalGST += Adt * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_GST"].ToString().Trim())) + Chd * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_GST"].ToString().Trim()));

                            if (Convert.ToInt32(dtItinary.Rows[i]["Chd"].ToString().Trim()) > 0)
                            {
                                ChdSTPlusSF = (ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_ST"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_SF"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_MU"].ToString().Trim())));
                                ChdTotalTaxCharge = ChdSTPlusSF * Chd;

                                ChdTotalTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalTax"].ToString().Trim())) * Chd;
                                ChdTotalBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalBasic"].ToString().Trim())) * Chd;

                                sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                                sbHtmlFareBasic.Append("<span class='adtfare'>Children (" + Chd + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalBasic"].ToString().Trim())) + ")</span>");
                                sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalBasic + "</span>");
                                sbHtmlFareBasic.Append("</div>");

                                sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTax.Append("<span class='adtfare'>Children (" + Chd + ")</span>");
                                sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalTax + "</span>");
                                sbHtmlFareTax.Append("</div>");

                                sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTaxOther.Append("<span class='adtfare'>Children (" + Chd + ")</span>");
                                sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalTaxCharge + "</span>");
                                sbHtmlFareTaxOther.Append("</div>");
                            }
                            if (Convert.ToInt32(dtItinary.Rows[i]["Inf"].ToString().Trim()) > 0)
                            {
                                InfTotakTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalTax"].ToString().Trim())) * Inf;
                                InfTotakBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalBasic"].ToString().Trim())) * Inf;

                                sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                                sbHtmlFareBasic.Append("<span class='adtfare'>Infant (" + Inf + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalBasic"].ToString().Trim())) + ")</span>");
                                sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakBasic + "</span>");
                                sbHtmlFareBasic.Append("</div>");

                                sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTax.Append("<span class='adtfare'>Infant (" + Inf + ") </span>");
                                sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakTax + "</span>");
                                sbHtmlFareTax.Append("</div>");

                                sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTaxOther.Append("<span class='adtfare'>Infant (" + Inf + ")</span>");
                                sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakTaxCharge + "</span>");
                                sbHtmlFareTaxOther.Append("</div>");

                            }

                            AllBasicFare += (AdtTotalBasic + ChdTotalBasic + InfTotakBasic);
                            AllTaxFare += (AdtTotalTax + ChdTotalTax + InfTotakTax);
                            AllOtherFare += (AdtTotalTaxCharge + ChdTotalTaxCharge + InfTotakTaxCharge);

                            outb++;
                        }

                    }
                    else
                    {
                        Itinary.Append("<span class='depatdvi'>RETURN </span>");
                        if (inb.Equals(0) && HttpContextHelper.Current.Session.GetString("SearchValue").ToString().Equals("RW"))
                        {
                            if (companyID.IndexOf("-SA-") != -1 || companyID.IndexOf("C-") != -1 || companyID.Equals(string.Empty))
                            {
                                totaltds += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalTds_SA"].ToString().Trim()));
                                totalcomm += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalCommission_SA"].ToString().Trim()));
                            }
                            else
                            {
                                totaltds += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalTds"].ToString().Trim()));
                                totalcomm += ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["TotalCommission"].ToString().Trim()));
                            }

                            Adt = Convert.ToInt32(dtItinary.Rows[i]["Adt"].ToString().Trim());
                            Chd = Convert.ToInt32(dtItinary.Rows[i]["Chd"].ToString().Trim());
                            Inf = Convert.ToInt32(dtItinary.Rows[i]["Inf"].ToString().Trim());

                            AdtSTPlusSF = (ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_ST"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_SF"].ToString().Trim())) + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_MU"].ToString().Trim())));
                            AdtTotalTaxCharge = AdtSTPlusSF * Adt;

                            AdtTotalTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalTax"].ToString().Trim())) * Adt;
                            AdtTotalBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalBasic"].ToString().Trim())) * Adt;

                            sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                            sbHtmlFareBasic.Append("<span class='adtfare'>Adult(s) (" + Adt + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["AdtTotalBasic"].ToString().Trim())) + ")</span>");
                            sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalBasic + "</span>");
                            sbHtmlFareBasic.Append("</div>");

                            sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                            sbHtmlFareTax.Append("<span class='adtfare'>Adult(s) (" + Adt + ")</span>");
                            sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalTax + "</span>");
                            sbHtmlFareTax.Append("</div>");

                            sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                            sbHtmlFareTaxOther.Append("<span class='adtfare'>Adult(s) (" + Adt + ")</span>");
                            sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + AdtTotalTaxCharge + "</span>");
                            sbHtmlFareTaxOther.Append("</div>");

                            if (Convert.ToInt32(dtItinary.Rows[i]["Chd"].ToString().Trim()) > 0)
                            {
                                ChdSTPlusSF = (Convert.ToInt32(dtItinary.Rows[i]["Chd_ST"].ToString().Trim()) + Convert.ToInt32(dtItinary.Rows[i]["Chd_SF"].ToString().Trim()) + Convert.ToInt32(dtItinary.Rows[i]["Chd_MU"].ToString().Trim()));
                                ChdTotalTaxCharge = ChdSTPlusSF * Chd;

                                ChdTotalTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalTax"].ToString().Trim())) * Chd;
                                ChdTotalBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalBasic"].ToString().Trim())) * Chd;

                                sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                                sbHtmlFareBasic.Append("<span class='adtfare'>Children (" + Chd + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["ChdTotalBasic"].ToString().Trim())) + ")</span>");
                                sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalBasic + "</span>");
                                sbHtmlFareBasic.Append("</div>");

                                sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTax.Append("<span class='adtfare'>Children (" + Chd + ")</span>");
                                sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalTax + "</span>");
                                sbHtmlFareTax.Append("</div>");

                                sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTaxOther.Append("<span class='adtfare'>Children (" + Chd + ")</span>");
                                sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + ChdTotalTaxCharge + "</span>");
                                sbHtmlFareTaxOther.Append("</div>");
                            }
                            if (Convert.ToInt32(dtItinary.Rows[i]["Inf"].ToString().Trim()) > 0)
                            {
                                InfTotakTax = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalTax"].ToString().Trim())) * Inf;
                                InfTotakBasic = ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalBasic"].ToString().Trim())) * Inf;

                                sbHtmlFareBasic.Append("<div class='divsecadtbx'>");
                                sbHtmlFareBasic.Append("<span class='adtfare'>Infant (" + Inf + " X " + Currencytype + " " + ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["InfTotalBasic"].ToString().Trim())) + ")</span>");
                                sbHtmlFareBasic.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakBasic + "</span>");
                                sbHtmlFareBasic.Append("</div>");

                                sbHtmlFareTax.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTax.Append("<span class='adtfare'>Infant (" + Inf + ") </span>");
                                sbHtmlFareTax.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakTax + "</span>");
                                sbHtmlFareTax.Append("</div>");

                                sbHtmlFareTaxOther.Append("<div class='divsecadtbx'>");
                                sbHtmlFareTaxOther.Append("<span class='adtfare'>Infant (" + Inf + ")</span>");
                                sbHtmlFareTaxOther.Append("<span class='adtfarbx'>" + Currencytype + " " + InfTotakTaxCharge + "</span>");
                                sbHtmlFareTaxOther.Append("</div>");

                            }

                            AllBasicFare = AllBasicFare + AdtTotalBasic + ChdTotalBasic + InfTotakBasic;
                            AllTaxFare = AllTaxFare + AdtTotalTax + ChdTotalTax + InfTotakTax;
                            AllOtherFare = AllOtherFare + AdtTotalTaxCharge + ChdTotalTaxCharge + InfTotakTaxCharge;


                            //TotalFare = TotalFare + (Convert.ToInt32(dtItinary.Rows[i]["TotalFare"].ToString().Trim()) - Convert.ToInt32(dtItinary.Rows[i]["TotalTds"].ToString().Trim()));
                            totalFare = totalFare + ShowFlightDataHelper.ReturnFinalFare(dtItinary.Rows[i], companyID);

                            TotalGST = TotalGST + Adt * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Adt_GST"].ToString().Trim())) + Chd * ShowFlightDataHelper.GetConvert(Convert.ToInt32(dtItinary.Rows[i]["Chd_GST"].ToString().Trim()));

                            inb++;
                        }
                    }

                    Itinary.Append("<span class='destconde'>" + dtItinary.Rows[i]["DepartureStation"].ToString() + "-" + dtItinary.Rows[i]["ArrivalStation"].ToString() + " </span>");
                    Itinary.Append("</p>");
                    Itinary.Append("</span>");
                    string stops = string.Empty;
                    if (Convert.ToInt32(dtItinary.Rows[i]["Stops"].ToString()) == 0)
                    {
                        stops = "Non stop";
                    }
                    else if (Convert.ToInt32(dtItinary.Rows[i]["Stops"].ToString()) == 1)
                    {
                        stops = "1 stop";
                    }
                    else
                    {
                        stops = "2 stop";
                    }
                    string CabInBaggage = string.Empty;
                    string CheckInBaggage = string.Empty;
                    if (dtItinary.Rows[i]["BaggageDetail"].ToString().IndexOf("*") != -1)
                    {
                        CheckInBaggage = dtItinary.Rows[i]["BaggageDetail"].ToString().Split('*')[0].ToString();
                        if (dtItinary.Rows[i]["BaggageDetail"].ToString().Split('*')[1].ToString().IndexOf("0") != -1)
                        {

                        }
                        else
                        {
                            CabInBaggage = dtItinary.Rows[i]["BaggageDetail"].ToString().Split('*')[1].ToString();
                        }
                    }

                    string DepTerminal = string.Empty;
                    if (dtItinary.Rows[i]["DepartureTerminal"].ToString() != "")
                    {
                        DepTerminal = "Terminal " + dtItinary.Rows[i]["DepartureTerminal"].ToString();
                    }
                    string ArrTerminal = string.Empty;
                    if (dtItinary.Columns.Contains("ArrivalTerminal") && dtItinary.Rows[i]["ArrivalTerminal"].ToString() != "")
                    {
                        ArrTerminal = "Terminal " + dtItinary.Rows[i]["ArrivalTerminal"].ToString();
                    }
                    string RefundType = string.Empty;
                    if (dtItinary.Rows[i]["RefundType"].ToString().Trim().ToUpper() == "N")
                    {
                        RefundType = "Refundable";
                    }
                    else
                    {
                        RefundType = "Non-Refundable";
                    }
                    Itinary.Append("<span class='detabx1'>");
                    Itinary.Append("<p class='peradv2'>");
                    Itinary.Append("<span>" + dtItinary.Rows[i]["DepartureDate"].ToString() + "</span>");
                    //Itinary.Append("<span>| " + stops + " </span>");
                    Itinary.Append("<span>| " + dtItinary.Rows[i]["DurationDesc"].ToString() + " </span>");
                    Itinary.Append("<span>| " + dtItinary.Rows[i]["Cabin"].ToString() + "(" + dtItinary.Rows[i]["ClassOfService"].ToString() + ")</span>");
                    Itinary.Append("</p>");
                    Itinary.Append("</span>");

                    Itinary.Append("</div>");
                    //==========3 End======

                    //==========4 start======
                    Itinary.Append("<div class='righfdebox'>");

                    Itinary.Append("<div class='maindbagdvbx pull-left'>");
                    Itinary.Append("<div class='bagglad1 pull-left'> Baggage</div>");
                    Itinary.Append("<div class='bagglad2 pull-left'> </div>");
                    Itinary.Append("<div class='bagglad2 pull-left'> </div>");
                    Itinary.Append("</div>");

                    Itinary.Append("<div class='maindbagdvbx1 pull-left'>");
                    Itinary.Append("<div class='bagglad1 pull-left'>  " + CheckInBaggage + " </div>");
                    Itinary.Append("<div class='bagglad2 pull-left'></div>");
                    Itinary.Append("<div class='bagglad2 pull-left'>  </div>");
                    Itinary.Append("</div>");

                    Itinary.Append("</div>");
                    //==========4 end======

                    Itinary.Append("</div>");
                    //==========2 end====== 

                    //==========5 start======
                    Itinary.Append("<div class='itindvbxsec clearfix'>");
                    //==========6 start======
                    Itinary.Append("<div class='airlisdetimg0 pull-left'>");

                    Itinary.Append("<div class='airlogo pull-left'>");
                    Itinary.Append("<img src='/assets/img/airlogo_square/" + dtItinary.Rows[i]["CarrierCode"].ToString().Trim() + ".gif' style='height: 30px; width: 30px;' />");
                    Itinary.Append("</div>");

                    Itinary.Append("<div class='airlisdt pull-left'>");
                    Itinary.Append("<p class='airdtname'> " + dtItinary.Rows[i]["CarrierName"].ToString() + "</p>");
                    Itinary.Append("<p class='airdtcode'>" + dtItinary.Rows[i]["CarrierCode"].ToString().Trim() + "-" + dtItinary.Rows[i]["FlightNumber"].ToString() + "</p>");
                    Itinary.Append("</div>");

                    Itinary.Append("</div>");
                    //==========6 end======

                    //==========7 start======
                    Itinary.Append("<div class='inbondbx pull-left'>");

                    Itinary.Append("<div class='detaairbox1'>");
                    Itinary.Append("<div class='timesecdv'> " + dtItinary.Rows[i]["DepartureTime"].ToString() + "</div>");
                    Itinary.Append("<div class='detset'>" + dtItinary.Rows[i]["DepartureDate"].ToString() + " </div>");
                    Itinary.Append("<div class='citycondv'> " + dtItinary.Rows[i]["DepartureStationName"].ToString() + " <span class='bsdse'>" + DepTerminal + "</span> </div>");
                    // Itinary.Append("<div class='terstationdv'>" + DepTerminal + "</div>");
                    Itinary.Append("</div>");

                    Itinary.Append("</div>");
                    //==========7 end======

                    Itinary.Append("<div class='layoverdgt pull-left'>" + dtItinary.Rows[i]["DurationDesc"].ToString() + " </div>");

                    //==========8 start======
                    Itinary.Append("<div class='outbonbx pull-left'>");

                    Itinary.Append("<div class='detaairbox1'>");
                    Itinary.Append("<div class='timesecdv'>" + dtItinary.Rows[i]["ArrivalTime"].ToString() + "</div>");
                    Itinary.Append("<div class='detset'>" + dtItinary.Rows[i]["ArrivalDate"].ToString() + "</div>");
                    Itinary.Append("<div class='citycondv'>" + dtItinary.Rows[i]["ArrivalStationName"].ToString() + " <span class='bsdse1'>" + ArrTerminal + "</span> </div>");
                    // Itinary.Append("<div class='terstationdv'>" + ArrTerminal + "</div>");
                    Itinary.Append("</div>");

                    Itinary.Append("</div>");
                    //==========8 end======

                    //==========9 start======
                    Itinary.Append("<div class='refundvbx pull-left'>");
                    Itinary.Append("<div class='refundnonbx pull-left'>" + RefundType + "</div>");
                    Itinary.Append("</div>");
                    //==========9 end======

                    Itinary.Append("</div>");
                    //==========5 end======
                    Itinary.Append("</div>");
                    //==========1 end======


                    dtItinary.Rows[i]["Origin"].ToString();
                    dtItinary.Rows[i]["Destination"].ToString();
                    dtItinary.Rows[i]["DepartureStationAirport"].ToString();
                    dtItinary.Rows[i]["ArrivalStationAirport"].ToString();
                    dtItinary.Rows[i]["SeatsAvailable"].ToString();


                }


                #region  Fare Detail             

                sbHtmlFare.Append("<div class='faresummaindv'>");
                //==========Basic Fare start======
                sbHtmlFare.Append("<div class='mainfarsecbx'>");
                sbHtmlFare.Append("<div class='mainfarhadsecbx' onclick='basefare()'>");
                sbHtmlFare.Append("<span class='mainhded'>");
                sbHtmlFare.Append("<i class='fa fa-plus-square-o bullstdx mandvplmns' id='plusebase' aria-hidden='true' ></i>");
                sbHtmlFare.Append("<i class='fa fa-minus-square-o bullstdx mandvplmns' id='minusebase' aria-hidden='true' style='display: none;'  ></i> Base Fare");
                sbHtmlFare.Append("</span>");
                sbHtmlFare.Append("<span class='inrserv' id='bsfar'>" + Currencytype + " " + AllBasicFare + "</span>");
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("<div class='mainidsecbx' id='basefarewr' style='display: none;'>");
                sbHtmlFare.Append(sbHtmlFareBasic.ToString());
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("</div>");

                //==========Basic Fare End======

                //==========Tax Fare start======
                sbHtmlFare.Append("<div class='mainfarsecbx'>");
                sbHtmlFare.Append("<div class='mainfarhadsecbx' onclick='feesurch()'>");
                sbHtmlFare.Append("<span class='mainhded'>");
                sbHtmlFare.Append("<i class='fa fa-plus-square-o bullstdx mandvplmns' id='plusefee' aria-hidden='true'></i>");
                sbHtmlFare.Append("<i class='fa fa-minus-square-o bullstdx mandvplmns' id='minusefee' aria-hidden='true' style='display: none;'></i>Taxes & Charges");
                sbHtmlFare.Append("</span>");
                sbHtmlFare.Append("<span class='inrserv' id='feesurcha'>" + Currencytype + " " + AllTaxFare + "</span>");
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("<div class='mainidsecbx' id='feesurcharge' style='display: none;'>");
                sbHtmlFare.Append(sbHtmlFareTax.ToString());


                sbHtmlFare.Append("<div style='border-top:1px solid #ccc;'>");
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("<div class='divsecadtbx'>");
                sbHtmlFare.Append("<span class='adtfare'>Total GST:</span>");
                sbHtmlFare.Append("<span class='adtfarbx'>" + Currencytype + " " + TotalGST + "</span>");
                sbHtmlFare.Append("</div>");

                sbHtmlFare.Append("<div class='divsecadtbx'>");
                sbHtmlFare.Append("<span class='adtfare'>Total fee & surcharges:</span>");
                sbHtmlFare.Append("<span class='adtfarbx'>" + Currencytype + " " + AllTaxFare + "</span>");
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("</div>");
                //==========Tax Fare End======

                //==========Other Fare start======
                sbHtmlFare.Append("<div class='mainfarsecbx'>");
                sbHtmlFare.Append("<div class='mainfarhadsecbx' onclick='otherser()'>");
                sbHtmlFare.Append("<span class='mainhded'>");
                sbHtmlFare.Append("<i class='fa fa-plus-square-o bullstdx mandvplmns' id='pluseoth' aria-hidden='true'></i>");
                sbHtmlFare.Append("<i class='fa fa-minus-square-o bullstdx mandvplmns' id='minuseoth' aria-hidden='true' style='display: none;'></i>Other Charges");
                sbHtmlFare.Append("</span>");
                sbHtmlFare.Append("<span class='inrserv' id='otherserv'>" + Currencytype + " " + AllOtherFare + "</span>");
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("<div class='mainidsecbx' id='otherservice' style='display: none;'>");
                sbHtmlFare.Append(sbHtmlFareTaxOther.ToString());
                sbHtmlFare.Append("</div>");
                sbHtmlFare.Append("</div>");
                //==========Other Fare End======

                if (!IsPaymentPage)
                {
                    //==========Total Commission start======
                    sbHtmlFare.Append("<div class='mainfarsecbx'>");
                    sbHtmlFare.Append("<div class='divsecadtbx'>");
                    sbHtmlFare.Append("<span class='totalamtbx'>Total Commission:</span>");
                    sbHtmlFare.Append("<span class='totalamtbx'>" + Currencytype + " " + totalcomm + "</span>");
                    sbHtmlFare.Append("</div>");
                    sbHtmlFare.Append("</div>");
                    //==========Total Fare End======

                    //==========Total Tds start======
                    sbHtmlFare.Append("<div class='mainfarsecbx'>");
                    sbHtmlFare.Append("<div class='divsecadtbx'>");
                    sbHtmlFare.Append("<span class='totalamtbx'>Total Tds:</span>");
                    sbHtmlFare.Append("<span class='totalamtbx'>" + Currencytype + " " + totaltds + "</span>");
                    sbHtmlFare.Append("</div>");
                    sbHtmlFare.Append("</div>");
                    //==========Total Fare End======

                    //==========Total Fare start======
                    sbHtmlFare.Append("<div class='mainfarsecbx'>");
                    sbHtmlFare.Append("<div class='divsecadtbx'>");
                    sbHtmlFare.Append("<span class='totalamtbx'>Net Amount:</span>");
                    sbHtmlFare.Append("<span class='totalamtbx'>" + Currencytype + " " + (totalFare + totaltds + totalcomm) + "</span>");
                    sbHtmlFare.Append("</div>");
                    sbHtmlFare.Append("</div>");
                    //==========Total Fare End======
                }
                else
                {

                   int iSSR = TravellerInformationHelper.GETTotalSSR();
                    if (iSSR > 0)
                    {
                        //==========Total Fare start======
                        sbHtmlFare.Append("<div class='mainfarsecbx'>");
                        sbHtmlFare.Append("<div class='divsecadtbx'>");
                        sbHtmlFare.Append("<span class='totalamtbx'>Total SSR :</span>");
                        sbHtmlFare.Append("<span class='totalamtbx'>" + Currencytype + " " + (iSSR) + "</span>");
                        sbHtmlFare.Append("</div>");
                        sbHtmlFare.Append("</div>");
                        //==========Total Fare End======
                    }
                }

                sbHtmlFare.Append("</div>");
                #endregion
            }
            return Itinary;
        }
    }
}
