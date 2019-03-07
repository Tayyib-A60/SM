using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Persistence {
    public class ContactUsRepository : IContactUsRepository {
        private SMDbContext _context { get; }
        public ContactUsRepository (SMDbContext context) {
            _context = context;

        }
        public async Task<ICollection<ContactUs>> GetContacts ()
        {
            var result = _context.ContactUs
                            .ToListAsync();
            return await result;
        }
        public async  Task<ContactUs> GetContact(int id)
        {
            return await _context.ContactUs
                    .SingleOrDefaultAsync(c  => c.Id == id);
        }
        public void Add (ContactUs contactUs) 
        {
            _context.Add(contactUs);
        }
    }
}