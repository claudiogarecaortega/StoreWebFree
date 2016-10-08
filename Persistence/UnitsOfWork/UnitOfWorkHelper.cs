using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Persistence.UnitsOfWork
{
    public class UnitOfWorkHelper : IUnitOfWorkHelper
    {
        public ApplicationDbContext _sessionContext;
        public event EventHandler<ObjectCreatedEventArgs> ObjectCreated;

        public UnitOfWorkHelper()
        {

        }

        public IApplicationDbContext DBContext
        {
            get
            {
                if (_sessionContext == null)
                {

                    _sessionContext = new ApplicationDbContext();
                    ((IObjectContextAdapter)_sessionContext).ObjectContext.ObjectMaterialized += (sender, e) => OnObjectCreated(e.Entity);
                }

                return _sessionContext;
            }
        }

        private void OnObjectCreated(object entity)
        {
            if (ObjectCreated != null)
                ObjectCreated(this, new ObjectCreatedEventArgs(entity));
        }
        private void OnObjectCreateddb()
        {
            // if (_sessionContext == null)
            //_sessionContext=
        }
        public void SaveChanges()
        {
            this.DBContext.SaveChanges();
        }

        public void RollBack()
        {
            if (_sessionContext != null)
            {
                _sessionContext.ChangeTracker.Entries()
                    .ToList()
                    .ForEach(entry => entry.State = EntityState.Unchanged);
            }
        }

        public void Dispose()
        {
            if (_sessionContext != null)
                _sessionContext.Dispose();
        }
    }
}
