using System.Collections.Generic;
using System.Threading.Tasks;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Core
{
    public interface IContactUsRepository
    {
          void Add(ContactUs contactUs);
          Task<ContactUs> GetContact(int id);
          Task<ICollection<ContactUs>> GetContacts();
    }
}