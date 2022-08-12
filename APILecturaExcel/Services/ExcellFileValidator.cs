using APILecturaExcel.Interfaces;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace APILecturaExcel.Services
{
    public class ExcellFileValidator : IFileValidations, IExcellValidator
    {
        public List<string> Errors { get; set; }
        public ExcellFileValidator()
        {
            Errors = new List<string>();
        }
        
        public async Task<string> AllValidations(Stream stream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataset = reader.AsDataSet();
                var sheet = dataset.Tables[0];
                var descriptionColumn = 0;
                int[] numberColumns = { 10, 11, 12, 13, 14, 15, 20, 21, 22 };
                await IterateColumn(sheet, descriptionColumn, ValidateDescriptors);
                foreach (var numberColumn in numberColumns)
                    await IterateColumn(sheet, numberColumn, IsNumber);
                string AllErrors = "";
                Errors.ForEach(error => AllErrors += error + ",");
                return AllErrors;
            }
        }

        public bool ValidateFileContent(IFormFile file)
        {
            return file.Length > 0;
        }

        public bool ValidateFileExtension(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
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

            return ValidExtensions.Contains(fileExtension.ToLower());
        }

        public Task IterateColumn(DataTable table, int columnIndex, IExcellValidator.ValidatorDelegate validator)
        {
            for (int rowIndex = 6; rowIndex < table.Rows.Count - 4; rowIndex++)
            {
                validator(table, rowIndex, columnIndex);
            }
            return Task.CompletedTask;
        }
        public delegate void ValidatorDelegate(DataTable table, int rowIndex, int columnIndex);
        public void ValidateDescriptors(DataTable table, int rowIndex, int columnIndex)
        {
            if (rowIndex + 1 >= table.Rows.Count - 5) return;
            var current = table.Rows[rowIndex][columnIndex].ToString();
            var next = table.Rows[rowIndex + 1][columnIndex].ToString();
            if (!current.Equals(next))
                Errors.Add($"Descripciones diferentes \n" +
                                  $"{rowIndex}: {current} \n" +
                                  $"{rowIndex + 1}: {next}.");
        }
        public void IsNumber(DataTable table, int rowIndex, int columnIndex)
        {
            var number = table.Rows[rowIndex][columnIndex].ToString();
            if (!decimal.TryParse(number, out _))
                Errors.Add($"El campo en la celda ({rowIndex}, {columnIndex}) no es un numero valido.");
        }
    }
}
