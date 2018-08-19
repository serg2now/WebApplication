using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplicationExercise.Models.ContractModels;
using WebApplicationExercise.Models.EFModels;

namespace WebApplicationExercise.Converters
{
#pragma warning disable 618

    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly string baseServiceUrl;
        private string error;

        public CurrencyConverter()
        {
            baseServiceUrl = ConfigurationSettings.AppSettings["currencyConverterUrl"];
        }

        public string ConvertPrices(List<DbProduct> products, string targetCurrency)
        {
            error = string.Empty;
            double currency = GetCurrencyAsync(targetCurrency).Result;

            if (currency > 0)
            {
                products.ForEach(p =>
                {
                    p.Price = Math.Round(p.Price * currency, 2);
                });
            }

            return error;
        }

        public string ConvertPrices(List<DbOrder> orders, string targetCurrency)
        {
            error = string.Empty;
            double currency = GetCurrencyAsync(targetCurrency).Result;

            if (currency > 0)
            {
                orders.ForEach(o =>
                {
                    o.Products.ForEach(p =>
                    {
                        p.Price = Math.Round(p.Price * currency, 2);
                    });
                });
            }

            return error;
        }

        private async Task<double> GetCurrencyAsync(string targetCurrency)
        {
            string request = $"{baseServiceUrl}convert?q=USD_{targetCurrency}&compact=ultra";

            double currency = 0;

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var currencyString = await response.Content.ReadAsStringAsync();

                    var currencyObject = JsonConvert.DeserializeObject<Dictionary<string, double>>(currencyString);

                    currency = currencyObject[$"USD_{targetCurrency}"];
                }
            }
            catch (KeyNotFoundException kne)
            {
                error = $"Currency {targetCurrency} does not exist!";
            }
            catch (Exception e)
            {
                error = "Currency loading failed!";
            }

            return currency;
        }
    }
}