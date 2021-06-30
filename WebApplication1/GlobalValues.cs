using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ProfApreciat
{
    public static class GlobalValues
    {
        public static string Denumire_TIP_CICLU_INVATAMANT_LICENTA = "Licenta";
        public static string Denumire_TIP_CICLU_INVATAMANT_MASTER = "Master";

        public static Dictionary<string, int> Map_TIP_CICLU_INVATAMANT_ID = new Dictionary<string, int>() { { Denumire_TIP_CICLU_INVATAMANT_LICENTA, 1 }, { Denumire_TIP_CICLU_INVATAMANT_MASTER, 2 } };

        public static string DenumireScurta_CADRU_EXTERN = "CE";

        public static string ColumnHeader_NUME = "NUME";
        public static string ColumnHeader_PRENUME = "PRENUME";
        public static string ColumnHeader_EMAIL = "EMAIL";
        public static string ColumnHeader_GRAD_DIDACTIC = "GRAD DIDACTIC";
        public static string ColumnHeader_FACULTATEA = "FACULTATEA";

        public static string PATH_SOURCE_FILE = HostingEnvironment.MapPath("~/App_Data/Profesor_Apreciat_Source.xlsx");

        public static int PROCENT_REMUNERATI = int.Parse(ConfigurationManager.AppSettings.Get("pa_procent_remunerati"));
    }
}