
namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string? Token {get; set;}
    public DateTime Expiration {get; set;}
    public int IdUser {get; set;}
    public User? User {get; set;}
}