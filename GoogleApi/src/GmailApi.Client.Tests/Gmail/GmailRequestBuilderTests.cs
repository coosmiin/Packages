using System;
using System.Text.RegularExpressions;
using GmailApi.Client.Gmail;
using NUnit.Framework;

namespace GmailApi.Client.Tests.Gmail
{
	public class GmailRequestBuilderTests
	{
		[Test]
		public void UseFrom_FromInRequestQuery()
		{
			var request = new GmailRequestBuilder()
				.UseFrom("some@email.com")
				.Request;

			Assert.IsTrue(request.Query.Contains("from:\"some@email.com\""));
		}

		[Test]
		public void UseSubject_SubjectInRequestQuery()
		{
			var request = new GmailRequestBuilder()
				.UseSubject("something")
				.Request;

			Assert.IsTrue(request.Query.Contains("subject:\"something\""));
		}

		[Test]
		public void UseLabel_LabelInRequestQuery()
		{
			var request = new GmailRequestBuilder()
				.UseLabel("label")
				.Request;

			Assert.IsTrue(request.Query.Contains("in:\"label\""));
		}

		[Test]
		public void UseNewerThan_NewerThanInRequest()
		{
			var newerThan = DateTimeOffset.Now.AddDays(-1).AddMinutes(-10);

			var request = new GmailRequestBuilder()
				.UseNewerThan(newerThan)
				.Request;

			Assert.AreEqual(newerThan, request.NewerThan);
		}

		[Test]
		public void UseNewerThan_MoreThanOneDay_CorrectDaysInQuery()
		{
			var request = new GmailRequestBuilder()
				.UseNewerThan(DateTimeOffset.Now.AddDays(-1).AddMinutes(-10))
				.Request;

			Assert.IsTrue(request.Query.Contains("newer_than:2d"));
		}

		[Test]
		public void UseNewerThan_LessThanOneDay_CorrectDaysInQuery()
		{
			var request = new GmailRequestBuilder()
				.UseNewerThan(DateTimeOffset.Now.AddDays(-1).AddMinutes(10))
				.Request;

			Assert.IsTrue(request.Query.Contains("newer_than:1d"));
		}

		[Test]
		public void UseSnippetRegex_RegexInRequest()
		{
			var regex = new Regex("\\d{10}");
			var request = new GmailRequestBuilder()
				.UseSnippetRegex(regex)
				.Request;

			Assert.AreEqual(regex, request.SnippetRegex);
		}
	}
}