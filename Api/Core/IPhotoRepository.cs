using System.Collections.Generic;
using System.Threading.Tasks;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Core
{
    public interface IPhotoRepository
    {
         Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
         Task<Photo> GetPhoto(int id, int vehicleId);
        //  Task<Photo> GetPhoto(int id);
         void Remove(Photo photo);
    }
}