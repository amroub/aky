namespace aky.EmailService.Infrastructure.EFConfiguration
{
    using aky.EmailService.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FieldEntityTypeConfiguration : IEntityTypeConfiguration<Field>
    {
        public FieldEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Field> orderItemConfiguration)
        {
            orderItemConfiguration.ToTable("Field");
            orderItemConfiguration.HasKey(x => x.Id);

            orderItemConfiguration.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().UseSqlServerIdentityColumn();
            orderItemConfiguration.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
        }
    }
}
