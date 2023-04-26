namespace aky.EmailService.Infrastructure.EFConfiguration
{
    using aky.EmailService.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FieldTextEntityTypeConfiguration : IEntityTypeConfiguration<FieldText>
    {
        public FieldTextEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<FieldText> customerConfiguration)
        {
            customerConfiguration.ToTable("FieldText");
            customerConfiguration.HasKey(x => x.Id);

            customerConfiguration.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().UseSqlServerIdentityColumn();
            customerConfiguration.Property(x => x.FieldId).HasColumnName(@"FieldId").HasColumnType("int").IsRequired();
            customerConfiguration.Property(x => x.Value).HasColumnName(@"Value").HasColumnType("nvarchar(max)").IsRequired();
            customerConfiguration.Property(x => x.CultureId).HasColumnName(@"CultureId").HasColumnType("int").IsRequired();

            // Foreign keys
            customerConfiguration.HasOne(a => a.Culture).WithMany(b => b.FieldTexts).HasForeignKey(c => c.CultureId).OnDelete(DeleteBehavior.Cascade);
            customerConfiguration.HasOne(a => a.Field).WithMany(b => b.FieldTexts).HasForeignKey(c => c.FieldId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
