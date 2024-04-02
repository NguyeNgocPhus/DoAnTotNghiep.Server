using DoAn.Domain.Abstractions;

namespace DoAn.Domain.Entities
{    
   
    public class ImportTemplate : Entity<Guid>
    {       
        public string Name { get; set; }
        public string Tag { get; set; }
      
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool HasWorkflow { get; set; }
        public string? WorkflowDefinitionId { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<ImportHistory> ImportHistories { get; set; } = null!;

    }
}