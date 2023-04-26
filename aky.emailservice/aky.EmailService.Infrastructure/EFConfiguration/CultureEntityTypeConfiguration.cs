namespace aky.EmailService.Infrastructure.EFConfiguration
{
    using aky.EmailService.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CultureEntityTypeConfiguration : IEntityTypeConfiguration<Culture>
    {
        public CultureEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Culture> customerConfiguration)
        {
            customerConfiguration.ToTable("Culture");
            customerConfiguration.HasKey(x => x.Id);

            customerConfiguration.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().UseSqlServerIdentityColumn();
            customerConfiguration.Property(x => x.CultureName).HasColumnName(@"CultureName").HasColumnType("nvarchar").IsRequired().HasMaxLength(100);
            customerConfiguration.Property(x => x.LanguageCode).HasColumnName(@"LanguageCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
            customerConfiguration.Property(x => x.CultureCode).HasColumnName(@"CultureCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(10);
        }
    }
}
