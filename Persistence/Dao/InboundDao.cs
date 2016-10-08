using System.Collections.Generic;
using Domain.Commodity;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Utils;

namespace Persistence.Dao
{ 
    public class InboundDao : Dao<Inbound>, IInboundDao
    {
		
		  public InboundDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public virtual IEnumerable<Inbound> GetAllFilter(int[] id,bool edit)
          {
              if (!edit)
              {
                  var colecion =
                      this.GetAllInternal().Where(x => id.Contains(x.ClientTo.Ubication.Id) && x.IsUsed == false).ToList();
                  return colecion.Where(item => id.Contains(item.ClientTo.Ubication.Id)).ToList();
              }
              var colecion2 =
                      this.GetAllInternal().Where(x => id.Contains(x.ClientTo.Ubication.Id)).ToList();
              return colecion2.Where(item =>id.Contains(item.ClientTo.Ubication.Id)&& !item.IsDelete).ToList();
          }
          public IQueryable<Inbound> GetAllQRestore(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Inbound>().Where(s => !s.IsDelete && s.IsUsed && s.Envio.IsFinishig).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable().OrderByDescending(f => f.DateIn);
          }
        public IQueryable<Inbound> GetAllQ(string filtro)
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<Inbound>().Where(s=>!s.IsDelete).AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                modelos = this.SetFiltro(modelos, filtro);

            return modelos.AsQueryable().OrderByDescending(f => f.DateIn);
        }
        public IQueryable<Inbound> GetAllQPlus(string filtro)
        {
            var id = filtro.Split('-');
            var modelos = UnitOfWorkHelper.DBContext.Set<Inbound>().Where(s => !s.IsDelete).AsQueryable();
            if (filtro.Any() && id[0]!="0")
            {
                modelos = modelos.Where(s => id.Contains(s.Id.ToString()));
            }
            else if (id[0] == "0" || id[0] == "")
            {
                modelos = modelos.Where(s => s.Id==-1);
            }          
            return modelos.AsQueryable().OrderByDescending(f => f.DateIn);
        }
        public IQueryable<Inbound> GetAllQLess(string filtro,string filtro2, bool viaje, bool end, bool init)
        {
            var id = filtro.Split('-');
            var modelos = UnitOfWorkHelper.DBContext.Set<Inbound>().Where(s => !s.IsDelete ).AsQueryable(); 
            if (filtro.Any()&&id[0]!="0")
            {
                modelos = modelos.Where(s => !id.Contains(s.Id.ToString()));
            }
            if (viaje)
            {
                modelos = modelos.Where(s => s.Envio.IsTraveling || s.Envio.IsSent && !s.Envio.IsFinishig && !s.IsDelivered);

            }
            if (end)
            {
                modelos = modelos.Where(u => u.Envio.IsFinishig || u.IsDelivered);
            }
            if (init)
            {
                modelos = modelos.Where(i =>  i.Envio==null);
            }
            if (!string.IsNullOrEmpty(filtro2))
                modelos = this.SetFiltro(modelos, filtro2);
            
            return modelos.AsQueryable().OrderByDescending(f => f.DateIn);
        }
        public IQueryable<Inbound> GetAllQFiltros(string filtro, bool viaje, bool end, bool init)
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<Inbound>().Where(s => !s.IsDelete).AsQueryable();
            if (viaje)
            {
                modelos = modelos.Where(s => s.Envio.IsTraveling);

            }
            if (end)
            {
                modelos = modelos.Where(u => u.Envio.IsFinishig);
            }
            if (init)
            {
                modelos = modelos.Where(i => i.Envio.IsSent);
            }
            if (!string.IsNullOrEmpty(filtro))
                modelos = this.SetFiltro(modelos, filtro);

            return modelos.AsQueryable().OrderByDescending(f => f.DateIn);
        }
        protected override IQueryable<Inbound> SetFiltro(IQueryable<Inbound> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) && !modelo.IsDelete || modelo.ClientTo.Name.ToLower().Contains(filtro.ToLower()));
        }
	}
}