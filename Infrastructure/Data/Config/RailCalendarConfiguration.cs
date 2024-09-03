using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class RailCalendarConfiguration : IEntityTypeConfiguration<RailCalendar>
{
    public void Configure(EntityTypeBuilder<RailCalendar> builder)
    {
        _ = builder.Property(p => p.Id).IsRequired();
        _ = builder.Property(p => p.Date).IsRequired();
        _ = builder.Property(p => p.DayInWeek).IsRequired();
        _ = builder.Property(p => p.NumberOfDayInWeek).IsRequired();
        _ = builder.Property(p => p.NameOfHoliday).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.CMO60).IsRequired().HasMaxLength(100);
    }
}
