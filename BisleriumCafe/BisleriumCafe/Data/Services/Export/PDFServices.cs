using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;
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
                                container.Page(page =>
                                {
                                    page.Header().Text("\nBislerium Cafe Sale current month top 5 sale transaction Details\n");

                                    // Description with current data, time, total income, and top-selling coffee flavor
                                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    string totalIncome = CalculateTotalIncome(deserializedData).ToString();
                                    string topFiveCoffeeNames = GetTopFiveCoffeeNames(deserializedData);
                                    string topFiveAddInsFlavorNames = GetTopFiveAddInsNames(deserializedData);

                                    // Table 1: Top sale revenue details
                                    page.Content().Table(table =>
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
                                            header.Cell().Text("Current Date");
                                            header.Cell().Text("Total Income");
                                            header.Cell().Text("Top selling coffee");
                                            header.Cell().Text("Top selling add-ins flavor");
                                        });
                                        table.Cell().Text(currentDate.ToString());
                                        table.Cell().Text(totalIncome.ToString());
                                        table.Cell().Text(topFiveCoffeeNames.ToString());
                                        table.Cell().Text(topFiveAddInsFlavorNames.ToString());
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
            }
            catch
            {
                return null; // Return null to exceptions
            }
            return null; // Return null to indicate failure or no data
        }

        public static double CalculateTotalIncome(List<CoffeeOrder> orders)
        {
            double totalIncome = 0;
            foreach (var order in orders)
            {
                totalIncome += order.TotalPrice;
            }
            return totalIncome;
        }

        public static string GetTopFiveCoffeeNames(List<CoffeeOrder> orders)
        {
            Dictionary<string, int> coffeeFrequency = new Dictionary<string, int>();

            // Assuming CoffeeOrder has a DateTime property like OrderDate to track the order date
            var currentMonthOrders = orders.Where(order => order.OrderDate.Month == DateTime.Now.Month);

            foreach (var order in currentMonthOrders)
            { 
                if (coffeeFrequency.ContainsKey(order.CoffeeName))
                {
                    coffeeFrequency[order.CoffeeName]++;
                }
                else
                {
                    coffeeFrequency.Add(order.CoffeeName, 1);
                }
            }

            List<string> topFiveCoffeeNames = coffeeFrequency.OrderByDescending(pair => pair.Value)
                                                             .Take(5)
                                                             .Select(pair => pair.Key)
                                                             .ToList();

            string topFiveCoffee = string.Join(", ", topFiveCoffeeNames.Select(name => $"{name}"));
            return topFiveCoffee;
        }

        public static string GetTopFiveAddInsNames(List<CoffeeOrder> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                return string.Empty;
            }

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
        public static string PDFFileOrderHistoryGenerate()
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
                                container.Page(page =>
                                {
                                    page.Header().Text("\n Bislerium Cafe Sale Revenue History Details\n");

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
                            }).GeneratePdf(Path.Combine(pdfFilePath, "sale_transactions.pdf"));
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