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
      [XmlRpcMember("bits")]
      public byte[] Bits;

      [XmlRpcMember("name")]
      public string Name;

      [XmlRpcMember("type")]
      public string Type;
   }
}