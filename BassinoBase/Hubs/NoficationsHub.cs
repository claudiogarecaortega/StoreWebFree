using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BassinoBase.Hubs
{
    //public abstract class Hub<T> : Hub where T : Hub
    //{
    //    private static IHubContext hubContext;
    //    /// <summary>Gets the hub context.</summary>
    //    /// <value>The hub context.</value>
    //    public static IHubContext HubContext
    //    {
    //        get
    //        {
    //            if (hubContext == null)
    //                hubContext = GlobalHost.ConnectionManager.GetHubContext<T>();
    //            return hubContext;
    //        }
    //    }
    //}
    public class NoficationsHub :Hub//: Hub<NoficationsHub>
    {
        /// <summary>Tells the clients that some item has changed.</summary>
        //public async Task ItemHasChangedFromClient()
        //{
        //    await ItemHasChangedAsync().ConfigureAwait(false);
        //}
        /// <summary>Tells the clients that some item has changed.</summary>
        //public static async Task ItemHasChangedAsync()
        //{
        //    // my custom logic
        //    await HubContext.Clients.All.NofifyClientAction("");
        //}
        public void Send(string[] id)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(id);
        }
        public void SendMessage(int id, string ids)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NoficationsHub>();
            hubContext.Clients.All.nofifyClientAction(id, ids);
            //Clients.All.nofifyClientAction(msg);
        }
    }
}