using System;
using System.Text.RegularExpressions;

namespace GmailApi.Client.Gmail
{
	public class GmailRequest
	{
		public string Query { get; internal set; }
		public DateTimeOffset NewerThan { get; internal set; }
		public Regex SnippetRegex { get; internal set; }
	}
}