using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace SearchHub.Infrastructure.Context;

public class PageDownloaderDbContext(DbContextOptions<PageDownloaderDbContext> options) : DbContext(options)
{
    public DbSet<Page> Pages { get; set; } = null!;
}
