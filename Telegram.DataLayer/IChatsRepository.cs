using System;
using System.Collections.Generic;
using Telegram.Model;

namespace Telegram.DataLayer
{
	public interface IChatsRepository
	{
		Chat Create(IEnumerable<Guid> members, string name);
		IEnumerable<Chat> GetUserChats(Guid userId);
		void DeleteChat(Guid chatId);
	}
}
