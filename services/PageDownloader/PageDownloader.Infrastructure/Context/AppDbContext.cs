using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace PageDownloader.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Page> Pages { get; set; } = null!;
}