namespace GmailApi.Client.Authorization
{
	public class AuthSettings
	{
		public string ClientId { get; set; }

		public string ClientSecret { get; set; }

		public string RefreshToken { get; set; }

		public string[] Scopes { get; set; }
	}
}