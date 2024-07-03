﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerData.Entities;

namespace TaskManagerData.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.CompanyId)
                .IsRequired();

            builder.HasOne(t => t.Company)
                .WithMany(c => c.Teams)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.TeamLeader)
                .WithMany()
                .HasForeignKey(t => t.TeamLeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Members)
                .WithOne() 
                .HasForeignKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
