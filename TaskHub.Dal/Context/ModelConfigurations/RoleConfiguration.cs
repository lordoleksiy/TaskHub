using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context.ModelConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasData
            (
                new RoleEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = "ADMIN".ToUpper()
                },
                new RoleEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "User",
                    NormalizedName = "USER".ToUpper(),
                }
            );
        }
    }
}
