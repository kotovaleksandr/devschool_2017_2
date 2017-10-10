using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Model;

namespace Telegram.DataLayer.Sql
{
	public class UsersRepository : IUsersRepository
	{
		private readonly string _connectionString;

		public UsersRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public User Create(User user)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "insert into users (id, name, avatar, password) values (@id, @name, @avatar, @password)";
					user.Id = Guid.NewGuid();
					command.Parameters.AddWithValue("@id", user.Id);
					command.Parameters.AddWithValue("@name", user.Name);
					command.Parameters.AddWithValue("@avatar", user.Avatar);
					command.Parameters.AddWithValue("@password", user.Password);

					command.ExecuteNonQuery();
					return user;
				}
			}
		}

		public void Delete(Guid id)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "delete from users where id = @id";
					command.Parameters.AddWithValue("@id", id);
					command.ExecuteNonQuery();
				}
			}
		}

		public User Get(Guid id)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = "select top(1) id, name, avatar, password from users where id = @id";
					command.Parameters.AddWithValue("@id", id);
					using (var reader = command.ExecuteReader())
					{
						if (!reader.Read())
							throw new ArgumentException($"Пользователь с id {id} не найден");
						return new User
						{
							Id = reader.GetGuid(reader.GetOrdinal("id")),
							Avatar = reader.GetSqlBinary(reader.GetOrdinal("avatar")).Value,
							Name = reader.GetString(reader.GetOrdinal("name")),
							Password = reader.GetString(reader.GetOrdinal("password"))
						};
					}
				}
			}
		}
	}
}
