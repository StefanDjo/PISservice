using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PISservice.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PISservice.Controllers
{
    [ApiController]
    [Route("service")]
    public class UploadDownload : Controller
    {
        [HttpPost("POST")]
        [Produces("application/json")]
        public async Task<IActionResult> Upload([FromBody] ClientFileModel model)
        {
            try
            {
                var stream = new MemoryStream(model.fajl);
                IFormFile file = new FormFile(stream, 0, model.fajl.Length, "NazivPropertija", model.fileName + model.fileExtension);
                string fileDir = Path.Combine(Directory.GetCurrentDirectory(), @"UploadovaniFajlovi");
                string fileFullPath = Path.Combine(fileDir + "\\", model.fileName + model.requestID + model.fileExtension);

                if (!Directory.Exists(fileDir))
                    Directory.CreateDirectory(fileDir);

                using (var fileSteam = new FileStream(fileFullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileSteam);
                }

                LogFileModel logModel = new LogFileModel();
                logModel.fileExtension = model.fileExtension;
                logModel.fileName = model.fileName;
                logModel.fileOwner = model.fileOwner;
                logModel.fileType = model.fileType;
                logModel.requestID = model.requestID;
                logModel.fileSize = model.fajl.Length / 1024;//ovo je u kilobajtima kada se deli sa 1024
                string logFileContext = JsonConvert.SerializeObject(logModel);
                System.IO.File.WriteAllText(Path.Combine(fileDir + "\\", model.fileName + model.requestID + ".txt"), logFileContext);

                return Ok(model.requestID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GET")]
        [Produces("application/json")]
        public byte[] Get(string requestID)
        {
            string fileDir = Path.Combine(Directory.GetCurrentDirectory(), @"UploadovaniFajlovi");
            DirectoryInfo dir = new DirectoryInfo(fileDir + "\\");
            FileInfo[] files = dir.GetFiles("*" + requestID + ".pdf");

            byte[] myfile = null;

            foreach (FileInfo file in files)
            {
                myfile = System.IO.File.ReadAllBytes(file.FullName);
            }

            return myfile;
        }
    }
}
