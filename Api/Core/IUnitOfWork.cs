using System.Threading.Tasks;

namespace SkineroMotors.Core
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}