using System;

namespace GoogleApi.Authorization.Credentials
{
	public class AuthSettings
	{
		public string ClientId { get; set; } = string.Empty;

		public string ClientSecret { get; set; } = string.Empty;

		public string RefreshToken { get; set; } = string.Empty;

		public string[] Scopes { get; set; } = Array.Empty<string>();
	}
}