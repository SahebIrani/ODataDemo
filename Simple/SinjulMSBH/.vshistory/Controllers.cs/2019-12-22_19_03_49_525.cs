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
		public ActionResult<IEnumerable<Student>> Get()
		{
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
						Name = "SinjulMSBH",
						ByteValue = 8,
						Data = new byte[] { 1, 2, 3 ,4},
						HomeAddress = new Address { City = "Sari", Street = "Sa'at"},
						Order = new Order { Title = "103m" }
					},
					new Customer
					{
						Name = "JackSlater",
						ByteValue = 13,
						Data = new byte[] { 5, 6, 7,8 },
						HomeAddress = new Address { City = "Neka", Street = "RahAhan"},
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
