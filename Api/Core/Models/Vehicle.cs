using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SkineroMotors.Core.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        public int CostPrice { get; set; }
        public DateTime LastUpdate { get; set; }
        [Required]
        public int SellingPrice { get; set; }
        public int? Discount { get; set; }
        public bool? Featured { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public Vehicle()
        {
            Photos = new Collection<Photo>();
        }

    }
}