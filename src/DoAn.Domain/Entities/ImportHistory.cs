using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoAn.Domain.Abstractions;
using DoAn.Domain.Entities.Identity;

namespace DoAn.Domain.Entities
{    
   
    public class ImportHistory : AuditableEntity<Guid>
    {       
        public Guid? RoleProcessNextId { get; set; }
        public Guid ImportTemplateId { get; set; }
        public Guid FileId { get; set; }
        public Guid UserId { get; set; }
        public int Version { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }

        public virtual ImportTemplate ImportTemplate { get; set; } = null!;
        public virtual AppUser User { get; set; }


    }
}