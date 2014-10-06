// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SemanticLoggingHelper.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Test
{
   using System;
   using System.Collections.Generic;
   using System.Diagnostics.Tracing;

   using FluentAssertions;

   using Jane.Infrastructure;

   using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

   public class SemanticLoggingHelper
   {
      public static void TestServerErrorEntry(
         List<EventEntry> entries,
         object correlationId,
         string controller,
         string action,
         ArgumentException exception)
      {
         entries.Count.Should().Be(1);
         var entry = entries[0];
         entry.EventId.Should().Be(1);
         entry.FormattedMessage.Should().Be("ServerError");
         entry.Payload[0].Should().Be(correlationId);
         entry.Payload[1].Should().Be(controller);
         entry.Payload[2].Should().Be(action);
         entry.Payload[3].Should().Be(exception.Message);
         entry.Payload[4].Should().Be(exception.ToString());
      }

      public static List<EventEntry> SubscribeToEvents()
      {
         var listener = new ObservableEventListener();

         listener.EnableEvents(Tracing.Log, EventLevel.LogAlways);
         var entries = new List<EventEntry>();
         listener.Subscribe(entries.Add);
         return entries;
      }
   }
}