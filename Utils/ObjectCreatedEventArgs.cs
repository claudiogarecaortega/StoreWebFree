using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ObjectCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     The object that was materialized.
        /// </summary>
        private readonly object _Entity;

        /// <summary>
        ///     Constructs new arguments for the ObjectMaterialized event.
        /// </summary>
        /// <param name="entity">The object that has been materialized. </param>
        public ObjectCreatedEventArgs(object entity)
        {
            _Entity = entity;
        }

        /// <summary>
        ///     Gets the entity object that was created.
        /// </summary>
        /// <returns>
        ///     The entity object that was created.
        /// </returns>
        public object Entity
        {
            get { return _Entity; }
        }
    }
}
