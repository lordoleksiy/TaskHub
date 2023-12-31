﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context.ModelConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder
                .HasKey(u => u.Id)
                .HasName("UserId");

            builder
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .HasMany(u => u.Tasks)
                .WithMany(t => t.AssignedUsers)
                .UsingEntity("UsersTasksJoinTable");
        }
    }
}
