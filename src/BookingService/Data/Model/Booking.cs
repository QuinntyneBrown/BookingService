using BookingService.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Booking: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }

        [ForeignKey("Resource")]
        public int? ResourceId { get; set; }

        [Index("NameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]        
		public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
