namespace GmailApi.Client.Gmail
{
	public interface IGmailServiceWrapper
	{
		string ExtractEmailSnippetContent(GmailRequest request);
	}
}