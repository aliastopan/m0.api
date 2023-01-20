using Microsoft.EntityFrameworkCore;
using m0.api.Entities;

namespace m0.api.Services;

internal sealed class ApplicationDbContext : DbContext, IDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<GasRecord> GasRecords => Set<GasRecord>();

    public int Commit()
    {
        return SaveChanges();
    }
}