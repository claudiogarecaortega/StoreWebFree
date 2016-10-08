using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Web.Mvc;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{
    public class ClientViewModel : IIdentifiableObject
    {
        public int Id { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "NameOrSocial")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Alias")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorAlias")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Direction")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorDirection")]
        public string Direction { get; set; }
        
        [Display(ResourceType = typeof(Resources), Name = "ZipCode")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorZipCode")]
        public long ZipCode { get; set; }
        
        //[Display(ResourceType = typeof(Resources), Name = "NameofContact")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorNameContact")]
        //public string Contact { get; set; }
        
        //[Display(ResourceType = typeof(Resources), Name = "IiBb")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorIiBb")]
        //public string IiBb { get; set; }
        
        //[Display(ResourceType = typeof(Resources), Name = "Cuit")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorCuit")]
        //public string Cuit { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Comentaries")]
        public string Description { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Sequence")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorSequence")]
        public decimal Sequence { get; set; }

        public bool IsClientDestination { get; set; }
        public bool IsClientOrigen { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Ubication")]
        public string Ubication { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Ubication")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorUbication")]
        public int? UbicationId { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "Seller")]
        //public string Seller { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "Seller")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorSeller")]
        //public int SellerId { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "TaxConditionLabel")]
        //public string TaxCondition { get; set; }//
        //[Display(ResourceType = typeof(Resources), Name = "TaxConditionLabel")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorTaxCondition")]
        //public int TaxConditionId { get; set; }//
        [Display(ResourceType = typeof(Resources), Name = "NameContact")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorNameContact")]
        public string NameContact { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "MailContact")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorMailContact")]
        public string MailContact { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "PhoneContact")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorPhoneContact")]
        public string PhoneContact { get; set; }

        //[Display(ResourceType = typeof(Resources), Name = "NameContact2")]
        
        //public string NameContact2 { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "MailContact2")]
        
        //public string MailContact2 { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "PhoneContact2")]
        
        //public string PhoneContact2 { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "MeasureUnitClient")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorMeasureUnitClient")]
        //public string MeasureUnit { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "DeliveryDirection")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorDeliveryDirection")]
        public string DeliveryDirection { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Neighborhood")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorNeighborhood")]
        public string Neighborhood { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Localidad")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorLocalidad")]
        public string Localidad { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "TimeToDelivery")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorTimeToDelivery")]
        //public string TimeToDelivery { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "ServicePrice")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorServicePrice")]
        //public string ServicePrice { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "Services")]
        //[Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorServices")]
        //public int[] Serviceses { get; set; }
        //public string ServicesesDescription { get; set; }

        //[Display(ResourceType = typeof(Resources), Name = "DeliveryMorningStart")]
        //public string TimeToDeliveryMorningStart { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "DeliveryEnd")]
        //public string TimeToDeliveryMorningEnd { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "DeliveryAfterMoonStart")]
        //public string TimeToDeliveryAftermonStart { get; set; }
        //[Display(ResourceType = typeof(Resources), Name = "DeliveryEnd")]
        //public string TimeToDeliveryAftermonEnd { get; set; }

        public string Contrato { get; set; }
        public int? ContratoId { get; set; }
        [AllowHtml]
        public string ContratoDoc { get; set; }

        public string DocumentoIdentidad { get; set; }
        public string NombreCompleto { get; set; }
    }
}