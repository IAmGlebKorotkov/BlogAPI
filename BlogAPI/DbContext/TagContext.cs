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
            entity.ToTable("tag", "fias"); // Указываем схему и имя таблицы
            entity.HasKey(e => e.Id); // Указываем ключ
            entity.Property(e => e.Id)
                .HasColumnName("id") // Указываем имя столбца в базе данных
                .HasColumnType("uuid"); // Указываем тип столбца в базе данных

            entity.Property(e => e.CreateTime).HasColumnName("createtime");
            entity.Property(e => e.Name).HasColumnName("name");
        });
    }
}