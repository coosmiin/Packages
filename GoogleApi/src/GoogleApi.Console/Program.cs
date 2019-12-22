using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace GoogleApi.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var credential = CreateToken();

			CallGmail(credential);
		}

		private static UserCredential CreateToken()
		{
			var scopes = new[] { GmailService.Scope.GmailReadonly };
			UserCredential credential;

			using (var stream =
				new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
			{
				// The file token.json stores the user's access and refresh tokens, and is created
				// automatically when the authorization flow completes for the first time.
				string credPath = "token.json";
				credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					scopes,
					"user",
					CancellationToken.None,
					new FileDataStore(credPath, true)).Result;
				System.Console.WriteLine("Credential file saved to: " + credPath);
			}

			return credential;
		}

		private static void CallGmail(UserCredential credential)
		{
			// Create Gmail API service.
			var service = new GmailService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "Trading App",
			});

			// Define parameters of request.

			UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List("me");
			request.Q = "from:(bt-trade@btcapitalpartners.ro) subject:(Cod de autentificare) in:\"BT Capital Auth\" newer_than:1d";

			var response = request.Execute();

			var messages = new List<Message>();
			foreach (var message in response.Messages)
			{
				UsersResource.MessagesResource.GetRequest msgRequest = service.Users.Messages.Get("me", message.Id);
				messages.Add(msgRequest.Execute());
			}

			var authDateTime = DateTime.Parse("Mon, 9 Dec 2019 20:37:31 +0200");
			var authMessage = messages
				.Where(m => DateTime.Parse(m.Payload.Headers.FirstOrDefault(h => h.Name == "Date").Value) >= authDateTime)
				.OrderByDescending(m => DateTime.Parse(m.Payload.Headers.FirstOrDefault(h => h.Name == "Date").Value))
				.FirstOrDefault();

			var codeRegex = new Regex("Codul de autentificare generat este: \\d{2}-(?<code>\\d{5})");

			var code = codeRegex.Matches(authMessage.Snippet)[0].Groups["code"].Value;

			System.Console.Read();
		}
	}
}
