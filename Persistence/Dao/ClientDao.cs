using System.Collections.Generic;
using Domain.Clients;
using Persistence.Dao.Interfaces;
using Persistence.UnitsOfWork;
using Domain;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Utils;

namespace Persistence.Dao
{ 
    public class ClientDao : Dao<Client>, IClientDao
    {
		
		  public ClientDao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
            : base(unitOfWorkHelper, activator)
        {
        }
          public virtual IEnumerable<Client> GetAutoComplete(string text)
          {
              return
                  GetAll()
                      .Where(diagnostico => diagnostico.Name.ToLower().Contains(text.ToLower()) || diagnostico.DocumentoIdentidad.ToLower().Contains(text.ToLower()))
                      .AsEnumerable().Take(10);
          }
          public virtual IQueryable<Client> GetAllDestino(string filtro)
          {
              //var modelos = UnitOfWorkHelper.DBContext.Set<Client>().Where(x=>x.IsClientDestination && !x.IsClientOrigen&& !x.IsDelete).AsQueryable();

              //if (!string.IsNullOrEmpty(filtro))
              //    modelos = this.SetFiltro(modelos, filtro);

              //return modelos.AsQueryable();
              return base.GetAll().AsQueryable();
          }
       
        public virtual IQueryable<Client> GetAllOrigen(string filtro)
          {
              //var modelos = UnitOfWorkHelper.DBContext.Set<Client>().Where(x => x.IsClientOrigen && !x.IsClientDestination && !x.IsDelete).AsQueryable();

              //if (!string.IsNullOrEmpty(filtro))
              //    modelos = this.SetFiltro(modelos, filtro);

              //return modelos.AsQueryable();
              return base.GetAll().AsQueryable();
          }
          public  IEnumerable<Client> GetAllOrigenList()
          {
              return base.GetAll().Where(x =>  !x.IsDelete);
          }
          public IEnumerable<Client> GetAllDestinoList()
          {
              return base.GetAll().Where(x => !x.IsDelete);
          }
          public IEnumerable<Client> GetAllClients()
          {
              return base.GetAll().Where(x => !x.IsDelete);
          }
          public virtual IQueryable<Client> GetAllClients(string filtro)
          {
              var modelos = UnitOfWorkHelper.DBContext.Set<Client>().Where(x =>  !x.IsDelete).AsQueryable();

              if (!string.IsNullOrEmpty(filtro))
                  modelos = this.SetFiltro(modelos, filtro);

              return modelos.AsQueryable();
          }
		protected override IQueryable<Client> SetFiltro(IQueryable<Client> modelos, string filtro)
        {
            return modelos.Where(modelo => modelo.Description.ToLower().Contains(filtro.ToLower()) || modelo.Name.ToLower().Contains(filtro) && !modelo.IsDelete);
        }
	}
}