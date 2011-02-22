using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace ClientCommanding.Controllers {
	public class CommandInvokerController : Controller {
		public ActionResult Invoke(string command) {
			// Convert "GET /Users" to GetUsers:
			command = BuildFullCommandName(command);

			// Find the appropriate command type, eg ClientCommanding.Commands.GetUsers
			var commandType = Type.GetType("ClientCommanding.Commands." + command);

			// Ensure it's a valid comand
			if (commandType == null) {
				throw new HttpException(404, "Command Not found");
			}

			var commandInstance = Activator.CreateInstance(commandType);

			// Bind request params to the command instance.
			BindCommandProperties(commandType, commandInstance);

			var invoker = new CommandInvoker();
			var result = invoker.InvokeCommand(commandInstance, commandType);

			return result;
		}


		private void BindCommandProperties(Type commandType, object commandInstance) {
			var binder = Binders.GetBinder(commandType);

			var bindingContext = new ModelBindingContext {
				ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => commandInstance, commandType),
				ModelName = null,
				ModelState = ModelState,
				ValueProvider = ValueProvider
			};

			binder.BindModel(ControllerContext, bindingContext);
		}


		private string BuildFullCommandName(string command) {
			var httpMethod = Request.HttpMethod.ToLower();
			httpMethod = Request.HttpMethod[0].ToString().ToUpper() + httpMethod.Substring(1);

			command = httpMethod + command;
			return command;
		}
	}
}