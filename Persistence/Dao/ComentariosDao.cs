using Domain.Products;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class ComentariosDao : Dao<Comentarios>, IComentariosDao
    {
		
		  public ComentariosDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

		protected override IQueryable<Comentarios> SetFiltro(IQueryable<Comentarios> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Comentario.ToLower().Contains(filtro.ToLower()));
        }
	}
}