using System.Collections.Generic;

namespace SkineroMotors.Core.Models
{
    public class QueryResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Vehicles { get; set; }
    }
}