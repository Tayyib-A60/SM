using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {   
        private SMDbContext _context { get; }
        public PhotoRepository(SMDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {
            return await _context.Photos
            .Where(p => p.VehicleId == vehicleId)
            .ToListAsync();
        }
        public async Task<Photo> GetPhoto(int id, int vehicleId)
        {
            var result = _context.Photos
            .SingleOrDefaultAsync(p => (p.Id == id) && (p.VehicleId == vehicleId));
            return await result;
        }
        // public async Task<Photo> GetPhoto(int id)
        // {
        //     var result = _context.Photos
        //     .SingleOrDefaultAsync(p => (p.Id == id));
        //     return await result;
        // }
        public void Remove(Photo photo)
        {
            _context.Remove(photo);
        }
    }
}