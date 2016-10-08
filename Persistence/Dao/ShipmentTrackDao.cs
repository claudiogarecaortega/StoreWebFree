using Domain.Products;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class ShipmentTrackDao : Dao<ShipmentTrack>, IShipmentTrackDao
    {
		
		  public ShipmentTrackDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        protected override IQueryable<ShipmentTrack> SetFiltro(IQueryable<ShipmentTrack> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Observaciones.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete);
        }
	}
}