using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.App.DTOs.Common
{
	public class BaseRequest<TModel>
	{
		public required TModel Model { get; set; }
	}
}
