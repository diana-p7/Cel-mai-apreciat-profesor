﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
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
        static List<String> profesori = new List<string>();

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult AdaugaProfesor(String email, String nume)
        {
            try
            {
                profesori.Add(email + nume);
                return Ok("Add success");
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
        }

        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Get()
        {
            if (profesori.Count > 0)
            {
                return Ok(profesori[0]);
            }
            else return Ok("Setup success");
        }

        [HttpGet]
        public IHttpActionResult Err()
        {
            throw new ArgumentException("exception message");

            return Ok("da");
        }
    }
}