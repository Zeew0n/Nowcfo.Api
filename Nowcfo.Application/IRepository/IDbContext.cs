using System.Threading;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface  IDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
