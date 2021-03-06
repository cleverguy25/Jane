﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TracingTest.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Tracing;
   using System.Runtime.InteropServices;

   using FluentAssertions;

   using Jane.Infrastructure;

   using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
   using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   [TestClass]
   public class TracingTest
   {
      [TestMethod, TestCategory("UnitTest")]
      public void ServerErrorLogged()
      {
         var entries = SemanticLoggingHelper.SubscribeToEvents();

         var correlationId = Guid.NewGuid().ToString();
         const string Controller = "MyController";
         const string Action = "MyAction";
         var exception = new ArgumentException("Test");

         Tracing.Log.ServerError(correlationId, Controller, Action, exception.Message, exception.ToString());

         SemanticLoggingHelper.TestServerErrorEntry(entries, correlationId, Controller, Action, exception);
      }

      [TestMethod, TestCategory("UnitTest")]
      public void ShouldValidateEventSource()
      {
         EventSourceAnalyzer.InspectAll(SemanticLoggingEventSource.Log);
      }
   }
}