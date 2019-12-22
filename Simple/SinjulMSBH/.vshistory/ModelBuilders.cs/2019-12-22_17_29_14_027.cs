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
				ODataConventionModelBuilder edmBuilder = new ODataConventionModelBuilder();

				edmBuilder.EntitySet<Customer>("Customers");
				edmBuilder.EntitySet<Order>("Orders");

				edmBuilder.EntitySet<Product>("Products");
				edmBuilder.EntitySet<Product>("Catogery");

				edmBuilder.EntitySet<Student>("Students");

				_edmModel = edmBuilder.GetEdmModel();
			}

			return _edmModel;
		}

		private IEdmModel GetEdmModel()
		{
			var edmBuilder = new ODataConventionModelBuilder();
			edmBuilder.EntitySet<Student>("Students");

			return edmBuilder.GetEdmModel();
		}
	}
}
