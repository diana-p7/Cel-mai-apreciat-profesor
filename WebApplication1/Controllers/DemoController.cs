using System;
using System.Collections.Generic;
using System.Data;
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
                    return Ok("Data load success");
                }
                else throw new ArgumentException(message);
            }
            catch(Exception e)
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