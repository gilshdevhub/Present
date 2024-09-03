using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class MeetingsConfiguration : IEntityTypeConfiguration<Meetings>
{
    public void Configure(EntityTypeBuilder<Meetings> builder)
    {
        _ = builder.HasKey(p => p.MeetingsId);
        _ = builder.Property(p => p.MeetingDate).IsRequired();
        _ = builder.Property(p => p.RegistretionLink).IsRequired().HasMaxLength(500); ;
        _ = builder.Property(p => p.Location).IsRequired();
    }
}