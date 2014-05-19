// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tracing.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the Tracing type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System;
   using System.Diagnostics.Tracing;

   [EventSource(Name = "Jane-Tracing")]
   public class Tracing : EventSource
   {
      private static readonly Lazy<Tracing> Instance = new Lazy<Tracing>(() => new Tracing());

      public static Tracing Log
      {
         get
         {
            return Instance.Value;
         }
      }

      [Event(1, Message = "ServerError", Level = EventLevel.Error)]
      public void ServerError(
         string correlationId, 
         string controllerName, 
         string action, 
         string exceptionMessage, 
         string fullError)
      {
         this.WriteEvent(1, correlationId, controllerName, action, exceptionMessage, fullError);
      }
   }
}