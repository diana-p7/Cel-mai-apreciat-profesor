using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
    public class FileController : DnnApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> UploadFile()
        {
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/App_Data");
            var provider =
                new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content
                    .ReadAsMultipartAsync(provider);

                foreach (var file in provider.FileData)
                {
                    var name = file.Headers
                        .ContentDisposition
                        .FileName;

                    // remove double quotes from string.
                    name = name.Trim('"');

                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(root, name);

                    File.Move(localFileName, filePath);
                }
            }
            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }

            return "File uploaded!";
        }
    }
}