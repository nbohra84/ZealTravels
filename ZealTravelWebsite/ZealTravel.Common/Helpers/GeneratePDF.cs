using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using System.Collections.Generic;
using System.Reflection;
using iText.Layout.Borders;
using iText.Html2pdf;
using System.IO;
using iText.Kernel.Geom;

namespace ZealTravel.Common.Helpers
{
    public class GeneratePDF
    {
        public void ExportToPDF<T>(string filePath, List<T> records, List<string> headers, string pageSize = "A4")
        {
            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    if (pageSize == "A2")
                    {
                        pdf.SetDefaultPageSize(PageSize.A2.Rotate());
                        var document = new Document(pdf);

                        var table = new Table(UnitValue.CreatePercentArray(headers.Count)).UseAllAvailableWidth();
                        foreach (var header in headers)
                        {
                            Cell headerCell = new Cell()
                                .Add(new Paragraph(header))
                                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                                .SetFontSize(7)
                                .SetBold()
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetPaddingLeft(5)
                                .SetPaddingRight(5)
                                .SetPaddingTop(1)
                                .SetPaddingBottom(1)
                                .SetBorder(new SolidBorder(1));
                            table.AddHeaderCell(headerCell);
                        }

                        for (int i = 0; i < records.Count; i++)
                        {
                            var record = records[i];
                            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                            foreach (var prop in properties)
                            {
                                var value = prop.GetValue(record)?.ToString() ?? string.Empty;
                                Cell dataCell = new Cell()
                                    .Add(new Paragraph(value))
                                    .SetFontSize(7)
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetPaddingLeft(5)
                                    .SetPaddingRight(5)
                                    .SetPaddingTop(1)
                                    .SetPaddingBottom(1)
                                    .SetBorder(new SolidBorder(1))
                                    .SetHeight(10);


                                if (i % 2 == 0)
                                {
                                    dataCell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                                }

                                table.AddCell(dataCell);
                            }
                        }

                        document.Add(table);
                        document.Close();
                    }
                    else
                    {
                        var document = new Document(pdf);

                        var table = new Table(UnitValue.CreatePercentArray(headers.Count)).UseAllAvailableWidth();
                        foreach (var header in headers)
                        {
                            Cell headerCell = new Cell()
                                .Add(new Paragraph(header))
                                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                                .SetFontSize(10)
                                .SetBold()
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetPadding(3)
                                .SetBorder(new SolidBorder(1));
                            table.AddHeaderCell(headerCell);
                        }

                        for (int i = 0; i < records.Count; i++)
                        {
                            var record = records[i];
                            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                            foreach (var prop in properties)
                            {
                                var value = prop.GetValue(record)?.ToString() ?? string.Empty;
                                Cell dataCell = new Cell()
                                    .Add(new Paragraph(value))
                                    .SetFontSize(7)
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetPadding(3)
                                    .SetBorder(new SolidBorder(1));

                                if (i % 2 == 0)
                                {
                                    dataCell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                                }

                                table.AddCell(dataCell);
                            }
                        }

                        document.Add(table);
                        document.Close();
                    }

                }
            }
        }
        public void ExportTicketToPDF(string filePath, string htmlContent)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                HtmlConverter.ConvertToPdf(htmlContent, fileStream);
            }
        }


        public static MemoryStream GeneratePdfStream(string htmlContent, (float Top, float Right, float Bottom, float Left) margins, string baseUri)
        {
            var memoryStream = new MemoryStream();

            using (var writer = new PdfWriter(memoryStream))
            using (var pdfDocument = new PdfDocument(writer))
            using (var document = new iText.Layout.Document(pdfDocument))
            {
                writer.SetCloseStream(false);
                // Set document margins
                document.SetMargins(margins.Top, margins.Right, margins.Bottom, margins.Left);

                // Initialize converter properties with base URI
                var converterProperties = new ConverterProperties();
                if (!string.IsNullOrWhiteSpace(baseUri))
                {
                    converterProperties.SetBaseUri(baseUri);
                }

                // Convert HTML to PDF
                HtmlConverter.ConvertToPdf(htmlContent, pdfDocument, converterProperties);
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

    }
}
