using Microsoft.EntityFrameworkCore;

namespace GrowiEfExtensions;

public interface IFunctionsConfiguration
{
    void ApplyConfiguration(ModelBuilder modelBuilder);
}
