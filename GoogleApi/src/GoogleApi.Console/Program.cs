using GmailApi.Client.Gmail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using GoogleApi.Authorization.Credentials;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleApi.Console
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var credential = CreateTokenUsingRawApi();
			// var credential = await CreateTokenUsingCredentialProvider();

			// var content = ExtractEmailContentUsingWrapperApi(credential);
		}

		private static async Task<UserCredential> CreateTokenUsingCredentialProvider()
		{
			var settings = new AuthSettings
			{
				ClientId = "{your app client id from credentials.json}",
				ClientSecret = "{your app client secret from credentials.json}",
				RefreshToken = "{your user refresh token from token.json}",
				Scopes = new[] { GmailService.Scope.GmailReadonly }
			};
			var credentialProvider = new UserCredentialsProvider(settings);

			return await credentialProvider.FetchAsync();
		}

		private static UserCredential CreateTokenUsingRawApi()
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

		private static string ExtractEmailContentUsingWrapperApi(UserCredential credential)
		{
			var gmailService = new GmailServiceWrapper("{your app name}", credential);

			var request = new GmailRequestBuilder()
				.UseFrom("{from email address}")
				.UseSubject("{email subject}")
				.UseLabel("{email label/category}")
				.UseNewerThan(DateTime.Now.AddMinutes(-10))
				.UseSnippetRegex(new Regex(".*"))
				.Request;

			var content = gmailService.ExtractEmailSnippetContent(request);

			return content;
		}
	}
}
