using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DotNetNuke.Web.Api;
using ProfApreciat.Filters;
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
    public class FileController : DnnApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> UploadFile()
        {
            HttpContext ctx = HttpContext.Current;
            string root = ctx.Server.MapPath("~/App_Data");
            MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string name = file.Headers.ContentDisposition.FileName;

                    // remove double quotes from string.
                    name = name.Trim('"');

                    string localFileName = file.LocalFileName;
                    string filePath = Path.Combine(root, Path.GetFileName(GlobalValues.PATH_SOURCE_FILE));

                    if(!Path.GetExtension(name).Equals(".xlsx"))
                    {
                        return "File type not supported.";
                    }
                    
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    File.Move(localFileName, filePath);
                    File.Delete(localFileName);
                    string msgInsert = ProcessInputData.InsertData();
                    if (!String.IsNullOrEmpty(msgInsert)) 
                    {
                        File.Delete(filePath);
                        return msgInsert;
                    }
                }
            }
            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }

            return "File uploaded!";
        }

        [HttpGet]
        public HttpResponseMessage GetResourceFile(string type)
        {          
            if (String.IsNullOrEmpty(type))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            type = type.ToUpper();

            string destFilePath = String.Empty;

            switch (type)
            {
                case "SOURCE":
                    destFilePath = GlobalValues.PATH_SOURCE_FILE;
                    break;
                case "VOTING_STATUS":
                    destFilePath = ProcessOutputData.ExportVotingStatus();
                    break;
                case "FINAL_RESULTS":
                    destFilePath = ProcessOutputData.ExportFinalResults();
                    break;
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (!File.Exists(destFilePath))
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }

            byte[] dataBytes = File.ReadAllBytes(destFilePath);

            //adding bytes to memory stream   
            MemoryStream dataStream = new MemoryStream(dataBytes);
            
            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = Path.GetFileName(destFilePath);
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }
    }
}