using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace SyncElastic.Infrastructure.Contexts;

public class TagExtractorDbContext(DbContextOptions<TagExtractorDbContext> options) : DbContext(options)
{
    public DbSet<LinkTags> Tags { get; set; } = null!;
}