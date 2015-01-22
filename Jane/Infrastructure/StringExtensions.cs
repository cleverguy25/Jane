// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Jane OSS">
//   Copyright (c) Jane Contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Jane.Infrastructure
{
   using System.Security.Cryptography;
   using System.Text;

   public static class StringExtensions
   {
      public static string CalculateMd5Hash(this string input)
      {
         var hash = CalculateHash(input);

         return ConvertHashToHexString(hash);
      }

      private static string ConvertHashToHexString(byte[] hash)
      {
         var sb = new StringBuilder();
         foreach (var t in hash)
         {
            sb.Append(t.ToString("X2"));
         }

         return sb.ToString();
      }

      private static byte[] CalculateHash(string input)
      {
         var md5 = MD5.Create();
         var inputBytes = Encoding.ASCII.GetBytes(input);
         var hash = md5.ComputeHash(inputBytes);
         return hash;
      }
   }
}