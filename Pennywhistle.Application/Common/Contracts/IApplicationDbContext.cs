using Microsoft.EntityFrameworkCore;
using Pennywhistle.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Common.Contracts
{
    /// <summary>
    /// DB context contract
    /// </summary>
    public interface IApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
