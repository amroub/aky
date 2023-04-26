namespace aky.EmailService.Infrastructure.EFConfiguration
{
    using aky.EmailService.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TemplateEntityTypeConfiguration : IEntityTypeConfiguration<Template>
    {
        public TemplateEntityTypeConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<Template> customerConfiguration)
        {
            customerConfiguration.ToTable("Template");
            customerConfiguration.HasKey(x => x.Id);

            customerConfiguration.Property(x => x.Id).HasColumnName(@"Id").HasColumnType("int").IsRequired().UseSqlServerIdentityColumn();
            customerConfiguration.Property(x => x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").IsRequired().HasMaxLength(250);
            customerConfiguration.Property(x => x.EventCode).HasColumnName(@"EventCode").HasColumnType("nvarchar").IsRequired().HasMaxLength(500);
            customerConfiguration.Property(x => x.TemplatePathId).HasColumnName(@"TemplatePathId").HasColumnType("int").IsRequired();
            customerConfiguration.Property(x => x.SubjectId).HasColumnName(@"SubjectId").HasColumnType("int").IsRequired();

            // Foreign keys
            customerConfiguration.HasOne(a => a.Subject).WithMany(b => b.TemplatesSubjectId).HasForeignKey(c => c.SubjectId).OnDelete(DeleteBehavior.Cascade);
            customerConfiguration.HasOne(a => a.TemplatePath).WithMany(b => b.TemplatesTemplatePathId).HasForeignKey(c => c.TemplatePathId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
