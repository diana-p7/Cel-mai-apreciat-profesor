using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using ProfApreciat.Filters;

namespace ProfApreciat.Controllers
{
    //atribut care prelucreaza raspunsul si face logging in admin logs in dnn
    [ExceptionHandling]

    //preprocessor definition care determina ignorarea atributului de securitate cand aplicatia ruleaza in modul de debug - util pentru dezvoltarea aplicatiei de front-end 
#if DEBUG
    [AllowAnonymous]
#else
     [DnnAuthorize(StaticRoles = "Cadru Didactic")]
#endif
    public class DemoController : DnnApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {

                String message = ExcelDataReader.InsertData();

                if (String.IsNullOrEmpty(message))
                {
                    string connStr = "data source=DESKTOP-K0KFAGK;initial catalog=MyDNNDatabase;User Id=MyDNNSQLUser;Password=pass;";
                    SqlConnection conn = new SqlConnection(connStr);
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("spRezultateVoturiLicenta", conn);
                    //SqlCommand cmd = new SqlCommand("spEligibilRemunerareLicentaSiMaster1", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                  
                    string temp = "";

           
                   /* SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        temp += reader["Nume"].ToString();
                        temp += " ";
                        temp += reader["Prenume"].ToString();
                        temp += " ";
                        temp += reader["DenumireScurta"].ToString();
                        temp += " ";
                        temp += "<br>";
                    } */

                     SqlDataReader reader = cmd.ExecuteReader();
                     while (reader.Read())
                     {
                         temp += reader["Nume"].ToString();
                         temp += " ";
                         temp += reader["Prenume"].ToString();
                         temp += " ";
                         temp += reader["Denumire"].ToString();
                         temp += " ";
                         temp += reader["VoturiTotale"].ToString();
                         temp += " ";
                         temp += "<br>";
                     } 

                    conn.Close();
                    return Ok(temp);


                }
                else throw new ArgumentException(message);
            }

            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpGet]
        public IHttpActionResult Err()
        {
            throw new ArgumentException("exception message");

            return Ok("da");
        }
    }
}