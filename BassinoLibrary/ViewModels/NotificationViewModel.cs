using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;
namespace BassinoLibrary.ViewModels
{ 
    public class NotificationViewModel :IIdentifiableObject
    {
		public int Id { get; set; }

      //  [Display(ResourceType = typeof(Resources), Name = "Description")]
        //public string Description { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Mensaje")]
        public string Messages { get; set; }
        public string Url { get; set; }
        public int IdProduct { get; set; }
        //public string Module { get; set; }
        //public string Controller { get; set; }
        public string Importance { get; set; }
        public bool IsForAllUSers { get; set; }
        public bool IsRead { get; set; }

    }
}