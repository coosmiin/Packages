using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Http;
using Google.Apis.Services;

namespace GmailApi.Client.Gmail
{
	public class GmailServiceClient : IGmailServiceClient
	{
		private readonly GmailService _gmailService;

		public GmailServiceClient(string appName, IConfigurableHttpClientInitializer clientInitializer)
		{
			_gmailService = new GmailService(
				new BaseClientService.Initializer
				{
					HttpClientInitializer = clientInitializer,
					ApplicationName = appName,
				});
		}

		public string? ExtractEmailSnippetContent(GmailRequest request)
		{
			UsersResource.MessagesResource.ListRequest listRequest = _gmailService.Users.Messages.List("me");
			listRequest.Q = request.Query;

			var response = listRequest.Execute();

			if (response.Messages == null)
				return null;

			var messages = new List<Message>();
			foreach (var message in response.Messages)
			{
				UsersResource.MessagesResource.GetRequest msgRequest = _gmailService.Users.Messages.Get("me", message.Id);
				messages.Add(msgRequest.Execute());
			}

			var authMessage = messages	
				.Where(m => m.InternalDate >= request.NewerThan?.ToUnixTimeMilliseconds())
				.OrderByDescending(m => m.InternalDate)
				.FirstOrDefault();

			if (authMessage == null)
				return null;

			if (request.SnippetRegex == null)
				return null;
				
			var matches = request.SnippetRegex.Matches(authMessage.Snippet);

			if (matches.Any() && matches[0].Groups.Count > 1)
			{
				return matches[0].Groups[1].Value;
			}

			return null;
		}
	}
}