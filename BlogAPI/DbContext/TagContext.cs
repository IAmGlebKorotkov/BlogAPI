using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DbContext;

public class TagContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TagContext(DbContextOptions<TagContext> options) : base(options)
    {
    }

    public DbSet<TagDto> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TagDto>(entity =>
        {
            entity.ToTable("tag", "fias"); 
            entity.HasKey(e => e.Id); 
            entity.Property(e => e.Id)
                .HasColumnName("id") 
                .HasColumnType("uuid"); 

            entity.Property(e => e.CreateTime).HasColumnName("createtime");
            entity.Property(e => e.Name).HasColumnName("name");
        });
    }
}