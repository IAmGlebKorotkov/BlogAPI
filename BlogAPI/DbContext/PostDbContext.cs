using BlogAPI.Models;
using BlogAPI.Models.Author;
using BlogAPI.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DbContext;
public class PostContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<UserLike> UserLikes { get; set; }
    public PostContext(DbContextOptions<PostContext> options) : base(options)
    {
    }

    public DbSet<PostDto> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostDto>(entity =>
        {
            entity.ToTable("post", "fias");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(p => p.CreateTime).HasColumnName("create_time").IsRequired();
            entity.Property(p => p.Title).HasColumnName("title").IsRequired();
            entity.Property(p => p.Description).HasColumnName("description").IsRequired();
            entity.Property(p => p.ReadingTime).HasColumnName("reading_time").IsRequired();
            entity.Property(p => p.Image).HasColumnName("image");
            entity.Property(p => p.AddressId).HasColumnName("address_id");
            entity.Property(p => p.AuthorId).HasColumnName("author_id").IsRequired();
            entity.Property(p => p.Author).HasColumnName("author").IsRequired();
            entity.Property(p => p.CommunityId).HasColumnName("community_id"); 
            entity.Property(p => p.CommunityName).HasColumnName("community_name");
            entity.Property(p => p.TagPosts).HasColumnName("tag_posts").IsRequired();
            entity.Property(p => p.Likes).HasColumnName("likes").IsRequired();
            entity.Property(p => p.HasLike).HasColumnName("has_like").IsRequired();
            entity.Property(p => p.CommentsCount).HasColumnName("comments_count").IsRequired();
        });
        modelBuilder.Entity<UserLike>(entity =>
        {
            entity.ToTable("user_like", "fias");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.PostId).IsRequired();

            // Настройка внешних ключей
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Post)
                .WithMany()
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
} 