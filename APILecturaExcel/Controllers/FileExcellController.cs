using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APILecturaExcel.Interfaces;

namespace APILecturaExcel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileExcellController : ControllerBase
    {
        private readonly IFileValidations _fileValidations;
        private readonly IExcellValidator _excellValidator;
        public FileExcellController(IFileValidations fileValidations, IExcellValidator excellValidator)
        {
            _fileValidations= fileValidations;
            _excellValidator= excellValidator;
        } 
        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile file2)
        {
            
            if (_fileValidations.ValidateFileContent(file2) && _fileValidations.ValidateFileExtension(file2))
            {
                using var stream = file2.OpenReadStream();
                string hasErrors;
                hasErrors = await _excellValidator.AllValidations(stream);
                if (string.IsNullOrEmpty(hasErrors))
                    return Ok("Excell has been accepted with no errors succesfuly");
                return BadRequest(hasErrors);
            }
            return BadRequest("File extension or file content empty is not accepted.");

        }
    }
}
