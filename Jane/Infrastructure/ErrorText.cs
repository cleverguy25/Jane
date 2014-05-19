// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorText.cs" company="Jane OSS">
//   Copyright (c) Jane Blog Contributors
// </copyright>
// <summary>
//   Defines the ErrorText type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Jane.Infrastructure
{
   using System;

   public static class ErrorText
   {
      private static readonly string[] Jokes =
         {
            "What's brown and sticky?  A stick.", 
            "I’d tell you a UDP joke, but you may not get it.", 
            "I prefer IP jokes; it’s all in the delivery.", 
            "I could tell you a joke about TCP, but I’d have to keep repeating it until you got it.", 
            "Entropy isn’t what it used to be.", 
            "A biologist, a chemist, and a statistician are out hunting.The biologist shoots at a deer and misses 5ft to the left, the chemist takes a shot and misses 5ft to the right, the statistician yells “We got ‘em!”", 
            "There are two types of people in the world: Those who can extrapolate from incomplete data sets", 
            "There are two types of people in the world: Those who crave closure", 
            "Did you hear about the man who got cooled to absolute zero? He’s 0K now.", 
            "The programmer’s wife tells him: “Run to the store and pick up a loaf of bread. If they have eggs, get a dozen. The programmer comes home with 12 loaves of bread.", 
            "A Photon checks into a hotel and the bellhop asks him if he has any luggage. The Photon replies “No I’m traveling light.”", 
            "It’s hard to explain puns to kleptomaniacs because they always take things literally.", 
            "Helium walks into a bar and orders a beer, the bartender says, “Sorry, we don’t serve noble gases here.” He doesn’t react.", 
            "A Buddhist monk approaches a hotdog stand and says “make me one with everything”.", 
            "C, E flat, and G walk into a bar. The bartender says, “Sorry, no minors”", 
            "The barman says, “We don’t serve time travellers in here.” A time traveller walks into a bar.", 
            "My friend's girlfriend is so slutty she has 67 protons.", 
            "How many surrealists does it take to screw in a light bulb? A fish.", 
            "Silver and Gold walk into a bar. Bartender says “‘ey you, get outta here!” Gold leaves the bar.", 
            "The first rule of Tautology club, is the first rule of Tautology club.", 
            "What do you get when you cross a joke with a rhetorical question?", 
            "A SQL query goes into a bar, walks up to two tables and asks, “Can I join you?”", 
            "Eight bytes walk into a bar.  The bartender asks, “Can I get you anything?” “Yeah,” reply the bytes.  “Make us a double.”", 
            "Q. How did the programmer die in the shower? A. He read the shampoo bottle instructions: Lather.Rinse.Repeat.", 
            "If Iron Man and Silver Surfer team up, they would be alloys.", 
            "What does a sub-atomic duck say?  Quark.", 
            "I read a book on Anti-gravity once.  I couldn't put it down.", 
            "Why did I divide SIN by TAN?  Just COS.", 
            "Two chemists walk into a bar.  The first says “I would like some H20.” The second says “I would like some H20 too.”  The second chemist dies.", 
            "A cop pulls Heisenberg over and says “Do you know how fast you were going?” Heisenberg replies “No, but I know where I am."
         };

      private static readonly Quote[] Quotes =
         {
            new Quote("What did you DO, Ray?", "Ghostbusters"), 
            new Quote(
               "Hello? Hello? Anybody home? Hey! Think, McFly. Think!", 
               "Back to the Future"), 
            new Quote("You're killing me, Smalls!", "The Sandlot"), 
            new Quote("SQUIRREL!", "Up"), 
            new Quote(
               "1466. '67. 1469. 1514. 1981? 1986? Please do not do that. Come on, I swear... Just hang in there one second. Please, God, give me the answer!", 
               "Billy Madison")
         };

      private static readonly string[] Exclamations =
         {
            "Great Odin's Raven!", "By the beard of Zeus, ouch!", 
            "Knights of Columbus, that hurt!", "Son of a bee-sting!", 
            "Oh Shit---ake Mushrooms"
         };

      private static readonly string[] ErrorMessages =
         {
            "Uhh, something went very wrong, our bad.  One hundred little bugs in the code, One hundred little bugs.  Fix a bug, check the fix in, One hundred little bugs in the code.", 
            "Uhh, something went very wrong, sorry about that.  You know what they say, Programming is 10% science, 20% ingenuity, and 70% getting the ingenuity to work with the science."
         };

      public static string GetJoke()
      {
         var index = DateTime.Now.Second % Jokes.Length;

         return Jokes[index];
      }

      public static Quote GetQuote()
      {
         var index = DateTime.Now.Second % Quotes.Length;

         return Quotes[index];
      }

      public static string GetExclamation()
      {
         var index = DateTime.Now.Second % Exclamations.Length;

         return Exclamations[index];
      }

      public static string GetErrorMessage()
      {
         var index = DateTime.Now.Second % ErrorMessages.Length;

         return ErrorMessages[index];
      }

      public class Quote
      {
         public Quote(string text, string source)
         {
            this.Text = text;
            this.Source = source;
         }

         public string Text { get; set; }

         public string Source { get; set; }
      }
   }
}