using BlogAPI.Models;
using BlogAPI.Models.Community;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DbContext;


public class CommunityContext : Microsoft.EntityFrameworkCore.DbContext
{
    public CommunityContext(DbContextOptions<CommunityContext> options) : base(options)
    {
    }

    public DbSet<CommunityDto> Communities { get; set; }
    public DbSet<CommunityUserDto> CommunityUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommunityDto>(entity =>
        {
            entity.ToTable("community", "fias"); 
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsClosed).HasColumnName("is_closed");
            entity.Property(e => e.SubscribersCount).HasColumnName("subscribers_count");
        });
        
        modelBuilder.Entity<CommunityUserDto>(entity =>
        {
            entity.ToTable("user_community", "fias");
            entity.HasKey(e => new { e.UserId, e.CommunityId });
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CommunityId).HasColumnName("community_id");
            entity.Property(e => e.Role).HasColumnName("role");


            entity.HasOne(uc => uc.User)
                .WithMany()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(uc => uc.Community)
                .WithMany()
                .HasForeignKey(uc => uc.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<UserDto>(entity =>
        {
            entity.ToTable("AspNetUsers", "public"); 
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.FullName).HasColumnName("FullName");
            entity.Property(e => e.Email).HasColumnName("Email");
        });
    }
}