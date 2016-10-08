using Domain.Products;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{ 
    public class PreguntasDao : Dao<Preguntas>, IPreguntasDao
    {
		
		  public PreguntasDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

		protected override IQueryable<Preguntas> SetFiltro(IQueryable<Preguntas> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Pregunta.ToLower().Contains(filtro.ToLower()));
        }
	}
}