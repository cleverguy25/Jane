using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jane.Models
{
   public class PostView
   {
      public Post Post { get; set; }

      public Post PreviousPost { get; set; }

      public Post NextPost { get; set; }
   }
}