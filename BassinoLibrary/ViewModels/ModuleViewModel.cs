using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class ModuleViewModel :IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string ModuleName { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string Name { get; set; }
        public string ModuleParent { get; set; }
        public int? ModuleParentId { get; set; }
        public int? Secuencia { get; set; }
	}
}