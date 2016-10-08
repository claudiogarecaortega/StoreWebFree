using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ActivatorWrapper : IActivatorWrapper
    {
        public event EventHandler<ObjectCreatedEventArgs> ObjectCreated;

        public virtual T CreateInstance<T>()
        {
            var instance = Activator.CreateInstance<T>();
            OnObjectCreated(instance);
            return instance;
        }

        public virtual object CreateInstance(string assemblyName, string typeName)
        {
            var instance = Activator.CreateInstance(assemblyName, typeName).Unwrap();
            OnObjectCreated(instance);
            return instance;
        }

        protected void OnObjectCreated(object entity)
        {
            if (ObjectCreated != null)
                ObjectCreated(this, new ObjectCreatedEventArgs(entity));
        }
    }
}
