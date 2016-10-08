using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class CuentasCobrarViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Comentarios { get; set; }
        public decimal Monto { get; set; }
        public bool Cobrado { get; set; }
        public string FechaVencimiento { get; set; }
        public string CuentaDeposito { get; set; }
        public int CuentaDepositoId { get; set; }
        public string  Credito { get; set; }
        public int CreditoId { get; set; }
	}
}