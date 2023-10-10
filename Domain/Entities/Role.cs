
namespace Domain.Entities;

public class Role : BaseEntity
{
    public string? Rolename {get; set;}
    public ICollection<User>? Users {get; set;}
}