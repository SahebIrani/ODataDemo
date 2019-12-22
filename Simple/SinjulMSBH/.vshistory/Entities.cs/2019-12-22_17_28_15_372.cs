using System;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Simple.SinjulMSBH
{
	public class Customer
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public byte ByteValue { get; set; }

		public byte[] Data { get; set; }

		public Address HomeAddress { get; set; }

		public Order Order { get; set; }
	}

	public class Order
	{
		public int Id { get; set; }

		public string Title { get; set; }
	}

	[Owned, ComplexType]
	public class Address
	{
		public string City { get; set; }

		public string Street { get; set; }
	}



	public class Student
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Score { get; set; }
	}



	public class Product
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string[] EMails { get; set; }

		public byte ByteValue { get; set; }

		public byte[] Data { get; set; }

		public Address HomeAddress { get; set; }

		public Category Category { get; set; }
	}

	public class Category
	{
		public int Id { get; set; }

		public string Title { get; set; }
	}
}
