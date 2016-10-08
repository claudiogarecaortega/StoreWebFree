using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Dao.Interfaces
{
    public interface IDao<TModelo>
    {
    
        TModelo Get(object id);
        void Save();
        TModelo Get(object[] id);
        TModelo GetForEdit(object id);
        TModelo GetForEdit(object[] id);
        IEnumerable<TModelo> GetAll();
        IQueryable<TModelo> GetAllQ(string filtro);
        TModelo Create();
        void Add(TModelo modelo);
        void Delete(TModelo modelo);
        bool Existe(object id);
        bool Existe(object[] id);

        bool SePuedeBorrar(TModelo modelo);
        TModelo GetForDelete(object id);
        TModelo GetForDelete(object[] id);
    }
}
