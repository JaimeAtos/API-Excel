using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace APILecturaExcel.Interfaces
{
    public interface IFileValidations
    {
        public bool ValidateFileContent(IFormFile file);

        public bool ValidateFileExtension(IFormFile file);

        public Task<string> AllValidations(Stream stream);

        public List<string> Errors { get; set; }
    }
}
