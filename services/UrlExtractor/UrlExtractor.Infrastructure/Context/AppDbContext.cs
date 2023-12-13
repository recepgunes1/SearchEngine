using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace UrlExtractor.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ExtractedUrl> ExtractedUrls { get; set; } = null!;
}