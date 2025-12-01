using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Xml;

namespace ZealTravel.Infrastructure.DBCommon
{
    public class ConnectionString
    {
        private static IConfiguration _config;
        ConnectionString(IConfiguration configuration)
        {
            _config= configuration;
        }
        public  static string dbConnect
        {
            get
            {
                /*string Server = "localhost";
                string Database = "zealdb_N";
                string User = "sb";
                string Password = "nehal@123";
                */
                
                string Server = ".\\SQLEXPRESS";
                //string Server = "IT2-PC\\SQLEXPRESS01";
                string Database = "zealdb_O";
                string User = "sa";
                string Password = "madnat";




                //Server = objcrypto.encrypt(xmlflt.SelectSingleNode("root/s").InnerText);
                //Database = objcrypto.encrypt(xmlflt.SelectSingleNode("root/d").InnerText);
                //User = objcrypto.encrypt(xmlflt.SelectSingleNode("root/u").InnerText);
                //Password = objcrypto.encrypt(xmlflt.SelectSingleNode("root/p").InnerText);

                //GetFile(out Server, out Database, out User, out Password);
                return _config.GetConnectionString("DefaultConnection");//"Server=" + Server + ";Database=" + Database + ";User Id=" + User + ";Password=" + Password + ";";
                    
            }
        }

        public static string ConnectionLIVE { get; protected set; }

        //private static void GetFile(out string Server, out string Database, out string User, out string Password)
        //{
        //    string file = string.Empty;
        //    Server = string.Empty;
        //    Database = string.Empty;
        //    User = string.Empty;
        //    Password = string.Empty;

        //    try
        //    {
        //        System.IO.StreamReader myFile22 = new System.IO.StreamReader(@"C:\Zeal\zealdb.xml");
        //        file = myFile22.ReadToEnd();
        //        myFile22.Close();

        //        AIRS.Cryptography.scCryptography objcrypto = new AIRS.Cryptography.scCryptography();
        //        if (file != null && file.Length > 0 && file.IndexOf("root") != -1 && file.IndexOf("s") != -1 && file.IndexOf("d") != -1 && file.IndexOf("u") != -1 && file.IndexOf("p") != -1)
        //        {
        //            XmlDocument xmlflt = new XmlDocument();
        //            xmlflt.LoadXml(file);

        //           // string encrySvrr = objcrypto.encrypt("209.209.40.159");

        //            Server = objcrypto.decrypt(xmlflt.SelectSingleNode("root/s").InnerText);
        //            Database = objcrypto.decrypt(xmlflt.SelectSingleNode("root/d").InnerText);
        //            User = objcrypto.decrypt(xmlflt.SelectSingleNode("root/u").InnerText);
        //            Password = objcrypto.decrypt(xmlflt.SelectSingleNode("root/p").InnerText);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}
    }
}