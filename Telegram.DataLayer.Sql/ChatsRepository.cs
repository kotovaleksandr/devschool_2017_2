using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Model;

namespace Telegram.DataLayer.Sql
{
	public class ChatsRepository : IChatsRepository
	{
		private readonly string _connectionString;
		private readonly IUsersRepository _usersRepository;

		public ChatsRepository(string connectionString, IUsersRepository usersRepository)
		{
			_connectionString = connectionString;
			_usersRepository = usersRepository;
		}

		public Chat Create(IEnumerable<Guid> members, string name)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var transaction = connection.BeginTransaction())
				{
					var chat = new Chat
					{
						Name = name,
						Id = Guid.NewGuid()
					};
					using (var command = connection.CreateCommand())
					{
						command.Transaction = transaction;
						command.CommandText = "insert into chats (id, name) values (@id, @name)";
						command.Parameters.AddWithValue("@id", chat.Id);
						command.Parameters.AddWithValue("@name", chat.Name);

						command.ExecuteNonQuery();
					}

					foreach (var userId in members)
					{
						using (var command = connection.CreateCommand())
						{
							command.Transaction = transaction;
							command.CommandText = "insert into chatmembers (chatid, userid) values (@chatid, @userid)";
							command.Parameters.AddWithValue("@chatid", chat.Id);
							command.Parameters.AddWithValue("@userid", userId);
							command.ExecuteNonQuery();
						}
					}
					transaction.Commit();
					chat.Members = members.Select(x => _usersRepository.Get(x));
					return chat;
				}
			}
		}

		public IEnumerable<Chat> GetUserChats(Guid userId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "select id, name from chatmembers inner join chats on chatmembers.chatid = chats.id where userid = @userid";
					command.Parameters.AddWithValue("@userid", userId);
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							yield return new Chat
							{
								Id = reader.GetGuid(reader.GetOrdinal("id")),
								Name = reader.GetString(reader.GetOrdinal("name")),
								Members = GetChatMembers(reader.GetGuid(reader.GetOrdinal("id")))
							};
						}
					}
				}
			}
		}

		public void DeleteChat(Guid chatId)
		{
			throw new NotImplementedException();
		}

		private IEnumerable<User> GetChatMembers(Guid id)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "select userId from chatmember where chatid = @chatid";
					command.Parameters.AddWithValue("chatid", id);
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
							yield return _usersRepository.Get(reader.GetGuid(reader.GetOrdinal("userid")));
					}
				}
			}
		}
	}
}
