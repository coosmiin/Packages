using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;

namespace GoogleApi.Authorization.Stores
{
	internal class InMemoryDataStore : IDataStore
	{
		private TokenResponse _authToken;

		public InMemoryDataStore(string refreshToken)
		{
			_authToken = new TokenResponse
			{
				RefreshToken = refreshToken
			};
		}

		public Task ClearAsync()
		{
			_authToken = new TokenResponse();
			return Task.CompletedTask;
		}

		public Task DeleteAsync<T>(string key)
		{
			_authToken = new TokenResponse();
			return Task.CompletedTask;
		}

		public Task<T> GetAsync<T>(string key)
		{
			return Task.FromResult((T)(object)_authToken);
		}

		public Task StoreAsync<T>(string key, T value)
		{
			if (value != null)
			{
				_authToken = (TokenResponse)(object)value;
			}

			return Task.CompletedTask;
		}
	}
}