/// <reference path="jquery-1.4.4.js" />

// Commands that refer to the same C# command files without the command suffix.
// Note that the command type must be explicitly specified.
// To use, just invoke: Commands.GetUser(1)

var Commands = {

	GetUsers: function (callback) {
		InvokeCommand({ Name: 'GetUsers' }, callback);
	},

	GetUser: function (id, callback) {
		InvokeCommand({ Name: 'GetUser', id: id }, callback);
	},

	PostUser: function (name, callback) {
		InvokeCommand({ Name: 'PostUser', name: name }, callback);
	}
};

// Loop and set the name on each cmd
// also set the template rendering helper
for(var commandName in Commands) {
	Commands[commandName].Name = commandName;
	Commands[commandName].RenderTemplate = RenderTemplateForCommand(Commands[commandName]);
}

// Infrastructure gubbins

// Add a startsWith function to string
String.prototype.startsWith = function (str) { return (this.match("^" + str) == str) }

function InvokeCommand(command, callback) {
	// strip the 'Get' from the command, so "GetUsers" would become an HTTP GET request to /Users
	var httpMethod = 'GET';

	// SiteUrl is a global var inside the layout.
	var url = SiteUrl;

	if (command.Name.startsWith('Get')) {
		httpMethod = 'GET';
		url += command.Name.substr(3); //Trim the Get. GetUsers -> Users
	}
	else if (command.Name.startsWith('Post')) { // Could support other verbs too
		httpMethod = 'POST';
		url += command.Name.substr(4); //Trim the Post. PostUsers -> Post
	}

	var commandName = command.Name;
	// Remove the command name from the command object - this should be identified by the URL.
	delete command.Name;

	jQuery.ajax({
		type: httpMethod,
		url: url,
		data: command,
		success: callback || function () { },
		dataType: 'json'
	});


}

function RenderTemplateForCommand(command) {
	var name = command.Name + "Template";

	return function (result) {
		var childViewModel = {
			result: result,
		}

		$('#results').html('');

		var elem = $('<div></div>')
			.attr('data-bind', 'template: { name: "' + name + '"}')
			.appendTo('#results');

			window.x = childViewModel;

		ko.applyBindings(childViewModel, $('#results')[0]);
	}
}

