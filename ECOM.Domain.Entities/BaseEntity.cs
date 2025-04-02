using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.Domain.Entities
{
	public abstract class BaseEntity
	{
		public Guid Id { get; set; }
		public DateTime CreatedAt_Utc { get; set; } = DateTime.UtcNow;
		public DateTime LastUpdatedAt_Utc { get; set; } = DateTime.UtcNow;
	}
}
