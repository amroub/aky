namespace Diatly.Foundation.Test.Domain
{
    using global::Diatly.Foundation.Repository.EF;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CatalogContext : BaseDataContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Culture> Cultures { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<FieldText> FieldTexts { get; set; }

        public DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(this.ConfigureProduct);
            builder.Entity<Culture>(this.ConfigureCulture);
            builder.Entity<Field>(this.ConfigureField);
            builder.Entity<FieldText>(this.ConfigureFieldText);
            builder.Entity<Template>(this.ConfigureTemplate);
        }

        private void ConfigureProduct(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.Description).IsRequired(false);

            builder.Property(e => e.CategoryID)
                .IsRequired();

            builder.Property(e => e.BrandID)
                .IsRequired();

            builder.Property(e => e.AvailableStock)
                .IsRequired();

            builder.Property(e => e.Price)
                .IsRequired();

            builder.HasOne(a => a.Category).WithMany(b => b.Products).HasForeignKey(c => c.CategoryID).OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureCategory(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.Name)
                .IsRequired();
        }

        private void ConfigureTemplate(EntityTypeBuilder<Template> builder)
        {
            builder.ToTable("Template");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
            builder.Property(x => x.EventCode).HasColumnName(@"EventCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            builder.Property(x => x.TemplatePathId).HasColumnName(@"TemplatePathId").HasColumnType("int").IsRequired();
            builder.Property(x => x.SubjectId).HasColumnName(@"SubjectId").HasColumnType("int").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.Subject).WithMany(b => b.TemplatesSubjectId).HasForeignKey(c => c.SubjectId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.TemplatePath).WithMany(b => b.TemplatesTemplatePathId).HasForeignKey(c => c.TemplatePathId).OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureField(EntityTypeBuilder<Field> builder)
        {
            builder.ToTable("Field");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
        }

        private void ConfigureFieldText(EntityTypeBuilder<FieldText> builder)
        {
            builder.ToTable("FieldText");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.FieldId).HasColumnName(@"FieldId").HasColumnType("int").IsRequired();
            builder.Property(x => x.Value).HasColumnName(@"Value").HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(x => x.CultureId).HasColumnName(@"CultureId").HasColumnType("int").IsRequired();

            // Foreign keys
            builder.HasOne(a => a.Culture).WithMany(b => b.FieldTexts).HasForeignKey(c => c.CultureId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Field).WithMany(b => b.FieldTexts).HasForeignKey(c => c.FieldId).OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureCulture(EntityTypeBuilder<Culture> builder)
        {
            builder.ToTable("Culture");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.CultureName).HasColumnName(@"CultureName").HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            builder.Property(x => x.LanguageCode).HasColumnName(@"LanguageCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            builder.Property(x => x.CultureCode).HasColumnName(@"CultureCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
        }
    }
}
