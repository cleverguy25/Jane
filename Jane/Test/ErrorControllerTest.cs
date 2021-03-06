﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorControllerTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System.Net;
   using System.Web;
   using System.Web.Mvc;

   using FluentAssertions;

   using Jane.Controllers;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using Moq;

   using TestStack.FluentMVCTesting;

   [TestClass]
   public class ErrorControllerTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public void NotFoundAction()
      {
         var controller = new ErrorController();
         controller.SetupControllerContext();
         controller.WithCallTo((c) => c.NotFound()).ShouldRenderDefaultView();
         controller.Response.StatusCode.Should().Be(404);
      }

      [TestMethod, TestCategory("UnitTest")]
      public void ServerErrorAction()
      {
         var controller = new ErrorController();
         controller.SetupControllerContext();
         controller.WithCallTo((c) => c.ServerError()).ShouldRenderView("Error");
         controller.Response.StatusCode.Should().Be(500);
      }
   }
}