using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class AlertViewModel :IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "IsForAll")]
        public bool IsForAll { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Importance")]
        public int Importance { get; set; }
        public int[] UsersAlert { get; set; }
        public bool IsHtml { get; set; }
        [AllowHtml]
        public string Html { get; set; }
        public int? Secuencia { get; set; }
	}
}