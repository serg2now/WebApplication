using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationExercise.Models.EFModels
{
    /// <summary>
    /// Product model
    /// </summary>
    public class DbProduct
    {
        /// <summary>
        /// Guid id of the product
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public double Price { get; set; }
    }
}