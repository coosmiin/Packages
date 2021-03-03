using GmailApi.Client.Gmail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using GoogleApi.Authorization.Credentials;
using SheetsApi.Client.Sheets;
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
			// Before using any Google Api it needs to be enabled. 
			// For example Sheets Api can be enabled here: https://console.developers.google.com/apis/library/sheets.googleapis.com?project=api-project-348374937002
			
			var credential = CreateTokenUsingRawApi(SheetsService.Scope.Spreadsheets); // SheetsService.Scope.Spreadsheets // GmailService.Scope.GmailReadonly
			
			// var content = ExtractEmailContentUsingClientApi(credential);
			GetCellsUsingClientApi(credential);
		}

		private static async Task<UserCredential> CreateTokenUsingCredentialProvider(string scope)
		{
			var settings = new AuthSettings
			{
				ClientId = "{your app client id from credentials.json}",
				ClientSecret = "{your app client secret from credentials.json}",
				RefreshToken = "{your user refresh token from token.json}",
				Scopes = new[] { scope }
			};
			var credentialProvider = new UserCredentialsProvider(settings);

			return await credentialProvider.FetchAsync();
		}

		private static UserCredential CreateTokenUsingRawApi(string scope)
		{
			var scopes = new[] { scope };
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

		private static string? ExtractEmailContentUsingClientApi(UserCredential credential)
		{
			var gmailService = new GmailServiceClient("{your app name}", credential);

			var request = new GmailRequestBuilder()
				.UseFrom("{from email address}")
				.UseSubject("{email subject}")
				.UseLabel("{email label/category}")
				.UseNewerThan(DateTimeOffset.Now.AddMinutes(-10))
				.UseSnippetRegex(new Regex(".*"))
				.Request;

			var content = gmailService.ExtractEmailSnippetContent(request);

			return content;
		}

		private static void GetCellsUsingClientApi(UserCredential credential)
		{
			var sheetsService = new SheetsServiceClient("{your app name}", credential);

			// The same sample as the one used in the official docs: https://developers.google.com/sheets/api/quickstart/dotnet
			var dataRequest = new DataRequest("1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms","Class Data!A2:E");

			var values = sheetsService.GetCells(dataRequest);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            if (values != null && values.Count > 0)
            {
                System.Console.WriteLine("Name, Major");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    System.Console.WriteLine("{0}, {1}", row[0], row[4]);
                }
            }

		}
	}
}
