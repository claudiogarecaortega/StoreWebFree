using System.Collections.Generic;
using System.Linq;
using Domain.Products;

namespace Persistence.Dao.Interfaces
{ 
    public interface IProductDao : IDao<Product>
    {
        IQueryable<Product> GetAllActive();
        IEnumerable<Product> GetAutoComplete(string text);
        int GetBajoStock();
    }
}