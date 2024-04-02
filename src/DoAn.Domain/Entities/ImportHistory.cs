using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoAn.Domain.Abstractions;

namespace DoAn.Domain.Entities
{    
   
    public class ImportHistory : Entity<Guid>
    {       
        public Guid? RoleProcessNextId { get; set; }
        public Guid ImportTemplateId { get; set; }
       
        public int Version { get; set; }
        public string Status { get; set; }

        public virtual ImportTemplate ImportTemplate { get; set; } = null!;


    }
}