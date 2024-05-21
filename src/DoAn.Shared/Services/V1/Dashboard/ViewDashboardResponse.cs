namespace DoAn.Shared.Services.V1.Dashboard;

public class ViewDashboardResponse
{
    public List<DashboardDto> Approves { get; set; }
    public List<DashboardDto> Rejects { get; set; }
    public List<DashboardDto> Uploads { get; set; }
    public int CountUpload { get; set; }
    public int CountReject { get; set; }
    public int CountApprove { get; set; }
}

public class DashboardDto
{
    public string Hour { get; set; }
    public int Value { get; set; }
    public string Category { get; set; }

    public static List<DashboardDto> initCoutUpload()
    {
        var data = new List<DashboardDto>();
        for (var i = 1; i <= 23; i++)
        {
            data.Add(new()
            {
                Category = "số báo cáo nhập",
                Hour = i.ToString(),
                Value = 0,
            });
        }
        return data;
    }
    public static List<DashboardDto> initCoutApprove()
    {
        var data = new List<DashboardDto>();
        for (var i = 1; i <= 23; i++)
        {
            data.Add(new()
            {
                Category = "số lượt phê duyệt",
                Hour = i.ToString(),
                Value = 0,
            });
        }
        return data;
    }
    public static List<DashboardDto> initCoutReject()
    {
        var data = new List<DashboardDto>();
        for (var i = 1; i <= 23; i++)
        {
            data.Add(new()
            {
                Category = "số lượt từ chối",
                Hour = i.ToString(),
                Value = 0,
            });
        }
        return data;
    }
}