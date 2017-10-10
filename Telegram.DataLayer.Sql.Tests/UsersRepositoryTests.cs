using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Model;

namespace Telegram.DataLayer.Sql.Tests
{
	[TestClass]
	public class UsersRepositoryTests
	{
		private const string ConnectionString = @"Data Source=106SV0013.digdes.com\sql2012;Initial Catalog=telegram;Integrated Security=False;User ID=sa;";

		private readonly List<Guid> _tempUsers = new List<Guid>();

		[TestMethod]
		public void ShouldCreateUser()
		{
			//arrange
			var user = new User
			{
				Name = "testUser",
				Avatar = Encoding.UTF8.GetBytes("ava"),
				Password = "password"
			};

			//act
			var repository = new UsersRepository(ConnectionString);
			var result = repository.Create(user);

			_tempUsers.Add(result.Id);

			//asserts
			Assert.AreEqual(user.Name, result.Name);
			Assert.AreEqual(user.Avatar, result.Avatar);
			Assert.AreEqual(user.Password, result.Password);
		}

		[TestMethod]
		public void ShouldStartChatWithUser()
		{
			//arrange
			var user = new User
			{
				Name = "testUser",
				Avatar = Encoding.UTF8.GetBytes("ava"),
				Password = "password"
			};

			const string chatName = "чатик";

			//act
			var usersRepository = new UsersRepository(ConnectionString);
			var result = usersRepository.Create(user);

			_tempUsers.Add(result.Id);

			var chatRepository = new ChatsRepository(ConnectionString, usersRepository);
			var chat = chatRepository.Create(new[] { user.Id }, chatName);
			var userChats = chatRepository.GetUserChats(user.Id);
			//asserts
			Assert.AreEqual(chatName, chat.Name);
			Assert.AreEqual(user.Id, chat.Members.Single().Id);
			Assert.AreEqual(chat.Id, userChats.Single().Id);
			Assert.AreEqual(chat.Name, userChats.Single().Name);
		}

		[TestCleanup]
		public void Clean()
		{
			foreach (var id in _tempUsers)
				new UsersRepository(ConnectionString).Delete(id);
		}
	}
}
