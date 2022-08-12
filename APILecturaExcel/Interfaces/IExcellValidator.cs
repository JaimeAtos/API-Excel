using System.Data;
using System.Threading.Tasks;

namespace APILecturaExcel.Interfaces
{
    public interface IExcellValidator : IFileValidations
    {
        public delegate void ValidatorDelegate(DataTable table, int rowIndex, int columnIndex);

        public Task IterateColumn(DataTable table, int columnIndex, ValidatorDelegate validator);
        public void ValidateDescriptors(DataTable table, int rowIndex, int columnIndex);

        public void IsNumber(DataTable table, int rowIndex, int columnIndex);
    }
}
