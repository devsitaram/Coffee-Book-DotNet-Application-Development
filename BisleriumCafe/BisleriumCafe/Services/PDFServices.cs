﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Models;
using QuestPDF.Fluent;

namespace BisleriumCafe.Services
{
    public static class PDFServices
    {
        //public static void PDFGenerate()
        //{
        //    var filePath = Utils.GetCoffeeFilePath();
        //    if (File.Exists(filePath))
        //    {
        //        var json = File.ReadAllText(filePath);
        //        if (json.Trim().Length > 0)
        //        {
        //            var deserializedData = JsonSerializer.Deserialize<List<Coffee>>(json);
        //            if (deserializedData != null)
        //            {
        //                var appPath = Utils.GetAppDirectoryPath();

        //                Document.Create(container =>
        //                {
        //                    container.Page(page =>
        //                    {
        //                        page.Header().Text("Coffee");

        //                        page.Content().Table(table =>
        //                        {
        //                            table.ColumnsDefinition(column =>
        //                            {
        //                                column.RelativeColumn();
        //                                column.RelativeColumn();
        //                            });

        //                            table.Header(header =>
        //                            {
        //                                header.Cell().Text("Coffee Name: ");
        //                                header.Cell().Text("Coffee Price: ");
        //                            });

        //                            foreach (var item in deserializedData)
        //                            {
        //                                table.Cell().Text(item.coffeeName.ToString());
        //                                table.Cell().Text(item.coffeePrice.ToString());
        //                            }
        //                        });

        //                        page.Footer().Text(text =>
        //                        {
        //                            text.Span("page :");
        //                            text.CurrentPageNumber();
        //                        });
        //                    });
        //                }).GeneratePdf(Path.Combine(appPath, "Coffee.pdf"));
        //            }
        //        }
        //    }
        //}

        public static void PDFGenerate()
        {
            var filePath = Utils.GetCoffeeFilePath();
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
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
                                page.Header().Text("Title: Coffee");

                                page.Content().Table(table =>
                                {
                                    table.ColumnsDefinition(column =>
                                    {
                                        column.RelativeColumn();
                                        column.RelativeColumn();
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Text("Coffee Name: ");
                                        header.Cell().Text("Coffee Price: ");
                                    });

                                    foreach (var item in deserializedData)
                                    {
                                        table.Cell().Text(item.coffeeName.ToString());
                                        table.Cell().Text(item.coffeePrice.ToString());
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