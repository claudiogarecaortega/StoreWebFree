using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class ContractTemplateViewModel :IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Documento")]
        [AllowHtml]
        public string Contrato { get; set; }
        public int? Secuencia { get; set; }
	}
}