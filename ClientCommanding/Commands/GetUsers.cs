using System;
using System.Collections.Generic;
using ClientCommanding.Models;
using System.Linq;

namespace ClientCommanding.Commands
{
	public class GetUsers
	{
		public List<User> Result { get; set; } 

		public class Handler : CommandHandler<GetUsers>
		{
			public override void Handle(GetUsers command)
			{
				var db = new InMemoryDatabase();
				command.Result = db.OfType<User>().ToList();
			}
		}
	}

}