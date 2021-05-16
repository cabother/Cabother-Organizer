using System;
using Cabother.Organizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cabother.Organizer.Infraestructure.Data.Configs
{
    internal class TeamEntityTypeConfiguration : BaseEntityTypeConfiguration<Team>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("team", OrganizerDbContext.Schema);

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Alias)
                .HasColumnName("alias")
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired();

            builder.HasIndex(x => x.Status);
        }
    }
}