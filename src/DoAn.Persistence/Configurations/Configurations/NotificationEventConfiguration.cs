using DoAn.Domain.Entities;
using DoAn.Persistence.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoAn.Persistence.Configurations.Configurations;

public class NotificationEventConfiguration: IEntityTypeConfiguration<NotificationEvent>
{
    public void Configure(EntityTypeBuilder<NotificationEvent> builder)
    {
        builder.ToTable(TableNames.NotificationEvents);
        builder.HasKey(x => x.Id);
        builder.HasData(new List<NotificationEvent>()
        {
            new()
            {
                Id = 1,
                Type = NotificationType.Upload,
                Text = "Tài khaonr [UserName] vừa nhập dữ liệu cho mẫu [ImportTemplateName]",
                Title = "Dữ liệu báo cáo vừa được nhập"
            },
            new()
            {
                Id = 2,
                Type = NotificationType.Approve,
                Text = "Tài khoản [UserName] vừa phê duyệt dữ liệu nhập của bạn",
                Title = "Dữ liệu nhập được phê duyệt"
            },
            new()
            {
                Id = 3,
                Type = NotificationType.Reject,
                Text = "Tài khoản [UserName] đã từ chối dữ liệu nhập của bạn",
                Title = "Dữ liệu nhập bị từ chối"
            }
        });

    }
}
