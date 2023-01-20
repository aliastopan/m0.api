using Microsoft.EntityFrameworkCore;
using m0.api.Entities;

namespace m0.api.Interfaces;

public interface IDbContext
{
    public DbSet<GasRecord> GasRecords { get; }

    int Commit();
}
