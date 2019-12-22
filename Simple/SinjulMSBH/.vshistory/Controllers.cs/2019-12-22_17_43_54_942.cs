using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Simple.SinjulMSBH
{
	[Route("api/[controller]"), ApiController]
	public class StudentsController : ControllerBase
	{
		[HttpGet, EnableQuery()]
		public ActionResult<IReadOnlyList<Student>> Get()
		{
			return new List<Student>
			{
				new Student
				{
					Id = Guid.NewGuid(),
					Name = "SinjulMSBH",
					Score = 85
				},
				new Student
				{
					Id = Guid.NewGuid(),
					Name = "JackSlater_4",
					Score = 130
				}
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
						Title = "Product_1",
						EMails = new string[] { "e1@abc.com", "e2@xyz.com" },
						ByteValue = 8,
						Data = new byte[] { 1, 2, 3 },
						HomeAddress = new Address { City = "Redmond", Street = "156 AVE NE"},
						Category = new Category { Id = 11, Title = "104m" }
					},
					new Product
					{
						Title = "Product_2",
						EMails = new string[] { "cd1@abc.com", "ad2@xyz.com" },
						ByteValue = 18,
						Data = new byte[] { 4, 5, 6 },
						HomeAddress = new Address { City = "Bellevue", Street = "Main St NE"},
						Category = new Category { Id = 12, Title = "Zhang" }
					},
					new Product
					{
						Title = "Product_3",
						EMails = new string[] { "fe1@abc.com", "fa2@xyz.com" },
						ByteValue = 28,
						Data = new byte[] { 7, 8, 9 },
						HomeAddress = new Address {  City = "Hollewye", Street = "Main St NE"},
						Category = new Category { Id = 13, Title = "Jian" }
					},
				};

			}
		}

		[EnableQuery]
		public IActionResult Get() => Ok(_products);

		[EnableQuery]
		public IActionResult Get(int key) => Ok(_products.FirstOrDefault(c => c.Id == key));
	}

	//◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘◘

	[Route("api/[controller]"), ApiController]
	public class CustomersController : ODataController
	{
		public CustomersController(AppDbContext context)
		{
			if (!Context.Customers.Any())
			{
				IList<Customer> customers = new List<Customer>
				{
					new Customer
					{
						Name = "Jonier",
						ByteValue = 8,
						Data = new byte[] { 1, 2, 3 },
						HomeAddress = new Address { City = "Redmond", Street = "156 AVE NE"},
						Order = new Order { Title = "104m" }
					},
					new Customer
					{
						Name = "Sam",
						ByteValue = 18,
						Data = new byte[] { 4, 5, 6 },
						HomeAddress = new Address { City = "Bellevue", Street = "Main St NE"},
						Order = new Order { Title = "Zhang" }
					},
					new Customer
					{
						Name = "Peter",
						ByteValue = 28,
						Data = new byte[] { 7, 8, 9 },
						HomeAddress = new Address {  City = "Hollewye", Street = "Main St NE"},
						Order = new Order { Title = "Jichan" }
					},
				};

				foreach (var customer in customers)
				{
					Context.Customers.Add(customer);
					Context.Orders.Add(customer.Order);
				}

				Context.SaveChanges();
			}

			Context = context;
		}

		public AppDbContext Context { get; }


		[EnableQuery]
		public async Task<ActionResult<IReadOnlyList<Customer>>> GetAsync()
		{
			// Be noted: without the NoTracking setting, the query for $select=HomeAddress with throw exception:
			// A tracking query projects owned entity without corresponding owner in result. Owned entities cannot be tracked without their owner...
			//Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

			return Ok(value: await Context.Customers.AsNoTracking().ToListAsync());
		}

		[EnableQuery]
		public async Task<ActionResult<Customer>> GetAsync(int key) =>
			Ok(await Context.Customers.SingleOrDefaultAsync(c => c.Id == key));
	}

}
