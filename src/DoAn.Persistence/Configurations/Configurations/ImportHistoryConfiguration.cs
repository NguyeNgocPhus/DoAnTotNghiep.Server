using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
using DoAn.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Persistence.Configurations.Configurations;

internal sealed class ImportHistoryConfiguration : IEntityTypeConfiguration<ImportHistory>
{
    public void Configure(EntityTypeBuilder<ImportHistory> builder)
    {
        builder.ToTable(TableNames.ImportHistory);

        builder.HasKey(x => x.Id);
      
    }
}
