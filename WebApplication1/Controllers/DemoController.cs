using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DotNetNuke.Web.Api;
using Newtonsoft.Json.Linq;
using ProfApreciat.Filters;
using ProfApreciat.Models;
using ProfApreciat.Utils;

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
            string URL = "http://localhost:8080/moodle/webservice/rest/server.php";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
               string msg = ExcelDataReader.InsertData();

                using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
                {
                    int nrAbs = context.ProgramStudius.Sum(ps => ps.NumarAbsolventi);
                    var results = context.Profesors.ToList();
                    FileWriter.work(results, nrAbs);
                }
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                return Ok(msg + " | Total time : " + elapsedTime);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            //try
            //{
            //    HttpClient client = new HttpClient();
            //    client.BaseAddress = new Uri(URL);

            //    // Add an Accept header for JSON format.
            //    client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));


            //    HttpResponseMessage httpResponseMessage = client.GetAsync("?wstoken=6b912293ca1e233f254bcad16d1fed43&wsfunction=mod_questionnaire_get_questionnaire_responses&moodlewsrestformat=json&substrcmidnumber=CEL").Result;

            //    if (httpResponseMessage.IsSuccessStatusCode)
            //    {
            //        var jsonString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //        var json = JObject.Parse(jsonString);

            //        foreach(var report in json["reports"])
            //        {

            //            using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
            //            {
            //                string cmidnumber = report["cmidnumber"].ToString();
            //                string denumireScurtaPS = cmidnumber.Substring(cmidnumber.IndexOf("-") + 1);
            //                var listPS = context.ProgramStudius.Where(ps => ps.DenumireScurta.ToUpper().Equals(denumireScurtaPS.ToUpper())).ToList();

            //                if (listPS.Count == 0)
            //                {
            //                    // issue warning?
            //                    continue;
            //                }

            //                ProgramStudiu programStudiu = listPS[0];

            //                foreach (var option in report["options"])
            //                {
            //                    string optionText = option["optiontext"].ToString();
            //                    if (optionText.IndexOf("(") == -1)
            //                    {
            //                        continue;
            //                    }
            //                    optionText = optionText.Substring(optionText.IndexOf("(") + 1);
            //                    if (optionText.IndexOf("_") == -1)
            //                    {
            //                        continue;
            //                    }
            //                    optionText = optionText.Substring(optionText.IndexOf("_") + 1);
            //                    if (optionText.IndexOf(")") == -1)
            //                    {
            //                        continue;
            //                    }
            //                    string nrcrtProfesorStr = optionText.Substring(0, optionText.IndexOf(")"));
            //                    int nrcrtProfesor = 0;

            //                    if (!int.TryParse(nrcrtProfesorStr, out nrcrtProfesor))
            //                    {
            //                        // issue warning?
            //                        continue;
            //                    }

            //                    var listProf = context.Profesors.Where(prf => prf.ID_Profesor.Equals(nrcrtProfesor)).ToList();

            //                    if (listProf.Count == 0)
            //                    {
            //                        // issue warning?
            //                        continue;
            //                    }

            //                    Profesor profesor = listProf[0];
            //                    var listRez = programStudiu.RezultatVotProfesorProgramStudius.Where(res => res.ID_Profesor.Equals(profesor.ID_Profesor)).ToList();

            //                    if (listRez.Count == 0)
            //                    {
            //                        // issue warning?
            //                        continue;
            //                    }

            //                    var rez = listRez[0];

            //                    foreach (var resp in option["responses"])
            //                    {
            //                        if (resp["response"].ToString().Equals("1"))
            //                        {
            //                            ++rez.NumarVoturi;
            //                        }
            //                    }

            //                }
            //                context.SaveChanges();
            //            }
            //        }

            //        return Ok(jsonString);
            //    }
            //    else
            //    {
            //        return StatusCode(httpResponseMessage.StatusCode);
            //    }
            //}
            //catch (Exception e)
            //{
            //    return InternalServerError(e);
            //}
        }

        [HttpGet]
        public IHttpActionResult Err()
        {
            throw new ArgumentException("exception message");

            return Ok("da");
        }
    }
}