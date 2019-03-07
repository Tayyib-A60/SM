using System.Threading.Tasks;
using SkineroMotors.Core;

namespace SkineroMotors.Persistence {
    public class UnitOfWork : IUnitOfWork {
        private SMDbContext _context { get; }
        public UnitOfWork (SMDbContext context) {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            // await _context.Vehicles.Update();
            await _context.SaveChangesAsync();
        }
    }
}