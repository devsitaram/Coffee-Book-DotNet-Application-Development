using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;
using QuestPDF.Fluent;

namespace BisleriumCafe.Data.Services.Export
{
    public static class PDFServices
    {
        public static string PDFFileGenerate()
        {
            var filePath = Utils.GetOrderFilePath();
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                if (json.Trim().Length > 0)
                {
                    var deserializedData = JsonSerializer.Deserialize<List<CoffeeOrder>>(json);
                    if (deserializedData != null)
                    {
                        var pdfFilePath = Utils.GetAppDirectoryPath();

                        Document.Create(container =>
                        {
                            container.Page(page =>
                            {
                                page.Header().Text("\nBislerium Cafe Sale Revenue Details\n");

                                page.Content().Table(table =>
                                {
                                    table.ColumnsDefinition(column =>
                                    {
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Coffee Name: ");
                                        header.Cell().Text("Coffee Price: ");
                                        header.Cell().Text("Add In Flavor: ");
                                        header.Cell().Text("Add In Price: ");
                                        header.Cell().Text("Customer Number: ");
                                        header.Cell().Text("Total Price: ");
                                        header.Cell().Text("Order Date: ");
                                    });

                                    foreach (var item in deserializedData)
                                    {
                                        table.Cell().Text(item.CoffeeName?.ToString() ?? "");
                                        table.Cell().Text(item.CoffeePrice.ToString() ?? "");
                                        table.Cell().Text(item.AddFlavorName?.ToString() ?? "");
                                        table.Cell().Text(item.AddFlavorPrice.ToString() ?? "");
                                        table.Cell().Text(item.CustomerPhoneNumber?.ToString() ?? "");
                                        table.Cell().Text(item.TotalPrice.ToString() ?? "");
                                        table.Cell().Text(item.OrderDate.ToString() ?? "");
                                    }
                                });
                                page.Footer().Text(text =>
                                {
                                    text.Span("Page :");
                                    text.CurrentPageNumber();
                                    text.Span("\n");
                                });
                            });
                        }).GeneratePdf(Path.Combine(pdfFilePath, "sale_transactions.pdf"));
                        return pdfFilePath;
                    }
                }
            }
            return null; // Return null to indicate failure or no data
        }
    }
}