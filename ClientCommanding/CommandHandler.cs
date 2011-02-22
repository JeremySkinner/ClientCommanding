using System;

namespace ClientCommanding
{
	public abstract class CommandHandler<T> : ICommandHandler
	{
		public CommandHandler()
		{
		}

		public abstract void Handle(T command);

		void ICommandHandler.Handle(object command)
		{
			Handle((T) command);
		}
	}

	public interface ICommandHandler
	{
		void Handle(object command);
	}
}