using System.Collections.Generic;
using Domain.Products;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Domain.Misc;
using Utils;

namespace Persistence.Dao
{ 
    public class ProductDao : Dao<Product>, IProductDao
    {
		
		  public ProductDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<Product> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Product>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }

        public int GetBajoStock()
        {
            return GetAll().Count(g => g.Stock.Any(f => f.Cantidad < 10) &&!g.IsDelete);
        }
          public virtual IEnumerable<Product> GetAutoComplete(string text)
          {
              return
                  GetAll()
                      .Where(diagnostico => diagnostico.Description.ToLower().Contains(text.ToLower()) || diagnostico.Id.ToString().Contains(text))
                      .AsEnumerable().Take(10);
          }
        public IQueryable<Product> GetAllActive()
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<Product>().Where(d => !d.IsDelete).AsQueryable();

          
            return modelos.AsQueryable();
        }
        protected override IQueryable<Product> SetFiltro(IQueryable<Product> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}