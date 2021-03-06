using System;
using System.Text;
using Moq;
using NUnit.Framework;
using NServiceKit.Common.Web;
using NServiceKit.ServiceInterface.Testing;
using NServiceKit.WebHost.Endpoints.Extensions;
using NServiceKit.WebHost.Endpoints.Tests.Mocks;
using NServiceKit.WebHost.Endpoints.Tests.Support;

namespace NServiceKit.WebHost.Endpoints.Tests
{
    /// <summary>A HTML result metadata tests.</summary>
	[TestFixture]
	public class HtmlResultMetadataTests : TestBase
	{
        /// <summary>Configures the given container.</summary>
        ///
        /// <param name="container">The container.</param>
		protected override void Configure(Funq.Container container) {}

        /// <summary>A html.</summary>
		public static class Html
		{
            /// <summary>Redirect to.</summary>
            ///
            /// <param name="url">URL of the document.</param>
            ///
            /// <returns>A HttpResult.</returns>
			public static HttpResult RedirectTo(string url)
			{
				var html = string.Format(
					"<html><head><meta http-equiv=\"refresh\" content=\"0;url={0}\"></head></html>",
					url);

				return new HttpResult(html, ContentType.Html) {
					Headers = { { "Location", url } },
				};
			}
		}

        /// <summary>Tests response with HTML result.</summary>
		[Test]
		public void Test_response_with_html_result()
		{
			var mockResponse = new HttpResponseMock();

			const string url = "http://www.NServiceKit.net";
			var htmlResult = Html.RedirectTo(url);

			var reponseWasAutoHandled = mockResponse.WriteToResponse(htmlResult, "text/xml");

			Assert.That(reponseWasAutoHandled, Is.True);

			var expectedOutput = string.Format(
				"<html><head><meta http-equiv=\"refresh\" content=\"0;url={0}\"></head></html>", url);

			var writtenString = mockResponse.GetOutputStreamAsString();
			Assert.That(writtenString, Is.EqualTo(expectedOutput));
			Assert.That(mockResponse.Headers["Location"], Is.EqualTo(url));
		}
	}
}