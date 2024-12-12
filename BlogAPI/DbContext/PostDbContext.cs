using BlogAPI.Models;
using BlogAPI.Models.Post;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.DbContext;

public class PostDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
        {
        }

        public DbSet<PostDto> Posts { get; set; }
        public DbSet<TagDto> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostDto>(entity =>
            {
                entity.ToTable("post", "fias");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CreateTime).HasColumnName("create_time");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.ReadingTime).HasColumnName("reading_time");
                entity.Property(e => e.Image).HasColumnName("image");
                entity.Property(e => e.AuthorId).HasColumnName("author_id");
                entity.Property(e => e.CommunityId).HasColumnName("community_id");
                entity.Property(e => e.CommunityName).HasColumnName("community_name");
                entity.Property(e => e.AddressId).HasColumnName("address_id");
                entity.Property(e => e.Likes).HasColumnName("likes");
                entity.Property(e => e.HasLike).HasColumnName("has_like");
                entity.Property(e => e.CommentsCount).HasColumnName("comments_count");
            });

            modelBuilder.Entity<TagDto>(entity =>
            {
                entity.ToTable("tag", "fias");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<PostTag>(entity =>
            {
                entity.ToTable("post_tag", "fias");
                entity.HasKey(e => new { e.PostId, e.TagId });
                entity.Property(e => e.PostId).HasColumnName("post_id");
                entity.Property(e => e.TagId).HasColumnName("tag_id");
            });
        }
    }