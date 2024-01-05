using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Models;
using QuestPDF.Fluent;

namespace BisleriumCafe.Services
{
    public class PDFService
    {
        public static void SavePDF()
        {
            var coffeeFilePath = Utils.GetCoffeeFilePath();
            if (File.Exists(coffeeFilePath))
            {
                var json = File.ReadAllText(coffeeFilePath);
                if (json.Trim().Length > 0)
                {
                    var deserializedData = JsonSerializer.Deserialize<List<Coffee>>(json);
                    if (deserializedData != null)
                    {
                        var appPath = Utils.GetAppDirectoryPath();
                        Document.Create(container =>
                        {
                            container.Page(page =>
                            {
                                page.Header().Text("Coffee");

                                page.Content().Table(table =>
                                {
                                    table.ColumnsDefinition(column =>
                                    {
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Username");
                                        header.Cell().Text("Role");
                                    });

                                    foreach (var item in deserializedData)
                                    {
                                        table.Cell().Text(item.Price);
                                        table.Cell().Text(item.Name);
                                    }
                                });

                                page.Footer().Text(text =>
                                {
                                    text.Span("Page :");
                                    text.CurrentPageNumber();
                                });
                            });
                        }).GeneratePdf(Path.Combine(appPath, "Coffee.pdf"));
                    }
                }
            }
        }
    }
}
