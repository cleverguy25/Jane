// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResult.cs" company="Jane">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the XmlResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System.Web.Mvc;
   using System.Xml;
   using System.Xml.Linq;

   public class XmlResult : ActionResult
   {
      private readonly XDocument document;

      private readonly string etag;

      public XmlResult(XDocument document, string etag)
      {
         this.document = document;
         this.etag = etag;
      }

      public override void ExecuteResult(ControllerContext context)
      {
         if (this.etag != null)
         {
            context.HttpContext.Response.AddHeader("ETag", this.etag);
         }

         context.HttpContext.Response.ContentType = "text/xml";

         using (var xmlWriter = XmlWriter.Create(context.HttpContext.Response.Output))
         {
            this.document.WriteTo(xmlWriter);
            xmlWriter.Flush();
         }
      }
   }
}