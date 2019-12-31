using System;
using System.Text.RegularExpressions;

namespace GmailApi.Client.Gmail
{
	public class GmailRequest
	{
		public string Query { get; internal set; }
		public DateTime NewerThan { get; internal set; }
		public Regex SnippetRegex { get; internal set; }
	}
}