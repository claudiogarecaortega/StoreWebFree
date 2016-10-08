using System.Collections.Generic;
using System.Linq;
using Domain.Clients;

namespace Persistence.Dao.Interfaces
{ 
    public interface IClientDao : IDao<Client>
    {
        IQueryable<Client> GetAllDestino(string filtro);
        IQueryable<Client> GetAllClients(string filtro);
        IEnumerable<Client> GetAllDestinoList();
        IEnumerable<Client> GetAllOrigenList();
        IQueryable<Client> GetAllOrigen(string filtro);
        IEnumerable<Client> GetAllClients();
        IEnumerable<Client> GetAutoComplete(string text);
    }
}