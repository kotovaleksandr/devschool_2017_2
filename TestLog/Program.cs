using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace TestLog
{
	class Program
	{
		static void Main(string[] args)
		{
			var logger = LogManager.GetCurrentClassLogger();
			logger.Info("Инфо параметр: {0}", DateTime.Now);

			//бизнес-логика

			logger.Error("Ошибка");
			Console.ReadKey();
		}
	}
}
