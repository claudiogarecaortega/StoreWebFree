using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{
    public class ProviderViewModel : IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NameOrSocial")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Alias")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorAlias")]
        public string Alias { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Direction")]
        public string Direction { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "ZipCode")]
        public long ZipCode { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NameofContact")]
        public string Contact { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "IiBb")]
        public decimal IiBb { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Cuit")]
        public string Cuit { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Ubication")]
        public int? UbicationId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Ubication")]
        public string Ubication { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "TaxCondition")]
        public string TaxCondition { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "TaxCondition")]
        public int? TaxConditionId { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NameContact")]
        public string NameContact { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "MailContact")]
        public string MailContact { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "PhoneContact")]
        public string PhoneContact { get; set; }
	}
}