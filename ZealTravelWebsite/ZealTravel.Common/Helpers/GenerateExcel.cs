using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace ZealTravel.Common.Helpers
{
    public class GenerateExcel
    {
        public void ExportToExcel<T>(List<T> data, string filePath, List<string> headers = null, string sheetName = "Reports") where T : class
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(sheetName);

                var properties = typeof(T).GetProperties();

                int maxColumns = headers != null && headers.Any()
                                 ? Math.Min(headers.Count, properties.Length)
                                 : properties.Length;

                for (int i = 0; i < maxColumns; i++)
                {
                    var headerName = headers != null && i < headers.Count ? headers[i] : properties[i].Name;
                    worksheet.Cell(1, i + 1).Value = headerName;
                }

                int rowIndex = 2;

                foreach (var report in data)
                {
                    for (int i = 0; i < maxColumns; i++)
                    {
                        var value = properties[i].GetValue(report);
                        worksheet.Cell(rowIndex, i + 1).Value = value != null ? value.ToString() : string.Empty;
                    }
                    rowIndex++;
                }

                workbook.SaveAs(filePath);
            }
        }
    }
}
