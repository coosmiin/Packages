using System;
using System.Text;
using System.Text.RegularExpressions;

namespace GmailApi.Client.Gmail
{
	public class GmailRequestBuilder
	{
		private string _from;
		private string _subject;
		private string _label;
		private DateTimeOffset _newerThan;
		private Regex _contentRegex;

		public GmailRequest Request
		{
			get
			{
				var queryBuilder = new StringBuilder();

				queryBuilder.AppendSearchTerm("from", _from);
				queryBuilder.AppendSearchTerm("subject", _subject);
				queryBuilder.AppendSearchTerm("in", _label);

				var newerThanDays = (DateTime.Now - _newerThan).Days;
				queryBuilder.AppendSearchTerm("newer_than", $"{newerThanDays + 1}d", quotedValue: false);

				return new GmailRequest { Query = queryBuilder.ToString(), NewerThan = _newerThan, SnippetRegex = _contentRegex };
			}
		}

		public GmailRequestBuilder UseFrom(string from)
		{
			_from = from;
			return this;
		}

		public GmailRequestBuilder UseSubject(string subject)
		{
			_subject = subject;
			return this;
		}

		public GmailRequestBuilder UseLabel(string label)
		{
			_label = label;
			return this;
		}

		public GmailRequestBuilder UseNewerThan(DateTimeOffset date)
		{
			_newerThan = date;
			return this;
		}

		public GmailRequestBuilder UseSnippetRegex(Regex regex)
		{
			_contentRegex = regex;
			return this;
		}
	}

	public static class StringBuilderExtentions
	{
		public static StringBuilder AppendSearchTerm(this StringBuilder builder, string term, string value, bool quotedValue = true)
		{
			string spacing = builder.Length == 0 ? string.Empty : " ";
			string quote = quotedValue ? "\"" : string.Empty;
			return builder.Append($"{spacing}{term}:{quote}{value}{quote}");
		}
	}
}