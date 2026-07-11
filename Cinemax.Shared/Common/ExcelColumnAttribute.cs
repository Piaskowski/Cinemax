
namespace Cinemax.Shared.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnAttribute(string columnName) : Attribute
    {
        public string ColumnName { get; } = columnName;
        public bool Required { get; set; }
    }
}
