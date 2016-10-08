using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class VentaViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
		public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public decimal MontoTotal { get; set; }
        public bool Pagada { get; set; }
        public string Cliente { get; set; }
        public int? ClienteId { get; set; }
        public string Producto { get; set; }
        public string ProductoDescripcion { get; set; }
        public int? ProductoId { get; set; }
        public int? UserId { get; set; }
        public int[] ProductosId { get; set; }
	}
}