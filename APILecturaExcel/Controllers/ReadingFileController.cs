using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;

namespace APILecturaExcel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadingFileController : ControllerBase
    {
        private bool ValidateFileIsEmpty(IFormFile file)
        {
            return file.Length <= 0;
        }

        [HttpPost]
        public IActionResult PostFile(IFormFile file)
        {
            if(ValidateFileIsEmpty(file))
                return BadRequest("The file can't be empty");
            var FileExtension = Path.GetExtension(file.FileName);

            if(!FileValidateExtension(FileExtension))
                return BadRequest("The extension of the file is not valid");

            //////////////////////////////////////////////////////////
            using (var stream = file.OpenReadStream()/*Open(@"C:\ScriptExcel\ScriptModel.xlsx", FileMode.Open, FileAccess.Read)*/)
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader);

                        }
                    } while (reader.NextResult());
                }
            }
            return Ok($"Recived file: {file.FileName} ");
        }

        //[HttpPost]
        //public IActionResult ReadingFile(IFormFile file)
        //{


        //    return Ok("Se leyo el archivo");
        //}


        private bool FileValidateExtension(string fileExtensionToCheck)
        {
            List<string> ValidExtensions = new List<string>();
            ValidExtensions.Add(".xlsx");
            ValidExtensions.Add(".xlsm");
            ValidExtensions.Add(".xlsb");
            ValidExtensions.Add(".xltx");
            ValidExtensions.Add(".xltm");
            ValidExtensions.Add(".xls");
            ValidExtensions.Add(".xlt");
            ValidExtensions.Add(".xml");
            ValidExtensions.Add(".xlam");
            ValidExtensions.Add(".xla");
            ValidExtensions.Add(".xlw");
            ValidExtensions.Add(".xlr");

            return ValidExtensions.Contains(fileExtensionToCheck.ToLower());
                
        }


    }
}
