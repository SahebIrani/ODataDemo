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


				ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
				builder.EntitySet<Project>("Projects");
				builder.EntitySet<Contributor>("Contributors");
				builder.EntitySet<PullRequest>("PullRequests");

				FunctionConfiguration pullRequestsByProjectByContributor =
					builder.EntityType<Project>().Collection
						.Function("ByProjectByContributor")
						.ReturnsCollectionFromEntitySet<Project>("Projects");
				pullRequestsByProjectByContributor.Parameter<int>("projectId").Required();
				pullRequestsByProjectByContributor.Parameter<int>("contributorId").Required();

				return builder.GetEdmModel();
			}

			return _edmModel;
		}
	}
}
