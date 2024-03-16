using DoAn.Domain.Abstractions;

namespace DoAn.Domain.Entities;

public class Product : Entity<Guid>
{
    public string Name { get; set; }
    public string Price { get; set; }
}