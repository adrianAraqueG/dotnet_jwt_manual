
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Blacklist : BaseEntity
{
    public string? Token {get; set;}
    public DateTime Expiration {get; set;}
}