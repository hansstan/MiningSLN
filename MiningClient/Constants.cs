using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiningClient
{
    public class Constants
    {
        // =========================================================
        // TODO: Update with YOUR Azure WebSite Url!!!!
        // =========================================================
        public static string MiningWebAppBaseUrl = "MiningWebApp";

        ///z.B. "MiningWebApp-Meier"; // <<<==== Update ONLY THIS LINE No special chars like ÄÖÜ ....!!!
        // =========================================================
        // ===>>>>    <YourName>.azurewebsites.net will be appended after publishing

        // DO NOT CHANGE this!!!!
        public static string MiningCenterWebAppUrl = "http://MiningCenterWebApp.azurewebsites.net/";
        //public static string MiningCenterWebAppUrl = "http://localhost/MiningCenterWebApp/"; 
    }
}
