
namespace Cinemax.Shared.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExcelSheetAttribute(string sheetName) : Attribute
    {
        public string SheetName { get; } = sheetName;
    }
}
