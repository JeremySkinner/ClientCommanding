using System;
using System.Reflection;
using System.Web.Mvc;
using StructureMap;

namespace ClientCommanding
{
	public class CommandInvoker
	{
		public ActionResult InvokeCommand(object command, Type commandType)
		{
			// Invoke the command
			var handler = GetCommandHandler(commandType);
			handler.Handle(command);


			// If the command has a Result property, get the value
			object result = null;
			var resultProperty = commandType.GetProperty("Result", BindingFlags.Public | BindingFlags.Instance);

			if (resultProperty != null) {
				result = resultProperty.GetValue(command, null);
			}

			// If the result is an ActionResult, return as is
			// Otherwise, wrap it in a JsonResult.
			if (result != null) {
				
				if (result is ActionResult) 
				{
					return (ActionResult)result;
				}

				return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
			}

			return new EmptyResult();
		}

		private ICommandHandler GetCommandHandler(Type commandType) {
			var handlerType = typeof(CommandHandler<>).MakeGenericType(commandType);
			var handler = (ICommandHandler)ObjectFactory.GetInstance(handlerType);

			return handler;
		}
	}
}