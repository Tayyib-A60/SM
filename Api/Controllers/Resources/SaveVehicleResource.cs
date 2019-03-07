using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SkineroMotors.Controllers.Resources
{
    public class SaveVehicleResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CostPrice { get; set; }
        public int SellingPrice { get; set; }
        public int? Discount { get; set; }
        public bool? Featured { get; set; }
        public ICollection<PhotoResource> Photos { get; set; }
        public SaveVehicleResource()
        {
            Photos = new Collection<PhotoResource>();
        }
    }
}