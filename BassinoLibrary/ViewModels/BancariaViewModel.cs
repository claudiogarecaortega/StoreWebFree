using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class BancariaViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }

       // [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string NumeroCuenta { get; set; }
        public string Alias { get; set; }
        public string Entidad { get; set; }
        public decimal Saldo { get; set; }
        public bool Principal { get; set; }
        public bool Ingresos { get; set; }
        public bool Egresos { get; set; }
	}
}