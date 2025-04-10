using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.Shared.Utilities.Constants
{
	public static class MinIOStorageConstants
	{
		public static class BucketName
		{
			public const string Products = "products";
			public const string UserProfiles = "profiles";
			public const string Localizations = "localizations";
		}

		public static class ObjectName
		{
			public const string Translation = "translation";
		}
	}
}
