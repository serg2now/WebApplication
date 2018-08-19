using System.Linq;
using System.Text;
using System.Linq.Dynamic;

namespace WebApplicationExercise.Repositories
{
    public static class SortingExtension
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sortString)
        {
            var lstSort = sortString.Replace(" ",string.Empty).Split(',');

            var sortExpression = new StringBuilder();

            foreach (var option in lstSort)
            {
                sortExpression.Append(option.StartsWith("-") ? $"{option.Remove(0, 1)} descending," : $"{option},");
            }

            var queryString = sortExpression.Remove(sortExpression.Length - 1, 1).ToString();

            return (!string.IsNullOrWhiteSpace(queryString)) ? source.OrderBy(queryString) : source;
        }
    }
}