using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;
using QuestPDF;
using QuestPDF.Fluent;

namespace BisleriumCafe.Data.Services.Export
{
    public static class PDFServices
    {
            // top 5 selling coffees and add in falvoes
            public static string TopSellingPDFFileGenerate()
            {
                try
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
                                    Settings.CheckIfAllTextGlyphsAreAvailable = false;
                                    container.Page(page =>
                                    {
                                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                                        page.Header().Text("\nBislerium Cafe current month top 5 purchase transaction details\n").Bold().FontSize(15);

                                        // Description with current data, time, total income, and top-selling coffee flavor
                                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                                        var coffeeInfo = GetTopFiveCoffeeInfo(deserializedData);
                                        string topFiveAddInsFlavorNames = GetTopFiveAddInsNames(deserializedData);

                                        // Table 1: Top sale revenue details
                                        page.Content().Column(column =>
                                        {
                                            column.Item().Text($"Date : {currentDate.ToString()}").SemiBold();
                                            column.Item().Text("");
                                            column.Item().Table(table =>
                                            {
                                                table.ColumnsDefinition(column =>
                                                {
                                                    column.RelativeColumn();
                                                    column.RelativeColumn();
                                                    column.RelativeColumn();
                                                    column.RelativeColumn();
                                                });

                                                table.Header(header =>
                                                {
                                                    header.Cell().Text("Top selling coffee");
                                                    header.Cell().Text("Total Income");
                                                    header.Cell().Text("Total Quantity");
                                                    header.Cell().Text("Top selling add-ins flavor");
                                                });
                                                table.Cell().Text(coffeeInfo.topFiveCoffee);
                                                table.Cell().Text($"{coffeeInfo.totalCoffeePrice}");
                                                table.Cell().Text($"{coffeeInfo.totalCoffeeQuantity}");
                                                table.Cell().Text(topFiveAddInsFlavorNames.ToString());
                                            });
                                        }
                                        );
                                        page.Footer().Text(text =>
                                        {
                                            text.Span("Page :");
                                            text.CurrentPageNumber();
                                            text.Span("\n");
                                        });
                                    });
                                }).GeneratePdf(Path.Combine(pdfFilePath, "top_purchase_coffee.pdf"));
                                return pdfFilePath;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return null; // Return null to indicate failure or no data
            }

            private static (string topFiveCoffee, int totalCoffeeQuantity, double totalCoffeePrice) GetTopFiveCoffeeInfo(List<CoffeeOrder> orders)
            {
                Dictionary<string, int> coffeeFrequency = new Dictionary<string, int>();

                // Assuming CoffeeOrder has a DateTime property like OrderDate to track the order date
                var currentMonthOrders = orders.Where(order => order.OrderDate.Month == DateTime.Now.Month);

                int totalCoffeeQuantity = 0;
                double totalCoffeePrice = 0.0;

                foreach (var order in currentMonthOrders)
                {
                    // Process coffee names
                    if (coffeeFrequency.ContainsKey(order.CoffeeName))
                    {
                        coffeeFrequency[order.CoffeeName]++;
                    }
                    else
                    {
                        coffeeFrequency.Add(order.CoffeeName, 1);
                    }

                    // Increment total coffee quantity and total coffee price
                    totalCoffeeQuantity += order.TotalQuantity;
                    totalCoffeePrice += order.TotalPrice;
                }

                // Retrieve top 5 coffee names
                List<string> topFiveCoffeeNames = coffeeFrequency.OrderByDescending(pair => pair.Value)
                                                                 .Take(5)
                                                                 .Select(pair => pair.Key)
                                                                 .ToList();

                string topFiveCoffee = string.Join(", ", topFiveCoffeeNames.Select(name => $"{name}"));

                return (topFiveCoffee, totalCoffeeQuantity, totalCoffeePrice);
            }

            // get the top 5 add-ins name
            public static string GetTopFiveAddInsNames(List<CoffeeOrder> orders)
            {
                if (orders == null || orders.Count == 0)
                {
                    return string.Empty;
                }

                // create the new dictionary and also get the coffee name where current month
                Dictionary<string, int> addInsFrequency = new Dictionary<string, int>();
                var currentMonthOrders = orders.Where(order => order.OrderDate.Month == DateTime.Now.Month);
                foreach (var order in currentMonthOrders)
                {
                    if (order.AddFlavorName != null && order.AddFlavorName != "N/S")
                    {
                        if (addInsFrequency.ContainsKey(order.AddFlavorName))
                        {
                            addInsFrequency[order.AddFlavorName]++;
                        }
                        else
                        {
                            addInsFrequency.Add(order.AddFlavorName, 1);
                        }
                    }
                }

                List<string> topFiveAddInsNames = addInsFrequency.OrderByDescending(pair => pair.Value)
                                                                 .Take(5)
                                                                 .Select(pair => pair.Key)
                                                                 .ToList();

                string topFiveAddIns = string.Join(", ", topFiveAddInsNames.Select(name => $"{name}"));
                return topFiveAddIns;
            }


            // all the selling history
            public static string PDFReportFileGenerate()
            {
                try
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
                                    Settings.CheckIfAllTextGlyphsAreAvailable = false;
                                    container.Page(page =>
                                    {
                                        page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                                        page.Header().Text("\n Bislerium Cafe Sale Revenue History Details\n\n").Bold().FontSize(15);
                                        
                                        // Table 2: All the history of sale transactions
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
                                                header.Cell().Text("Coffee Name");
                                                header.Cell().Text("Coffee Price");
                                                header.Cell().Text("Add-In Flavor");
                                                header.Cell().Text("Flavor Price");
                                                header.Cell().Text("Customer Num");
                                                header.Cell().Text("Total Price");
                                                header.Cell().Text("Order Date");
                                            });

                                            foreach (var item in deserializedData)
                                            {
                                                table.Cell().Text(item.CoffeeName?.ToString() ?? "N/S");
                                                table.Cell().Text(item.CoffeePrice.ToString() ?? "N/S");
                                                table.Cell().Text(item.AddFlavorName?.ToString() ?? "N/S");
                                                table.Cell().Text(item.AddFlavorPrice.ToString() ?? "N/S");
                                                table.Cell().Text(item.CustomerNumber.ToString() ?? "N/S");
                                                table.Cell().Text(item.TotalPrice.ToString() ?? "N/S");
                                                table.Cell().Text(item.OrderDate.ToString() ?? "N/S");
                                            }
                                        });
                                        page.Footer().Text(text =>
                                        {
                                            text.Span("Page :");
                                            text.CurrentPageNumber();
                                            text.Span("\n");
                                        });
                                    });
                                }).GeneratePdf(Path.Combine(pdfFilePath, "report_generate.pdf"));
                                return pdfFilePath;
                            }
                        }
                    }
                }
                catch
                {
                    return null; // Return null to exceptions
                }
                return null; // Return null to indicate failure or no data
            }
        }
    }