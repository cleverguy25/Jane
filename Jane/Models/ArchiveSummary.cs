// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArchiveSummary.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Models
{
   public class ArchiveSummary
   {
      public int Year { get; set; }

      public int Month { get; set; }

      public string MonthDisplay { get; set; }

      public int Count { get; set; }
   }
}