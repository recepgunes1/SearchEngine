using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace SearchHub.Infrastructure.Context;

public class TagExtractorDbContext(DbContextOptions<TagExtractorDbContext> options) : DbContext(options)
{
    public DbSet<LinkTags> Tags { get; set; } = null!;
}