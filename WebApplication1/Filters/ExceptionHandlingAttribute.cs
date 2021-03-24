using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using DotNetNuke.Services.Log.EventLog;

namespace WebApplication1.Filters
{
        /// <summary>
        /// Un atribut care se poate pune pe un controller sau un endpoint si care sa gestioneze exceptiile care sunt aruncate la nuvelul acestora
        /// Este configurat sa faca logging pentru exceptie in admin logs in dnn si sa ofere pentru erori mai mult context si coduri de eroare mai potrivite
        /// </summary>
        public class ExceptionHandlingAttribute : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext context)
            {
                //face logging in DNN in Admin logs
                DotNetNuke.Services.Exceptions.Exceptions.LogException(context.Exception);


                //face logging in DNN in Admin logs
                DotNetNuke.Services.Exceptions.Exceptions.LogException(context.Exception);

                EventLogController eventLog = new EventLogController();
                DotNetNuke.Services.Log.EventLog.LogInfo logInfo = new LogInfo();
                logInfo.LogTypeKey = EventLogController.EventLogType.ADMIN_ALERT.ToString();
                logInfo.AddProperty("exceptionMessage:", context.Exception.Message);
                logInfo.AddProperty("exception:", context.Exception.ToString());
                eventLog.AddLog(logInfo);


                //pentru exceptiile de validare aruncate de fluent validation
                if (context.Exception is ValidationException)
                {
                    throw new HttpResponseException(context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, context.Exception.Message));
                }

                string exceptionMessage = context.Exception.Message;

                var statusCode = HttpStatusCode.InternalServerError;

                if (context.Exception is ArgumentException)
                    statusCode = HttpStatusCode.BadRequest;
                else if (context.Exception is UnauthorizedAccessException)
                    statusCode = HttpStatusCode.Unauthorized;
                else if (context.Exception is InvalidOperationException)
                    statusCode = HttpStatusCode.Forbidden;
                else if (context.Exception is ArgumentNullException || context.Exception is ValidationException)
                    statusCode = HttpStatusCode.BadRequest;
                else if (context.Exception is SqlException)
                    exceptionMessage = "SQL exception: " + context.Exception.Message;


                throw new HttpResponseException(new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(exceptionMessage),
                });
            }
        }
}