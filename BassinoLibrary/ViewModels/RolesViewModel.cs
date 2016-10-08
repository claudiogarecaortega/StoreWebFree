using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class RolesViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        public int? Secuencia { get; set; }
	}

    public class RolesModuleViewModel
    {
        public int RolNameId { get; set; }      
        public int ModuleNameId { get; set; }
        public string RolName { get; set; }
        public string ModuleName { get; set; }
        public string[] Actions { get; set; }
    }
}