using System;
using System.Collections.Generic;

namespace WebApplicationExercise.Models.ContractModels
{
    /// <summary>
    /// Contract class for Order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Customer Name
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Creation date of the order
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// List of products in order
        /// </summary>
        public List<Product> Products { get; set; }
    }
}