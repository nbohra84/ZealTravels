using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

namespace ZealTravel.Common.Helpers
{
    public class UserHelper
    {

        public static string GetCompanyID(ClaimsPrincipal user)
        {
            string CompanyID = string.Empty;
            
            if ( user != null && user.Identity.IsAuthenticated)
            { 
              CompanyID = user.FindFirstValue("CompanyId");
           
                if (CompanyID.IndexOf("-ST-") != -1)
                {
                    CompanyID = user.FindFirstValue("AccountID").ToString().Trim();
                }
                else if (CompanyID.IndexOf("ST-") != -1 && CompanyID.IndexOf("-ST-") == -1)
                {
                    CompanyID = CompanyID.ToString().Trim();
                }
            }
            return CompanyID;
        }

        public static string GetUserType(ClaimsPrincipal user) //flightdisplay,landing page (chanegs done in airlinebooking dll)
        {
            string UserType = "";
            string CompanyID = GetCompanyID(user);
            if (CompanyID.IndexOf("AD-") != -1)
            {
                UserType = "AD";
            }
            else if (CompanyID.IndexOf("A-") != -1 && CompanyID.IndexOf("-SA-") != -1)
            {
                UserType = "B2B2B";
            }
            else if (CompanyID.IndexOf("A-") != -1 && CompanyID.IndexOf("-C-") != -1)
            {
                UserType = "B2B2C";
            }
            else if (CompanyID.IndexOf("A-") != -1 && CompanyID.IndexOf("-SA-") == -1 && CompanyID.IndexOf("-C-") == -1)
            {
                UserType = "B2B";
            }
            else if (CompanyID.IndexOf("C-") != -1 && CompanyID.IndexOf("-C-") == -1)
            {
                UserType = "B2C";
            }
            return UserType;
        }

        public static Int32 GetStaffAccountID(ClaimsPrincipal user) // new search flight hotel
        {
            Int32 AccountID = 0;
            if (user != null && user.Identity.IsAuthenticated) {
                if (user.FindFirstValue("AccountID") != null && user.FindFirstValue("CompanyDetails") != null)
                {
                    var companyDetailsJson = user.FindFirstValue("CompanyDetails");
                    var companyDetails = JsonSerializer.Deserialize<JsonElement>(companyDetailsJson);

                    if (companyDetails.TryGetProperty("Staff_AccountID", out JsonElement staffAccountId) && staffAccountId.ValueKind != JsonValueKind.Null)
                    {
                        AccountID = staffAccountId.GetInt32();
                    }
                    else
                    {
                        AccountID = Convert.ToInt32(user.FindFirstValue("AccountID").Trim());
                    }
                }
            }
            return AccountID;
        }

        public static string GetStaffID(ClaimsPrincipal user) // new search flight hotel
        {
          
            var staffCompanyID = string.Empty;
            if (user!= null && user.Identity.IsAuthenticated)
            {
                if (user.FindFirstValue("AccountID") != null && user.FindFirstValue("CompanyDetails") != null)
                {
                    var companyDetailsJson = user.FindFirstValue("CompanyDetails");
                    var companyDetails = JsonSerializer.Deserialize<JsonElement>(companyDetailsJson);

                    if (companyDetails.TryGetProperty("CompanyID", out JsonElement companyID) && companyID.ValueKind != JsonValueKind.Null)
                    {
                        staffCompanyID = companyID.GetString();
                    }
                    
                }
            }
            return staffCompanyID;
        }

        public static bool IsCorporate(ClaimsPrincipal user)// done in Dashboard_FrontOffice_Proc
        {
            bool isCorporateAgent = false;
            if (user != null &&  user.Identity.IsAuthenticated != null)
            {
                if (user.FindFirstValue("AccountID") != null && user.FindFirstValue("CompanyDetails") != null)
                {

                    var companyDetailsJson = user.FindFirstValue("CompanyDetails");
                    var companyDetails = JsonSerializer.Deserialize<JsonElement>(companyDetailsJson);
                    if (companyDetails.TryGetProperty("CorporateAgent", out JsonElement corporateAgent) && corporateAgent.ValueKind != JsonValueKind.Null)
                    {
                        isCorporateAgent = corporateAgent.GetBoolean();
                    }
                }

            }
            return isCorporateAgent;
        }

        public static bool IsCorporateUser(ClaimsPrincipal user)// done in Dashboard_FrontOffice_Proc
        {
            bool isCorpUser = false;

            if ( user != null && user.Identity.IsAuthenticated)
            {
                if (user.FindFirstValue("AccountID") != null && user.FindFirstValue("CompanyDetails") != null)
                {

                    var companyDetailsJson = user.FindFirstValue("CompanyDetails");
                    var companyDetails = JsonSerializer.Deserialize<JsonElement>(companyDetailsJson);
                    if (companyDetails.TryGetProperty("StaffType", out JsonElement staffType) && staffType.ValueKind != JsonValueKind.Null)
                    {
                        isCorpUser = staffType.GetString().ToUpper().Equals("USER");
                    }
                }

            }
            return isCorpUser;
        }

        public static bool FindCorporateUser(ClaimsPrincipal user)
        {
            bool isUser = false;
            try
            {
                if (IsCorporate(user) && IsCorporateUser(user))
                {
                    isUser = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isUser;
        }

        public static string GetCompanyAddress(ClaimsPrincipal user)
        {
            string Address = string.Empty;

            if (user != null && user.Identity.IsAuthenticated)
            {
                if (user.FindFirstValue("AccountID") != null && user.FindFirstValue("CompanyDetails") != null)
                {

                    var companyDetailsJson = user.FindFirstValue("CompanyDetails");
                    var companyDetails = JsonSerializer.Deserialize<JsonElement>(companyDetailsJson);
                    var state = companyDetails.GetProperty("State").GetString();
                    var city = companyDetails.GetProperty("City").GetString();
                    var address = companyDetails.GetProperty("Address").GetString();
                    Address = state + " " + city + " " + address;
                }

            }

            return Address;
        }
        public static string GetCompanyEmail(ClaimsPrincipal user) //new
        {
            string email = string.Empty;
            if (user != null && user.Identity.IsAuthenticated)
            {
                if (user.FindFirstValue("AccountID") != null && user.FindFirstValue("CompanyDetails") != null)
                {

                    var companyDetailsJson = user.FindFirstValue("CompanyDetails");
                    var companyDetails = JsonSerializer.Deserialize<JsonElement>(companyDetailsJson);
                    email = companyDetails.GetProperty("Email").GetString();
                }

            }
            return email;
        }
        public static string GetCompanyMobile(ClaimsPrincipal user) //new
        {
            string mobile = string.Empty;
            if (user != null && user.Identity.IsAuthenticated)
            {
                if (user.FindFirstValue("AccountID") != null && user.FindFirstValue("CompanyDetails") != null)
                {

                    var companyDetailsJson = user.FindFirstValue("CompanyDetails");
                    var companyDetails = JsonSerializer.Deserialize<JsonElement>(companyDetailsJson);
                    mobile = companyDetails.GetProperty("Mobile").GetString();
                }

            }
            return mobile;
        }

    }
}
