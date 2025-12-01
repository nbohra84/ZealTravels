using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class RoleManagement
    {
        public static bool RoleStatus(string PageName, out bool Edit)
        {
            PageName = PageName.Replace(".aspx", "").Trim();

            bool Show = false;
            Edit = false;
            try
            {
                string CompanyID = HttpContext.Current.Session["accid"].ToString();
                string MenuData = HttpContext.Current.Session["menudata"].ToString();

                DataSet dsMenu = dbCommon.CommonFunction.StringToDataSet(MenuData);
                Show = RoleStatus(CompanyID, PageName, dsMenu.Tables["Product"], out Edit);
            }
            catch
            {

            }
            return Show;
        }
        public static bool RoleStatus(string CompanyID, string PageName, DataTable dtRole, out bool Edit)
        {
            bool Show = false;
            Edit = false;
            DataRow[] drRole = dtRole.Select("PageName like '" + PageName + "%'");
            if (drRole.Length > 0)
            {
                Show = Convert.ToBoolean(drRole.CopyToDataTable().Rows[0]["Show"].ToString());
                Edit = Convert.ToBoolean(drRole.CopyToDataTable().Rows[0]["Edit"].ToString());
            }
            return Show;
        }
    }
}
