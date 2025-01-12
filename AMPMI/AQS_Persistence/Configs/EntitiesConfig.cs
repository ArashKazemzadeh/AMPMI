using AQS_Domin.Entities;
using Domin.Entities;
using Microsoft.EntityFrameworkCore;

namespace AQS_Persistence.Configs;

public static class EntitiesConfig
{
    public static void FluentAPI_Config_Entities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPicture>(entity =>
        {
            //entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BlogId).HasColumnName("Blog_Id");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogPictures)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_BlogPicture_Blog");
        });

        modelBuilder.Entity<CompanyPicture>(entity =>
        {

            //entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyPictures)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_CompanyPicture_Company");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.Company).WithMany(p => p.Products)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Products_Company");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("FK_Products_SubCategory");
        });

        modelBuilder.Entity<SeenNotifByCompany>(entity =>
        {
            entity.HasOne(d => d.Company).WithMany(p => p.SeenNotifByCompanies)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_SeenNotifByCompany_Company");

            entity.HasOne(d => d.Notification).WithMany(p => p.SeenNotifByCompanies)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("FK_SeenNotifByCompany_Notification");
        });

        modelBuilder.Entity<SiteAdmin>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });
        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });
        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_SubCategory_Category");
        });
        modelBuilder.Entity<MiscellaneousData>(entity =>
        {
            entity.HasKey(e => e.Key);

            entity.Property(m => m.Value)
                .HasColumnType("nvarchar(max)");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .ValueGeneratedNever();
        });
    }
}
