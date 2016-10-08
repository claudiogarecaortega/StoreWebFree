using BassinoLibrary.Resource;
using System.ComponentModel.DataAnnotations;
using Domain.IdentificableObject;

namespace BassinoLibrary.ViewModels
{ 
    public class MessagingViewModel :IIdentifiableObject
    {
		public int Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Description")]
        public string Description { get; set; }
        public virtual string UserSend { get; set; }
        public virtual int? UserSendId { get; set; }
        public int? Secuencia { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UserDestination")]
        
        public virtual string UserReciver { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "UserDestination")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorUserReciper")]
        public virtual int[] UserReciverId { get; set; }
        public virtual string MessagingParent { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Subject")]
        public string Subject { get; set; }
        public virtual int? MessagingParentId { get; set; }
        public string Message { get; set; }
        public bool IsSend { get; set; }
        public bool IsReciper { get; set; }
        public bool IsRead { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "IsUrgent")]
        public bool IsUrgent { get; set; }
	}
}