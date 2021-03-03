using System;
using System.Text.RegularExpressions;

namespace GmailApi.Client.Gmail
{
	public record GmailRequest(string Query, DateTimeOffset? NewerThan, Regex? SnippetRegex);
}