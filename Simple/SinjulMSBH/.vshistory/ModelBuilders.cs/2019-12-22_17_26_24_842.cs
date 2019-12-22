using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;

namespace Simple.SinjulMSBH
{
	public static class EdmModelBuilder
	{
		private static IEdmModel _edmModel;

		public static IEdmModel GetEdmModel()
		{
			if (_edmModel == null)
			{
				var builder = new ODataConventionModelBuilder();
				builder.EntitySet<Customer>("Customers");
				builder.EntitySet<Order>("Orders");

				//
				builder.EntitySet<Product>("Products");
				builder.EntitySet<Product>("Catogery");

				_edmModel = builder.GetEdmModel();
			}

			return _edmModel;
		}
	}
}
