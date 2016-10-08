using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class CuentasPagarViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Comentarios { get; set; }
        public decimal Monto { get; set; }
        public bool Pagado { get; set; }
        public string FechaVencimiento { get; set; }
        public string CuentaExtracto { get; set; }
        public int CuentaExtractoId { get; set; }
        public string Credito { get; set; }
        public int CreditoId { get; set; }
	}
}