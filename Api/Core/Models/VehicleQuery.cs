using SkineroMotors.Extensions;

namespace SkineroMotors.Core.Models
{
    public class VehicleQuery : IQueryObject
    {
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
        public string Search { get; set; }
    }
}