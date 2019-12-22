using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace GmailApi.Client.Gmail
{
	public interface IUserCredentialsProvider
	{
		Task<UserCredential> FetchAsync();
	}
}