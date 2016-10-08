using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Dao.Interfaces
{
    public interface IDaoDependence<TModel> : IDao<TModel>
    {
        IQueryable<TModel> GetAllQ(int id, string filtro);
    }
}
