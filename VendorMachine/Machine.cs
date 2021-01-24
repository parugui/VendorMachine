using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VendorMachine.Application.Interfaces;
using VendorMachine.Application.ViewModels;

namespace VendorMachine
{
    class Machine 
    {
        static void Main(string[] args)
        {
            ShowHeader();
            ShowSintaxe();
            ControllerBase controller = new ControllerBase();
            controller.provider.GetService<IMachine>().Initialize();

            Console.Write("  Order a product: ");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(false);
                if (key.Key != ConsoleKey.Escape)
                {
                    StringBuilder sentence = new StringBuilder();
                    sentence.Append(key.KeyChar);
                    sentence.Append(Console.ReadLine());

                    if (sentence.ToString().Split(" ").Length < 2)
                    {
                        ShowInvalidSentence();
                    }
                    else
                    {
                        Arguments CommandLine = new Arguments(sentence.ToString());
                        if (CommandLine.ListCoins.Count == 0 || CommandLine.ListProducts.Count == 0)
                        {
                            ShowInvalidSentence();
                        }
                        else
                        {
                            OrderProduct(CommandLine.ListCoins, CommandLine.ListProducts, sentence.ToString().ToUpper().Contains("CHANGE"));
                        }
                    }

                    Console.Write("  Order another product: ");
                }

            } while (key.Key != ConsoleKey.Escape);
        }

        private static void OrderProduct(List<vmCoin> ListCoins, List<vmProduct> ListProduct, bool isGetChange)
		{
            ControllerBase controller = new ControllerBase();
            var service = controller.provider.GetService<IMachine>();
            try
            {
                
                service.InsertCoins(ListCoins);
                vmMachine machine = service.RequestProduct(ListProduct);
                
                StringBuilder output = new StringBuilder();
                foreach(vmProductOutput outProduct in machine.OutputProducts)
                {
                    output.AppendFormat("{0} ={1} ", outProduct.Name, outProduct.Change.ToString("0.00", new CultureInfo("us-EN", false)));
                }

                if (isGetChange)
                {
                    if (machine.TotalInsertedCoins > machine.TotalRequiredProducts)
                    {
                        machine = service.GetChange();
                        output.AppendFormat(" COINS");
                    }
                    else
                        output.Append(" NO_CHANGE");
                }

                foreach (vmCoin coin in machine.DueChange)
				{
                    output.AppendFormat(" {0}", coin.Coin.ToString("0.00", new CultureInfo("us-EN", false)));
                }
                //Console.Write("");
                Console.WriteLine("  "+ output);
                Console.WriteLine("");

            }
            catch (ValidationException ex)
            {
                Console.WriteLine("  " + ex.Message);
                Console.WriteLine("");
            }
        }

        private static void ShowHeader()
        {
            Console.WriteLine("");
            Console.WriteLine("  Vendor Machine - Dell Interview Exercise");
            Console.WriteLine("  Copyright (c) Dell. All rights reserved.");
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("");
        }

        private static void ShowInvalidSentence()
		{
            Console.WriteLine("");
            Console.WriteLine("  Invalid Sentence.... I can't understand.");
            Console.WriteLine("");
            ShowSintaxe();
        }
        private static void ShowSintaxe()
        {

            Console.WriteLine("  Welcome!!");
            Console.WriteLine("");
            Console.WriteLine("  To Insert some Coins, please provide the values separated by space.");
            Console.WriteLine("");
            Console.WriteLine("  To request a product, please write the name of the product.");
            Console.WriteLine("");
            Console.WriteLine("  If you have change to get, please write the word CHANGE at the end of sentence.");
            Console.WriteLine("");
            Console.WriteLine("  Samples:");
            Console.WriteLine("      0.50 1.00 Coke");
            Console.WriteLine("");
            Console.WriteLine("      1.00 Pastelina CHANGE");
            Console.WriteLine("");
            Console.WriteLine("      0.25 0.05 Pastelina CHANGE");
            Console.WriteLine("");
            Console.WriteLine("      1.00 Pastelina Pastelina Pastelina");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("  Please Press 'Esc' to exit the Machine.");
            Console.WriteLine("");


        }
    }
}
