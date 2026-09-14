using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

/// <summary>
/// Container establishing connection between <see cref="DbContext"/> and <see cref="IEnumsConfiguration"/>.
/// </summary>
/// <typeparam name="TContext">Type of the DbContext.</typeparam>
internal interface IEnumConfigurationWrapper<TContext> where TContext : DbContext
{
    void Apply(ModelBuilder modelBuilder);
}
