using System;
using ClientCommanding.Models;
using System.Linq;

namespace ClientCommanding.Commands
{
	public class GetUser
	{
		public int Id { get; set; }

		public User Result { get; set; }

		public class Handler : CommandHandler<GetUser>
		{
			public override void Handle(GetUser command)
			{
				var db = new InMemoryDatabase();
				command.Result = db.OfType<User>().Single(x => x.Id == command.Id);
			}
		}
	}
}