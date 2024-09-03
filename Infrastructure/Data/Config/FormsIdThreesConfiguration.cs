using Core.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class FormsIdThreesConfiguration : IEntityTypeConfiguration<FormsIdThrees>
{
    public void Configure(EntityTypeBuilder<FormsIdThrees> builder)
    {
        _ = builder.Property(p => p.name).IsRequired(true);
        _ = builder.Property(p => p.formId).ValueGeneratedOnAdd();
        _ = builder.Property(p => p.firstThree).IsRequired(false);
        _ = builder.Property(p => p.secondThree).IsRequired(false);
        _ = builder.Property(p => p.thiredThree).IsRequired(false);
    }
}
