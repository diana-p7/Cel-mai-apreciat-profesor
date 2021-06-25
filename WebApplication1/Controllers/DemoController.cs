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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                //string msg = ProcessInputData.InsertData();
                string moodleData = ProcessInputData.FetchMoodleData();
                return Ok(moodleData);
                //using (MyDNNDatabaseEntities context = new MyDNNDatabaseEntities())
                //{
                //    List<Profesor> profesori = context.Profesors.ToList();
                //    ProcessOutputData.WriteOutputFile(profesori);
                //}

                //stopWatch.Stop();
                //TimeSpan ts = stopWatch.Elapsed;
                //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                //    ts.Hours, ts.Minutes, ts.Seconds,
                //    ts.Milliseconds / 10);
                //return Ok("Total time : " + elapsedTime);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }          
        }

        [HttpGet]
        public IHttpActionResult GetVotingListForPs(string ps)
        {
            JObject response = ProcessInputData.GetProfesorsForPS(ps);

            if(response.HasValues)
            {
                return Ok(response);
            }

            return Ok("No list");
        }
    }
}