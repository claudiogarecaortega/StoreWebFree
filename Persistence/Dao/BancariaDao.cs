using Domain.Contable;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using Utils;

namespace Persistence.Dao
{
    public class BancariaDao : Dao<Bancaria>, IBancariaDao
    {

        public BancariaDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }

        protected override IQueryable<Bancaria> SetFiltro(IQueryable<Bancaria> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Entidad.ToLower().Contains(filtro.ToLower()));
        }

        public Bancaria GetCuentaIgresos()
        {
            return GetAll().FirstOrDefault(e => !e.IsDelete && e.Ingresos && e.Principal);
        }
        public Bancaria GetCuentaEgresos()
        {
            return GetAll().FirstOrDefault(e => !e.IsDelete && e.Egresos && e.Principal);
        }
    }
}