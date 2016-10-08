using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class CuotasViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public string Status { get; set; }
        public int? Secuencia { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public string Vencimiento { get; set; }
        public bool Pagada { get; set; }
        public decimal Interes { get; set; }
        public string MontoTotal { get; set; }
        public string MontoPendiente { get; set; }
        public string CreditoCliente { get; set; }
        public string Credito { get; set; }
        public int CreditoId { get; set; }
	}
}