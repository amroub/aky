namespace Ordering.Infrastructure
{
    using aky.EmailService.Domain.Entities;
    using aky.EmailService.Infrastructure.EFConfiguration;
    using aky.Foundation.Repository.EF;
    using Microsoft.EntityFrameworkCore;

    public class EmailDbContext : BaseDataContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options)
            : base(options)
        {
        }

        public DbSet<Culture> Cultures { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<FieldText> FieldTexts { get; set; }

        public DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CultureEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FieldEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FieldTextEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TemplateEntityTypeConfiguration());
        }
    }
}