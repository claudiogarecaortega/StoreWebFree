using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.UnitsOfWork;
using Utils;

namespace Persistence.Dao
{
    public abstract class Dao<TModelo> where TModelo : class, new()
    {

        protected IUnitOfWorkHelper UnitOfWorkHelper;
        protected readonly IActivatorWrapper Activator;

        protected Dao(IUnitOfWorkHelper unitOfWorkHelper, IActivatorWrapper activator)
        {
            UnitOfWorkHelper = unitOfWorkHelper;
            Activator = activator;
        }

        public virtual TModelo Get(object id)
        {
            return UnitOfWorkHelper.DBContext.Set<TModelo>().Find(id);
        }

        public virtual TModelo Get(object[] id)
        {
            return UnitOfWorkHelper.DBContext.Set<TModelo>().Find(id);
        }

        public virtual TModelo GetForEdit(object id)
        {
            return this.Get(id);
        }

        public virtual TModelo GetForEdit(object[] id)
        {
            return this.Get(id);
        }

        public virtual TModelo GetForDelete(object id)
        {
            return this.Get(id);
        }

        public virtual TModelo GetForDelete(object[] id)
        {
            return this.Get(id);
        }

        public virtual IEnumerable<TModelo> GetAll()
        {
            return this.GetAllInternal().ToList();
        }

        protected virtual IQueryable<TModelo> GetAllInternal()
        {
            return UnitOfWorkHelper.DBContext.Set<TModelo>();
        }

        public virtual IQueryable<TModelo> GetAllQ(string filtro)
        {
            var modelos = UnitOfWorkHelper.DBContext.Set<TModelo>().AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                modelos = this.SetFiltro(modelos, filtro);

            return modelos.AsQueryable();
        }

        public virtual TModelo Create()
        {
            return Activator.CreateInstance<TModelo>();
        }

        public virtual void Add(TModelo modelo)
        {
            UnitOfWorkHelper.DBContext.Set<TModelo>().Add(modelo);
        }

        public virtual void Delete(TModelo modelo)
        {
            UnitOfWorkHelper.DBContext.Set<TModelo>().Remove(modelo);
        }

        public virtual bool SePuedeBorrar(TModelo modelo)
        {
            return true;
        }

        public virtual bool Existe(object id)
        {
            return this.Get(id) != null;
        }
        public virtual void Save()
        {
            UnitOfWorkHelper.SaveChanges();
        }
        public virtual bool Existe(object[] id)
        {
            return this.Get(id) != null;
        }

        protected virtual IQueryable<TModelo> SetFiltro(IQueryable<TModelo> modelos, string filtro)
        {
            return modelos;
        }
        /* private readonly IApplicationDbContext _dbContext;

         protected Dao(IApplicationDbContext dbContext)
         {
             _dbContext = dbContext;
         }

         public virtual TModelo Get(object id)
         {
             return _dbContext.Set<TModelo>().Find(id);
         }

         public virtual TModelo Get(object[] id)
         {
             return _dbContext.Set<TModelo>().Find(id);
         }

         public virtual TModelo GetForEdit(object id)
         {
             return Get(id);
         }

         public virtual TModelo GetForEdit(object[] id)
         {
             return Get(id);
         }

         public virtual TModelo GetForDelete(object id)
         {
             return Get(id);
         }

         public virtual TModelo GetForDelete(object[] id)
         {
             return Get(id);
         }

         public virtual IEnumerable<TModelo> GetAll()
         {
             return GetAllInternal().ToList();
         }

         protected virtual IQueryable<TModelo> GetAllInternal()
         {
             return _dbContext.Set<TModelo>();
         }

         public virtual IQueryable<TModelo> GetAllQ(string filtro)
         {
             var modelos = _dbContext.Set<TModelo>().AsQueryable();

             if (!string.IsNullOrEmpty(filtro))
                 modelos = SetFiltro(modelos, filtro);

             return modelos.AsQueryable();
         }

         public virtual TModelo Create()
         {
             return Activator.CreateInstance<TModelo>();
         }

         public virtual void Add(TModelo modelo)
         {
             _dbContext.Set<TModelo>().Add(modelo);
         }

         public virtual void Save()
         {
             _dbContext.SaveChanges();
         }

         public virtual void Delete(TModelo modelo)
         {
             _dbContext.Set<TModelo>().Remove(modelo);
         }

         public virtual bool SePuedeBorrar(TModelo modelo)
         {
             return true;
         }

         public virtual bool Existe(object id)
         {
             return Get(id) != null;
         }

         public virtual bool Existe(object[] id)
         {
             return Get(id) != null;
         }

         protected virtual IQueryable<TModelo> SetFiltro(IQueryable<TModelo> modelos, string filtro)
         {
             return modelos;
         }*/
    }
}
