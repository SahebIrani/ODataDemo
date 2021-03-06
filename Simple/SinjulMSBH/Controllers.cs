using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Simple.SinjulMSBH
{

	//? OData API Multiple Parameters
	//? https://wellsb.com/csharp/aspnet/odata-api-multiple-parameters
	public class ProjectsController : Controller
	{
		//? GET api/projects(1)?$expand=PullRequests&$filter=PullRequests/ContributorId eq 5
		[EnableQuery]
		[ODataRoute("({id})")]
		public async Task GetProject([FromODataUri] int id)
		{
			//var project = await _context.Projects.Where(s => s.Id == id)
			//	.Include(s => s.PullRequests)
			//	.FirstOrDefaultAsync();
		}

		//? GET: api/pullrequests/ByProjectByContributor(projectId=1,contributorId=5)
		//? GET: api/pullrequests/ByProjectByContributor(projectId=1,contributorId=5)
		[ODataRoute("ByProjectByContributor(projectId={projectId},contributorId={contributorId})")]
		public IQueryable<PullRequest> ByProjectByContributor([FromODataUri] int projectId, [FromODataUri] int contributorId)
		{
			//var pullRequests = _context.PullRequests.Where(
			//	pr => pr.ProjectId == projectId
			//	&& pr.ContributorId == contributorId);
			return default;
		}
	}


	// https://wellsb.com/csharp/aspnet/odata-api-ef-core-blazor/
	[ODataRoutePrefix("contacts")]
	[Route("api/[controller]"), ApiController]
	public class StudentsController : ControllerBase
	{
		[HttpGet, EnableQuery(PageSize = 85), ODataRoute]
		[ODataRoute("({id})")]
		public ActionResult<IEnumerable<Student>> Get()
		{
			//var response = await _httpClient.GetAsync($”api / contacts ?$count = true &$orderby ={ orderBy}
			//&$skip ={ skip}
			//&$top ={ top}”);

			return new List<Student>
			{
				CreateNewStudent("Sinjul MSBH", 850),
				CreateNewStudent("Jack Slater", 130),
				CreateNewStudent("Saheb Irai", 170)
			};
		}

		private static Student CreateNewStudent(string name, int score)
		{
			return new Student
			{
				Id = Guid.NewGuid(),
				Name = name,
				Score = score
			};
		}
	}

	//◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘

	[Route("api/[controller]"), ApiController]
	public class ProductsController : ODataController
	{
		private static IList<Product> _products;

		public ProductsController()
		{
			if (_products == null)
			{
				_products = new List<Product>
				{
					new Product
					{
						Id = 1,
						Title = "Product 01",
						EMails = new string[] { "jackslater.irani@gmail.com", "sinjul.msbh@yahoo.com" },
						ByteValue = 8,
						Data = new byte[] { 1, 2, 3 ,4},
						HomeAddress = new Address { City = "Sari", Street = "Sa'at"},
						Category = new Category { Id = 1, Title = "GoToosh" }
					},
					new Product
					{
						Id = 2,
						Title = "Product 02",
						EMails = new string[] { "jackslater.irani@gmail.com", "sinjul.msbh@yahoo.com" },
						ByteValue = 13,
						Data = new byte[] { 5, 6, 7,8 },
						HomeAddress = new Address { City = "Neka", Street = "RahAhan"},
						Category = new Category { Id = 2, Title = "GoToosh" }
					},
				};
			}
		}

		[EnableQuery]
		public IActionResult Get() => Ok(_products);

		[HttpGet("{key}"), EnableQuery]
		public IActionResult Get(int key) => Ok(_products.FirstOrDefault(c => c.Id == key));
	}

	//◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘

	[Route("api/[controller]"), ApiController]
	public class CustomersController : ODataController
	{
		public AppDbContext Context { get; }
		public CustomersController(AppDbContext context)
		{
			Context = context;

			if (!Context.Customers.Any())
			{
				IList<Customer> customers = new List<Customer>
				{
					new Customer
					{
						Id = 1,
						Name = "SinjulMSBH",
						ByteValue = 8,
						Data = new byte[] { 1, 2, 3 ,4},
						HomeAddress = new Address { City = "Sari", Street = "Sa'at"},
						Order = new Order { Id = 1, Title = "103m" }
					},
					new Customer
					{
						Id = 2,
						Name = "JackSlater",
						ByteValue = 13,
						Data = new byte[] { 5, 6, 7,8 },
						HomeAddress = new Address { City = "Neka", Street = "RahAhan"},
						Order = new Order { Id = 2, Title = "44m" }
					},
				};

				foreach (var customer in customers)
				{
					Context.Customers.Add(customer);
					Context.Orders.Add(customer.Order);
				}

				Context.SaveChanges();
			}
		}

		[EnableQuery]
		public async Task<ActionResult<IEnumerable<Customer>>> GetAsync()
		{
			// Be noted: without the NoTracking setting, the query for $select=HomeAddress with throw exception:
			// A tracking query projects owned entity without corresponding owner in result ..
			// Owned entities cannot be tracked without their owner ..
			Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			return Ok(value: await Context.Customers.AsNoTracking().ToListAsync());
		}

		[EnableQuery]
		public async Task<ActionResult<Customer>> GetAsync(int key) =>
			Ok(await Context.Customers.SingleOrDefaultAsync(c => c.Id == key));
	}
}
