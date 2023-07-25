using IBetting.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IBetting.DataAccess.Configurations
{
    public class OddConfig : IEntityTypeConfiguration<Odd>
    {
        public void Configure(EntityTypeBuilder<Odd> builder)
        {
            builder.Property(x => x.Value).HasColumnType("decimal(6,2)").IsRequired(true);
        }
    }
}
