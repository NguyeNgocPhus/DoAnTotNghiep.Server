using DoAn.Domain.Entities;
using DoAn.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Persistence.Configurations.Configurations;

internal sealed class ImportTemplateConfiguration : IEntityTypeConfiguration<ImportTemplate>
{
    public void Configure(EntityTypeBuilder<ImportTemplate> builder)
    {
        builder.ToTable(TableNames.ImportTemplate);
        builder.HasKey(x => x.Id);
        
        // Each User can have many Permission
        builder.HasMany(e => e.ImportHistories)
            .WithOne(e=>e.ImportTemplate)
            .HasForeignKey(p => p.ImportTemplateId)
            .IsRequired();
    }
}
