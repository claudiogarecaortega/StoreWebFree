using System;
using Domain.Products;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class ShipmentDao : Dao<Shipment>, IShipmentDao
    {
		
		  public ShipmentDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public override IQueryable<Shipment> GetAllQ(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Shipment>().Where(d => !d.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
          public  IQueryable<Shipment> GetAllAccount(int cuenta, bool infecha, DateTime start,DateTime end)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Shipment>().Where(d => !d.IsDelete).AsQueryable();
              if(cuenta>0)
                  modelos = UnitOfWorkHelper.DBContext.Set<Shipment>().Where(d => !d.IsDelete && d.Cargars.Select(r => r.ClientFrom.Id).Contains(cuenta)).AsQueryable();
              if (infecha)
                  modelos = modelos.Where(d => d.DateCreate >= start && d.DateCreate <= end);

              return modelos.AsQueryable();
          }

          public IQueryable<Shipment> GetAllQFiltros(string filtro, bool viaje, bool end, bool init)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Shipment>().Where(s => !s.IsDelete).AsQueryable();
              if (viaje)
              {
                  modelos = modelos.Where(s => s.IsTraveling);

              }
              if (end)
              {
                  modelos = modelos.Where(u => u.IsFinishig);
              }
              if (init)
              {
                  modelos = modelos.Where(i => i.IsSent);
              }
              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
		protected override IQueryable<Shipment> SetFiltro(IQueryable<Shipment> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Observations.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}