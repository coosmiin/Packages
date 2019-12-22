using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;

namespace GmailApi.Client.Gmail
{
	public class UserCredentialsProvider : IUserCredentialsProvider
	{
		private readonly string[] _scopes;
		private readonly ClientSecrets _clientSecrets;

		// TODO: Find a way to inject token.json (only refresh token is mandatory). Is there an IDataStore alternative to FileDataStore?
		// TODO: Create builder or extract constructor parameters into an GoogleApiAuthSettings
		public UserCredentialsProvider(string[] scopes, string clientId, string clientSecret)
		{
			_scopes = scopes;
			_clientSecrets = new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret };
		}

		public async Task<UserCredential> FetchAsync()
		{
			var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
				_clientSecrets,
				_scopes,
				"user",
				CancellationToken.None,
				new FileDataStore("token.json", true));

			return credential;
		}
	}
}
