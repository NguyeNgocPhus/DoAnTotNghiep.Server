using DoAn.Domain.Entities.Identity;
using DoAn.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Persistence.Configurations.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permissions);

        builder.HasKey(x => new { x.RoleId, x.FunctionId, x.ActionId });
    }
}
