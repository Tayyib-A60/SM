namespace SkineroMotors.Controllers.Resources
{
    public class VehicleQueryResource
    {
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; } 
    }
}