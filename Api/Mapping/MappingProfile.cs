using AutoMapper;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Vehicle, VehicleResource>();
            CreateMap<Vehicle, SaveVehicleResource>();
            CreateMap<Photo, PhotoResource>();
            CreateMap<ContactUs, ContactUsResource>();
            CreateMap<User, UserResource>();

            // API to Domain Resource
            CreateMap<VehicleResource, Vehicle>();
            CreateMap<ContactUsResource, ContactUs>();
            CreateMap<SaveVehicleResource, Vehicle>()
            .ForMember(v => v.Id, opt => opt.Ignore());
            CreateMap<UserResource, User>();
        }
    }
}