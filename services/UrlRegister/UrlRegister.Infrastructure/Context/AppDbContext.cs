using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace UrlRegister.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<RegisteredUrl> RegisteredUrls { get; set; } = null!;
}