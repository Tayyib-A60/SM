using System.Collections.Generic;

namespace SkineroMotors.Controllers.Resources
{
    public class QueryResultResource<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Vehicles { get; set; }
    }
}