namespace DoAn.Application.DTOs;

public class QueryRule 
{
    public string Condition { get; set; }
    public string Field { get; set; }
    public string Id { get; set; }
    public string Input { get; set; }
    public string Operator { get; set; }
    public IEnumerable<QueryRule> Rules { get; set; }
    public string Type { get; set; }
    public object Value { get; set; }
    public string Valid { get; set; }
}