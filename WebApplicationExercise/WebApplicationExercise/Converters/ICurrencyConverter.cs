using System.Collections.Generic;
using WebApplicationExercise.Models.ContractModels;
using WebApplicationExercise.Models.EFModels;

namespace WebApplicationExercise.Converters
{
    public interface ICurrencyConverter
    {
        string ConvertPrices(List<DbProduct> products, string targetCurrency);

        string ConvertPrices(List<DbOrder> orders, string targetCurrency);
    }
}
