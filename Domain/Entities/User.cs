
namespace Domain.Entities;

public class User : BaseEntity 
{
        public string? Username {get; set;}
        public string? Password {get; set;}
        public string? Email {get; set;}
        public int IdRole {get; set;}
        public Role? Role {get; set;}
        public ICollection<Vehicle>? Vehicles {get; set;}
        public ICollection<RefreshToken>? RefreshTokens {get; set;}
}
