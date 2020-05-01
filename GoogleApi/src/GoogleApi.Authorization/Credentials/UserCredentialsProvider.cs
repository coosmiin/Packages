using Google.Apis.Auth.OAuth2;
using GoogleApi.Authorization.Stores;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleApi.Authorization.Credentials
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
