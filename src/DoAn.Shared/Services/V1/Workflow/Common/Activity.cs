namespace DoAn.Shared.Services.V1.Workflow.Common;

public class Activity
{
    public string ActivityId { get; set; }
    public string DisplayName { get; set; }
    public List<ActivityProperty> Properties { get; set; }
    public string Type { get; set; }
    public IDictionary<string, string> PropertyStorageProviders { get; set; }
}