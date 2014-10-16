// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeoController.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Controllers
{
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using System.Web.Mvc;
   using System.Xml.Linq;

   using Jane.Infrastructure;
   using Jane.Infrastructure.Interfaces;
   using Jane.Models;

   public class SeoController : Controller
   {
      private readonly IPostQueries postQueries;

      public SeoController(IPostQueries postQueries)
      {
         this.postQueries = postQueries;
      }

      public ActionResult Robots()
      {
         Response.ContentType = "text/plain";

         return this.Content(string.Concat(
@"User-agent: *
Disallow: /views/

sitemap: ", 
          Request.Url.Scheme, 
          "://", 
          Request.Url.Authority, 
          "/sitemap.xml"));
      }

      public async Task<ActionResult> SiteMap()
      {
         var posts = await this.postQueries.GetAllPostsAsync();
         this.Response.ContentType = "text/xml";

         XNamespace documentNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
         var document = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"), 
            new XElement(documentNamespace + "urlset", this.GetPosts(documentNamespace, posts)));

         return new XmlResult(document, string.Empty);
      }

      private IEnumerable<XElement> GetPosts(XNamespace documentNamespace, IEnumerable<Post> posts)
      {
         foreach (var post in posts)
         {
            var routeData = new { slug = post.Slug };
            yield return
               new XElement(
                  documentNamespace + "url", 
                  new XElement(
                     documentNamespace + "loc", 
                     this.Url.Action("GetBySlug", "Blog", routeData, this.Request.Url.Scheme)), 
                  new XElement(documentNamespace + "lastmod", post.UpdatedDate.ToString("o")));
         }
      }
   }
}