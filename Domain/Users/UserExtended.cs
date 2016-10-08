using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Products;
using Domain.Security;

namespace Domain.Users
{
    public class UserExtended:Audit
    {
        public int Id { get; set; }
        public string ValidationToken { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Imagen { get; set; }
        public DateTime BornDate { get; set; }
        public bool Sex { get; set; }
        public bool ConfirmedEmail { get; set; }
        public bool IsFristLogin { get; set; }
        public DateTime? LastAccess { get; set; }
        public bool IsOnline { get; set; }
        public virtual Roles UserRol { get; set; }
        public virtual IList<ModuleUserActions> Modules { get; set; }
        public virtual Person PersonUser { get; set; }
        public virtual IList<UserAlert> Alerts { get; set; }
        public virtual IList<UserNotification> Notifications { get; set; }
        public virtual IList<Messaging> MessagingsInbox { get; set; }
        public virtual IList<Comentarios> Comentarios { get; set; }
        public virtual IList<Preguntas> Preguntas { get; set; }
        public virtual IList<Messaging> MessagingsSend { get; set; }
         
      
    }
    public class Person :Audit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string CellPhone { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string FullName()
        {
            var name = String.IsNullOrEmpty(this.Name)?"":this.Name;
            var lastname = String.IsNullOrEmpty(this.Name) ? "" : this.Name;
            var comppleto= Name +"  "+ LastName;
            return comppleto;
        }
    }
    public class Audit
    {
       
        public bool IsDelete { get; set; }
        public int? Secuencia { get; set; }
        public string DeleteUser { get; set; }
        public string UpdateUser { get; set; }
        public string CreateUser { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public DateTime? DateDelete { get; set; } 
    }
}
