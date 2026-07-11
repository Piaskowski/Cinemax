using System.Reflection;
using ExcelDataReader;
using Cinemax.Shared.Resources.Excel;
using Cinemax.Shared.Contracts.Common;
using Cinemax.Shared.Common;

namespace Cinemax.Application.Common
{
    public class ExcelConverter
    {
        public static async Task<ImportResult<T>> ReadSheet<T>(MemoryStream stream, bool leaveStreamOpen = false) 
            where T : class, new()
        {
            stream.Seek(0, SeekOrigin.Begin);

            var sheetAttribute = typeof(T).GetCustomAttribute<ExcelSheetAttribute>();
            if (sheetAttribute == null)
                throw new Exception($"Brak sprecyzowanego atrybutu arkusza w klasie: {typeof(T).Name}");

            using var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration
            {
                LeaveOpen = leaveStreamOpen
            });
            var result = new ImportResult<T>();

            var containsSheet = false;
            do
            {
                if (reader.Name.Equals(sheetAttribute.SheetName, StringComparison.OrdinalIgnoreCase))
                {
                    containsSheet = true;
                    break;
                }
            } while (reader.NextResult());

            if (!containsSheet)
            {
                result.Errors.Add(string.Format(ExcelMessages.Error_SheetNotFound, sheetAttribute.SheetName));
                return result;
            }

            return await ReadFromReader<T>(reader);
        }

        private static async Task<ImportResult<T>> ReadFromReader<T>(IExcelDataReader reader)
            where T : class, new()
        {
            var result = new ImportResult<T>();

            var dicCols = new Dictionary<string, int>();
            var rowIndex = 0;
            var properties = typeof(T).GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Column = p.GetCustomAttributes(typeof(ExcelColumnAttribute), false)
                    .Cast<ExcelColumnAttribute>()
                    .FirstOrDefault()
                })
                .Where(p => p.Column != null)
                .OrderBy(p => p.Column.ColumnName)
                .ToList();

            // == Header validation (first row) ==
            if(reader.RowCount > 0 && reader.Read())
            {
                var colsCount = reader.FieldCount;
                var colErrors = new List<string>();

                for (int i = 0; i < colsCount; i++) 
                {
                    if (!reader.IsDBNull(i))
                    {
                       var colName = reader.GetValue(i).ToString().Trim();

                        if (!string.IsNullOrEmpty(colName))
                        {
                            var normalizedColName = colName.Trim().ToLower();
                            if (properties.Any(p =>
                                p.Column.ColumnName!.Trim().Equals(normalizedColName, StringComparison.OrdinalIgnoreCase)))
                            {
                                dicCols.Add(normalizedColName, i);
                            }
                        }
                        else
                        {
                            colErrors.Add(string.Format(ExcelMessages.Error_HeaderNoValue, i));
                        }
                    }
                    else
                    {
                        colErrors.Add(string.Format(ExcelMessages.Error_HeaderNoValue, i));
                    }
                }

                properties.Select(p => p.Column.ColumnName.ToLower())
                    .Except(dicCols.Select(s => s.Key.ToLower()))
                    .ToList()
                    .ForEach(f => colErrors.Add(string.Format(ExcelMessages.Error_HeaderNotFound, f)));

                if (colErrors.Count > 0)
                {
                    result.Errors = colErrors;
                    return result;
                }

                // == Items ==
                while (reader.RowCount > 1 && reader.Read())
                {
                    rowIndex++;

                    var errors = new List<string>();
                    var fields = new Dictionary<string, object?>();
                    var item = new T();

                    foreach (var property in properties)
                    {
                        var colName = property.Column!.ColumnName!;
                        var isRequired = property.Column!.Required;

                        if (!dicCols.TryGetValue(colName.Trim().ToLower(), out int colIndex))
                            continue;

                        try
                        {
                            if (reader.IsDBNull(colIndex))
                            {
                                if (isRequired)
                                    errors.Add(string.Format(ExcelMessages.Error_FieldNoValue, rowIndex, colIndex + 1, colName));
                                {
                                    try
                                    {
                                        property.Property.SetValue(item, null);
                                    }
                                    catch
                                    {
                                        errors.Add(string.Format(ExcelMessages.Error_FieldError, rowIndex, colIndex + 1, colIndex, TypeInfo(property.Property.PropertyType)));
                                    }
                                }
                            }
                            else
                            {
                                var itemVal = reader.GetValue(colIndex);
                                try
                                {
                                    property.Property.SetValue(item, MapItem(property.Property.PropertyType, itemVal));
                                }
                                catch
                                {
                                    errors.Add(string.Format(ExcelMessages.Error_FieldError, rowIndex, colIndex + 1, colIndex, TypeInfo(property.Property.PropertyType)));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add(string.Format(ExcelMessages.Error_FieldTypeError, rowIndex, colIndex + 1, colName, TypeInfo(property.Property.PropertyType)));
                        }
                    }
                    if (errors.Any())
                    {
                        result.Errors.AddRange(errors);
                    }
                    else
                    {
                        result.Items.Add(item);
                    }
                }
            }
            return result;
        }

        private static object? MapItem(Type type, object item)
        {
            if (string.IsNullOrEmpty(item.ToString()?.Trim()))
                return null;

            var targetType = Nullable.GetUnderlyingType(type) ?? type;

            if (targetType.IsEnum)
            {
                return Enum.Parse(targetType, item.ToString()!, ignoreCase: true);
            }

            if (targetType == typeof(string))
                return Convert.ToString(item);

            if (targetType == typeof(int))
                return Convert.ToInt32(item);

            if (targetType == typeof(bool))
                return new List<string> { "true", "1", "yes", "y", "tak", "t" }
                    .Contains(item.ToString(), StringComparer.OrdinalIgnoreCase);

            if (targetType == typeof(DateTime))
                return Convert.ToDateTime(item);

            if (targetType == typeof(decimal))
                return Convert.ToDecimal(item);

            if (targetType == typeof(double))
                return Convert.ToDouble(item);

            if (targetType == typeof(float))
                return Convert.ToSingle(item);

            if (targetType == typeof(Guid))
                return Guid.Parse(item.ToString()!);

            if (targetType == typeof(long))
                return Convert.ToInt64(item);

            return item;
        }

        private static string TypeInfo(Type type)
        {
            var targetType = Nullable.GetUnderlyingType(type) ?? type;

            if (targetType.IsEnum)
            {
                var values = string.Join(", ", Enum.GetNames(targetType));
                return $"jedna z wartości: {values}";
            }

            if (targetType == typeof(string))
                return "tekst";

            if (targetType == typeof(int))
                return "liczba";

            if (targetType == typeof(bool))
                return "wartość logiczna (tak, yes, t, y, 1)";

            if (targetType == typeof(DateTime))
                return "data";

            if (targetType == typeof(decimal))
                return "liczba";

            if (targetType == typeof(double))
                return "liczba";

            if (targetType == typeof(float))
                return "liczba";

            if (targetType == typeof(Guid))
                return "unikalny identyfikator (GUID)";

            if (targetType == typeof(long))
                return "liczba";

            return "tekst";
        }
    }
}
