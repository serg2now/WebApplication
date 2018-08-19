using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationExercise.Models.EFModels
{
    /// <summary>
    /// Order data model
    /// </summary>
    public class DbOrder
    {
        /// <summary>
        /// Guid id of the order
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Creation date of the order
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Customer Name
        /// </summary>
        [MaxLength(30)]
        public string Customer { get; set; }

        /// <summary>
        /// List of products in order
        /// </summary>
        public List<DbProduct> Products { get; set; }
    }
}