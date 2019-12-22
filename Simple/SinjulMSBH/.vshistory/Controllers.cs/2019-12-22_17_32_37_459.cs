using System;
using System.Collections.Generic;

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Simple.SinjulMSBH
{
	[Route("api/[controller]"), ApiController]
	public class StudentsController : ControllerBase
	{
		[HttpGet, EnableQuery()]
		public IEnumerable<Student> Get()
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
}
