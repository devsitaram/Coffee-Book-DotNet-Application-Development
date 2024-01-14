﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BisleriumCafe.Data.Models;

namespace BisleriumCafe.Data.Services
{
	public class CustomerService
	{
		// new customer add
		public static string CreateCustomer(long customerNumber)
		{
			try
			{
				List<Customer> listOfCustomer = GetAllCustomer();
				bool existingCustomer = listOfCustomer.Any(x => x.CustomerNumber == customerNumber);

				if (existingCustomer)
				{
					// Customer with the same name already exists, update its price
					return "Customer is already exists.";
				}
				else
				{
					// Customer doesn't exist, add a new one
					listOfCustomer.Add(new Customer
					{
						CustomerNumber = customerNumber,
					});
					SaveAllCustomer(listOfCustomer);
					return "success";
				}
			}
			catch (Exception ex)
			{
				return $"Error in CreateOrUpdateCustomer: {ex.Message}";
			}
		}

		public static List<Customer> GetAllCustomer()
		{
			try
			{
                string CustomerFilePath = Utils.GetCustomersFilePath();
                if (!File.Exists(CustomerFilePath))
                {
                    return new List<Customer>();
                }
                var json = File.ReadAllText(CustomerFilePath);
                return JsonSerializer.Deserialize<List<Customer>>(json);
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new List<Customer>();
			}
			
		}

		public static Customer GetByCustomerNumber(long customerNumber)
		{
			List<Customer> listOfCustomer = GetAllCustomer();
			return listOfCustomer.FirstOrDefault(x => x.CustomerNumber == customerNumber);
		}

		public static bool UpdateCustomer(long CustomerNumber)
		{
			try
			{
				List<Customer> listOfCustomer = GetAllCustomer();
				Customer existingCustomer = listOfCustomer.FirstOrDefault(x => x.CustomerNumber == CustomerNumber);

				if (existingCustomer != null)
				{
					existingCustomer.Frequency += 1;
					SaveAllCustomer(listOfCustomer);
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static int DiscountForCustomer(long CustomerNumber)
		{
			try
			{
				var existingCustomer = GetByCustomerNumber(CustomerNumber);
				if (existingCustomer != null)
				{
					return existingCustomer.Frequency;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception ex)
			{
				return 0;
			}
		}

		private static void SaveAllCustomer(List<Customer> Customer)
		{
			try
			{
                string CustomerFilePath = Utils.GetCustomersFilePath();
                string appCustomerFilePath = Utils.GetAppDirectoryPath();

                if (!Directory.Exists(appCustomerFilePath))
				{
					Directory.CreateDirectory(appCustomerFilePath);
				}

				var json = JsonSerializer.Serialize(Customer);

				// Write the JSON content to the file
				using (StreamWriter streamWriter = File.CreateText(CustomerFilePath))
				{
					streamWriter.Write(json);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error while saving Customer data: {ex.Message}");
			}
		}
	}
}
