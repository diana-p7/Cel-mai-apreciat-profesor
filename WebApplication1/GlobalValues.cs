using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProfApreciat
{
    public static class GlobalValues
    {
        public static string Denumire_TIP_CICLU_INVATAMANT_LICENTA = "Licenta";
        public static string Denumire_TIP_CICLU_INVATAMANT_MASTER = "Master";

        public static (byte, byte, byte) PatternForegroundColor_LICENTA_RGB = (5, 67, 191);
        public static (byte, byte, byte) PatternForegroundColor_MASTER_RGB = (195, 23, 170);
        public static Dictionary<string, int> Map_TIP_CICLU_INVATAMANT_ID = new Dictionary<string, int>() { { Denumire_TIP_CICLU_INVATAMANT_LICENTA, 1 }, { Denumire_TIP_CICLU_INVATAMANT_MASTER, 2 } };

        public static string DenumireScurta_CADRU_EXTERN = "CE";

        public static string ColumnHeader_NUME = "NUME";
        public static string ColumnHeader_PRENUME = "PRENUME";
        public static string ColumnHeader_EMAIL = "EMAIL";
        public static string ColumnHeader_GRAD_DIDACTIC = "GRAD DIDACTIC";
        public static string ColumnHeader_FACULTATEA = "FACULTATEA";

        public static string Path_FILE_INPUT = @"C:\Excel\0X_Cel_mai_apreciat_profesor_2021_univ_de lucru.xlsx";

        public static int PROCENT_ELIGIBILI = 10;

    }
}