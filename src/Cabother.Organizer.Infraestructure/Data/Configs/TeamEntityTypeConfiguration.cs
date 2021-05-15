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
            builder.ToTable("printer", OrganizerDbContext.Schema);

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

            builder.Property<Guid>("type_id")
                .IsRequired();

            builder.Property<Guid>("branch_id")
                .IsRequired();

            builder.HasIndex(x => x.Status);
        }
    }
}