// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tracing.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
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

      [Event(2, Message = "Load Social Providers Error", Level = EventLevel.Error)]
      public void LoadSocialProvidersError(
         string exceptionMessage,
         string fullError)
      {
         this.WriteEvent(2, exceptionMessage, fullError);
      }
   }
}