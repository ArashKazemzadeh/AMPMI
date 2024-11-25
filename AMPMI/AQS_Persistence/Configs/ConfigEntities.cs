using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Persistence.Configs;

public class ConfigEntities
{
    public static void FluentAPI_Config_Entities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Subject).HasMaxLength(200);
        });

        modelBuilder.Entity<BlogPicture>(entity =>
        {
            entity.ToTable("BlogPicture");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BlogId).HasColumnName("Blog_Id");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogPictures)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_BlogPicture_Blog");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(400);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TeaserGuid).HasMaxLength(200);
        });

        modelBuilder.Entity<CompanyPicture>(entity =>
        {
            entity.ToTable("CompanyPicture");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CompanyId).HasColumnName("Company_Id");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyPictures)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_CompanyPicture_Company");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Subject).HasMaxLength(200);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Brands).HasMaxLength(4000);
            entity.Property(e => e.CompanyId).HasColumnName("Company_Id");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(400);
            entity.Property(e => e.SubCategoryId).HasColumnName("SubCategory_Id");

            entity.HasOne(d => d.Company).WithMany(p => p.Products)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Products_Company");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("FK_Products_SubCategory1");
        });

        modelBuilder.Entity<SeenNotifByCompany>(entity =>
        {
            entity.ToTable("SeenNotifByCompany");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CompanyId).HasColumnName("Company_Id");
            entity.Property(e => e.NotificationId).HasColumnName("Notification_Id");

            entity.HasOne(d => d.Company).WithMany(p => p.SeenNotifByCompanies)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_SeenNotifByCompany_Company");

            entity.HasOne(d => d.Notification).WithMany(p => p.SeenNotifByCompanies)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK_SeenNotifByCompany_Notification");
        });

        modelBuilder.Entity<SiteAdmin>(entity =>
        {
            entity.ToTable("SiteAdmin");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.ToTable("SubCategory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CategoryId).HasColumnName("Category_Id");
            entity.Property(e => e.Name).HasMaxLength(400);

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_SubCategory_Category");
        });
    }
}