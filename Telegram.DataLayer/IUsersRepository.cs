using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Model;

namespace Telegram.DataLayer
{
	public interface IUsersRepository
	{
		User Create(User user);
		void Delete(Guid id);
		User Get(Guid id);
	}
}
