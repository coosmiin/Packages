namespace GmailApi.Client.Gmail
{
	public interface IGmailServiceClient
	{
		string? ExtractEmailSnippetContent(GmailRequest request);
	}
}