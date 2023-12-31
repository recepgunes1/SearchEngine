using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace SyncElastic.Infrastructure.Contexts;

public class PageDownloaderDbContext(DbContextOptions<PageDownloaderDbContext> options) : DbContext(options)
{
    public DbSet<Page> Pages { get; set; } = null!;
}