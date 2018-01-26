using Microsoft.Extensions.Configuration;

namespace SecretStore
{
	public class LocalSecretStore<T> : ISecretStore where T : class
	{
		IConfigurationRoot _configurationRoot;

		public LocalSecretStore()
		{
			_configurationRoot = BuildConfiguration();
		}

		public string GetSecret(string key)
		{
			return _configurationRoot[key];
		}

		private static IConfigurationRoot BuildConfiguration()
		{
			IConfigurationBuilder builder = new ConfigurationBuilder();

			builder.AddUserSecrets<T>();

			IConfigurationRoot configuration = builder.Build();

			return configuration;
		}
	}
}
