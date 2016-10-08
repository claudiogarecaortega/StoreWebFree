using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public interface IApplicationDbContext
    {
        Database Database { get; }
       // IDbSet<Culture> Cultures { get; set; }
        //IDbSet<Currency> Currencies { get; set; }
        //IDbSet<Ubication> Ubications { get; set; }
        int SaveChanges();
        DbSet<T> Set<T>() where T : class;
    }
}
