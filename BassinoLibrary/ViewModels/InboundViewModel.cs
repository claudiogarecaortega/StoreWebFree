using System;
using System.ComponentModel;
using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.ModelBinding;
using System.Web.Mvc;
using BassinoLibrary.Attributes;
using Domain.Clients;
using Domain.IdentificableObject;
using Domain.Misc;
using Domain.Products;
using Domain.Users;

namespace BassinoLibrary.ViewModels
{
    public class InboundViewModel : IIdentifiableObject
    {
		public int Id { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "DateIn")]
       
        public string DateIn { get; set; }
        public int? Secuencia { get; set; }
        public bool IsUsed { get; set; }
        public string Status { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "DateTicket")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public string DateTicket { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "BillNumer")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public string BillNumber { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Price")]
        [DecimaCustomlAttribute]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorFieldDecimal")]
        public string PriceDecimal { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Quantity")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public string Quantity { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "IsCold")]
        public bool IsCold { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Comentaries")]
        public string Description { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Kilos")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public string Kilos { get; set; }

        public bool IsDecline { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "ControlUser")]
        public string UserControl { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "ControlUser")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public int UserControlId { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "CuentaDestino")]
        public string ClientTo { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "CuentaDestino")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public int ClientToId { get; set; }
        
       
        
        [Display(ResourceType = typeof(Resources), Name = "CuentaOrigen")]
        public string ClientOrigen { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "CuentaOrigen")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public int ClientOrigenId { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "MeasureUnit")]
        public string MeasureUnit { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "MeasureUnit")]
        public int? MeasureUnitId { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "PackageType")]
        public string PackageType { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "PackageType")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public int PackageTypeId { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Product")]
        public string Product { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "Product")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public int ProductId { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "BillDoc")]
        public string Bill { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "BillDoc")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public int BillId { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "BillTypeDoc")]
        public string BillType { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "BillTypeDoc")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorField")]
        public int BillTypeId { get; set; }


        [Display(ResourceType = typeof(Resources), Name = "Cuenta")]
        public string ClientFrom { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Cuenta")]
        public int? ClientFromId { get; set; }
        public string UbicationDescription { get; set; }
        //[NumeralCustomAttribute]
        public string UsoFisico { get; set; }

      
	}
}