using System;

namespace WebApplicationExercise.Models.ContractModels
{
    /// <summary>
    /// Contract class for Product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public double Price { get; set; }
    }
}