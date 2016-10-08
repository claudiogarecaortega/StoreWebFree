using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class PedidosViewModel :IIdentifiableObject
    {
		public int Id { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Descripcion { get; set; }
        public string Referencia { get; set; }
        public virtual string Usuario { get; set; }
        public bool Cumplido { get; set; }
        public virtual int  UsuarioId { get; set; }
        
	}
}