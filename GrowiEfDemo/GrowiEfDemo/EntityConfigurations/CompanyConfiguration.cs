using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowiEfDemo;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.SearchVector)
            .IsRequired()
            .HasComputedColumnSql("""
                                  to_tsvector('simple', "Ogrn") ||
                                  to_tsvector('simple', "Inn") ||
                                  to_tsvector('simple', "Name")
                                  """, stored: true);

        builder.HasIndex(p => p.SearchVector)
            .HasMethod("GIN");
    }
}