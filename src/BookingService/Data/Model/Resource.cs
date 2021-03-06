using System;
using System.Collections.Generic;
using BookingService.Data.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Data.Model
{
    [SoftDelete("IsDeleted")]
    public class Resource: ILoggable
    {
        public int Id { get; set; }
        
		[ForeignKey("Tenant")]
        public int? TenantId { get; set; }
        
		[Index("NameIndex", IsUnique = false)]
        [Column(TypeName = "VARCHAR")]        
		public string Name { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        
		public DateTime CreatedOn { get; set; }
        
		public DateTime LastModifiedOn { get; set; }
        
		public string CreatedBy { get; set; }
        
		public string LastModifiedBy { get; set; }
        
		public bool IsDeleted { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
