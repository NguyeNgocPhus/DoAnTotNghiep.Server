using DoAn.Shared.Abstractions.Messages;

namespace DoAn.Shared.Services.V1.Dashboard;

public class  ViewDashboardQuery: IQuery<ViewDashboardResponse>
{
    public DateTime Date { get; set; }
}