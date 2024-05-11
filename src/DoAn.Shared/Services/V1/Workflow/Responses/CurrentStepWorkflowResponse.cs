namespace DoAn.Shared.Services.V1.Workflow.Responses;

public class CurrentStepWorkflowResponse
{
    public string WorkflowInstanceId { get; set; }
    public List<CurrentActivity> Activities { get; set; }
}

public class CurrentActivity
{
    public string Type { get; set; }
    public string Signal { get; set; }
    public string ActivityId { get; set; }
}
