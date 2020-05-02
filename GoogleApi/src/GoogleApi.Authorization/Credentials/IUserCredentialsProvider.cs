using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace GoogleApi.Authorization.Credentials
{
	public interface IUserCredentialsProvider
	{
		Task<UserCredential> FetchAsync();
	}
}