using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder){
        builder.ToTable("RefreshTokens");

        builder.Property(r => r.Token).IsRequired();
        builder.Property(r => r.Expiration).IsRequired();
    }
}
