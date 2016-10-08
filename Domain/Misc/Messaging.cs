using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Users;

namespace Domain.Misc
{
    public class Messaging : Audit, IIdentifiableObject
    {
        public int Id { get; set; }
        public virtual UserExtended UserSend { get; set; }
        public virtual IList<UserExtended> UserReciver { get; set; }
        public virtual Messaging MessagingParent { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool IsSend { get; set; }
        public bool IsReciper { get; set; }
        public bool IsRead { get; set; }
        public bool IsUrgent { get; set; }
        public DateTime? DateSend { get; set; }
        public DateTime? DateRead { get; set; }

        public IEnumerable<Messaging> GetAllNodes()
        {
            var id = new List<Messaging>();
            var message = MessagingParent;
            while (message != null)
            {
                id.Add(message);
                message = message.MessagingParent;
            }
            return id;
        }

        public int GetLastNode()
        {
            var id = Id;
            var message = MessagingParent;
            while (message != null)
            {
                id = message.Id;
                message = message.MessagingParent;
            }
            return id;
        }

        public string GetTime()
        {
            return (Math.Round((DateTime.Now.Date - DateSend.GetValueOrDefault()).TotalMinutes/60).ToString())+" Minutos";
        }

        public int GetParent()
        {
            return Id;
        }
    }
}
