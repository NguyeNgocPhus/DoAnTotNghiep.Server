using Azure.Core;
using DoAn.Domain.Entities;
using DoAn.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Persistence.Configurations.Configurations;

public class FileStorageConfiguration : IEntityTypeConfiguration<FileStorage>
{
    public void Configure(EntityTypeBuilder<FileStorage> builder)
    {
        builder.ToTable(TableNames.FileStorages);
        // Define keys + index
        builder.HasKey(u => new { u.Id });
        builder.Property(u => u.Code).ValueGeneratedOnAdd();


        builder.HasIndex(u => u.Name).IsUnique();

        // Define other fields
        builder.Property(u => u.Status)
            .IsRequired().IsUnicode()
            ;


        builder.Property(t => t.Data).IsUnicode().IsRequired().HasColumnType("varbinary(max)");
    }
}