// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeoControllerTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System.IO;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;
   using System.Web.Mvc;
   using System.Xml.Linq;

   using FluentAssertions;

   using Jane.Controllers;
   using Jane.Infrastructure;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class SeoControllerTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public async Task SiteMap()
      {
         var stringBuilder = new StringBuilder();
         var writer = new StringWriter(stringBuilder);
         var postQueries = new PostQueries(FakePostData.GetStorage());
         var controller = new SeoController(postQueries);
         controller.SetupControllerContext(writer, "http://localhost/sitemap.xml");

         var result = await controller.SiteMap();
         result.ExecuteResult(controller.ControllerContext);

         controller.Response.ContentType.Should().Be("text/xml");
         var document = XDocument.Parse(stringBuilder.ToString());

         XNamespace siteMap = "http://www.sitemaps.org/schemas/sitemap/0.9";
         var urlset = document.Element(siteMap + "urlset");
         var urls = urlset.Elements(siteMap + "url").ToList();
         urls.Count.Should().Be(6);
         var url = urls[4];
         url.Element(siteMap + "loc").Value.Should().Be("http://localhost/blog/post5");
         url.Element(siteMap + "lastmod").Value.Should().Be("2014-05-05T01:02:03.0000000Z");
      }

      [TestMethod, TestCategory("UnitTest")]
      public void Robots()
      {
         var postQueries = new PostQueries(FakePostData.GetStorage());
         var controller = new SeoController(postQueries);
         controller.SetupControllerContext("http://localhost/robots.txt");

         var content = controller.Robots() as ContentResult;
         content.Content.Should().Contain("sitemap: http://localhost/sitemap.xml");
         content.Content.Should().Contain("User-agent: *");
         content.Content.Should().Contain("Disallow: /views/");
      }
   }
}