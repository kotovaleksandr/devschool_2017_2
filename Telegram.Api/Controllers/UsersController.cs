using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Telegram.DataLayer;
using Telegram.DataLayer.Sql;
using Telegram.Model;

namespace Telegram.Api.Controllers
{
	public class UsersController : ApiController
	{
		private readonly IUsersRepository _usersRepository;
		private const string ConnectionString = @"Data Source=106SV0013.digdes.com\sql2012;Initial Catalog=telegram;Integrated Security=False;User ID=sa;";

		public UsersController()
		{
			_usersRepository = new UsersRepository(ConnectionString);
		}

		/// <summary>
		/// Get user by id
		/// </summary>
		/// <param name="id">User id</param>
		/// <returns></returns>
		[HttpGet]
		[Route("api/users/{id}")]
		public User Get(Guid id)
		{
			return _usersRepository.Get(id);
		}

		[HttpPost]
		[Route("api/users")]
		public User Create([FromBody] User user)
		{
			return _usersRepository.Create(user);
		}

		[HttpDelete]
		[Route("api/users/{id}")]
		public void Delete(Guid id)
		{
			_usersRepository.Delete(id);
		}

		[HttpGet]
		[Route("api/users/{id}/childs")]
		public string[] GetUserChilds(Guid id)
		{
			return new[] { id.ToString() };
		}
	}
}
