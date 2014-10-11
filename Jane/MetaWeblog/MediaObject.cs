// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaObject.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.MetaWeblog
{
   using CookComputing.XmlRpc;

   [XmlRpcMissingMapping(MappingAction.Ignore)]
   public struct MediaObject
   {
      public byte[] bits;

      public string name;

      public string type;
   }
}