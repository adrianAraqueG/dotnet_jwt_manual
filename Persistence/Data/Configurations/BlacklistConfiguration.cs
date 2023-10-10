using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class BlacklistConfiguration : IEntityTypeConfiguration<Blacklist>
    {
        public void Configure(EntityTypeBuilder<Blacklist> builder){
            builder.ToTable("Blacklist");

            builder.Property(b => b.Token).IsRequired();
            builder.Property(b => b.Expiration).IsRequired();
        }
    }
}