using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace TagExtractor.Infrastructure.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<LinkTags> Tags { get; set; } = null!;
}