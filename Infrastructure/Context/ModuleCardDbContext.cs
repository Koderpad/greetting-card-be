using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public partial class ModuleCardDbContext : DbContext
{
    public ModuleCardDbContext()
    {
    }

    public ModuleCardDbContext(DbContextOptions<ModuleCardDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<SampleGreeting> SampleGreetings { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TemplateCard> TemplateCards { get; set; }

    public virtual DbSet<UserCard> UserCards { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<UserUpload> UserUploads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07F05E199F");

            entity.ToTable("Category");

            entity.Property(e => e.Id)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ParentId)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Category_Category");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07B2B18EAA");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.FamilyId)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RefreshToken_UserInfo");
        });

        modelBuilder.Entity<SampleGreeting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SampleGr__3214EC07E9D5B85D");

            entity.ToTable("SampleGreeting");

            entity.Property(e => e.Id)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CategoryId)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.SampleGreetings)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_SampleGreeting_Category");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC07C66665EF");

            entity.ToTable("Tag");

            entity.Property(e => e.Id)
                .HasMaxLength(26)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TemplateCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Template__3214EC07FAE607CF");

            entity.ToTable("TemplateCard");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.ImageLink).HasMaxLength(2048);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasMany(d => d.Categories).WithMany(p => p.TemplateCards)
                .UsingEntity<Dictionary<string, object>>(
                    "TemplateCardCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TemplateCard_Category_Category"),
                    l => l.HasOne<TemplateCard>().WithMany()
                        .HasForeignKey("TemplateCardId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TemplateCard_Category_TemplateCard"),
                    j =>
                    {
                        j.HasKey("TemplateCardId", "CategoryId").HasName("PK__Template__02D134E73F31D97B");
                        j.ToTable("TemplateCard_Category");
                        j.IndexerProperty<string>("TemplateCardId")
                            .HasMaxLength(36)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("CategoryId")
                            .HasMaxLength(26)
                            .IsUnicode(false);
                    });

            entity.HasMany(d => d.Tags).WithMany(p => p.TemplateCards)
                .UsingEntity<Dictionary<string, object>>(
                    "TemplateCardTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TemplateCard_Tag_Tag"),
                    l => l.HasOne<TemplateCard>().WithMany()
                        .HasForeignKey("TemplateCardId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TemplateCard_Tag_TemplateCard"),
                    j =>
                    {
                        j.HasKey("TemplateCardId", "TagId").HasName("PK__Template__751668DDCCB33A50");
                        j.ToTable("TemplateCard_Tag");
                        j.IndexerProperty<string>("TemplateCardId")
                            .HasMaxLength(36)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("TagId")
                            .HasMaxLength(26)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<UserCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserCard__3214EC07C85ECD6C");

            entity.ToTable("UserCard");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.ImageLink).HasMaxLength(2048);
            entity.Property(e => e.TemplateCardId)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);

            entity.HasOne(d => d.TemplateCard).WithMany(p => p.UserCards)
                .HasForeignKey(d => d.TemplateCardId)
                .HasConstraintName("FK_UserCard_TemplateCard");

            entity.HasOne(d => d.User).WithMany(p => p.UserCards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserCard_UserInfo");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3214EC073F729C92");

            entity.ToTable("UserInfo");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserUplo__3214EC074E581694");

            entity.ToTable("UserUpload");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.FileLink).HasMaxLength(2048);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserUploads)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserUpload_UserInfo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
