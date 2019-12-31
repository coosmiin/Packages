using GmailApi.Client.GoogleApi;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using System.Threading.Tasks;

namespace GmailApi.Client.Authorization
{
	public class UserCredentialsProvider : IUserCredentialsProvider
	{
		private readonly AuthSettings _settings;

		public UserCredentialsProvider(AuthSettings settings)
		{
			_settings = settings;
		}

		public async Task<UserCredential> FetchAsync()
		{
			var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
				new ClientSecrets { ClientId = _settings.ClientId, ClientSecret = _settings.ClientSecret },
				_settings.Scopes,
				"user",
				CancellationToken.None,
				new InMemoryDataStore(_settings.RefreshToken));

			return credential;
		}
	}
}
